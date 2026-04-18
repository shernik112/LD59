using UnityEngine;

public class EntryPointPlayScene : OnBehaviour
{
    [SerializeField] private HandlerSpeed handlerSpeed = default;
    private void Awake()
    {
        ServiceLocator.Instance.Register<HandlerSpeed>(handlerSpeed);
    }
}