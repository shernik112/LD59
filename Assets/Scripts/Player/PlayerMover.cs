using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : OnBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float maxSteerAngle = 35f;
    [SerializeField] private float steerSpeed = 240f;
    [SerializeField] private float returnToCenterSpeed = 150f; // Ускорил возврат для отзывчивости

    private Rigidbody _rb;
    private float _steerInput;
    private float _currentSteerAngle;
    private float _baseYaw;

    protected override void OnInitialize()
    {
        _rb = GetComponent<Rigidbody>();
        // Сохраняем начальный поворот, чтобы знать, куда возвращаться
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
    }

    private void HandleRotation()
    {
        float targetSteerAngle = _steerInput * maxSteerAngle;

        // Выбираем скорость: steerSpeed при вводе, returnToCenterSpeed когда отпускаем
        float speed = Mathf.Abs(_steerInput) > 0.01f ? steerSpeed : returnToCenterSpeed;

        _currentSteerAngle = Mathf.MoveTowards(
            _currentSteerAngle,
            targetSteerAngle,
            speed * Time.fixedDeltaTime
        );

        // Применяем вращение относительно начального _baseYaw
        Quaternion targetRotation = Quaternion.Euler(0f, _baseYaw + _currentSteerAngle, 0f);
        _rb.MoveRotation(targetRotation);
    }

    private void HandleMovement()
    {
        Vector3 velocity = _rb.linearVelocity;

        // Вместо transform.forward используем только ввод по оси X
        // Если тебе нужно движение вперед, добавь в playerData переменную forwardSpeed
        float targetX = _steerInput * playerData.strafeSpeed;
        
        // Плавный разгон и торможение по горизонтали
        velocity.x = Mathf.MoveTowards(
            velocity.x, 
            targetX, 
            playerData.acceleration * Time.fixedDeltaTime
        );

        // ОБНУЛЯЕМ Z, чтобы он не ехал прямо сам по себе
        velocity.z = 0f; 

        _rb.linearVelocity = velocity;
    }
}