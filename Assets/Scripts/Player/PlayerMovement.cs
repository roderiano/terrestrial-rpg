using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour 
{
    [SerializeField]
    private float speed;
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
    private CharacterController characterController;
    private PlayerStats playerStats;
    


    //--> MonoBehaviour methods
    void Awake() 
    {
        active = true;
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        characterController = GetComponent<CharacterController>();
        groundCheckPoint = transform.Find("GroundCheckPoint");
        chestBoneTransform = animator.GetBoneTransform(HumanBodyBones.Chest);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(groundCheckPoint != null)
            Gizmos.DrawSphere(groundCheckPoint.position, radiusCheckPoint);
    }


    //--> Public methods
    public void Movement() 
    {
        SetRotation();
        ApplyGravity();
        SetAnimatorParams();
    }


    //--> Private methods
    private void SetRotation() 
    {
        if(active)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            Transform cameraTransform = Camera.main.transform;
            
            direction = Input.GetButton("Aim") && playerStats.GetFireGunSlot() != null ? cameraTransform.forward : cameraTransform.TransformDirection(direction).normalized;
            direction.y = 0f;
            
            if(direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime); 
        }
    }

    private void ApplyGravity()  {
        float gravity = 0f;
        if (IsGrounded()) 
        {
            gravity =  -characterController.stepOffset / Time.deltaTime;
        } else {
            gravity -= 200f * Time.deltaTime;
        }

        Vector3 gravityDirection = new Vector3 (0f, gravity, 0f);
        characterController.Move(gravityDirection * Time.deltaTime);
    }

    private void SetAnimatorParams() 
    {
        float speed;

        if(active)
        {
            speed = Input.GetAxis("Horizontal") * Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") * Input.GetAxis("Vertical");
        } 
        else
        {
            pausingSpeed = Mathf.Lerp(pausingSpeed, 0, Time.deltaTime * 10);
            speed = pausingSpeed;
        }  
        
        animator.SetFloat("Speed", speed);
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal")); 
    }

    private bool IsGrounded() 
    {
        if(characterController.isGrounded)
        {
            return true;
        }
        else if(Physics.CheckSphere(groundCheckPoint.position, radiusCheckPoint, groundMask))
        {
            return true;
        }
        
        return false;
    }

    public void SetActive(bool value) 
    {
        active = value;
        
        if(!active)
            pausingSpeed = Input.GetAxis("Horizontal") * Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") * Input.GetAxis("Vertical");
    }
}
