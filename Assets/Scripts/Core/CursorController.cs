using UnityEngine;

public class CursorController : OnBehaviour
{
    [SerializeField] private bool lockOnStart = true;

    private void Start()
    {
        if (lockOnStart)
        {
            SetCursorState(true);
        }
    }
    
    public void SetCursorState(bool isLocked)
    {
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}