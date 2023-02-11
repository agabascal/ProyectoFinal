using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        //States
        public enum PlayerState
        {
            Ground,
            Flight,
            Glide
        }

        public PlayerState state;

        #region Movement

        [Header("Movement")]
        //Movement
        public float speed = 6f;

        private float turnSmooth = 0.1f;
        private float smoothVelocity;
        public float smoothturn;
        private Transform cam;
        private Vector3 direction;
        private Vector3 moveDir;
        public CharacterController controller;
        public bool isMovementEnabled;

        //Jump
        public float jumpSpeed = 8f;
        public float gravity = -20f;
        public Transform groundCheck;
        public float checkDistance = 0.4f;
        public LayerMask groundMask;
        private Vector3 velocity;
        public bool isGrounded;
        public bool flightUnlocked;

        #endregion

        #region Flight Mechanic

        [Header("Flight")]
        [SerializeField] private float flightGravity = -1f;
        [SerializeField] private float xSensitivity;
        [SerializeField] private float zSensitivity;
        [SerializeField] private float lerpingSpeed = 2.5f;
        [SerializeField] private float forwardSpeed = 25;
        private float boostTimer;
        private float horizontalInput;
        private float verticalInput;

        [SerializeField] private GameObject[] trails;

        [SerializeField] private bool isFlying;
        [SerializeField] private bool isBoosted;

        private Vector3 currentRotation;

        [SerializeField] private AudioSource windSource;
        [SerializeField] private AudioClip windClip;

        #endregion

        #region Combat

        [Header("Combat")] public int life = 3;
        public float knockTimer = 1f;
        public bool isKnocked;
        public float knockForce = 5f;
        public CapsuleCollider capsuleCol;
        public Rigidbody rb;
        public bool isHurt;
        public BoxCollider tailHitbox;

        [Header("Range")]
        //Range Attack
        public bool shootingUnlocked;

        public Transform shootPoint;
        public GameObject bullet;

        #endregion

        #region Gliding

        [Header("Gliding")] public float glidingSpeed = 5f;
        public bool isOnGlidingDistace = false;
        public float glideCheckDistance = 5f;

        #endregion

        [Header("Animation")] public Animator anim;

        public bool IsFlying => isFlying;

        // Start is called before the first frame update
        //get all respective components needed for the different mechanics
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            controller = GetComponent<CharacterController>();
            capsuleCol = GetComponent<CapsuleCollider>();
            anim = GetComponent<Animator>();
            cam = Camera.main.transform;
            rb = GetComponent<Rigidbody>();
        }

        //Handle physics and movement
        private void FixedUpdate()
        {
            switch (state)
            {
                case PlayerState.Ground:

                    checkDistance = 0.1f;
                    currentRotation = transform.eulerAngles;
                    if (controller.enabled && isMovementEnabled)
                    {
                        Movement();
                    }

                    velocity.y += gravity * Time.deltaTime;
                    if (velocity.y < -2f)
                    {
                        velocity.y = -2f;
                    }

                    trails[0].SetActive(false);
                    trails[1].SetActive(false);
                    break;

                case PlayerState.Flight:


                    Flight(horizontalInput, verticalInput);
                    checkDistance = 1f;
                    trails[0].SetActive(true);
                    trails[1].SetActive(true);

                    velocity.y += flightGravity * Time.deltaTime;
                    if (velocity.y < -5)
                    {
                        velocity.y = -5f;
                    }

                    transform.Translate(velocity * Time.deltaTime);
                    break;
                case PlayerState.Glide:

                    Glide();
                    velocity.y += flightGravity * Time.deltaTime;
                    if (velocity.y < -5)
                    {
                        velocity.y = -5f;
                    }

                    break;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (isHurt)
            {
                transform.Translate(-transform.forward * -knockForce / 2 * Time.deltaTime);
                return;
            }

            if (life == 0)
            {
                if (isMovementEnabled)
                {
                    isMovementEnabled = false;
                    anim.SetTrigger("death");
                    AudioManager.PlayDeathAudio();
                }

                return;
            }

            #region Input

            if (isMovementEnabled)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                isMovementEnabled = false;
                anim.Play("shoot");
                AudioManager.PlayShootAudio();
            }

            if (GameManager.Instance != null && GameManager.Instance.partsCollected == 4)
            {
                if (Input.GetKeyDown(KeyCode.F) && isGrounded)
                {
                    AudioManager.PlayVoiceSaruAudio();
                    currentRotation = transform.eulerAngles;
                    anim.SetTrigger("takeOff");
                }
            }

            if (!isGrounded && flightUnlocked && velocity.y < -6 && Input.GetKeyDown(KeyCode.F))
            {
                windSource.Play();
                state = PlayerState.Flight;
                anim.Play("Fly");
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && isOnGlidingDistace)
            {
                state = PlayerState.Glide;
            }

            if (Input.GetKeyDown(KeyCode.F) && state == PlayerState.Flight)
            {
                state = PlayerState.Ground;
                anim.SetTrigger("land");

                AudioManager.PlayLandingAudio();
            }

            #endregion

            #region raycasts

            isOnGlidingDistace = !Physics.Raycast(groundCheck.position, -transform.up, glideCheckDistance, groundMask);
            isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);

            #endregion

            switch (state)
            {
                case PlayerState.Ground:
                    if (isMovementEnabled)
                    {
                        //Gravity
                        anim.SetBool("isGrounded", isGrounded);

                        isFlying = false;

                        if (Input.GetMouseButtonDown(0))
                        {
                            anim.Play("Attack");
                        }

                        if (windSource != null)
                        {
                            windSource.Stop();
                        }
                    }

                    if (isGrounded && !isHurt)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
                    }

                    break;
                case PlayerState.Flight:
                    isFlying = true;

                    if (isGrounded && velocity.y < 0)
                    {
                        state = PlayerState.Ground;
                        velocity.y = -2f;
                    }

                    break;
                case PlayerState.Glide:

                    if (!isFlying)
                    {
                        anim.Play("Fly");
                        trails[0].SetActive(true);
                        trails[1].SetActive(true);
                        isFlying = true;
                    }

                    break;
            }
        }


        #region player movement

        private void Movement()
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            direction = new Vector3(h, 0, v);

            anim.SetFloat("speed", direction.magnitude);

            if (direction.magnitude >= 0.1f)
            {
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                var angle =
                    Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmooth);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * 2 : speed;

                moveDir = moveDir.normalized * currentSpeed;
                controller.Move(moveDir * Time.deltaTime);
            }

            // jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                anim.SetTrigger("jump");
            }

            anim.SetBool("isFlying", isFlying);
            controller.Move(velocity * Time.deltaTime);
        }

        public void Jump()
        {
            velocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
        }

        private void Glide()
        {
            controller.Move(transform.forward * (glidingSpeed * Time.deltaTime));
            //velocity.y += gravity * Time.deltaTime;
        }

        private void Flight(float horizontalInput, float verticalInput)
        {
            //Manage Direction and lift forces media character rotation

            float rotationLimitX = Mathf.Clamp(currentRotation.x + verticalInput * xSensitivity, -80f, 80f);
            float rotationLimitZ = 0f;

            rotationLimitZ = horizontalInput == 0f
                ? rotationLimitZ = Mathf.Lerp(currentRotation.z, 0f, Time.deltaTime * lerpingSpeed)
                : rotationLimitZ = Mathf.Clamp(currentRotation.z - horizontalInput * xSensitivity, -45f, 45f);
                
            currentRotation = new Vector3(rotationLimitX, currentRotation.y + horizontalInput, rotationLimitZ);
            
            transform.rotation = Quaternion.Euler(currentRotation);
            
            if (currentRotation.magnitude > 0.1f)
            {
                transform.Rotate(verticalInput, 0.0f, -horizontalInput);
            }

            float currentForwardSpeed = transform.forward.y + forwardSpeed;
            controller.Move(transform.forward * (currentForwardSpeed * Time.deltaTime));
            forwardSpeed -= transform.forward.y * Time.deltaTime * 35.0f;

            //Contact with WindBoostItems gameobjects 
            if (isBoosted)
            {
                forwardSpeed *= 1.5f;
                boostTimer += Time.deltaTime;
                if (boostTimer >= 1.5f)
                {
                    isBoosted = false;
                    boostTimer = 0;
                }
            }

            if (forwardSpeed > (isBoosted ? 180 : 85))
            {
                forwardSpeed = isBoosted ? 180 : 85;
            }

            if (forwardSpeed < 65)
            {
                forwardSpeed = 65;
            }
        }

        public void boostFlight()
        {
            isBoosted = true;
        }

        public void StartFlight()
        {
            state = PlayerState.Flight;
            windSource.Play();
        }

        public void StepAudio()
        {
            AudioManager.PlayFootStepAudio();
        }

        public void FlutterAudio()
        {
            AudioManager.PlayFlyAudio();
        }

        public void AttackAudio()
        {
            AudioManager.PlayAttackSaruAudio();
        }

        public void JumpAudio()
        {
            AudioManager.PlayJumpAudio();
        }

        public void RunAudio()
        {
            AudioManager.PlayRunSaruAudio();
        }

        #endregion

        #region Combat

        public void RangeAttack()
        {
            GameObject newBullet = Instantiate(bullet, shootPoint.transform.position, transform.rotation);
            Physics.IgnoreCollision(newBullet.GetComponentInChildren<Collider>(), GetComponent<Collider>());
            isMovementEnabled = true;
        }

        public void MeleeAttack()
        {
            isHurt = true;
            tailHitbox.enabled = true;
        }

        public void StopAttack()
        {
            tailHitbox.enabled = false;
            isHurt = false;
        }

        //Knockback on contact with enemies
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy") && !isHurt)
            {
                Knock();
                life--;
            }
        }

        private IEnumerator KnockBack()
        {
            isHurt = true;
            rb.isKinematic = false;
            rb.useGravity = true;
            controller.enabled = false;
            //apply force to char controller

            rb.velocity = (Vector3.up * knockForce);

            //Delay to reactivate components
            yield return new WaitForSeconds(knockTimer);

            //Reactivate components
            controller.enabled = true;
            rb.isKinematic = true;
            rb.useGravity = false;

            isHurt = false;
        }

        public void Knock()
        {
            isKnocked = true;
            StartCoroutine(KnockBack());
            isKnocked = false;
        }


        public void PlayerDeath()
        {
            //reload scene  
            GameManager.Instance.GameOver();
        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundCheck.position, checkDistance);
        }
    }
}