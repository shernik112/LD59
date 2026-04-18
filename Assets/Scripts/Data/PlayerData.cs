using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float acceleration = 25f;
    public float returnToCenterSpeed;
    public float maxSteerAngle = 35f;
    public float strafeSpeed = 5f;
    public float rotationSmoothTime = 0.15f;
    
    [Header("Steering")]
    public float steerSpeed = 240f;

    [Header("Stability")]
    public bool freezeTilt = true;
}
