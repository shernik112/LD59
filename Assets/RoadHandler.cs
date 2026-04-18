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
        }
    }

    private void Move()
    {
        var handlerSpeed = ServiceLocator.Instance.Get<HandlerSpeed>();
        var delta = Vector3.back * handlerSpeed.CurrentSpeed * Time.deltaTime;

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
            first.position = last.position + Vector3.forward * segmentLength;

            segments.RemoveAt(0);
            segments.Add(first);
        }
    }
}