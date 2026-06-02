using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastNonZeroInput; // track last direction separately
    private Animator animator;

    [Header("Mobile")]
    [SerializeField] private VirtualJoystick joystick;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (joystick != null)
        {
            if (joystick.IsPressed)
                SetMoveInput(joystick.InputDirection, isWASD: false);
            else if (joystick.WasReleased)
                SetMoveInput(Vector2.zero, isWASD: false);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * movementSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (joystick != null && joystick.IsPressed) return;

        if (context.canceled)
        {
            SetMoveInput(Vector2.zero, isWASD: true);
            return;
        }

        SetMoveInput(context.ReadValue<Vector2>(), isWASD: true);
    }

    private void SetMoveInput(Vector2 input, bool isWASD)
    {
        if (input != Vector2.zero)
        {
            // Update last known direction while moving
            lastNonZeroInput = input;
        }

        moveInput = input;

        bool isWalking = input != Vector2.zero;
        animator.SetBool("isWalking", isWalking);
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

        if (!isWalking)
        {
            // Player just stopped — decide facing direction from lastNonZeroInput
            if (isWASD && Mathf.Abs(lastNonZeroInput.x) > 0f)
            {
                // Diagonal or horizontal: face left/right
                animator.SetFloat("LastInputX", Mathf.Sign(lastNonZeroInput.x));
                animator.SetFloat("LastInputY", 0f);
            }
            else
            {
                // Joystick, or pure vertical WASD: use direction as-is
                animator.SetFloat("LastInputX", lastNonZeroInput.x);
                animator.SetFloat("LastInputY", lastNonZeroInput.y);
            }
        }
    }
}