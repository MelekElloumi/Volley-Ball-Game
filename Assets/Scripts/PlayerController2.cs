/*using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 9.8f;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {

        // We are grounded, so recalculate
        // move direction directly from axes
        if (characterController.isGrounded)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical")!=0) { 
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection *= -speed;
            }
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            characterController.Move(moveDirection * Time.deltaTime);
        }
        else
            characterController.Move(moveDirection * Time.deltaTime * 0.5f);
    }
}
*/

using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float speed;
    //public float rotationSpeed;
    public float jumpSpeed;

    private CharacterController characterController;
    public Animator animator;
    private float ySpeed;
    private float originalStepOffset;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal2");
        float verticalInput = Input.GetAxis("Vertical2");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        if (movementDirection != Vector3.zero)
            animator.SetBool("isrunning", true);
        else
            animator.SetBool("isrunning", false);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Input.GetButtonDown("Jump2"))
            {
                ySpeed = jumpSpeed;
                animator.SetBool("isjumping", true);
            }
        }
        else
        {
            characterController.stepOffset = 0;
            animator.SetBool("isjumping", false);
        }

        Vector3 velocity = movementDirection * magnitude;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, -700f * Time.deltaTime);
        }
    }
}