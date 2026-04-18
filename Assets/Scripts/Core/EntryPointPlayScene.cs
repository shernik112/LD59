using UnityEngine;

public class EntryPointPlayScene : OnBehaviour
{
    [SerializeField] private HandlerSpeed handlerSpeed = default;
    [SerializeField] private HandlerFlyingCars handlerFlyingCars = default;
    [SerializeField] private EnemySpawner enemySpawner = default;
    private void Awake()
    {
        ServiceLocator.Instance.Register<HandlerSpeed>(handlerSpeed);
        ServiceLocator.Instance.Register<HandlerFlyingCars>(handlerFlyingCars);
        ServiceLocator.Instance.Register<EnemySpawner>(enemySpawner);
    }
}