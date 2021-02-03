using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement
    public float speed=6f;
    public CharacterController controller;
    private float turnSmooth=0.1f;
    private float smoothVelocity;
    private Transform cam;
    Vector3 direction;
    Vector3 moveDir;

    //Jump
    public float jumpSpeed = 8f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    private float checkDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    private bool isGrounded, doubleJump;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        velocity.y += gravity * Time.deltaTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position,checkDistance,groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            doubleJump = false;
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

        if (!isGrounded && !doubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt((jumpSpeed* 0.75f) * -2f * gravity);
            doubleJump = true;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
       
    }
}
