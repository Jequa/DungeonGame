using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //Movement
    [SerializeField] float mouseSensitivity = 3f;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float mass = 1f;
    [SerializeField] Transform cameraTransform;
    private Vector3 originalScale;

    //Sliding
    public float slideDistance = 6f; // Distance to slide
    public float slideSpeed = 5f;    // Initial speed of sliding
    public float slideDeceleration = 2f; // Deceleration factor

    private bool isSliding = false;
    private Vector3 slideDirection;
    private float currentSlideSpeed;

    //References
    private Rigidbody rb;
    CharacterController controller;
    Vector3 velocity;
    Vector2 look;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGravity();
        UpdateMovement();
        UpdateLook();
        UpdateCrouch();
        UpdateSprint();
        UpdateSlide();
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 8f;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            movementSpeed = 5f;
        }
        if (Input.GetKey(KeyCode.C))
        {
            movementSpeed = 2.5f;
        }
    }

    void UpdateSlide()
    {
        // Check if Left Control key is pressed down
        if (Input.GetKeyDown(KeyCode.LeftControl) && movementSpeed >=7f)
        {
            StartSlide();
        }

        // Check if Left Control key is released
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StopSlide();
        }

        // Update slide movement if sliding
        if (isSliding)
        {
            float slideStep = currentSlideSpeed * Time.deltaTime;
            transform.Translate(slideDirection * slideStep, Space.World);
            slideDistance -= slideStep;

            // Reduce slide speed over time
            currentSlideSpeed -= slideDeceleration * Time.deltaTime;

            // Stop sliding when slide distance is zero or speed is very low
            if (slideDistance <= 0f || currentSlideSpeed <= -15f)
            {
                StopSlide();
            }
        }
    }
    void StartSlide()
    {
        if (!isSliding)
        {
            isSliding = true;
            currentSlideSpeed = slideSpeed;
            slideDirection = transform.forward; // Slide in the forward direction of the object
            transform.localScale = new Vector3(1, 0.5f, 1);
        }
    }
    void StopSlide()
    {
        isSliding = false;
        slideDistance = 6f; // Reset slide distance for next slide
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        Physics.SyncTransforms();
        look.x = rotation.eulerAngles.y;
        look.y = rotation.eulerAngles.z;
        velocity = Vector3.zero;
    }

    
}