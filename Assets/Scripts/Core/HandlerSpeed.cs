using UnityEngine;

public class HandlerSpeed : OnBehaviour, IService
{
    [Header("Settings")]
    [SerializeField] private float initialSpeed = 60f;      
    [SerializeField] private float maxSpeed = 140f;        
    [SerializeField] private float acceleration = 0.1f;    

    public float CurrentSpeed { get; private set; }

    protected override void OnInitialize()
    {
        CurrentSpeed = initialSpeed;
    }

    protected override void OnUpdate()
    {
        if (CurrentSpeed < maxSpeed)
        {
            CurrentSpeed += acceleration * Time.deltaTime;
        }
        
        CurrentSpeed = Mathf.Min(CurrentSpeed, maxSpeed);
    }
    
    public void ResetSpeed()
    {
        CurrentSpeed = initialSpeed;
    }
}
