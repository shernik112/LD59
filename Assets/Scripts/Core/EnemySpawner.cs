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
    private bool _previousSegmentWasEmpty;

    private GameObject _pendingPrefab;
    private string _pendingTruckType;

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
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
            return;

        if (_pendingPrefab != null)
        {
            SpawnPrefab(segment, _pendingPrefab, _pendingTruckType);
            _pendingPrefab = null;
            _pendingTruckType = null;
            _previousSegmentWasEmpty = false;
            return;
        }
        
        if (!_previousSegmentWasEmpty && Random.value > spawnChance)
        {
            _previousSegmentWasEmpty = true;
            return;
        }

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        string currentTruckType = GetTruckType(prefab);
        
        if (!_previousSegmentWasEmpty &&
            !string.IsNullOrEmpty(_lastSpawnedTruckType) &&
            !string.IsNullOrEmpty(currentTruckType) &&
            _lastSpawnedTruckType != currentTruckType)
        {
            _pendingPrefab = prefab;
            _pendingTruckType = currentTruckType;
            _previousSegmentWasEmpty = true;
            return;
        }

        SpawnPrefab(segment, prefab, currentTruckType);
        _previousSegmentWasEmpty = false;
    }

    private void SpawnPrefab(Transform segment, GameObject prefab, string truckType)
    {
        GameObject spawnedGroup = Instantiate(prefab, segment);
        spawnedGroup.transform.localPosition = spawnOffset;

        if (FlyingCars != null)
        {
            FlyingCar[] carsInGroup = spawnedGroup.GetComponentsInChildren<FlyingCar>();
            foreach (var car in carsInGroup)
                FlyingCars.RegisterCar(car.gameObject);
        }

        _lastSpawnedTruckType = truckType;
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