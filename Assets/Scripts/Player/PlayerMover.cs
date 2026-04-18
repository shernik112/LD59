using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : OnBehaviour
{
    [SerializeField] private PlayerData playerData;

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
        var targetSteerAngle = _steerInput * playerData.maxSteerAngle;

        var speed = Mathf.Abs(_steerInput) > 0.01f
            ? playerData.steerSpeed
            : playerData.returnToCenterSpeed;

        _currentSteerAngle = Mathf.MoveTowards(
            _currentSteerAngle,
            targetSteerAngle,
            speed * Time.fixedDeltaTime
        );

        var targetRotation = Quaternion.Euler(0f, _baseYaw + _currentSteerAngle, 0f);
        _rb.MoveRotation(targetRotation);

        var velocity = _rb.linearVelocity;
        
        var targetX = _steerInput * playerData.strafeSpeed;

        velocity.x = Mathf.MoveTowards(
            velocity.x,
            targetX,
            playerData.acceleration * Time.fixedDeltaTime
        );
        
        velocity.z = 0f;
        _rb.linearVelocity = velocity;
    }
}