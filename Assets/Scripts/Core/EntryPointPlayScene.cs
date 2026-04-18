using UnityEngine;

public class EntryPointPlayScene : MonoBehaviour
{
    [SerializeField] private HandlerSpeed handlerSpeed = default;
    [SerializeField] private HandlerFlyingCars handlerFlyingCars = default;
    [SerializeField] private EnemySpawner enemySpawner = default;
    [SerializeField] private PlayerMover playerMover = default;
    private void Awake()
    {
        ServiceLocator.Instance.Register<HandlerSpeed>(handlerSpeed);
        ServiceLocator.Instance.Register<HandlerFlyingCars>(handlerFlyingCars);
        ServiceLocator.Instance.Register<EnemySpawner>(enemySpawner);
        ServiceLocator.Instance.Register<PlayerMover>(playerMover);
        handlerSpeed.Initialize();
        handlerFlyingCars.Initialize();
        enemySpawner.Initialize();
        playerMover.Initialize();
    }
}