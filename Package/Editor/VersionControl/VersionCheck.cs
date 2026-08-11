using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEditor.PackageManager.Requests;
using System.Linq;
using System.Threading.Tasks;

public static class PackageVersionChecker
{
    private const string PackageName = "com.mogutech.coretools";      // 你的包名
    private const string GithubRepoUrl = "https://github.com/dhes-qngh/MoGuTechCoreTools.git";// 仓库地址
    private const string PackageSubPath = "/Package"; // 包体子路径
    private const string GithubApiUrl = "https://api.github.com/repos/dhes-qngh/MoGuTechCoreTools/tags"; // API地址

    private static bool _isUpdating = false; // 防止重复更新

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.delayCall += CheckForUpdate;
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
        Debug.Log($"Current version: {currentVersion}");

        // 2. 从 GitHub 获取最新版本
        string latestVersion = await GetLatestVersionFromGitHub();
        if (string.IsNullOrEmpty(latestVersion))
        {
            Debug.LogWarning("Could not fetch the latest version from GitHub.");
            return;
        }

        // 3. 比较版本
        if (!IsNewerVersionAvailable(currentVersion, latestVersion))
        {
            Debug.Log($"{PackageName} is up to date (v{currentVersion}).");
            return;
        }

        // 4. 弹窗询问用户
        bool shouldUpdate = EditorUtility.DisplayDialog(
            "核心工具版本过旧",
            $"最新版本({latestVersion}) \n本地版本为{currentVersion}.\n\n请通知负责人员更新",
            "我是负责人员",
            "好的"
        );

        if (!shouldUpdate)
        {
            Debug.Log("稍后请从根目录更新SVN");
            return;
        }

        // 5. 执行自动更新
        _isUpdating = true;
        await PerformUpdate(latestVersion);
        _isUpdating = false;
    }

    private static async Task PerformUpdate(string targetVersion)
    {
        // 构造Git URL
        string gitUrlWithTag = $"{GithubRepoUrl}?path={PackageSubPath}#{targetVersion}";

        Debug.Log($"Updating to {targetVersion} via: {gitUrlWithTag}");

        // 发起更新请求
        var addRequest = Client.Add(gitUrlWithTag);
        while (!addRequest.IsCompleted) await Task.Delay(100);

        if (addRequest.Status == StatusCode.Success)
        {
            Debug.Log($"Successfully updated {PackageName} to version {targetVersion}.");
            EditorUtility.DisplayDialog(
                "升级成功",
                $"已经升级到版本{targetVersion}.\n\n请等待Unity编译",
                "OK"
            );
            AssetDatabase.Refresh();
        }
        else
        {
            string errorMsg = addRequest.Error?.message ?? "Unknown error";
            Debug.LogError($"Update failed: {errorMsg}");
            bool retry = EditorUtility.DisplayDialog(
                "升级失败",
                $"无法升级，请检查网络后重试",
                "重试",
                "取消"
            );
            if (retry)
            {
                // 重置 _isUpdating 状态
                _isUpdating = false; 
                await PerformUpdate(targetVersion); // 递归重试
            }
            else
            {
                Debug.Log("User cancelled retry.");
            }
        }
    }

    private static async Task<string> GetLatestVersionFromGitHub()
    {
        using (var client = new System.Net.Http.HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "UnityPackageVersionChecker");
            try
            {
                var response = await client.GetAsync(GithubApiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.LogError($"GitHub API error: {response.StatusCode}");
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync();
                // 简单解析第一个 "name" 字段（即最新 tag）
                const string nameKey = "\"name\":\"";
                int start = json.IndexOf(nameKey);
                if (start == -1)
                {
                    Debug.LogError("No tag name found in GitHub response.");
                    return null;
                }
                start += nameKey.Length;
                int end = json.IndexOf('"', start);
                if (end == -1) return null;

                string tag = json.Substring(start, end - start);
                return tag; // 保留原样（可能带 v 前缀）
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Exception while fetching latest version: {ex.Message}");
                return null;
            }
        }
    }

    private static bool IsNewerVersionAvailable(string current, string latest)
    {
        // 尝试用 System.Version 比较（支持 "1.2.3" 或 "1.2.3.4"）
        if (System.Version.TryParse(current, out var v1) && System.Version.TryParse(latest, out var v2))
        {
            return v2 > v1;
        }
        // 字符串比较
        return string.Compare(latest, current, System.StringComparison.Ordinal) > 0;
    }
}