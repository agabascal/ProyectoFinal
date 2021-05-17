using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    //States
    public enum playerState {ground, flight}
    public playerState state;

    #region Movement
    [Header("Movement")]
    //Movement
    public float speed=6f;    
    private float turnSmooth=0.1f;
    private float smoothVelocity;
    public float smoothturn;
    private Transform cam;
    private Vector3 direction;
    private Vector3 moveDir;
    public CharacterController controller;
    public bool canMove;

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
    public float flightGravity = -1;
    public float forwardSpeed = 25;    
    private float boostTimer;
    public float xSensitivity, zSensitivity;

    public GameObject[] trails;
    
    public bool isFlying;
    public bool isBoosted;

    public Vector3 currentRotation;
    #endregion

    #region Combat
    [Header("Combat")]
    public int life = 3;
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

    [Header("Animation")]
    public Animator anim;    

    // Start is called before the first frame update
    //get all respective components needed for the different mechanics
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        capsuleCol = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    //Handle physics and movement
    private void FixedUpdate()
    {
        if (state == playerState.ground)
        {
            checkDistance = 0.1f;
            currentRotation = transform.eulerAngles;
            if (controller.enabled && canMove)
            {
                Movement();
            }
            velocity.y += gravity * Time.deltaTime;
            trails[0].SetActive(false);
            trails[1].SetActive(false);
        }
        else
        {
            
            Flight();
            checkDistance = 1f;
            trails[0].SetActive(true);
            trails[1].SetActive(true);

            velocity.y += flightGravity * Time.deltaTime;
            if (velocity.y <-5)
            {
                velocity.y = -5f;
            }
            transform.Translate(velocity * Time.deltaTime);

           /* if (Input.GetAxis("Horizontal") == 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x,transform.rotation.y,0f),.5f);
            }*/
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Knock();
        }
        if (state == playerState.ground && canMove)
        {
            //Gravity
            isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);
            anim.SetBool("isGrounded",isGrounded);

            isFlying = false;
            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("Attack");
                AudioManager.PlayAttackSaruAudio();
            }
        }

        else
        {
            isFlying = true;
            isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);

            if (isGrounded && velocity.y<0)
            {
                state = playerState.ground;
                velocity.y = -2f;                
            }
        }

        if (Input.GetMouseButtonDown(1) && shootingUnlocked)
        {
            canMove = false;
            anim.Play("shoot");
            AudioManager.PlayShootAudio();
        }        

        if (GameManager.Instance.partsCollected == 4)
        {
            if (Input.GetKeyDown(KeyCode.F) && isGrounded)
            {                
                currentRotation = transform.eulerAngles;
                anim.SetTrigger("takeOff");
                AudioManager.PlayTakeoffAudio();
            }
        }
       
        if (Input.GetKeyDown(KeyCode.F) && state == playerState.flight)
        {
            state = playerState.ground;
            anim.SetTrigger("land");
            transform.rotation = Quaternion.Euler(0,0,0);
            AudioManager.PlayLandingAudio();
        }

        if (isGrounded && !isHurt)
        {
            state = playerState.ground;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0f);
        }

        if (life==0 && canMove)
        {
            canMove = false;
            anim.SetTrigger("death");
            AudioManager.PlayDeathAudio();
        }

        if (isHurt)
        {
            transform.Translate(-transform.forward * -knockForce/2 * Time.deltaTime);
        }
    }
    
    #region player movement

    private void Movement()
    {
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        direction = new Vector3(h,0,v);

        anim.SetFloat("speed", direction.magnitude);

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                if (controller.enabled)
                {
                    moveDir = moveDir.normalized * speed;
                    controller.Move(moveDir * Time.deltaTime);
                }

            }
            else
            {
                if (controller.enabled)
                {
                    anim.SetFloat("speed", direction.magnitude * 2);
                    moveDir = moveDir.normalized * speed * 2;
                    controller.Move(moveDir * Time.deltaTime);

                }
            }
        }
        
        // jump
        if (Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        {
            anim.SetTrigger("jump");
            AudioManager.PlayJumpAudio();
        }
        anim.SetBool("isFlying",isFlying);
        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
        
    }

    private void Flight()
    {

        //Manage Direction and lift forces media character rotation

        currentRotation.x += Input.GetAxis("Vertical") * xSensitivity;
        currentRotation.x = Mathf.Clamp(currentRotation.x, -80, 80);

        currentRotation.z += -Input.GetAxis("Horizontal") * zSensitivity;
        currentRotation.z = Mathf.Clamp(currentRotation.z, -45, 45);

        currentRotation.y += Input.GetAxis("Horizontal");
        //if (Input.GetAxisRaw("Horizontal") != 0)
        //{
            transform.rotation = Quaternion.Euler(currentRotation);
        //}
        /*else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,
                                                                                        transform.rotation.eulerAngles.y,
                                                                                        0f), Time.deltaTime * 2);
            if (currentRotation.y >= 360 || currentRotation.y <=0)
            {
                currentRotation.y = transform.rotation.eulerAngles.y;
            }        
            currentRotation = new Vector3(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y , transform.rotation.eulerAngles.z);

        //hacer un if si current.y > 360 entonces sea 0
        }*/
        

        if (currentRotation.magnitude >0.1f)
        {
            transform.Rotate(Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal"));
        }       

        controller.Move(transform.forward * forwardSpeed * Time.deltaTime);
        forwardSpeed -= transform.forward.y * Time.deltaTime * 35.0f;


        //Contact with WindBoostItems gameobjects 
        if (isBoosted)
        {
            forwardSpeed *= 1.5f;
            boostTimer += Time.deltaTime;
            if (boostTimer >=1.5f)
            {
                
                isBoosted = false;
                boostTimer = 0;
            }         
        }

        if (!isBoosted)
        {
            if (forwardSpeed > 85)
            {
                forwardSpeed = 85;
            }
        }
        else
        {
            if (forwardSpeed > 180)
            {
                forwardSpeed = 180;
            }
        }
        
        if (forwardSpeed < 65)
        {
            forwardSpeed = 65;
        }              
    }

    public void StartFlight()
    {
        state = playerState.flight;
    }

    public void StepAudio()
    {
        AudioManager.PlayFootStepAudio();
    }

    public void FlutterAudio()
    {
        AudioManager.PlayFlyAudio();
    }
    #endregion

    #region Combat
   
    public void RangeAttack()
    {        
        GameObject newBullet = Instantiate(bullet,shootPoint.transform.position,transform.rotation);
        Physics.IgnoreCollision(newBullet.GetComponentInChildren<Collider>(),GetComponent<Collider>());
        canMove = true;
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
        Gizmos.DrawSphere(groundCheck.position,checkDistance);
    }
}
