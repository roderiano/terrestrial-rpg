using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    [SerializeField]
    private float speed;
    [Range(0f, 1f)]
    [SerializeField]
    private float radiusCheckPoint;
    [SerializeField]
    private LayerMask groundMask;
    
    private Animator animator;
    private PlayerInput playerInput;
    private Quaternion chestRotation;
    private Transform groundCheckPoint;
    private Transform chestBoneTransform;
    private CharacterController characterController;


    //--> MonoBehaviour methods
    void Awake() 
    {
        animator = GetComponent<Animator>();
        playerInput =  GetComponent<PlayerInput>();
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
    public void Move() 
    {
        SetRotation();
        ApplyGravity();
        SetAnimatorParams();
    }


    //--> Private methods
    private void SetRotation() 
    {
        Vector3 direction = new Vector3(playerInput.GetHorizontal(), 0f, playerInput.GetVertical());
        Transform cameraTransform = Camera.main.transform;
        
        direction = playerInput.IsAiming() ? cameraTransform.forward : cameraTransform.TransformDirection(direction).normalized;
        direction.y = 0f;
        
        if(direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime); 

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
        float speed = playerInput.GetHorizontal() * playerInput.GetHorizontal() + playerInput.GetVertical() * playerInput.GetVertical();
        
        animator.SetFloat("Speed", speed);
        animator.SetFloat("Vertical", playerInput.GetVertical());
        animator.SetFloat("Horizontal", playerInput.GetHorizontal());
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
}
