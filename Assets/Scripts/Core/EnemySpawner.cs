using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : OnBehaviour, IService
{
    [Header("Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [Range(0f, 1f)] [SerializeField] private float spawnChance = 0.5f;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.5f, 0);

    private HandlerFlyingCars _flyingCarsHandler;
    private string _lastSpawnedTruckType;

    private GameObject _pendingPrefab;
    private string _pendingTruckType;
    private bool _skipSpawnThisSegment;

    private HandlerFlyingCars FlyingCars
    {
        get
        {
            if (_flyingCarsHandler == null)
                _flyingCarsHandler = ServiceLocator.Instance.Get<HandlerFlyingCars>();

            return _flyingCarsHandler;
        }
    }

    public void SpawnEnemyOnSegment(Transform segment)
    {
        if (_skipSpawnThisSegment)
        {
            _skipSpawnThisSegment = false;
            return; // этот сегмент пустой
        }

        GameObject prefab;
        string currentTruckType;

        if (_pendingPrefab != null)
        {
            prefab = _pendingPrefab;
            currentTruckType = _pendingTruckType;

            _pendingPrefab = null;
            _pendingTruckType = null;
        }
        else
        {
            if (Random.value > spawnChance) return;
            if (enemyPrefabs == null || enemyPrefabs.Count == 0) return;

            prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            currentTruckType = GetTruckType(prefab);

            if (!string.IsNullOrEmpty(_lastSpawnedTruckType) &&
                !string.IsNullOrEmpty(currentTruckType) &&
                _lastSpawnedTruckType != currentTruckType)
            {
                _pendingPrefab = prefab;
                _pendingTruckType = currentTruckType;
                _skipSpawnThisSegment = true;
                return; // этот сегмент оставляем пустым между разными траками
            }
        }

        GameObject spawnedGroup = Instantiate(prefab, segment);
        spawnedGroup.transform.localPosition = spawnOffset;

        if (FlyingCars != null)
        {
            FlyingCar[] carsInGroup = spawnedGroup.GetComponentsInChildren<FlyingCar>();
            foreach (var car in carsInGroup)
                FlyingCars.RegisterCar(car.gameObject);
        }

        _lastSpawnedTruckType = currentTruckType;
    }

    private string GetTruckType(GameObject prefab)
    {
        if (prefab.GetComponentInChildren<TruckMarker>() != null)
            return "Truck";

        if (prefab.GetComponentInChildren<Truck2Marker>() != null)
            return "Truck2";

        return string.Empty;
    }
}