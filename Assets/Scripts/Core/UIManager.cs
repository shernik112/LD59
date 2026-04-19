using System;
using System.Collections;
using UnityEngine;

public class UIManager : OnBehaviour, IService
{
    [SerializeField] private CanvasGroup pauseCanvas;

    private readonly float _duration = 0.3f;
    private PauseHandler _pauseHandler;
    private Coroutine _fadeRoutine;

    protected override void OnInitialize()
    {
        _pauseHandler = ServiceLocator.Instance.Get<PauseHandler>();
        _pauseHandler.OnPause += ShowPauseCanvas;
    }

    private void OnDestroy()
    {
        if (_pauseHandler != null)
            _pauseHandler.OnPause -= ShowPauseCanvas;
    }

    private void ShowPauseCanvas(bool show)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeCanvas(show));
    }

    private IEnumerator FadeCanvas(bool show)
    {
        var start = pauseCanvas.alpha;
        var target = show ? 1f : 0f;

        var time = 0f;

        pauseCanvas.interactable = show;
        pauseCanvas.blocksRaycasts = show;

        while (time < _duration)
        {
            time += Time.unscaledDeltaTime;
            var t = time / _duration;

            pauseCanvas.alpha = Mathf.Lerp(start, target, t);
            yield return null;
        }

        pauseCanvas.alpha = target;
    }
}