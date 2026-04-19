using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : OnBehaviour, IService
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float maxSteerAngle = 35f;
    [SerializeField] private float steerSpeed = 240f;
    [SerializeField] private float returnToCenterSpeed = 150f;

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
        HandleRotation();
        HandleMovement();
        ClampPosition();
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
        
        Quaternion targetRotation = Quaternion.Euler(0f, _baseYaw + _currentSteerAngle, 0f);
        _rb.MoveRotation(targetRotation);
    }

    private void HandleMovement()
    {
        Vector3 velocity = _rb.linearVelocity;
        
        float targetX = _steerInput * playerData.strafeSpeed;
        
        velocity.x = Mathf.MoveTowards(
            velocity.x, 
            targetX, 
            playerData.acceleration * Time.fixedDeltaTime
        );
        
        velocity.z = 0f; 

        _rb.linearVelocity = velocity;
    }
    
    private void ClampPosition()
    {
        Vector3 pos = _rb.position;

        pos.x = Mathf.Clamp(pos.x, -3f, 3f);

        _rb.MovePosition(pos);
    }
}