using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 3f;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float mass = 1f;
    [SerializeField] Transform cameraTransform;
    private Vector3 originalScale;

    public float slideSpeed = 5f; // Speed where the player slides
    private Rigidbody rb;

    CharacterController controller;
    Vector3 velocity;
    Vector2 look;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGravity();
        UpdateMovement();
        UpdateLook();
        UpdateCrouch();
        UpdateSprint();
    }

    void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y;
    }

    void UpdateMovement()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var input = new Vector3();
        input += transform.forward * y;
        input += transform.right * x;
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y += jumpSpeed;
        }

        controller.Move((input * movementSpeed + velocity) * Time.deltaTime);
    }

    void UpdateLook()
    {
        look.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        look.y += Input.GetAxis("Mouse Y") * mouseSensitivity;

        look.y = Mathf.Clamp(look.y, -89f, 89f);

        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);
    }

    void UpdateCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.localScale = new Vector3(1, 0.5f, 1);
            movementSpeed = 2.5f;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            transform.localScale = new Vector3(1, 1, 1);
            movementSpeed = 5f;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transform.localScale = new Vector3(1, 1, 1);
            movementSpeed = 5f;
        }
    }

    void UpdateSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed = 8f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = 5f;
        }
    }

    void UpdateSlide()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 slideVelocity = transform.forward * slideSpeed; // Calculate sliding velocity in the forward direction

            // Apply the sliding velocity to the Rigidbody's velocity
            rb.velocity = slideVelocity;
        }
    }
}
