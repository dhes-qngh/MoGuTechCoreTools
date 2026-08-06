using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public enum SubtitleFadeMode
{
    [InspectorName("跟随每个片段设置")] FollowClipSettings,
    [InspectorName("全局强制开启")] OverrideOn,
    [InspectorName("全局强制关闭")] OverrideOff
}

[TrackColor(0.1f, 0.8f, 0.4f)]
[TrackBindingType(typeof(CanvasGroup))]
[TrackClipType(typeof(SubtitleData))]
public class SubtitleTrack : TrackAsset
{
    [Header("轨道全局设置")]
    public SubtitleFadeMode fadeMode = SubtitleFadeMode.OverrideOff;

    [Header("全局淡化时长")]
    [Range(0f, 2f)] public float globalFadeInDuration = 0.5f;
    [Range(0f, 2f)] public float globalFadeOutDuration = 0.5f;

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var playable = ScriptPlayable<SubtitleMixer>.Create(graph, inputCount);
        var mixer = playable.GetBehaviour();

        mixer.trackFadeMode = fadeMode;
        mixer.trackFadeIn = globalFadeInDuration;
        mixer.trackFadeOut = globalFadeOutDuration;

        return playable;
    }
}