using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEditor.PackageManager.Requests;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class PackageVersionChecker
{
    private const string PackageName = "com.mogutech.coretools";
    private const string Owner = "dhes-qngh";
    private const string Repo = "MoGuTechCoreTools";
    private static readonly string ReleaseApiUrl = $"https://api.github.com/repos/{Owner}/{Repo}/releases/latest";

    private static bool _isUpdating = false;

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.delayCall += CheckForUpdate;
    }

    [MenuItem("Tools/Check for Package Updates")]
    public static void ManualCheck()
    {
        CheckForUpdate();
    }

    private static async void CheckForUpdate()
    {
        if (_isUpdating) return;

        // 1. 获取当前安装的版本
        var listRequest = Client.List(true);
        while (!listRequest.IsCompleted) await Task.Delay(100);

        var installedPackage = listRequest.Result.FirstOrDefault(p => p.name == PackageName);
        if (installedPackage == null)
        {
            Debug.LogWarning($"Package '{PackageName}' is not installed in this project.");
            return;
        }

        string currentVersion = installedPackage.version;
        Debug.Log($"当前安装版本: {currentVersion}");

        // 2. 获取远程最新 Release 信息
        var releaseInfo = await GetLatestReleaseInfo();
        if (releaseInfo == null)
        {
            Debug.LogWarning("无法获取最新版本信息。");
            return;
        }

        string latestVersion = releaseInfo.tag_name;
        string downloadUrl = releaseInfo.download_url;

        if (string.IsNullOrEmpty(downloadUrl))
        {
            Debug.LogError("最新 Release 中未找到 .tgz 附件。");
            return;
        }

        // 3. 比较版本，如果已最新则退出
        if (!IsNewerVersionAvailable(currentVersion, latestVersion))
        {
            Debug.Log($"{PackageName} 已是最新 (v{currentVersion})。");
            return;
        }

        // 4. 检查本地 Packages 文件夹是否已有较新的 .tgz 包
        var localTgzInfo = FindLocalTgzPackage();
        if (localTgzInfo.path != null && IsNewerOrEqual(localTgzInfo.version, latestVersion))
        {
            Debug.Log($"发现本地 tgz 包，版本 {localTgzInfo.version}，直接安装...");
            _isUpdating = true;
            await InstallFromLocalTgz(localTgzInfo.path);
            _isUpdating = false;
            return;
        }

        // 5. 没有本地可用 tgz 或版本不够，弹窗询问下载
        bool shouldUpdate = EditorUtility.DisplayDialog(
            "核心工具版本过旧",
            $"最新版本 ({latestVersion}) \n本地版本为 {currentVersion}。\n\n是否立即更新？",
            "我是负责人员",
            "稍后"
        );

        if (!shouldUpdate)
        {
            Debug.Log("用户取消更新。");
            return;
        }

        // 6. 执行下载并安装
        _isUpdating = true;
        await PerformUpdate(latestVersion, downloadUrl);
        _isUpdating = false;
    }

    // ------------------------------------------------------------
    // 查找本地 .tgz 包
    // ------------------------------------------------------------
    private static (string path, string version) FindLocalTgzPackage()
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string packagesFolder = Path.Combine(projectRoot, "Packages");
        if (!Directory.Exists(packagesFolder)) return default;

        var files = Directory.GetFiles(packagesFolder, $"{PackageName}*.tgz");
        if (files.Length == 0) return default;

        // 提取版本号（去掉前缀和 .tgz）
        var versionedFiles = files
            .Select(f => {
                string name = Path.GetFileName(f);
                string versionPart = name.Substring(PackageName.Length + 1); // 去掉 "包名-"
                if (versionPart.EndsWith(".tgz"))
                    versionPart = versionPart.Substring(0, versionPart.Length - 4);
                return (path: f, version: versionPart);
            })
            .Where(t => System.Version.TryParse(t.version.TrimStart('v'), out _)) // 只保留合法版本号
            .ToList();

        if (!versionedFiles.Any()) return default;

        // 取版本号最大的（按 Version 比较）
        var best = versionedFiles
            .Select(t => (t.path, version: new System.Version(t.version.TrimStart('v'))))
            .OrderByDescending(t => t.version)
            .First();

        return (best.path, best.version.ToString());
    }

    // 版本比较（>=）
    private static bool IsNewerOrEqual(string versionA, string versionB)
    {
        if (System.Version.TryParse(versionA.TrimStart('v'), out var vA) &&
            System.Version.TryParse(versionB.TrimStart('v'), out var vB))
        {
            return vA >= vB;
        }
        return string.Compare(versionA, versionB, System.StringComparison.Ordinal) >= 0;
    }

    // ------------------------------------------------------------
    // 从本地 tgz 安装
    // ------------------------------------------------------------
    private static async Task InstallFromLocalTgz(string tgzPath)
    {
        string fileUrl = $"file:///{tgzPath.Replace('\\', '/')}";
        Debug.Log($"从本地安装: {fileUrl}");

        var addRequest = Client.Add(fileUrl);
        while (!addRequest.IsCompleted) await Task.Delay(100);

        if (addRequest.Status == StatusCode.Success)
        {
            Debug.Log($"安装成功: {tgzPath}");
            EditorUtility.DisplayDialog(
                "升级成功",
                $"已使用本地包升级。\n\nUnity 将重新编译。\n包路径：{tgzPath}",
                "OK"
            );
            AssetDatabase.Refresh();
        }
        else
        {
            string errorMsg = addRequest.Error?.message ?? "未知错误";
            Debug.LogError($"安装失败: {errorMsg}");
            EditorUtility.DisplayDialog(
                "安装失败",
                $"从本地 tgz 安装失败：{errorMsg}",
                "OK"
            );
        }
    }

    // ------------------------------------------------------------
    // 下载并安装（原有逻辑）
    // ------------------------------------------------------------
    private static async Task PerformUpdate(string targetVersion, string downloadUrl)
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string packagesFolder = Path.Combine(projectRoot, "Packages");
        if (!Directory.Exists(packagesFolder))
        {
            Debug.LogError("Packages folder not found!");
            EditorUtility.DisplayDialog("更新失败", "找不到 Packages 文件夹。", "OK");
            return;
        }

        string fileName = $"{PackageName}-{targetVersion}.tgz";
        string localTgzPath = Path.Combine(packagesFolder, fileName);

        // 下载
        bool downloadSuccess = await DownloadFileWithProgress(downloadUrl, localTgzPath);
        if (!downloadSuccess)
        {
            bool retry = EditorUtility.DisplayDialog(
                "下载失败",
                "无法下载更新包，请检查网络后重试。",
                "重试",
                "取消"
            );
            if (retry)
            {
                await PerformUpdate(targetVersion, downloadUrl);
            }
            else
            {
                Debug.Log("用户取消下载重试。");
            }
            return;
        }

        // 安装
        await InstallFromLocalTgz(localTgzPath);
    }

    // ------------------------------------------------------------
    // 下载文件（带进度）
    // ------------------------------------------------------------
    private static async Task<bool> DownloadFileWithProgress(string url, string destPath)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                if (EditorUtility.DisplayCancelableProgressBar(
                    "正在下载更新包",
                    $"下载 {Path.GetFileName(destPath)} ...",
                    request.downloadProgress))
                {
                    EditorUtility.ClearProgressBar();
                    request.Abort();
                    Debug.Log("用户取消下载。");
                    return false;
                }
                await Task.Delay(100);
            }
            EditorUtility.ClearProgressBar();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"下载错误: {request.error}");
                return false;
            }

            try
            {
                File.WriteAllBytes(destPath, request.downloadHandler.data);
                Debug.Log($"下载完成: {destPath}");
                return true;
            }
            catch (IOException ex)
            {
                Debug.LogError($"写入文件失败: {ex.Message}");
                return false;
            }
        }
    }

    // ------------------------------------------------------------
    // 获取最新 Release 信息
    // ------------------------------------------------------------
    private static async Task<ReleaseInfo> GetLatestReleaseInfo()
    {
        using (var request = UnityWebRequest.Get(ReleaseApiUrl))
        {
            request.SetRequestHeader("User-Agent", "UnityPackageUpdater");
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Delay(100);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"GitHub API 错误: {request.error}");
                return null;
            }

            try
            {
                string json = request.downloadHandler.text;
                return ParseReleaseJson(json);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"解析 JSON 失败: {ex.Message}");
                return null;
            }
        }
    }

    private static ReleaseInfo ParseReleaseJson(string json)
    {
        string tagName = ExtractJsonString(json, "tag_name");
        if (string.IsNullOrEmpty(tagName)) return null;

        string downloadUrl = null;
        const string assetsKey = "\"assets\":[";
        int assetsStart = json.IndexOf(assetsKey);
        if (assetsStart != -1)
        {
            int assetsEnd = FindMatchingBracket(json, assetsStart + assetsKey.Length - 1);
            if (assetsEnd != -1)
            {
                string assetsJson = json.Substring(assetsStart + assetsKey.Length, assetsEnd - assetsStart - assetsKey.Length);
                int objStart = assetsJson.IndexOf('{');
                while (objStart != -1)
                {
                    int objEnd = FindMatchingBracket(assetsJson, objStart);
                    if (objEnd == -1) break;
                    string assetJson = assetsJson.Substring(objStart, objEnd - objStart + 1);
                    string name = ExtractJsonString(assetJson, "name");
                    string url = ExtractJsonString(assetJson, "browser_download_url");
                    if (!string.IsNullOrEmpty(name) && name.EndsWith(".tgz") && !string.IsNullOrEmpty(url))
                    {
                        downloadUrl = url;
                        break;
                    }
                    objStart = assetsJson.IndexOf('{', objEnd + 1);
                }
            }
        }

        if (string.IsNullOrEmpty(downloadUrl))
        {
            Debug.LogError("未在 Release 中找到 .tgz 附件。");
            return null;
        }

        return new ReleaseInfo { tag_name = tagName, download_url = downloadUrl };
    }

    private static int FindMatchingBracket(string json, int startIndex)
    {
        char open = json[startIndex];
        char close = open == '{' ? '}' : ']';
        int depth = 0;
        for (int i = startIndex; i < json.Length; i++)
        {
            if (json[i] == open) depth++;
            else if (json[i] == close)
            {
                depth--;
                if (depth == 0) return i;
            }
        }
        return -1;
    }

    private static string ExtractJsonString(string json, string key)
    {
        string search = $"\"{key}\":\"";
        int start = json.IndexOf(search);
        if (start == -1) return null;
        start += search.Length;
        int end = json.IndexOf('"', start);
        if (end == -1) return null;
        return json.Substring(start, end - start);
    }

    private static bool IsNewerVersionAvailable(string current, string latest)
    {
        if (System.Version.TryParse(current.TrimStart('v'), out var v1) &&
            System.Version.TryParse(latest.TrimStart('v'), out var v2))
            return v2 > v1;
        return string.Compare(latest, current, System.StringComparison.Ordinal) > 0;
    }

    private class ReleaseInfo
    {
        public string tag_name;
        public string download_url;
    }
}