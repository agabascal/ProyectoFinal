using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //States
    public enum playerState {ground, flight}
    public playerState state;   

    [Header("Movement")]
    //Movement
    public float speed=6f;
    public CharacterController controller;
    private float turnSmooth=0.1f;
    private float smoothVelocity;
    private Transform cam;
    Vector3 direction;
    Vector3 moveDir;

    [Header("Flight")]
    public float flightGravity = -1;



    //Jump
    public float jumpSpeed = 8f;
    public float gravity = -20f;
    public Transform groundCheck;
    private float checkDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    private bool isGrounded, doubleJump;

    [Header("Combat")]

    [Header("Range")]
    //Range Attack
    public Transform shootPoint;
    public GameObject bullet;


    [Header("Animation")]
    public Animator anim;


    //Flight
    public float forwardSpeed = 25, strafeSpeed = 7.5f, hoverSpeed = 5f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (state == playerState.ground)
        {
            Movement();
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            Flight();
            velocity.y += flightGravity * Time.deltaTime;
            controller.Move(velocity*Time.deltaTime);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (state == playerState.ground)
        {
            //Gravity
            isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                state = playerState.ground;
                velocity.y = -2f;
                doubleJump = false;
            }
        }
        else
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                state = playerState.ground;
                velocity.y = -2f;
                doubleJump = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.Play("Shoot");
        }

        if (Input.GetKeyDown(KeyCode.F) && !isGrounded)
        {
            state = playerState.flight;
        }
        
    }

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
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDir.normalized * speed*2 * Time.deltaTime);

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
        controller.Move(velocity * Time.deltaTime);
    }

    private void Flight()
    {
        
    

    }

    public void RangeAttack()
    {
        Instantiate(bullet,shootPoint.transform.position,transform.rotation);
    }
}
