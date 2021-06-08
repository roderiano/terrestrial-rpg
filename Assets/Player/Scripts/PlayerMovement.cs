using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour 
{
    [SerializeField]
    private float speed, jumpForce, jumpTime;
    [Range(0f, 1f)]
    [SerializeField]
    private float radiusCheckPoint;
    [SerializeField]
    private LayerMask groundMask;
    
    private float pausingSpeed;
    private bool active;
    private Animator animator;
    private Quaternion chestRotation;
    private Transform groundCheckPoint;
    private Transform chestBoneTransform;
    private PlayerWeapons weapons;
    private Rigidbody rb;
    private bool isJumping;
    private float jumpTimeCounter;


    //--> MonoBehaviour methods
    void Awake() 
    {
        active = true;
        animator = GetComponent<Animator>();
        groundCheckPoint = transform.Find("GroundCheckPoint");
        chestBoneTransform = animator.GetBoneTransform(HumanBodyBones.Chest);
        weapons = GetComponent<PlayerWeapons>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(groundCheckPoint != null)
            Gizmos.DrawSphere(groundCheckPoint.position, radiusCheckPoint);
    }

    void Update()
    {
        if(IsGrounded() && Input.GetButtonDown("Get")) 
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector3.up * jumpForce;
        } 

        if(Input.GetButton("Get") && isJumping)
        {
            if(jumpTimeCounter > 0f)
            {
                rb.velocity = Vector3.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if(Input.GetButtonUp("Get"))
            isJumping = false;

        SetAnimatorParams();

    }


    //--> Public methods
    public void Movement() 
    {
        SetRotation();
        ApplyGravity();

        if(!IsGrounded())
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            Transform cameraTransform = Camera.main.transform;
            
            direction = (Input.GetButton("Aim") || Input.GetAxis("Aim") != 0f) && weapons.GetFireGunSlot() != null ? cameraTransform.forward : cameraTransform.TransformDirection(direction).normalized;
            rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, direction.z * speed);
        }
    }

    private void ApplyGravity() 
    {
        Vector3 gravity = -9.81f * 10f * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }


    //--> Private methods
    private void SetRotation() 
    {
        if(active)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            Transform cameraTransform = Camera.main.transform;
            
            direction = (Input.GetButton("Aim") || Input.GetAxis("Aim") != 0f) && weapons.GetFireGunSlot() != null ? cameraTransform.forward : cameraTransform.TransformDirection(direction).normalized;
            direction.y = 0f;


            if(direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime); 
        }
    }

    private void SetAnimatorParams() 
    {
        float speed;

        if(active)
        {
            speed = Input.GetAxisRaw("Horizontal") * Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") * Input.GetAxisRaw("Vertical");
        } 
        else
        {
            pausingSpeed = Mathf.Lerp(pausingSpeed, 0, Time.deltaTime * 10);
            speed = pausingSpeed;
        }  
        
        animator.SetFloat("Speed", speed);
        animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal")); 
        animator.SetBool("IsGrounded", IsGrounded());
    }

    private bool IsGrounded() 
    {
        return Physics.CheckSphere(groundCheckPoint.position, radiusCheckPoint, groundMask);
    }

    public void SetActive(bool value) 
    {
        active = value;
        
        if(!active)
            pausingSpeed = Input.GetAxis("Horizontal") * Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") * Input.GetAxis("Vertical");
    }
}
