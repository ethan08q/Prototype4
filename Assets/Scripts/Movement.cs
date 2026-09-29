using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float initialJumpVelocity = -2f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionReference moveAction;
   // [SerializeField] private InputActionReference jumpAction;

    private CharacterController characterController;
    private Vector2 moveInput;
    private bool isGrounded;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.performed += StoreMovementInput;
        moveAction.action.canceled += StoreMovementInput;
        //jumpAction.action.performed += Jump;
    }

    private void OnDisable()
    {
        moveAction.action.performed -= StoreMovementInput;
        moveAction.action.canceled -= StoreMovementInput;
        //jumpAction.action.performed -= Jump;
    }

    private void StoreMovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;
        HandleMovement();
        HandleGravity();
    }

    private void HandleGravity()
            {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = initialJumpVelocity;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void HandleMovement()
    {
        var move = cameraTransform.TransformDirection(new Vector3(moveInput.x, 0, moveInput.y)).normalized;
        var currentSpeed = speed;
        var finalMove = move * currentSpeed;
        characterController.Move(finalMove * Time.deltaTime);
        finalMove.y = verticalVelocity;
    }
}



