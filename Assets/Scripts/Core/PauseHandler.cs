using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseHandler : OnBehaviour, IService
{
    private float _autoUnpauseDelay = 2f;
    public event Action<bool> OnPause;
    
    protected override bool UpdateWhenPaused => true;

    private Coroutine _autoUnpauseRoutine;
    private PlayerMover _playerMover;
    
    protected override void OnInitialize()
    {
        _playerMover = ServiceLocator.Instance.Get<PlayerMover>();
        _playerMover.OnDestroyCar += TogglePause;
    }

    private void OnDestroy()
    {
        _playerMover.OnDestroyCar -= TogglePause;
    }

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

        if (_autoUnpauseRoutine != null)
            StopCoroutine(_autoUnpauseRoutine);

        _autoUnpauseRoutine = StartCoroutine(AutoUnpause());
        OnPause?.Invoke(true);
    }

    private void Unpause()
    {
        if (_autoUnpauseRoutine != null)
        {
            StopCoroutine(_autoUnpauseRoutine);
            _autoUnpauseRoutine = null;
        }

        PauseAll.Remove(this);
        Time.timeScale = 1f;
        OnPause?.Invoke(false);
    }

    private IEnumerator AutoUnpause()
    {
        yield return new WaitForSecondsRealtime(_autoUnpauseDelay);
        Unpause();
        RestartScene();
    }
    
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
