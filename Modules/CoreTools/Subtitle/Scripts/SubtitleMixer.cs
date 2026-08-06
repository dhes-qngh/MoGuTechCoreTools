using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Timeline;

public class SubtitleMixer : PlayableBehaviour
{
    public SubtitleFadeMode trackFadeMode;
    public float trackFadeIn;
    public float trackFadeOut;

    private TextMeshProUGUI _textMesh;
    private CanvasGroup _canvasGroup;
    private RectTransform _bgRect;
    private RectTransform _textRect;
    private string _lastText = "";

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        _canvasGroup = playerData as CanvasGroup;
        if (_canvasGroup == null) return;

        if (_textMesh == null)
        {
            _textMesh = _canvasGroup.GetComponentInChildren<TextMeshProUGUI>();
            if (_textMesh != null)
            {
                _textRect = _textMesh.GetComponent<RectTransform>();
                _bgRect = _textRect.parent.GetComponent<RectTransform>();
            }
        }
        if (_textMesh == null) return;

        int inputCount = playable.GetInputCount();
        string bestChinese = "";
        string bestEnglish = "";
        float currentMaxWeight = -1f;
        float finalAlpha = 0f;
        bool hasAnyActiveClip = false;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<SubtitleBehaviour> inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
            SubtitleBehaviour behaviour = inputPlayable.GetBehaviour();

            if (inputWeight > 0)
            {
                hasAnyActiveClip = true;

                //根据权重决定当前显示哪条文本
                if (inputWeight > currentMaxWeight)
                {
                    currentMaxWeight = inputWeight;
                    bestChinese = behaviour.chineseText;
                    bestEnglish = behaviour.englishText;
                }

                bool shouldFade = false;
                float fIn = 0, fOut = 0;

                switch (trackFadeMode)
                {
                    case SubtitleFadeMode.OverrideOn:
                        shouldFade = true;
                        fIn = trackFadeIn;
                        fOut = trackFadeOut;
                        break;
                    case SubtitleFadeMode.OverrideOff:
                        shouldFade = false;
                        break;
                    case SubtitleFadeMode.FollowClipSettings:
                        shouldFade = behaviour.useAutoFade;
                        fIn = behaviour.fadeInDuration;
                        fOut = behaviour.fadeOutDuration;
                        break;
                }

                float calculatedAlpha = 1.0f;
                if (shouldFade)
                {
                    double duration = inputPlayable.GetDuration();
                    double time = inputPlayable.GetTime();

                    float fadeInAlpha = (fIn > 0) ? Mathf.Clamp01((float)(time / fIn)) : 1f;
                    float fadeOutAlpha = (fOut > 0) ? Mathf.Clamp01((float)((duration - time) / fOut)) : 1f;
                    calculatedAlpha = Mathf.Min(fadeInAlpha, fadeOutAlpha);

                    //手动拉混合曲线
                    calculatedAlpha = Mathf.Min(calculatedAlpha, inputWeight);
                }
                else
                {
                    calculatedAlpha = 1.0f;
                }

                finalAlpha = Mathf.Max(finalAlpha, calculatedAlpha);
            }
        }

        if (!hasAnyActiveClip)
        {
            ClearUI();
            return;
        }

        string finalDisplay = "";
        bool hasCN = !string.IsNullOrEmpty(bestChinese);
        bool hasEN = !string.IsNullOrEmpty(bestEnglish);
        if (hasCN && hasEN)
            finalDisplay = $"{bestChinese}\n<size=75%>{bestEnglish}</size>";
        else if (hasCN)
            finalDisplay = bestChinese;
        else if (hasEN)
            finalDisplay = bestEnglish;

        if (_lastText != finalDisplay)
        {
            _textMesh.text = finalDisplay;
            _lastText = finalDisplay;
            if (_bgRect != null && _textRect != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_textRect);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_bgRect);
            }
        }

        _canvasGroup.alpha = finalAlpha;
    }

    /// <summary>
    /// Timeline停止/强制关闭
    /// </summary>
    /// <param name="playable"></param>
    public override void OnPlayableDestroy(Playable playable)
    {
        ClearUI();
    }

    private void ClearUI()
    {
        if (_canvasGroup != null) _canvasGroup.alpha = 0f;
        if (_textMesh != null) _textMesh.text = "";
        _lastText = "";
    }
}