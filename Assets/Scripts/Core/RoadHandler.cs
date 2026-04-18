using System.Collections.Generic;
using UnityEngine;

public class RoadHandler : MonoBehaviour
{
    [Header("Prefab (Plane)")]
    [SerializeField] private GameObject segmentPrefab;

    [Header("Setup")]
    [SerializeField] private int segmentCount = 10;
    [SerializeField] private int lengthMultiplier = 2;

    private readonly List<Transform> segments = new();
    private EnemySpawner _enemySpawner;
    private float segmentLength;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Move();
        Recycle();
    }

    private void Init()
    {
        segmentLength = 10f * lengthMultiplier;
        var startPos = transform.position;

        for (int i = 0; i < segmentCount; i++)
        {
            var pos = startPos + Vector3.forward * segmentLength * i;
            var obj = Instantiate(segmentPrefab, pos, Quaternion.identity, transform);
            segments.Add(obj.transform);

            _enemySpawner = ServiceLocator.Instance.Get<EnemySpawner>();
     
            if (i > 1 && _enemySpawner != null)
            {
                _enemySpawner.SpawnEnemyOnSegment(obj.transform);
            }
        }
    }

    private void Move()
    {
 
        var speedHandler = ServiceLocator.Instance.Get<HandlerSpeed>();
        if (speedHandler == null) return;

        var delta = Vector3.back * speedHandler.CurrentSpeed * Time.deltaTime;

        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].position += delta;
        }
    }

    private void Recycle()
    {
        var first = segments[0];
        var last = segments[^1];
        
        if (first.position.z < -segmentLength)
        {
            ClearEnemies(first);

            first.position = last.position + Vector3.forward * segmentLength;

            segments.RemoveAt(0);
            segments.Add(first);
            
            if (_enemySpawner != null)
            {
                _enemySpawner.SpawnEnemyOnSegment(first);
            }
        }
    }

    private void ClearEnemies(Transform segment)
    {
        for (int i = segment.childCount - 1; i >= 0; i--)
        {
            Transform child = segment.GetChild(i);
            
            if (child.CompareTag("Enemy"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}