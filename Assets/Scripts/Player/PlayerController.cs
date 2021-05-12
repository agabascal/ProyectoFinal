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
    private bool isGrounded, doubleJump;
    public bool flightUnlocked;
    #endregion

    #region Flight Mechanic
    [Header("Flight")]
    public float flightGravity = -1;
    public float forwardSpeed = 25;    
    private float boostTimer;
    public float xSensitivity, zSensitivity;

    public GameObject wings;
    
    public bool isFlying;
    public bool isBoosted;

    private Vector3 currentRotation;
    #endregion

    #region Combat
    [Header("Combat")]
    public int life = 3;
    private float knockTimer = 1f;
    public bool isKnocked;
    public float knockForce = 5f;
    public MeshCollider meshCol;
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
        meshCol = GetComponent<MeshCollider>();
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    //Handle physics and movement
    private void FixedUpdate()
    {
        if (state == playerState.ground)
        {
            currentRotation = Vector3.zero;
            if (controller.enabled && canMove)
            {
                Movement();
            }
            velocity.y += gravity * Time.deltaTime;
            //cam.GetComponent<CinemachineBrain>().enabled = true;
           // cam.GetComponent<FlightCameraControl>().enabled = false;
            wings.SetActive(false);
        }
        else
        {
            Flight();
            //cam.GetComponent<CinemachineBrain>().enabled = false;
            //cam.GetComponent<FlightCameraControl>().enabled = true;
            wings.SetActive(true);

            velocity.y += flightGravity * Time.deltaTime;
            if (velocity.y <-5)
            {
                velocity.y = -5f;
            }
            transform.Translate(velocity * Time.deltaTime);            
        }               
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (state == playerState.ground && canMove)
        {
            //Gravity
            isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                state = playerState.ground;
                velocity.y = -2f;
                doubleJump = false;
            }

            isFlying = false;
            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("attack");
                MeleeAttack();
            }
        }
        else
        {
            isFlying = true;
            isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                state = playerState.ground;
                velocity.y = -2f;
                doubleJump = false;
            }
        }

        if (Input.GetMouseButtonDown(1) && shootingUnlocked)
        {
            anim.Play("Shoot");
        }

        

        if (GameManager.Instance.partsCollected == 4)
        {
            if (Input.GetKeyDown(KeyCode.F) && !isGrounded)
            {
                
                currentRotation = transform.eulerAngles;
                state = playerState.flight;
            }
        }
       
        if (Input.GetKeyDown(KeyCode.X) && state == playerState.flight)
        {
            state = playerState.ground;
            transform.rotation = Quaternion.Euler(0,0,0);
        }

        if (isGrounded)
        {
            state = playerState.ground;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0f);
        }

        

    }
    
    #region player movement

    private void Movement()
    {
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        direction = new Vector3(h,0,v);
        
        if (direction.magnitude>=0.1f)
        {
            
            float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref smoothVelocity,turnSmooth);
            transform.rotation = Quaternion.Euler(0f,angle,0f);

            moveDir = Quaternion.Euler(0f,targetAngle,0f)*Vector3.forward;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                if (controller.enabled)
                {
                    controller.Move(moveDir.normalized * speed * Time.deltaTime);
                }
                
            }
            else
            {
                if (controller.enabled)
                {
                    controller.Move(moveDir.normalized * speed*2 * Time.deltaTime);
                }
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);            
        }

        if (!isGrounded && !doubleJump && Input.GetKeyDown(KeyCode.Space) && velocity.y <0)
        {
            velocity.y = Mathf.Sqrt((jumpSpeed* 0.75f) * -2f * gravity);
            doubleJump = true;
        }
        if (controller.enabled)
        {
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void Flight()
    {
            
        //Manage Direction and lift forces media character rotation

        currentRotation.x += Input.GetAxis("Vertical") * xSensitivity;
        currentRotation.x = Mathf.Clamp(currentRotation.x, -80, 80);

        currentRotation.z += -Input.GetAxis("Horizontal") *zSensitivity;
        currentRotation.z = Mathf.Clamp(currentRotation.z, -45, 45);

        currentRotation.y += Input.GetAxis("Horizontal");
       
        transform.rotation = Quaternion.Euler(currentRotation);

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
            if (forwardSpeed > 45)
            {
                forwardSpeed = 45;
            }
        }
        else
        {
            if (forwardSpeed > 180)
            {
                forwardSpeed = 180;
            }
        }
        
        if (forwardSpeed < 25)
        {
            forwardSpeed = 25;
        }              
    }
    #endregion

    #region Combat
   
    public void RangeAttack()
    {
        Instantiate(bullet,shootPoint.transform.position,transform.rotation);
    }

    public void MeleeAttack()
    {
        tailHitbox.enabled = true;
    }

    public void StopAttack()
    {
        tailHitbox.enabled = false;
    }



    //Knockback on contact with enemies
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {      
            StartCoroutine(KnockBack());
            life--;
        }                
    }

    private IEnumerator KnockBack()
    {


        controller.enabled = false;
        //rb.isKinematic = false;
        //capsuleCol.enabled = true;

        rb.velocity = (-transform.forward + (Vector3.up * knockForce / 7)) * knockForce;

        yield return new WaitForSeconds(knockTimer);

        //capsuleCol.enabled = false;
        controller.enabled = true;
        //rb.isKinematic = true;
        isHurt = false;

    }

    public void Knock()
    {
        isKnocked = true;
        StartCoroutine(KnockBack());
        isKnocked = false;
    }
    #endregion
}
