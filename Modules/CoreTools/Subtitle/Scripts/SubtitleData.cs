using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class SubtitleData : PlayableAsset, ITimelineClipAsset
{
    [Header("字幕内容")]

    [TextArea(3, 10)]
    public string chineseText;
    [TextArea(3, 10)]
    public string englishText;

    [Tooltip("独立设置 (仅在轨道设置为'跟随片段'模式有效)")]
    public bool useAutoFade = false;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.chineseText = chineseText;
        behaviour.englishText = englishText;
        behaviour.useAutoFade = useAutoFade;
        behaviour.fadeInDuration = fadeInDuration;
        behaviour.fadeOutDuration = fadeOutDuration;
        return playable;
    }
}