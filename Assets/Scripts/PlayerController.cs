﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    public GameObject wings;
    public float forwardSpeed = 25;
    private bool isFlying;
    public bool isBoosted;
    private float boostTimer;
    Vector3 currentRotation;
    public float xSensitivity, zSensitivity;

    //Jump
    public float jumpSpeed = 8f;
    public float gravity = -20f;
    public Transform groundCheck;
    private float checkDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    private bool isGrounded, doubleJump;

    [Header("Combat")]

    public int life = 3;
    private float knockTimer = 1f;
    public bool isKnocked;
    public float knockForce = 5f;
    public CapsuleCollider capsuleCol;
    public Rigidbody rb;
    public bool isHurt;

    [Header("Range")]
    //Range Attack
    public Transform shootPoint;
    public GameObject bullet;


    [Header("Animation")]
    public Animator anim;


    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        capsuleCol = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (state == playerState.ground)
        {
            if (controller.enabled)
            {
                Movement();
            }
            velocity.y += gravity * Time.deltaTime;
            cam.GetComponent<CinemachineBrain>().enabled = true;
            cam.GetComponent<FlightCameraControl>().enabled = false;
            wings.SetActive(false);
        }
        else
        {
            Flight();


            cam.GetComponent<CinemachineBrain>().enabled = false;
            cam.GetComponent<FlightCameraControl>().enabled = true;
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
        if (Input.GetKeyDown(KeyCode.X) && state == playerState.flight)
        {
            state = playerState.ground;

        }
        if (isGrounded)
        {
            state = playerState.ground;

        }

        

    }

    public void Knock()
    {
        isKnocked = true;
        StartCoroutine(KnockBack());
        isKnocked = false;
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
        

        currentRotation.x += Input.GetAxis("Vertical") * xSensitivity;

        currentRotation.x = Mathf.Clamp(currentRotation.x, -80, 80);

        currentRotation.z += -Input.GetAxis("Horizontal") *zSensitivity;

        currentRotation.y += Input.GetAxis("Horizontal");

        currentRotation.z = Mathf.Clamp(currentRotation.z,-45,45);

        transform.rotation = Quaternion.Euler(currentRotation);

        

        transform.Rotate(Input.GetAxis("Vertical"),0.0f, -Input.GetAxis("Horizontal"));

        controller.Move(transform.forward * forwardSpeed * Time.deltaTime);

        forwardSpeed -= transform.forward.y * Time.deltaTime * 35.0f;

        if (isBoosted)
        {
            Debug.Log(isBoosted);
            forwardSpeed *= 1.5f;
            boostTimer += Time.deltaTime;
            if (boostTimer >=1.5f)
            {
                
                isBoosted = false;
                Debug.Log(isBoosted);
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

        //Clamp Rotation        


    }

    private IEnumerator KnockBack()
    {
        

        controller.enabled = false;
        rb.isKinematic = false;
        //capsuleCol.enabled = true;

        rb.velocity = (-transform.forward + (Vector3.up * knockForce/7)) * knockForce;

        yield return new WaitForSeconds(knockTimer);

        //capsuleCol.enabled = false;
        controller.enabled = true;
        rb.isKinematic = true;
        isHurt = false;

    }

    public void RangeAttack()
    {
        Instantiate(bullet,shootPoint.transform.position,transform.rotation);
    }


    //Funciona con trigger no collision con la capsula mientras tanto
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {      
            StartCoroutine(KnockBack());
            life--;
        }                
    }

}
