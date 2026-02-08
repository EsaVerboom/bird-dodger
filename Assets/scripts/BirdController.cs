using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float horizontalForce = 10f; // force applied for A/D
    public float verticalForce = 5f;    // force applied for W
    public float maxSpeed = 5f;         // clamp speed so it feels controllable

    private Rigidbody rb;

    private float horizontalInput = 0f;
    private bool moveUp = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Freeze rotation so sphere doesn't spin
        rb.freezeRotation = true;
    }

    // Called from Unity Event for horizontal action (A/D)
    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<float>();
        // Debug.Log("Horizontal: " + horizontalInput);
    }

    // Called from Unity Event for vertical action (W)
    public void OnMoveUp(InputAction.CallbackContext context)
    {
        moveUp = context.performed;
        // Debug.Log("Move Up pressed: " + moveUp);
    }

    void FixedUpdate()
    {
        // Apply horizontal force
        Vector3 horizontalForceVec = transform.right * horizontalInput * horizontalForce;
        rb.AddForce(horizontalForceVec, ForceMode.Force);

        // Apply vertical force (W)
        if (moveUp)
        {
            rb.AddForce(Vector3.up * verticalForce, ForceMode.Impulse); // Impulse so it actually jumps
            moveUp = false; // Reset to only apply once per press
        }

        // Optional: clamp max speed
        Vector3 clampedVelocity = rb.linearVelocity;
        clampedVelocity.x = Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed);
        clampedVelocity.y = Mathf.Clamp(rb.linearVelocity.y, -maxSpeed, maxSpeed);
        rb.linearVelocity = clampedVelocity;
    }
}
