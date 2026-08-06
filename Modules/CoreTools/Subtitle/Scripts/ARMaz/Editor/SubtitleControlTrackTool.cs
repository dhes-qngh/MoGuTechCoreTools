using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SubtitleControlTrackTool : EditorWindow
{
    private string subtitlePath = "";
    private GameObject subtitlePool;
    private GameObject subtitlePrefab;
    private double defaultDuration = 2.0;

    [MenuItem("Tools/Subtitle/Control轨道字幕生成器（适配AR Maz）")]
    public static void ShowWindow() => GetWindow<SubtitleControlTrackTool>("Control轨道字幕工具");

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("ARMazPro适配方案", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("选择字幕 TXT 文件 (格式: 中文&英文)", GUILayout.Height(30)))
        {
            subtitlePath = EditorUtility.OpenFilePanel("选择字幕文件", "", "txt");
        }
        if (!string.IsNullOrEmpty(subtitlePath))
            EditorGUILayout.HelpBox("当前文件: " + Path.GetFileName(subtitlePath), MessageType.None);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        subtitlePool = (GameObject)EditorGUILayout.ObjectField("字幕挂载根节点", subtitlePool, typeof(GameObject), true);
        subtitlePrefab = (GameObject)EditorGUILayout.ObjectField("字幕模板(Prefab)", subtitlePrefab, typeof(GameObject), false);
        defaultDuration = EditorGUILayout.DoubleField("默认片段时长 (秒)", defaultDuration);

        EditorGUILayout.Space();

        var currentDirector = TimelineEditor.inspectedDirector;
        if (currentDirector == null)
        {
            EditorGUILayout.HelpBox("请先在 Timeline 窗口打开资源，这样我才能工作哦~", MessageType.Warning);
            GUI.enabled = false;
        }
        else
        {
            GUI.enabled = !string.IsNullOrEmpty(subtitlePath) && subtitlePool != null;
        }

        if (GUILayout.Button("开始生成", GUILayout.Height(40)))
        {
            BakeControlTrack();
        }
        GUI.enabled = true;
    }

    private void BakeControlTrack()
    {
        TimelineAsset timelineAsset = TimelineEditor.inspectedAsset;
        PlayableDirector director = TimelineEditor.inspectedDirector;

        if (timelineAsset == null || director == null)
        {
            EditorUtility.DisplayDialog("搞错了重来", "厚礼谢！先打开Timeline窗口再导入字幕！八嘎！！", "我知道错了");
            return;
        }

        List<Vector2d> existingTimes = new List<Vector2d>();
        var tracks = timelineAsset.GetOutputTracks();
        ControlTrack oldTrack = null;

        foreach (var t in tracks)
        {
            if (t.name == "切勿改名_Control字幕轨道")
            {
                oldTrack = t as ControlTrack;

                var clips = new List<TimelineClip>(oldTrack.GetClips());
                clips.Sort((a, b) => a.start.CompareTo(b.start));

                foreach (var clip in clips)
                {
                    existingTimes.Add(new Vector2d(clip.start, clip.duration));
                }
                break;
            }
        }

        string[] lines = File.ReadAllText(subtitlePath).Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        Undo.RecordObject(timelineAsset, "重新编辑字幕");
        if (oldTrack != null)
        {
            timelineAsset.DeleteTrack(oldTrack);
        }

        List<GameObject> oldChildren = new List<GameObject>();
        foreach (Transform child in subtitlePool.transform) oldChildren.Add(child.gameObject);
        oldChildren.ForEach(DestroyImmediate);

        ControlTrack mainTrack = timelineAsset.CreateTrack<ControlTrack>(null, "切勿改名_Control字幕轨道");
        double lastEndTime = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string cn = line.Contains("&") ? line.Split('&')[0] : line;
            string en = line.Contains("&") ? line.Split('&')[1] : "";

            GameObject subObj;
            if (subtitlePrefab != null)
                subObj = (GameObject)PrefabUtility.InstantiatePrefab(subtitlePrefab);
            else
            {
                subObj = new GameObject($"[Sub_{i:D2}]{(cn.Length > 5 ? cn.Substring(0, 5) : cn)}");
                subObj.AddComponent<TextMeshProUGUI>();
            }
            subObj.transform.SetParent(subtitlePool.transform, false);
            subObj.SetActive(false);

            var tmp = subObj.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = string.IsNullOrEmpty(en) ? cn : $"{cn}\n<size=75%>{en}</size>";

            TimelineClip clip = mainTrack.CreateDefaultClip();
            clip.displayName = cn;

            if (i < existingTimes.Count)
            {
                clip.start = existingTimes[i].x;
                clip.duration = existingTimes[i].y;
            }
            else
            {
                clip.start = (i == 0) ? 0 : lastEndTime;
                clip.duration = defaultDuration;
            }
            lastEndTime = clip.start + clip.duration;

            var controlAsset = clip.asset as ControlPlayableAsset;
            if (controlAsset != null)
            {
                controlAsset.updateITimeControl = false;
                controlAsset.updateDirector = false;
                controlAsset.updateParticle = false;

                string uuid = System.Guid.NewGuid().ToString();
                controlAsset.sourceGameObject.exposedName = new PropertyName(uuid);

                Undo.RecordObject(director, "更新timeline引用");
                director.SetReferenceValue(controlAsset.sourceGameObject.exposedName, subObj);
            }
        }

        EditorUtility.SetDirty(timelineAsset);
        EditorUtility.SetDirty(director);
        TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);

        EditorUtility.DisplayDialog("哦耶对了", $"叮~~主人：\n字幕已成功导入！\n成功保留了前 {Mathf.Min(lines.Length, existingTimes.Count)} 条字幕的对位信息~\n文件路径：{subtitlePath}", "朕知道了");
    }

    private struct Vector2d
    {
        public double x;
        public double y;
        public Vector2d(double x, double y) { this.x = x; this.y = y; }
    }
}