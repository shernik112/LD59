using UnityEngine;

public class EntryPointPlayScene : MonoBehaviour
{
    [SerializeField] private HandlerSpeed handlerSpeed = default;
    [SerializeField] private HandlerFlyingCars handlerFlyingCars = default;
    [SerializeField] private EnemySpawner enemySpawner = default;
    [SerializeField] private PlayerMover playerMover = default;
    [SerializeField] private UIManager uiManager = default;
    [SerializeField] private PauseHandler pauseHandler = default;
    [SerializeField] private AudioService audioService = default;
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<HandlerSpeed>(handlerSpeed);
        ServiceLocator.Instance.Register<HandlerFlyingCars>(handlerFlyingCars);
        ServiceLocator.Instance.Register<EnemySpawner>(enemySpawner);
        ServiceLocator.Instance.Register<PlayerMover>(playerMover);
        ServiceLocator.Instance.Register<UIManager>(uiManager);
        ServiceLocator.Instance.Register<PauseHandler>(pauseHandler);
        ServiceLocator.Instance.Register<AudioService>(audioService);
        handlerSpeed.Initialize();
        handlerFlyingCars.Initialize();
        enemySpawner.Initialize();
        playerMover.Initialize();
        uiManager.Initialize();
        pauseHandler.Initialize();      
        audioService.Initialize();
    }
}