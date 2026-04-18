using UnityEngine;

public class PauseHandler : OnBehaviour
{
    protected override bool UpdateWhenPaused => true;

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
            PauseAll.Remove(this);
            Time.timeScale = 1f;
        }
        else
        {
            PauseAll.Add(this);
            Time.timeScale = 0f;
        }
    }
}
