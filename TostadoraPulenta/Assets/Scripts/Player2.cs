using UnityEngine;




public class PlayerController2 : MonoBehaviour
{
    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float ascendMultiplier = 2f;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

    // Camera (solo offset fijo)
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 5, -7);

    // Projectile
    public GameObject projectilePrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;
    }

    void Update()
    {
        // Movimiento con flechas
        moveHorizontal = 0f;
        moveForward = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveHorizontal = 1f;
        if (Input.GetKey(KeyCode.UpArrow)) moveForward = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) moveForward = -1f;

        // Salto con RightShift
        if (Input.GetKeyDown(KeyCode.RightShift) && isGrounded)
            Jump();

        // Ground check
        if (!isGrounded && groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }

        // Disparo con RightControl
        if (Input.GetKeyDown(KeyCode.RightControl))
            Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);

        // Actualiza cámara fija
        if (cameraTransform)
            cameraTransform.position = transform.position + cameraOffset;
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void MovePlayer()
    {
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * MoveSpeed;

        Vector3 velocity = rb.velocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.velocity = velocity;

        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    void ApplyJumpPhysics()
    {
        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        else if (rb.velocity.y > 0)
            rb.velocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
    }
}
