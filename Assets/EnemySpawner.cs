using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : OnBehaviour, IService
{
    [Header("Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [Range(0f, 1f)] [SerializeField] private float spawnChance = 0.5f; 
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.5f, 0); 

    public void SpawnEnemyOnSegment(Transform segment)
    {
        if (Random.value > spawnChance) return;
        if (enemyPrefabs == null || enemyPrefabs.Count == 0) return;

        var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        var enemy = Instantiate(prefab, segment.position + spawnOffset, Quaternion.identity);
        
        enemy.transform.SetParent(segment);
        
        enemy.transform.localPosition = new Vector3(0, spawnOffset.y, 0);
    }
}
