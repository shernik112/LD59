using System;
using UnityEngine;
using System.Collections;

public class PauseHandler : OnBehaviour, IService
{
    [SerializeField] private float autoUnpauseDelay = 2f;
    public event Action<bool> OnPause;
    
    protected override bool UpdateWhenPaused => true;

    private Coroutine autoUnpauseRoutine;

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (PauseAll.True)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        PauseAll.Add(this);
        Time.timeScale = 0f;

        if (autoUnpauseRoutine != null)
            StopCoroutine(autoUnpauseRoutine);

        autoUnpauseRoutine = StartCoroutine(AutoUnpause());
        OnPause?.Invoke(true);
    }

    private void Unpause()
    {
        if (autoUnpauseRoutine != null)
        {
            StopCoroutine(autoUnpauseRoutine);
            autoUnpauseRoutine = null;
        }

        PauseAll.Remove(this);
        Time.timeScale = 1f;
        OnPause?.Invoke(false);
    }

    private IEnumerator AutoUnpause()
    {
        yield return new WaitForSecondsRealtime(autoUnpauseDelay);
        Unpause();
    }
}
