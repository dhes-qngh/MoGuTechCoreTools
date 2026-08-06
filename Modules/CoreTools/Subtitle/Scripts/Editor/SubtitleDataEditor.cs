using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using System.Linq;

[CustomEditor(typeof(SubtitleData))]
public class SubtitleDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("字幕内容设置", EditorStyles.boldLabel);

        // 中文文本框
        EditorGUILayout.LabelField("中文文本");
        SerializedProperty chineseProp = serializedObject.FindProperty("chineseText");
        string oldCn = chineseProp.stringValue;
        chineseProp.stringValue = EditorGUILayout.TextArea(chineseProp.stringValue, GUILayout.Height(60));

        EditorGUILayout.Space();

        // 英文文本框
        EditorGUILayout.LabelField("英文文本");
        SerializedProperty englishProp = serializedObject.FindProperty("englishText");
        englishProp.stringValue = EditorGUILayout.TextArea(englishProp.stringValue, GUILayout.Height(60));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("动画设置", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("useAutoFade"), new GUIContent("开启淡入淡出"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeInDuration"), new GUIContent("淡入秒数"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeOutDuration"), new GUIContent("淡出秒数"));

        //同步修改Timeline轨道上Clip的名称
        if (chineseProp.stringValue != oldCn)
        {
            SyncClipDisplayName(chineseProp.stringValue);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void SyncClipDisplayName(string newName)
    {
        var selectedClips = TimelineEditor.selectedClips;

        if (selectedClips != null && selectedClips.Length > 0)
        {
            foreach (var clip in selectedClips)
            {
                if (clip.asset == target)
                {
                    string shortName = newName.Length > 12 ? newName.Substring(0, 12) + "..." : newName;
                    clip.displayName = string.IsNullOrEmpty(shortName) ? "空字幕" : shortName;
                }
            }

            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }
    }
}