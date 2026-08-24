using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement playerMovement;
    public PlayerShooter shotMuzzle;
    private Animator animator;
    private CharacterController controller; // Component to control the character

    [Header("Movement")]
    private float moveSpeed = 0f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    private Vector3 moveDirection;
    private Vector3 velocity;

    // States
    [SerializeField] private bool isGrounded = true;
    private bool isRunning = false;
    public bool canMove = true;

    [Header("Ground Test")]    
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private float GroundedOffset = 0.25f;
    [SerializeField] private LayerMask groundLayer;


    [Header("Jumping Settings")]
    private float gravity = -9.81f;     // Gravity constant
    private float jumpHeight = 5f;    
    private float jumpHoldTime = 0f;    // Time the jump button has been held


    [Header("Camera Settings")]
    public Transform cam;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public CinemachineCamera freeLookCamera;  // Reference to the FreeLook Camera
    public CinemachineCamera TPCamera;  // Reference to the Virtual Camera
    private bool isAiming = false;
    private AudioSource audioSource;


    [Header("Animation + Rigging")]
    public Transform lookTarget;
    public Transform bone;


    [Header("Player Input")]
    public InputActionAsset inputActions;
    private InputAction m_jumpAction;
    private InputAction m_sprintAction;
    private InputAction m_shootAction;
    private InputAction m_aimAction;

    public InputAction m_talkAction;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (PlayerMovement.playerMovement != null)
        {
            Destroy(this.gameObject);
            return;
        }
        playerMovement = this;
        DontDestroyOnLoad(this);

        animator = GetComponent<Animator>();
        animator.SetFloat("MovementSpeed", 0f);

        audioSource = GetComponent<AudioSource>();
        TPCamera.gameObject.SetActive(false);
        
        m_jumpAction = InputSystem.actions.FindAction("Jump");
        m_sprintAction = InputSystem.actions.FindAction("Sprint");
        m_shootAction = InputSystem.actions.FindAction("Shoot");
        m_aimAction = InputSystem.actions.FindAction("Aim");
        m_talkAction = InputSystem.actions.FindAction("Talk");

    }

    void OnDrawGizmosSelected()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(spherePosition, groundCheckRadius);
    }

    void Update() // Update is called once per frame
    {
        if (!canMove)
            return;


        if (m_shootAction.WasPressedThisFrame()) // Shoot Fireball
            animator.Play("Shoot");
        
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore); // Update Grounded

        if (m_aimAction.WasPressedThisFrame()) // Trigger Aiming
        {
            if (!isAiming)
            {
                isAiming = true;
                LockCameraRotation(true);
                freeLookCamera.gameObject.SetActive(false);
                TPCamera.gameObject.SetActive(true);
            }
            else
            {
                isAiming = false;
                LockCameraRotation(false);
                freeLookCamera.gameObject.SetActive(true);
                TPCamera.gameObject.SetActive(false);
            }
        }

        AnimatePlayerMotion();
        Move();
        Jump(); 
        Fall(); 
        Land();

        if (isGrounded) // Speed does not change when midair
        {
            if (isRunning) // Adjust speed when walking or running
            {
                moveSpeed = runSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }
        }
        OnSlopeSliding(); // Interact or Slide on steep surfaces

    }

    private void Move() // Move the player by changing position and/or angle
    {
        if (!isAiming)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(moveX, 0, moveZ).normalized;

            if (IsMoving())
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirectionAngle = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                MoveOnSlope(moveDirectionAngle);
                Run();
            }
        }
        else
        {
            ShootAim();
            MoveAim();
            Run();
        }
    }

    private void MoveOnSlope(Vector3 moveDirectionAngle)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer)) // Go up and down the slope
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            Vector3 slopeDirection = Vector3.ProjectOnPlane(moveDirectionAngle, hit.normal).normalized;
            controller.Move(slopeDirection.normalized * moveSpeed * Time.deltaTime);
        }
        else
            controller.Move(moveDirectionAngle.normalized * moveSpeed * Time.deltaTime);
    }

    private void OnSlopeSliding()
    {
        float slideSpeed = 4f;
        RaycastHit hit;
        float rayLength = (controller.height / 2f) + 0.2f;

        if (Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hit, rayLength, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            //Debug.Log("Slope Angle: " + slopeAngle);
            if (slopeAngle > controller.slopeLimit)
            {
                Vector3 slopeRight = Vector3.Cross(hit.normal, Vector3.up);
                Vector3 slideDirection = Vector3.Cross(slopeRight, hit.normal);
                controller.Move(-slideDirection.normalized * slideSpeed * Time.deltaTime);
            }
        }
    }

    void Run()
    {   
        if (m_sprintAction.IsPressed())
            isRunning = true;
        else
            isRunning = false;
    }

    void Jump()
    {
        if (isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;
            
            if (m_jumpAction.WasPressedThisFrame()) // GetButtonDown
            {
                animator.Play("Jump");
                velocity.y = jumpHeight;
                jumpHoldTime = 0.2f;
            }
            
        }
        
        if (m_jumpAction.IsPressed() && jumpHoldTime > 0) // GetButton
        {      
            velocity.y += 4f * jumpHeight * Time.deltaTime;
            jumpHoldTime -= Time.deltaTime;
        }
        
    }

    void Fall()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Land()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                animator.Play("Land");
        }
    }

    private void AnimatePlayerMotion()
    {

        if (!isGrounded) return;

        if (!IsMoving())
            SetLocomotive(0f);
        else if (IsMoving() && !isRunning)
            SetLocomotive(0.5f);
        else
            SetLocomotive(1f);
    }

    public void SetLocomotive(float magnitude)
    {
        animator.SetFloat("MovementSpeed", magnitude, .2f, Time.deltaTime);
    }

    public void ShootAim()
    {
        if (lookTarget != null)
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Get camera's right and up directions (ignore Z for up, ignore Y for right)
            Vector3 cameraRight = Camera.main.transform.right;
            Vector3 cameraUp = Camera.main.transform.up;

            // Flatten the axes so lookTarget stays in the desired plane
            cameraRight.y = 0; // Keep horizontal movement flat
            cameraUp.z = 0;    // Keep vertical movement flat
            cameraRight.Normalize();
            cameraUp.Normalize();

            // Move the lookTarget based on mouse input and camera axes
            Vector3 moveDelta = cameraRight * mouseX + cameraUp * mouseY;
            lookTarget.position += moveDelta * 10f * Time.deltaTime;

            // Clamp relative to the initial local position 
            Vector3 localPos = lookTarget.localPosition;
            float minX = -10f, maxX = 10f;
            float minY = -5f, maxY = 5f;
            localPos.x = Mathf.Clamp(localPos.x, minX, maxX);
            localPos.y = Mathf.Clamp(localPos.y, minY, maxY);
            localPos.z = 5f; // Keep it in front of player
            lookTarget.localPosition = localPos;
        }
    }

    private void LockCameraRotation(bool lockRotation)
    {
        if (freeLookCamera != null)
        {
            var panTilt = freeLookCamera.GetComponent<CinemachinePanTilt>();
            var inputAxisOwner = freeLookCamera.GetComponent<CinemachineInputAxisController>();

            if (lockRotation)
            {
                if (inputAxisOwner != null) inputAxisOwner.enabled = false; // Disables camera rotation input
            }
            else
            {
                if (inputAxisOwner != null) inputAxisOwner.enabled = true; // Re-enables camera rotation input
                StartCoroutine(SmoothlyResetLookTargetPosition());
            }
        }
    }

    private IEnumerator SmoothlyResetLookTargetPosition()
    {
        Vector3 initialPosition = lookTarget.transform.localPosition;
        Vector3 targetPosition = new Vector3(0f, 1f, 3f);
        float duration = 0.5f;
        float timeElapsed = 0f;

        // Smooth transition using Lerp
        while (timeElapsed < duration)
        {
            lookTarget.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }
        lookTarget.transform.localPosition = targetPosition; // Final position is exactly the target position
    }

    private void MoveAim()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Rotate while aiming and hold SHIFT to lock rotation
        if (Mathf.Abs(moveX) > 0.01f && !Input.GetKey(KeyCode.LeftShift))
            transform.Rotate(Vector3.up, moveX * 180f * Time.deltaTime);
        
        // Get the camera's forward and right directions
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // Flatten the forward direction (ignore the Y component)
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Normalize the direction vectors
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction based on camera orientation
        moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

        if (IsMoving())
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        
    }

    public void MoveLookTarget(float x, float y, float z) // Move looking point at the desired position
    {
        Vector3 targetPos = lookTarget.transform.localPosition + new Vector3(x, y, z);
        lookTarget.transform.localPosition = targetPos;
    }

    public bool IsMoving()
    {
        if (moveDirection.magnitude >= 0.1f)
            return true;
        return false;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void ShootEvent()
    {
        shotMuzzle.Shoot();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC") && m_talkAction.IsPressed())
        {
            other.GetComponent<NPCInteractable>().Speak();
        }
    }

}

