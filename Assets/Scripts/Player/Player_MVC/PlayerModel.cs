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
}
