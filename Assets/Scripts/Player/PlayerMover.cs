using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : OnBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float maxSteerAngle = 35f;
    [SerializeField] private float steerSpeed = 240f;
    [SerializeField] private float returnToCenterSpeed = 90f;

    private Rigidbody _rb;
    private float _steerInput;
    private float _currentSteerAngle;
    private float _baseYaw;

    protected override void OnInitialize()
    {
        _rb = GetComponent<Rigidbody>();
        _baseYaw = _rb.rotation.eulerAngles.y;

        if (playerData.freezeTilt)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    protected override void OnUpdate()
    {
        _steerInput = Input.GetAxisRaw("Horizontal");
    }

    protected override void OnFixedUpdate()
    {
        float targetSteerAngle = _steerInput * maxSteerAngle;

        float speed = Mathf.Abs(_steerInput) > 0.01f
            ? steerSpeed
            : returnToCenterSpeed;

        _currentSteerAngle = Mathf.MoveTowards(
            _currentSteerAngle,
            targetSteerAngle,
            speed * Time.fixedDeltaTime
        );

        Quaternion targetRotation = Quaternion.Euler(0f, _baseYaw + _currentSteerAngle, 0f);
        _rb.MoveRotation(targetRotation);

        Vector3 desiredVelocity = Vector3.zero;

        if (Mathf.Abs(_steerInput) > 0.01f)
        {
            desiredVelocity = transform.forward * playerData.forwardSpeed;
        }

        desiredVelocity.y = _rb.linearVelocity.y;

        Vector3 velocity = _rb.linearVelocity;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, playerData.acceleration * Time.fixedDeltaTime);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, playerData.acceleration * Time.fixedDeltaTime);
        _rb.linearVelocity = velocity;
    }
}