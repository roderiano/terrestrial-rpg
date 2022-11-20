using UnityEngine;
using UnityEditor;

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
    private Transform groundCheckPoint;
    private PlayerWeapons weapons;
    private CharacterController characterController;


    //--> MonoBehaviour methods
    void Awake() 
    {
        active = true;
        animator = GetComponent<Animator>();
        groundCheckPoint = transform.Find("GroundCheckPoint");
        weapons = GetComponent<PlayerWeapons>();
        characterController = GetComponent<CharacterController>();
    }

    void OnDrawGizmos()
    {
        GUI.color = Gizmos.color = IsGrounded() ? Color.green : Color.red;
        GUIStyle style = GUI.skin.GetStyle("Label");
        style.alignment = TextAnchor.UpperCenter;

        if(groundCheckPoint != null)
        {
            Gizmos.DrawWireSphere(groundCheckPoint.position, radiusCheckPoint);
            Handles.Label(
                groundCheckPoint.position + new Vector3(0f, radiusCheckPoint + .25f, 0f), 
                IsGrounded() ? "isGrounded true" : "isGrounded false",
                style
            );
        }   
    }

    void FixedUpdate()
    {
        
    }
    
    void Update()
    {
        Movement();
        SetRotation();
        SetAnimatorParams();
    }


    //--> Public methods
    public void Movement() 
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Transform cameraTransform = Camera.main.transform;
        direction = cameraTransform.TransformDirection(direction).normalized;

        if(!IsGrounded())
            direction.y -= 70f * Time.deltaTime;
        else
            direction.y = 0f;

        characterController.Move(direction * speed * Time.deltaTime);
        
    }


    //--> Private methods
    private void SetRotation() 
    {
        if(active)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            Transform cameraTransform = Camera.main.transform;
            
            direction = Input.GetKey(KeyCode.Mouse1) && weapons.GetFireGunSlot() != null ? cameraTransform.forward : cameraTransform.TransformDirection(direction).normalized;
            direction.y = 0f;

            if(direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 25f * Time.deltaTime); 
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

        float currentSpeed = animator.GetFloat("Speed");
        float currentHorizontal = animator.GetFloat("Horizontal");
        float currentVertical = animator.GetFloat("Vertical");

        currentSpeed = Mathf.Lerp(currentSpeed, speed, 10f * Time.deltaTime);
        currentHorizontal = Mathf.Lerp(currentHorizontal, Input.GetAxisRaw("Horizontal"), 10 * Time.deltaTime);
        currentVertical = Mathf.Lerp(currentVertical, Input.GetAxisRaw("Vertical"), 10 * Time.deltaTime);
        
        animator.SetFloat("Speed", currentSpeed);
        animator.SetFloat("Vertical", currentVertical);
        animator.SetFloat("Horizontal", currentHorizontal); 
        animator.SetBool("IsGrounded", IsGrounded());
        animator.SetBool("IsAiming", Input.GetKey(KeyCode.Mouse1) && weapons.GetFireGunSlot() != null);
    }

    private bool IsGrounded() 
    {
        if(groundCheckPoint != null)
            return Physics.CheckSphere(groundCheckPoint.position, radiusCheckPoint, groundMask);

        return false;
    }

    public void SetActive(bool value) 
    {
        active = value;
        
        if(!active)
            pausingSpeed = Input.GetAxis("Horizontal") * Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") * Input.GetAxis("Vertical");
    }
}
