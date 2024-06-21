using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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



    public Animator CameraAnim;
    public GameObject BloodScreenOverlay;

    public TextMeshProUGUI HeathUI;
    public GameObject GameOverUI;

    public Transform GroundCheck;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        model = new PlayerModel();
        controller = new PlayerController(model, characterController, this , transform, GroundCheck);
        
        HeathUI.text = $"Health: {model.Health}";
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
            if(controller.isDead == false )
            {
                controller.TakeDamage(other.gameObject.GetComponent<ZombieHandDamage>().damage);
                StartCoroutine(BloodScreenEffect());
            }else if(controller.isDead) 
            {
                StartCoroutine(ShowGameOverUI());

            }

        }
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        GameOverUI.gameObject.SetActive(true);

    }

    public IEnumerator BloodScreenEffect()
    {
        if (BloodScreenOverlay.activeInHierarchy == false)
        {
            BloodScreenOverlay.SetActive(true);
        }


        var image = BloodScreenOverlay.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.
        }


        if (BloodScreenOverlay.activeInHierarchy)
        {
            BloodScreenOverlay.SetActive(false);
        }
    }

}
