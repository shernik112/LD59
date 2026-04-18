using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float forwardSpeed = 12f;
    public float acceleration = 25f;
    public float returnToCenterSpeed;
    public float maxSteerAngle = 35f;

    [Header("Steering")]
    public float steerSpeed = 240f;

    [Header("Stability")]
    public bool freezeTilt = true;
}
