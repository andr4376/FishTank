using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementScript : MonoBehaviour
{

    private CharacterController controller;

    [SerializeField]
    private float movementSpeed = 10;


    [SerializeField]
    private float jumpForce = 10;


    private Vector3 vel;

    [SerializeField]
    private float gravity = -6;


    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.2f;

    [SerializeField]
    private LayerMask groundLayerMask;

    private bool isGrounded = false;

    private Camera cam;

    private Camera Cam
    {
        get
        {
            if (cam==null)
            {
                cam = Camera.main;
            }
            return cam;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        GameObject gcGo= GameObject.Find("GroundCheck");

        if(gcGo == null)
        {
            Debug.LogWarning("Ground check could not be found");

            return;
        }

        groundCheck = gcGo.transform;
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(
                                 groundCheck.position,
                                 groundCheckRadius,
                                 groundLayerMask);

        if(isGrounded && vel.y<0)
        {
            vel.y = -1;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement =
            transform.right * x +
            Cam.transform.forward * z;

        controller.Move(movement * movementSpeed * Time.deltaTime);

        if (Input.GetButton("Jump"))
        {
            vel.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }

        vel.y += gravity * Time.deltaTime;

        controller.Move(vel * Time.deltaTime);
    }


    
}
