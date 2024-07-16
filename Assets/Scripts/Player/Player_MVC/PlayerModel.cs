using UnityEngine;

public class PlayerModel
{
    public float Speed = 12f;
    public float Gravity = -9.81f * 2;
    public float JumpHeight = 3f;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    public float Health = 100f;
    public Vector3 Velocity { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsMoving { get; set; }

    public bool isDead = false;

    public float MouseSensitivity = 100f;
    public float TopClamp = -90f;
    public float BottomClamp = 90f;

    public float xRotation = 0f;
    public float yRotation = 0f;
}
