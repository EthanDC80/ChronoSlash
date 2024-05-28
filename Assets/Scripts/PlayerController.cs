using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField] private bool testing;

    private RoomManager roomManager;
    public GameObject currentRoom;

    [Header("Movement")]
    public float walkSpeed = 7f;
    public float sprintSpeed = 10f;
    public float airSpeed = 5f;
    private float moveSpeed;

    public float groundDrag = 5f;

    [Header("Jumping")]
    public float jumpForce = 7f;
    public float poundForce = 20f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed = 3.5f;
    public float crouchYScale = 0.5f;
    private float startYScale;
    public bool crouched = false;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 40f;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Crouching")]
    public bool grappling = false;
    public float grappleSpeed = 25f;

    [Header("Grapple")]
    public LineRenderer lr;
    public Transform gunTip;

    private Vector3 swingPoint;
    private SpringJoint joint;

    public float horThrust;
    public float forThrust;
    public float extendCableSpeed;

    [Header("Mouse")]
    public float MouseSensitivity = 100;
    public static float sensitivity;
    public static bool cameraMoveable = true;
    private Vector2 mouse;

    //Sense Danger
    private bool inDanger;
    private Image vignetteImage;
    public bool enemyDestroyed;
    public int healthPoints = 5;
    [SerializeField] GameObject[] hpUI = new GameObject[5];
    private bool invincible;

    [Header("Components")]
    public Transform orientation;
    public GameObject cam;

    [Header("Ground Pround")]
    public float poundWindow = 1f;
    Shake shake;
    bool pounded = false;
    bool canPoundJump = false;
    float poundTimeElapsed = 0f;

    [Header("Animations")]
    public Animator _animator;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public GameObject gun;

    public MovementState state;
    public enum MovementState {
        walking,
        sprinting,
        crouching,
        air,
        grappling
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GameObject.FindWithTag("Sword").GetComponent<Animator>();
        vignetteImage = GameObject.FindWithTag("Vignette").GetComponent<Image>();

        if (!testing) roomManager = GameObject.FindWithTag("RoomManager").GetComponent<RoomManager>();
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        vignetteImage.color = new Color(vignetteImage.color.r, vignetteImage.color.g, vignetteImage.color.b, 0f);
        
        //rb.freezeRotation = true;

        readyToJump = true;

        mouse = new Vector2(0f, 0f);

        startYScale = transform.localScale.y;
        shake = cam.GetComponent<Shake>();

        setMouseSens();
    }

    void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (cameraMoveable) {
            mouse.x += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            mouse.y += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            mouse.y = Mathf.Clamp(mouse.y, -90f, 90f);
        }
        

        //Debug.Log(canPoundJump);
        if (canPoundJump && grounded) {
            poundTimeElapsed += Time.deltaTime;
            if (poundTimeElapsed > poundWindow) {
                canPoundJump = false;
                poundTimeElapsed = 0;
            }
        }

        if (pounded && grounded) {
            shake.start = true;
            pounded = false;
        }
        // Debug.Log(pounded + " " + canPoundJump);
        CheckForSwingPoints();

        if (enemyDestroyed) {
            inDanger = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            vignetteImage.color = new Color(vignetteImage.color.r, vignetteImage.color.g, vignetteImage.color.b, 0f);
            enemyDestroyed = false;
        }


    }

    void LateUpdate() {
        DrawRope();
    }

    void FixedUpdate() {
        MovePlayer();
        transform.rotation = Quaternion.Euler(0, mouse.x, 0);
        cam.transform.rotation = Quaternion.Euler(-mouse.y, mouse.x, 0);
    }

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "SlashRadius") {
            inDanger = true;
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            vignetteImage.color = new Color(vignetteImage.color.r, vignetteImage.color.g, vignetteImage.color.b, 0.2f);
            if (!other.GetComponentInParent<EnemyController>().killed)
                other.GetComponentInParent<EnemyController>().stopped = true;
        }
        
        if (other.gameObject.CompareTag("EnemySwordCollider")) {
            if(other.GetComponentInParent<EnemyController>().isAttacking && !invincible) {
                Debug.Log("Attacked");
                ChangeHP(-1);
                if (healthPoints == 0) {
                    Debug.Log("GAMEOVER");
                    //TODO ADD GAMEOVER METHOD
                } else 
                    StartCoroutine(InvincibleBuffer());

            }
        }

        if (other.gameObject.CompareTag("RoomEntrance")) {
            currentRoom = other.gameObject.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "SlashRadius") {
            inDanger = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            vignetteImage.color = new Color(vignetteImage.color.r, vignetteImage.color.g, vignetteImage.color.b, 0f);
            other.GetComponentInParent<EnemyController>().stopped = false;
        }
    }

    IEnumerator InvincibleBuffer() {
        invincible = true;
        yield return new WaitForSeconds(2);
        invincible = false;
    }

    private void ChangeHP(int hp) 
    {
        healthPoints += hp;

        for (int i = 0; i < hpUI.Length; i++) {
            if (i < hp)
                hpUI[i].SetActive(true);
            else
                hpUI[i].SetActive(false); 
        }
    }

    void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey) && !grappling && joint == null) {
            crouched = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            if (!grounded) {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector3.down * 20f, ForceMode.Impulse);
                pounded = true;
                canPoundJump = true;
            }
            else
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            shake.setHeight();

        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey) && !grappling && joint == null) {
            crouched = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            shake.setHeight();
        }
    }

    void StateHandler() {
        // Mode - Crouching
        if (Input.GetKey(crouchKey) && !grappling && joint == null) {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey)) {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded) {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else if (grappling) {
            state = MovementState.grappling;
            moveSpeed = grappleSpeed;
        }

        // Mode - Air
        else {
            state = MovementState.air;
            moveSpeed = walkSpeed;
        }
    }

    void MovePlayer() {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope) {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded && grappling && joint != null) {
            // right
            if (Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * horThrust * Time.deltaTime);
            // left
            if (Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * horThrust * Time.deltaTime);

            // forward
            if (Input.GetKey(KeyCode.W)) rb.AddForce(orientation.forward * horThrust * Time.deltaTime);
            if (Input.GetKey(KeyCode.S)) rb.AddForce(-orientation.forward * horThrust * Time.deltaTime);

            // shorten cable
            if (Input.GetKey(KeyCode.Space)) {
                Vector3 directionToPoint = swingPoint - transform.position;
                rb.AddForce(directionToPoint.normalized * forThrust * Time.deltaTime);

                float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.25f;
            }
            // extend cable
            if (Input.GetKey(KeyCode.LeftControl)) {
                float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

                joint.maxDistance = extendedDistanceFromPoint * 0.8f;
                joint.minDistance = extendedDistanceFromPoint * 0.25f;
            }
        }
        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    void SpeedControl() {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope) {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    void Jump() {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (canPoundJump) {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * poundForce, ForceMode.Impulse);
            canPoundJump = false;
        }
        else
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void ResetJump() {
        readyToJump = true;

        exitingSlope = false;
    }

    bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    Vector3 GetSlopeMoveDirection() {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void StartSwing(RaycastHit rayHit) {
        _animator.SetTrigger("Sheathe");

        grappling = true;
        swingPoint = rayHit.point;
        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distFromPoint = Vector3.Distance(transform.position, swingPoint);

        joint.maxDistance = distFromPoint * 0.8f;
        joint.minDistance = distFromPoint * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    public void StopSwing() {
        _animator.SetTrigger("Unsheathe");

        grappling = false;
        lr.positionCount = 0;
        Destroy(joint);
    }
    private Vector3 currentGrapplePosition;
    private void DrawRope() {
        // if not grappling, don't draw rope
        if (!joint)
            return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    private void CheckForSwingPoints() {
        if (joint != null)
            return;
        gun.GetComponent<GunSystem>().CheckSwingPoints();
    }

    public void setMouseSens() {
        sensitivity = MouseSensitivity;
	}
}