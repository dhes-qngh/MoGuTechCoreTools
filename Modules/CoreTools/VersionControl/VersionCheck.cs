using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEditor.PackageManager.Requests;
using System.Linq;

public static class VersionCheck
{
    
    private const string PackageName = "com.mogutech.coretools";
    
    private const string GithubApiUrl = "https://api.github.com/repos/yourusername/yourrepository/tags";

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        // 编辑器启动或脚本重载后执行检查
        EditorApplication.delayCall += CheckForUpdate;
    }

    private static async void CheckForUpdate()
    {
        // 1. 获取当前安装的包版本
        var listRequest = Client.List(true); // true表示包括间接依赖的包
        while (!listRequest.IsCompleted) { await System.Threading.Tasks.Task.Delay(100); }

        var installedPackage = listRequest.Result.FirstOrDefault(p => p.name == PackageName);
        if (installedPackage == null)
        {
            Debug.LogWarning($"Package '{PackageName}' is not installed in this project.");
            return;
        }

        string currentVersion = installedPackage.version;
        Debug.Log($"Current version of {PackageName}: {currentVersion}");

        // 2. 从GitHub获取最新版本
        string latestVersion = await GetLatestVersionFromGitHub();
        if (string.IsNullOrEmpty(latestVersion))
        {
            Debug.LogWarning("Could not fetch the latest version from GitHub.");
            return;
        }

        // 3. 比较版本并弹窗提示
        if (IsNewerVersionAvailable(currentVersion, latestVersion))
        {
            bool shouldUpdate = EditorUtility.DisplayDialog(
                "Package Update Available",
                $"A new version ({latestVersion}) of '{PackageName}' is available. You are currently on version {currentVersion}. Would you like to update?",
                "Yes, Update",
                "No, Thanks"
            );

            if (shouldUpdate)
            {
                // 此处可以触发更新逻辑，例如通过 Client.Add 或 Git URL 重新安装
                Debug.Log($"User chose to update {PackageName} to {latestVersion}.");
                // 注意: 实际更新通常需要用户通过Package Manager窗口手动操作，
                // 或者由脚本通过Git URL重新安装（风险较高，需谨慎处理）。
                // 这里仅作为示例，简单提醒。
                EditorUtility.DisplayDialog("Update", $"Please update '{PackageName}' manually via the Package Manager Window.", "OK");
            }
        }
        else
        {
            Debug.Log($"{PackageName} is up to date (version {currentVersion}).");
        }
    }

    private static async System.Threading.Tasks.Task<string> GetLatestVersionFromGitHub()
    {
        using (var client = new System.Net.Http.HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "UnityPackageVersionChecker");
            try
            {
                var response = await client.GetAsync(GithubApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                
                    // 简单解析：查找第一个 "name":"xxx" 
                    // 注意：GitHub 返回的 tags 数组第一个元素就是最新 tag
                    const string nameKey = "\"name\":\"";
                    int startIndex = json.IndexOf(nameKey);
                    if (startIndex != -1)
                    {
                        startIndex += nameKey.Length;
                        int endIndex = json.IndexOf('"', startIndex);
                        if (endIndex != -1)
                        {
                            string version = json.Substring(startIndex, endIndex - startIndex);
                            return version.TrimStart('v'); // 去掉可能的 'v' 前缀
                        }
                    }
                    Debug.LogError("Failed to parse version from GitHub response.");
                }
                else
                {
                    Debug.LogError($"Failed to fetch tags from GitHub: {response.StatusCode}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error fetching latest version: {ex.Message}");
            }
            return null;
        }
    }

    private static bool IsNewerVersionAvailable(string current, string latest)
    {
        // 简单的字符串比较，生产环境建议使用 System.Version 或 SemVer 库
        if (System.Version.TryParse(current, out var v1) && System.Version.TryParse(latest, out var v2))
        {
            return v2 > v1;
        }
        // 如果解析失败，回退到字符串比较
        return string.Compare(latest, current, System.StringComparison.Ordinal) > 0;
    }

    // 用于JSON反序列化的辅助类
    [System.Serializable]
    private class GitTag
    {
        public string name;
    }
}