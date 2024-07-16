using Assets.Scripts.Utlities;
using UnityEngine;

public class PlayerController  :IDamageable
{
    private PlayerModel model;
    private CharacterController controller;
    private PlayerView playerView;
    private Transform playerTransform;
    private Transform groundCheck;
    private Vector3 lastPosition;

    public PlayerController(PlayerModel model, CharacterController controller,PlayerView playerView, Transform playerTransform, Transform groundCheck)
    {
        this.model = model;
        this.controller = controller;
        this.playerView = playerView;
        this.playerTransform = playerTransform;
        this.groundCheck = groundCheck;
        lastPosition = playerTransform.position;
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
            ServiceLocator.Instance.SoundManager.SFXChannel.PlayOneShot(ServiceLocator.Instance.SoundManager.DeathMusic);
        }
        else
        {
            Debug.Log("Player hit");
            playerView.HeathUI.text = $"Health: {model.Health}";
            ServiceLocator.Instance.SoundManager.PlayerChannel.PlayOneShot(ServiceLocator.Instance.SoundManager.PlayerHurt);
        }
    }

    private void PlayerDead()
    {
        ServiceLocator.Instance.SoundManager.PlayerChannel.PlayOneShot(ServiceLocator.Instance.SoundManager.PlayerDead);
        playerView.CameraAnim.enabled = true;
        this.playerView.enabled = false;
        playerView.HeathUI.gameObject.SetActive(false);
        ScreenFader.Instance.StartFade();
        model.isDead = true;
    }
}
