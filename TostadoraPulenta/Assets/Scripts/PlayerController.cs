
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
      // Camera Rotation
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;

    //camara
    //public Transform cameraTransform;

    //proyectil
    public GameObject projectilePrefab;
    
    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;
    private Camera _camera;

    //Audio/SFX
    private AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip jumpSound;

    //Controlls
    private CharacterController controller;
    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;
    private bool attacked = false;

    //movimiento de camara
    private Vector2 lookInput;

	//Sistema de vida
	public int health = 10;

	PlayerInputManager pim = PlayerInputManager.instance;




	void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //cameraTransform = Camera.main.transform;

        controller = gameObject.GetComponent<CharacterController>();

        // Set the raycast to be slightly beneath the player's feet
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
  
    }

    public void OnMove(InputAction.CallbackContext context) { 
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Dispara();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Update()
    {

		Dead();
		moveHorizontal = movementInput.x;
        moveForward = movementInput.y;

        RotateCamera();

        if (jumped && isGrounded)
        {
            Jump();
        }

        // Checking when we're on the ground and keeping track of our ground check delay
        if (!isGrounded && groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }

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

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;

        // If we aren't moving and are on the ground, stop velocity so we don't slide
        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void RotateCamera()
    {
        float yaw = lookInput.x * mouseSensitivity;
        float pitch = lookInput.y * mouseSensitivity;

        transform.Rotate(0f, yaw, 0f);

        verticalRotation -= pitch;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        //cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Initial burst for the jump

        //Jump SFX
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(jumpSound);
    }

    void Dispara()
    {

        //Tirar comida
       
            Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);

            //Efecto de sonido
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(shootSound);
  

    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Falling: Apply fall multiplier to make descent faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        } // Rising
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
            // Rising: Change multiplier to make player reach peak of jump faster
        }
 }
    
	void Dead()
	{
		if (health <= 0)
		{
			DeSpawn();  
			Invoke("Spawn", 10);
		}

	}
	void DeSpawn()
	{
		gameObject.SetActive(false);
		pim?.DisableJoining();
	}
	void Spawn()
	{
		health = 10;
		gameObject.SetActive(true);
		pim?.EnableJoining();
		Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
	}
    
}


