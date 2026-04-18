using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : OnBehaviour, IService
{
    [Header("Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [Range(0f, 1f)] [SerializeField] private float spawnChance = 0.5f; 
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.5f, 0); 

    private HandlerFlyingCars _flyingCarsHandler;

    private HandlerFlyingCars FlyingCars
    {
        get
        {
            if (_flyingCarsHandler == null)
            {
                _flyingCarsHandler = ServiceLocator.Instance.Get<HandlerFlyingCars>();
            }
            return _flyingCarsHandler;
        }
    }

    public void SpawnEnemyOnSegment(Transform segment)
    {
        if (Random.value > spawnChance) return;
        if (enemyPrefabs == null || enemyPrefabs.Count == 0) return;
    
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        GameObject spawnedGroup = Instantiate(prefab, segment.position + spawnOffset, Quaternion.identity);

        spawnedGroup.transform.SetParent(segment);
        spawnedGroup.transform.localPosition = new Vector3(0, spawnOffset.y, 0);
        
        if (FlyingCars != null)
        {
            FlyingCar[] carsInGroup = spawnedGroup.GetComponentsInChildren<FlyingCar>();
            foreach (var carTag in carsInGroup)
            {
                FlyingCars.RegisterCar(carTag.gameObject);
            }
        }
    }
}