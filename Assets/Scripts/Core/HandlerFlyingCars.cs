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

    [Header("Truck Settings")]
    [SerializeField] private float truckJumpForce = 5f;

    [Header("Audio")]
    [SerializeField] private AudioClip launchSfx;

    private readonly List<GameObject> _cars = new List<GameObject>();
   

    public void RegisterCar(GameObject car)
    {
        if (!_cars.Contains(car))
        {
            _cars.Add(car);

            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;
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
            if (player != null)
                playerTransform = player.transform;
        }

        if (playerTransform == null)
            return;

        Vector3 origin = playerTransform.position;
        bool launchedAnyCar = false;

        foreach (var car in _cars)
        {
            if (car == null)
                continue;

            float distance = Vector3.Distance(origin, car.transform.position);

            if (distance > explosionRadius)
                continue;

            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb == null)
                rb = car.AddComponent<Rigidbody>();

            rb.isKinematic = false;
            rb.mass = 1f;

            if (car.CompareTag("Truck") || car.CompareTag("Truck2"))
            {
                rb.AddForce(Vector3.up * truckJumpForce, ForceMode.Impulse);
            }
            else
            {
                rb.AddExplosionForce(explosionForce, origin, explosionRadius, upwardModifier, ForceMode.Impulse);
            }

            rb.AddTorque(Random.insideUnitSphere * explosionForce, ForceMode.Impulse);
            launchedAnyCar = true;
        }

        if (launchedAnyCar && launchSfx != null)
        {
            AudioService.Instance?.PlaySFX(launchSfx);
        }
    }
}