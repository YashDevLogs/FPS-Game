using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    private PlayerController controller;
    private CharacterController characterController;
    [SerializeField] private TextMeshProUGUI healthUI;
    [SerializeField] private Animator cameraAnim;

    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private Transform GroundCheck;
    private bool gameOverTriggered;
    public TextMeshProUGUI HeathUI => healthUI;
    public Animator CameraAnim => cameraAnim;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        characterController = GetComponent<CharacterController>();
        controller = new PlayerController(characterController, this, transform, GroundCheck);
    }

    void Update()
    {
        controller.HandleMouseMovement();
        controller.HandleMovement();
        controller.UpdatePlayerState();
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
               ServiceLocator.Instance.HUDManager.StartBloodScreenEffect();
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
