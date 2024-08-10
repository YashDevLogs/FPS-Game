using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    private PlayerController controller;
    private CharacterController characterController;

    public Animator CameraAnim;
    [SerializeField] private GameObject bloodScreenOverlay;
    public GameObject BloodScreenOverlay => bloodScreenOverlay;

    public TextMeshProUGUI HeathUI;
    [SerializeField] private GameObject GameOverUI;

    public Transform GroundCheck;

    private bool gameOverTriggered;

    public Image bloodScreenImage;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        characterController = GetComponent<CharacterController>();
        controller = new PlayerController(characterController, this, transform, GroundCheck);

        // Cache the Image component
        bloodScreenImage = BloodScreenOverlay.GetComponentInChildren<Image>();
    }

    void Update()
    {
        controller.HandleMouseMovement();
        controller.HandleMovement();
        controller.UpdatePlayerState();
        controller.UpdateBloodScreenEffect();
        CheckGameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        ZombieHandDamage zombieHandDamage = other.GetComponent<ZombieHandDamage>();
        if (zombieHandDamage != null)
        {
            if (!controller.isDead)
            {
                controller.TakeDamage(zombieHandDamage.damage);
                controller.StartBloodScreenEffect();
            }
            else if (controller.isDead && !gameOverTriggered)
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


    private void CheckGameOver()
    {
        if (controller.isDead && !gameOverTriggered)
        {
            gameOverTriggered = true;
            ShowGameOverUI();
        }
    }
}
