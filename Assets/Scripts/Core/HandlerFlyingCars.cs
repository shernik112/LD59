using System.Collections.Generic;
using UnityEngine;

public class HandlerFlyingCars : OnBehaviour, IService
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 25f; 
    [SerializeField] private float explosionForce = 1500f;
    [SerializeField] private float upwardModifier = 3f;

    private List<GameObject> _cars = new List<GameObject>();

    public void RegisterCar(GameObject car)
    {
        if (!_cars.Contains(car))
        {
            _cars.Add(car);
            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchCars();
        }
    }

    private void LaunchCars()
    {
        _cars.RemoveAll(car => car == null);
        
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }

        Vector3 origin = playerTransform != null ? playerTransform.position : transform.position;

        foreach (var car in _cars)
        {
            if (car == null) continue;

            float distance = Vector3.Distance(origin, car.transform.position);

            // 2. Проверяем, попадает ли машина в радиус именно сейчас
            if (distance <= explosionRadius)
            {
                car.transform.SetParent(null);

                Rigidbody rb = car.GetComponent<Rigidbody>();
                if (rb == null) rb = car.AddComponent<Rigidbody>();

                rb.isKinematic = false;
                rb.mass = 1f;

                rb.AddExplosionForce(explosionForce, origin, explosionRadius, upwardModifier, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * explosionForce, ForceMode.Impulse);
            }
        }
    }
}