using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float horizontalForce = 10f;
    public float verticalForce = 5f;
    public float maxSpeed = 5f;

    [Header("Game Settings")]
    public Camera mainCamera;
    public HUDController hud; // Assign in inspector for safety

    private Rigidbody rb;
    private float horizontalInput = 0f;
    private bool moveUp = false;
    private bool isDead = false;
    public bool gameStarted = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;

        if (hud == null)
        {
            hud = FindFirstObjectByType<HUDController>();
            if (hud == null)
                Debug.LogError("[BirdController] HUDController not found! Assign it in inspector.");
        }

        Debug.Log("[BirdController] Awake complete.");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<float>();
    }

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        moveUp = context.performed;
    }

    void Update()
    {
        if (!gameStarted && moveUp)
        {
            gameStarted = true;
            rb.useGravity = true;
            Debug.Log("[BirdController] Game started!");
        }

        KeepInView();
    }

    void FixedUpdate()
    {
        if (!gameStarted || isDead) return;

        // Horizontal movement
        Vector3 horizontalForceVec = transform.right * horizontalInput * horizontalForce;
        rb.AddForce(horizontalForceVec, ForceMode.Force);

        // Vertical movement
        if (moveUp)
        {
            rb.AddForce(Vector3.up * verticalForce, ForceMode.Impulse);
            moveUp = false;
        }

        // Clamp velocity
        Vector3 clampedVelocity = rb.linearVelocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxSpeed, maxSpeed);
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -maxSpeed, maxSpeed);
        rb.linearVelocity = clampedVelocity;
    }

    void KeepInView()
    {
        if (mainCamera == null) return;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.05f, 0.95f);
        viewportPos.y = Mathf.Clamp(viewportPos.y, 0.05f, 0.95f);
        transform.position = mainCamera.ViewportToWorldPoint(viewportPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        GameObject obj = collision.gameObject;

        // Traverse up until we find a tag we care about or reach root
        while (obj != null && !obj.CompareTag("Obstacle") && !obj.CompareTag("Ground"))
        {
            if (obj.transform.parent != null)
                obj = obj.transform.parent.gameObject;
            else
                obj = null;
        }

        if (obj != null)
        {
            // Found a valid tagged object
            isDead = true;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;

            Debug.Log("[BirdController] Game over! Hit: " + collision.gameObject.name + " | Tag: " + obj.tag);

            if (hud != null)
                hud.GameOver();
        }
        else
        {
            Debug.Log("[BirdController] Hit untagged object: " + collision.gameObject.name);
        }
    }



}

