using Assets.Scripts.Utlities;
using UnityEngine;

public class PlayerController  : IDamageable
{
    private PlayerModel model;
    private CharacterController controller;
    private PlayerView playerView;
    private Transform playerTransform;
    private Transform groundCheck;
    private Vector3 lastPosition;

    private bool isDead = false;
    public bool IsDead => isDead; // reference required to set Game Over in Player View.

    public PlayerController(CharacterController controller,PlayerView playerView, Transform playerTransform, Transform groundCheck)
    {
        model = new PlayerModel(); 
        this.controller = controller;
        this.playerView = playerView;
        this.playerTransform = playerTransform;
        this.groundCheck = groundCheck;
        lastPosition = playerTransform.position;
        playerView.HeathUI.text = $"Health: {model.Health}";
    }

    public void HandleMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * model.MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * model.MouseSensitivity * Time.deltaTime;

        model.xRotation -= mouseY;
        model.xRotation = Mathf.Clamp(model.xRotation, model.TopClamp, model.BottomClamp);

        model.yRotation += mouseX;

        playerView.transform.localRotation = Quaternion.Euler(model.xRotation, model.yRotation, 0f);
    }

    public void HandleMovement()
    {
        model.IsGrounded = Physics.CheckSphere(groundCheck.position, model.GroundDistance, model.GroundMask);

        if (model.IsGrounded && model.Velocity.y < 0)
        {
            model.Velocity = new Vector3(model.Velocity.x, -2f, model.Velocity.z);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerTransform.right * x + playerTransform.forward * z;

        controller.Move(move * model.Speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && model.IsGrounded)
        {
            model.Velocity = new Vector3(model.Velocity.x, Mathf.Sqrt(model.JumpHeight * -2f * model.Gravity), model.Velocity.z);
        }

        model.Velocity += Vector3.up * model.Gravity * Time.deltaTime;

        controller.Move(model.Velocity * Time.deltaTime);
    }

    public void UpdatePlayerState()
    {
        if (lastPosition != playerTransform.position && model.IsGrounded)
        {
            model.IsMoving = true;
        }
        else
        {
            model.IsMoving = false;
        }

        lastPosition = playerTransform.position;
    }

    public void TakeDamage(float damageAmt)
    {
        model.Health -= damageAmt;

        if (model.Health <= 0)
        {
            Debug.Log("Player died");
            PlayerDead();
            ServiceLocator.Instance.SoundManager.PlayOneShot("DeathMusic", "SFXChannel");
        }
        else if(model.Health <= 30)
        {
            ServiceLocator.Instance.SoundManager.PlayOneShot("PlayerLowHealth", "PlayerChannel");
        }
        else
        {
            Debug.Log("Player hit");
            playerView.HeathUI.text = $"Health: {model.Health}";
            ServiceLocator.Instance.SoundManager.PlayOneShot("PlayerHurt", "PlayerChannel");
        }
    }

    private void PlayerDead()
    {
        ServiceLocator.Instance.SoundManager.PlayOneShot("PlayerDead", "PlayerChannel");
        playerView.CameraAnim.enabled = true;
        this.playerView.enabled = false;
        playerView.HeathUI.gameObject.SetActive(false);
        ScreenFader.Instance.StartFade();
        isDead = true;
    }
}
