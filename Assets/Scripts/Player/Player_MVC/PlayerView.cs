using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public float MouseSensitivity = 100f;
    public float TopClamp = -90f;
    public float BottomClamp = 90f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private PlayerModel model;
    private PlayerController controller;
    private CharacterController characterController;

    [SerializeField] private float health;

    public Transform GroundCheck;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        model = new PlayerModel();
        controller = new PlayerController(model, characterController, transform, GroundCheck);
        model.Health = health;
    }

    void Update()
    {
        HandleMouseMovement();
        controller.HandleMovement();
        controller.UpdatePlayerState();
    }

    void HandleMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, TopClamp, BottomClamp);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ZombieHand"))
        {
            controller.TakeDamage(other.gameObject.GetComponent<ZombieHandDamage>().damage);
        }
    }
}
