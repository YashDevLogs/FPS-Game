using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator characterAnimator;

    public float Speed= 12f;
    public float gravity = -9.81f * 2;
    public float Jumpheight = 3f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool IsGrounded;
    bool IsMoving;

    private Vector3 lastPosition = new Vector3 (0f,0f,0f);
    void Start()
    {
        controller  = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
    }


    void Update()
    {

        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, groundMask);

        if(IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        

        controller.Move(move * Speed * Time.deltaTime);  

        if(Input.GetButtonDown("Jump") && IsGrounded)
        {
            velocity.y = Mathf.Sqrt(Jumpheight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);


        if(lastPosition != gameObject.transform.position && IsGrounded == true) 
        {
            
            IsMoving = true;
        }
        else
        {
            IsMoving=false;
        }

        lastPosition = gameObject.transform.position;
    }

    
}
