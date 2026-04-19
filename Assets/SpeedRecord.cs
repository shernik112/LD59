using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedRecord : MonoBehaviour
{
    public static SpeedRecord Instance { get; private set; }

    public float BestSpeed { get; private set; }

    private HandlerSpeed _handlerSpeed;
    private PauseHandler _pauseHandler;

    private const string SAVE_KEY = "BestSpeed";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        BestSpeed = PlayerPrefs.GetFloat(SAVE_KEY, 0f);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Unsubscribe();
    }

    private void Start()
    {
        InitServices();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitServices();
    }

    private void InitServices()
    {
        Unsubscribe();

        _pauseHandler = ServiceLocator.Instance.Get<PauseHandler>();
        _handlerSpeed = ServiceLocator.Instance.Get<HandlerSpeed>();

        if (_pauseHandler != null)
            _pauseHandler.OnPause += HandlePause;
    }

    private void Unsubscribe()
    {
        if (_pauseHandler != null)
            _pauseHandler.OnPause -= HandlePause;
    }

    private void HandlePause(bool isPaused)
    {
        if (!isPaused || _handlerSpeed == null) return;

        float currentSpeed = _handlerSpeed.CurrentSpeed;

        if (currentSpeed > BestSpeed)
        {
            BestSpeed = currentSpeed;
            
            PlayerPrefs.SetFloat(SAVE_KEY, BestSpeed);
            PlayerPrefs.Save();

            Debug.Log($"New record: {BestSpeed}");
        }
    }
}