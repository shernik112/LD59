using UnityEngine;

public class MoveForward : OnBehaviour
{
    private Rigidbody _rb;
    private float _speed = 0.05f;
    protected override void OnInitialize()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected override void OnFixedUpdate()
    {
        _rb.linearVelocity = new Vector3(0, 0, _speed);
    }
}
