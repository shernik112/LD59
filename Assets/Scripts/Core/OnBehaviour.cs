using UnityEngine;

public class OnBehaviour : MonoBehaviour
{
    public static ReferenceSetToggle PauseAll = new ReferenceSetToggle();

    private bool _initialized = false;

    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;
        OnInitialize();
    }

    protected virtual bool UpdateWhenPaused => false;

    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnLateUpdate() { }
    protected virtual void OnInitialize() { }

    private void Update() 
    {
        if (CanUpdate())
        {
            OnUpdate();
        }
    }

    private void FixedUpdate() 
    {
        if (CanUpdate())
        {
            OnFixedUpdate();
        }
    }
    private void LateUpdate()
    {
        if (CanUpdate())
        {
            OnLateUpdate();
        }
    }

    private void Awake()
    {
        if (_initialized)
            return;
        _initialized = true;
        OnInitialize();
    }

    private bool CanUpdate()
    {
        return UpdateWhenPaused || !PauseAll.True;
    }
}