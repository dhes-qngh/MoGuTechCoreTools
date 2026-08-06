using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEditor.Timeline;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class SubtitleImporterEditor : EditorWindow
{
    private string subtitlePath = "";
    private double defaultClipDuration = 2.0;
    private double gapBetweenClips = 0.5;

    [MenuItem("Tools/Subtitle/自定义轨道字幕生成器")]
    public static void ShowWindow()
    {
        GetWindow<SubtitleImporterEditor>("字幕导入");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("字幕轨道生成工具", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("选择字幕文件 (.txt)", GUILayout.Height(30)))
        {
            subtitlePath = EditorUtility.OpenFilePanel("选择字幕文本文档", "", "txt");
        }

        if (!string.IsNullOrEmpty(subtitlePath))
        {
            EditorGUILayout.HelpBox($"当前选择: {Path.GetFileName(subtitlePath)}", MessageType.None);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        defaultClipDuration = EditorGUILayout.DoubleField("片段时长(秒)", defaultClipDuration);
        gapBetweenClips = EditorGUILayout.DoubleField("片段间隔(秒)", gapBetweenClips);

        EditorGUILayout.Space();

        var currentDirector = TimelineEditor.inspectedDirector;
        if (currentDirector == null)
        {
            EditorGUILayout.HelpBox("请先在 Timeline 窗口打开资源，这样我才能工作哦~", MessageType.Warning);
            GUI.enabled = false;
        }
        else
        {
            GUI.enabled = !string.IsNullOrEmpty(subtitlePath);
        }

        if (GUILayout.Button("开始生成", GUILayout.Height(40)))
        {
            ImportSubtitles();
        }
        GUI.enabled = true;
    }

    private void ImportSubtitles()
    {
        TimelineAsset timelineAsset = TimelineEditor.inspectedAsset;
        if (timelineAsset == null)
        {
            EditorUtility.DisplayDialog("搞错了重来", "厚礼谢！先打开Timeline窗口再导入字幕！八嘎！！", "我知道错了");
            return;
        }

        List<Vector2d> existingTimes = new List<Vector2d>();
        SubtitleTrack oldTrack = null;

        foreach (var t in timelineAsset.GetOutputTracks())
        {
            if (t is SubtitleTrack && t.name == "切勿改名_字幕轨道")
            {
                oldTrack = (SubtitleTrack)t;

                var clips = oldTrack.GetClips().ToList();
                clips.Sort((a, b) => a.start.CompareTo(b.start));

                foreach (var clip in clips)
                {
                    existingTimes.Add(new Vector2d(clip.start, clip.duration));
                }
                break;
            }
        }

        string fileContent = File.ReadAllText(subtitlePath);
        string[] lines = fileContent.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        Undo.RecordObject(timelineAsset, "重新编辑字幕");

        if (oldTrack != null)
        {
            timelineAsset.DeleteTrack(oldTrack);
        }

        SubtitleTrack track = timelineAsset.CreateTrack<SubtitleTrack>(null, "切勿改名_字幕轨道");

        double currentTime = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string cn = line;
            string en = "";

            if (line.Contains("&"))
            {
                string[] parts = line.Split('&');
                cn = parts[0].Trim();
                en = parts[1].Trim();
            }

            TimelineClip clip = track.CreateClip<SubtitleData>();

            if (i < existingTimes.Count)
            {
                clip.start = existingTimes[i].x;
                clip.duration = existingTimes[i].y;
            }
            else
            {
                clip.start = currentTime;
                clip.duration = defaultClipDuration;
                currentTime = clip.start + clip.duration + gapBetweenClips;
            }

            clip.displayName = cn.Length > 15 ? cn.Substring(0, 15) + "..." : cn;

            SubtitleData data = clip.asset as SubtitleData;
            if (data != null)
            {
                data.chineseText = cn;
                data.englishText = en;
            }

            if (i >= existingTimes.Count)
            {

            }
            else
            {
                currentTime = clip.start + clip.duration + gapBetweenClips;
            }
        }

        TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
        EditorUtility.DisplayDialog("哦耶对了", $"叮~~主人：\n字幕已成功导入！\n成功保留了前 {Mathf.Min(lines.Length, existingTimes.Count)} 条字幕的对位信息~\n文件路径：{subtitlePath}", "朕知道了");
    }

    private struct Vector2d
    {
        //X = 起始点时间  Y = Clip持续时间
        public double x, y;
        public Vector2d(double x, double y) { this.x = x; this.y = y; }
    }
}