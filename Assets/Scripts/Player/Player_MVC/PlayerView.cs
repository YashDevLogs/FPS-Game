using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    private PlayerModel model;
    private PlayerController controller;
    private CharacterController characterController;

    public Animator CameraAnim;
    [SerializeField] private GameObject BloodScreenOverlay;

    public TextMeshProUGUI HeathUI;
    [SerializeField] private GameObject GameOverUI;

    public Transform GroundCheck;

    private float bloodScreenTimer;
    private float bloodScreenDuration = 1.5f;
    private bool showBloodScreenEffect;
    private bool gameOverTriggered;

    private Image bloodScreenImage;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        characterController = GetComponent<CharacterController>();
        model = new PlayerModel();
        controller = new PlayerController(model, characterController, this, transform, GroundCheck);

        HeathUI.text = $"Health: {model.Health}";

        // Cache the Image component
        bloodScreenImage = BloodScreenOverlay.GetComponentInChildren<Image>();
    }

    void Update()
    {
        HandleMouseMovement();
        controller.HandleMovement();
        controller.UpdatePlayerState();

        UpdateBloodScreenEffect();
        CheckGameOver();
    }

    void HandleMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * model.MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * model.MouseSensitivity * Time.deltaTime;

        model.xRotation -= mouseY;
        model.xRotation = Mathf.Clamp(model.xRotation, model.TopClamp, model.BottomClamp);

        model.yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(model.xRotation, model.yRotation, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        ZombieHandDamage zombieHandDamage = other.GetComponent<ZombieHandDamage>();
        if (zombieHandDamage != null)
        {
            if (!model.isDead)
            {
                controller.TakeDamage(zombieHandDamage.damage);
                StartBloodScreenEffect();
            }
            else if (model.isDead && !gameOverTriggered)
            {
                gameOverTriggered = true;
                ShowGameOverUI();
            }
        }
    }

    private void ShowGameOverUI()
    {
        GameOverUI.gameObject.SetActive(true);

        int waveSurvived = ServiceLocator.Instance.GlobalReference.WaveNumber;

        if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore())
        {
            SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
        }

        Invoke(nameof(ReturnToMenu), 3f); // Delayed return to menu
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartBloodScreenEffect()
    {
        if (!BloodScreenOverlay.activeInHierarchy)
        {
            BloodScreenOverlay.SetActive(true);
        }

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = bloodScreenImage.color;
        startColor.a = 1f;
        bloodScreenImage.color = startColor;

        bloodScreenTimer = bloodScreenDuration;
        showBloodScreenEffect = true;
    }

    private void UpdateBloodScreenEffect()
    {
        if (showBloodScreenEffect)
        {
            bloodScreenTimer -= Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, bloodScreenTimer / bloodScreenDuration);
            Color newColor = bloodScreenImage.color;
            newColor.a = alpha;
            bloodScreenImage.color = newColor;

            if (bloodScreenTimer <= 0)
            {
                showBloodScreenEffect = false;
                BloodScreenOverlay.SetActive(false);
            }
        }
    }
    private void CheckGameOver()
    {
        if (model.isDead && !gameOverTriggered)
        {
            gameOverTriggered = true;
            ShowGameOverUI();
        }
    }
}
