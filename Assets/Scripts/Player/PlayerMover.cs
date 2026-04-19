using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : OnBehaviour, IService
{
    public event Action OnDestroyCar;

    [Header("Audio")]
    [SerializeField] private AudioClip crashSfx;

    [Header("Settings")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float maxSteerAngle = 35f;
    [SerializeField] private float steerSpeed = 240f;
    [SerializeField] private float returnToCenterSpeed = 150f;

    [Header("Speed")]
    [SerializeField] private float speedRampUp = 0.5f;
    [SerializeField] private float baseSpeedIncrease = 0.02f;
    [SerializeField] private float maxSpeedMultiplier = 2.5f;

    [Header("World Bounds")]
    [SerializeField] private float leftBound = -6f;
    [SerializeField] private float rightBound = 6f;

    private Rigidbody _rb;

    private float _steerInput;
    private float _lastInput;

    private float _currentSteerAngle;
    private float _baseYaw;

    private float _speedMultiplier = 1f;
    private float _baseSpeedBoost = 0f;

    protected override void OnInitialize()
    {
        _rb = GetComponent<Rigidbody>();
        _baseYaw = _rb.rotation.eulerAngles.y;

        _rb.constraints = RigidbodyConstraints.FreezeRotationX |
                          RigidbodyConstraints.FreezeRotationZ;
    }

    protected override void OnUpdate()
    {
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        if (Input.GetKeyDown(KeyCode.A))
            _lastInput = -1f;

        if (Input.GetKeyDown(KeyCode.D))
            _lastInput = 1f;

        if (!left && !right)
            _steerInput = 0f;
        else
            _steerInput = _lastInput;

        _baseSpeedBoost += baseSpeedIncrease * Time.deltaTime;
    }

    protected override void OnFixedUpdate()
    {
        HandleSpeed();
        HandleRotation();
        HandleMovement();
    }

    private void HandleSpeed()
    {
        if (Mathf.Abs(_steerInput) > 0.01f)
            _speedMultiplier += speedRampUp * Time.fixedDeltaTime;
        else
            _speedMultiplier = Mathf.Lerp(_speedMultiplier, 1f, 3f * Time.fixedDeltaTime);

        _speedMultiplier = Mathf.Clamp(_speedMultiplier, 1f, maxSpeedMultiplier);
    }

    private void HandleRotation()
    {
        float targetSteerAngle = _steerInput * maxSteerAngle;
        float speed = Mathf.Abs(_steerInput) > 0.01f ? steerSpeed : returnToCenterSpeed;

        _currentSteerAngle = Mathf.MoveTowards(
            _currentSteerAngle,
            targetSteerAngle,
            speed * Time.fixedDeltaTime
        );

        Quaternion targetRotation =
            Quaternion.Euler(0f, _baseYaw + _currentSteerAngle, 0f);

        _rb.MoveRotation(targetRotation);
    }

    private void HandleMovement()
    {
        Vector3 velocity = _rb.linearVelocity;

        float boostedSpeed = playerData.strafeSpeed * (1f + _baseSpeedBoost);
        float targetX = _steerInput * boostedSpeed * _speedMultiplier;

        float accel = playerData.acceleration * Time.fixedDeltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, targetX, accel);
        velocity.z = 0f;

        _rb.linearVelocity = velocity;

        ClampPositionSmooth();
    }

    private void ClampPositionSmooth()
    {
        Vector3 pos = _rb.position;
        
        pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);

        _rb.position = pos;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<FlyingCar>(out _))
        {
            OnDestroyCar?.Invoke();

            if (AudioService.Instance != null && crashSfx != null)
                AudioService.Instance.PlaySFX(crashSfx);
        }
    }
}