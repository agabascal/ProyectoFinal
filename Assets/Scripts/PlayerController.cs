using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    //States
    public enum playerState { ground, flight }
    public playerState state;

    [Header("Movement")]
    //Movement
    public float speed = 6f;
    public CharacterController controller;
    private float turnSmooth = 0.1f;
    private float smoothVelocity;
    private Camera cam;
    Vector3 direction;
    Vector3 moveDir;
  

    [Header("Flight")]
    public float forwardSpeed = 10f;
    public float flightGravity = -2f;
    private float rotSpeedX = 3f;
    private float rotSpeedY = 1.5f;
    public GameObject wings;

    //Jump
    public float jumpSpeed = 8f;
    public float gravity = -20f;
    public Transform groundCheck;
    private float checkDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    public bool isGrounded, doubleJump;

    [Header("Combat")]

    [Header("Range")]
    //Range Attack
    public Transform shootPoint;
    public GameObject bullet;


    [Header("Animation")]
    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = false;

        //GetComponents
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        cam = Camera.main;


    }

    private void FixedUpdate()
    {
        if (state == playerState.ground)
        {
            Movement();
            controller.enabled = true;
            cam.GetComponent<CinemachineBrain>().enabled = true;
            cam.GetComponent<FlightCameraControl>().enabled = false;
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {

            Flight();
            controller.enabled = false;

            cam.GetComponent<CinemachineBrain>().enabled = false;
            cam.GetComponent<FlightCameraControl>().enabled = true;

            //velocity.y += flightGravity * Time.deltaTime;
            //controller.Move(velocity * Time.deltaTime);
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
                wings.SetActive(false);
                state = playerState.ground;
                velocity.y = -2f;
                doubleJump = false;
            }
        }
        else
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance * 4, groundMask);
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

        if (Input.GetKeyDown(KeyCode.F) && !isGrounded && state == playerState.ground)
        {
            wings.SetActive(true);
            state = playerState.flight;
        }
        if (isGrounded)
        {
            state = playerState.ground;
        }
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        direction = new Vector3(h, 0, v);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDir.normalized * speed * 2 * Time.deltaTime);

            }
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
        }

        if (!isGrounded && !doubleJump && Input.GetKeyDown(KeyCode.Space) && velocity.y < 0)
        {
            velocity.y = Mathf.Sqrt((jumpSpeed * 0.75f) * -2f * gravity);
            doubleJump = true;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    


    private void Flight()
    {        

        transform.position += transform.forward * forwardSpeed * Time.deltaTime;

        transform.Rotate(Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal"));

        forwardSpeed -= transform.forward.y * Time.deltaTime * 25.0f;
        if (forwardSpeed >35)
        {
            forwardSpeed = 35;
        }
        if (forwardSpeed < 15)
        {
            forwardSpeed = 15;
        }


    }

    public void RangeAttack()
    {
        Instantiate(bullet,shootPoint.transform.position,transform.rotation);
    }
}
