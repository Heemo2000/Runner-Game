using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float slideSpeed = 50f;
    [SerializeField]private float slideDistance = 5f;
    [SerializeField]private float jumpForce = 20f;
    [Range(1f,100f)]
    [SerializeField]private float jumpMultiplier = 2.0f;
    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundCheckRadius = 0.3f;

    [SerializeField]private LayerMask groundMask;
    [SerializeField]private float moveSpeed = 10f;
    [SerializeField]private float gravity = 9.8f;

    [SerializeField]private Animator playerAnimator;
    private Rigidbody _playerRB;
    private bool _isSliding = false;
    private void Awake() {
        _playerRB = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool jumpInput = Input.GetKeyDown(KeyCode.Space);
        if(jumpInput == true)
        {
            Debug.Log("Jump Pressed");
            Jump();
        }
        bool isGrounded = Physics.CheckSphere(groundCheck.position,groundCheckRadius,groundMask.value);
        _playerRB.AddForce(Vector3.down * gravity);
        if(horizontalInput != 0)
        {
            bool isWallAtSide = Physics.Raycast(transform.position,Vector3.right * horizontalInput,slideDistance);
            if(!isWallAtSide && !_isSliding)
            {
                StartCoroutine(Slide(horizontalInput));
            }
        }    
    }

    private void FixedUpdate() 
    {
        _playerRB.velocity = Vector3.forward * moveSpeed;

    }

    public IEnumerator Slide(float input)
    {
        _isSliding = true;

        Vector3 initialPosition = _playerRB.position;
        Vector3 finalPosition = _playerRB.position + Vector3.right * input * slideDistance;

        Vector3 currentPosition = initialPosition;
        float distance = Vector3.Distance(currentPosition,initialPosition);

        while(distance < slideDistance)
        {
            currentPosition = Vector3.Lerp(initialPosition,finalPosition,distance);
            _playerRB.MovePosition(new Vector3(currentPosition.x,_playerRB.position.y,_playerRB.position.z));
            distance += slideSpeed * Time.deltaTime;
            yield return null;
        } 

        _isSliding = false;
    }

    public void Jump()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position,groundCheckRadius,groundMask.value);
        if(isGrounded)
        {
            Debug.Log("Jumping");
            playerAnimator.SetTrigger("Jump");
            _playerRB.AddForce(Vector3.up * jumpForce * jumpMultiplier);
        }
                
    }
}
