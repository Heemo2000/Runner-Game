using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float slideSpeed = 50f;
    [SerializeField]private float slideDistance = 5f;
    [Min(0f)]
    [SerializeField]private float jumpTime = 0.5f;
    [Min(0f)]
    [SerializeField]private float jumpHeight = 10f;

    [Min(0f)]
    [SerializeField]private float fallMultiplier = 1.5f;

    [Range(0.1f,1.0f)]
    [SerializeField]private float fallCoolDownTime = 0.5f;
    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundCheckRadius = 0.3f;

    [SerializeField]private LayerMask groundMask;
    [SerializeField]private float moveSpeed = 10f;

    [SerializeField]private Animator playerAnimator;

    private bool _isSliding = false;
    private Rigidbody _playerRB;
    private float _gravity;
    private float _velocityY;

    private float _initialJumpVelocity;

    private Vector3 _movePosition;
    
    private float _currentPositionX;

    private float _currentFallDownTime = 0.0f;
    private void Awake() {
        _playerRB = GetComponent<Rigidbody>();
         _velocityY = 0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerRB.isKinematic = true;
        _currentPositionX = _playerRB.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseController.Instance.IsGamePaused)
        {
            return;
        }
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool jumpInput = Input.GetKeyDown(KeyCode.Space);
        if(jumpInput == true)
        {
            Debug.Log("Jump Pressed");
            Jump();
        }
        bool isGrounded = Physics.CheckSphere(groundCheck.position,groundCheckRadius,groundMask.value);
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
        if(PauseController.Instance.IsGamePaused)
        {
            return;
        }
        HandleMovement();
    }

    public IEnumerator Slide(float input)
    {
        _isSliding = true;

        float initialPositionX = _playerRB.transform.position.x;
        float finalPositionX = _playerRB.transform.position.x + input * slideDistance;

        _currentPositionX = initialPositionX;
        float distance = 0;

        while(distance < slideDistance)
        {
            _currentPositionX = Mathf.Lerp(initialPositionX,finalPositionX,distance);
            //Debug.Log("Slide distance covered : " + distance);
            distance += slideSpeed * Time.deltaTime;
            yield return null;
        }
        _currentPositionX = finalPositionX; 
        _isSliding = false;
    }

    private void CalculateParameters()
    {
        float halfJumpTime = jumpTime/2f;
        _gravity = (-2f * jumpHeight)/(halfJumpTime * halfJumpTime);
        _initialJumpVelocity = 2f * jumpHeight/halfJumpTime;
    }

    private void HandleMovement()
    {
        CalculateParameters();
        _movePosition = _playerRB.transform.position + (Vector3.forward * moveSpeed + Vector3.up * _velocityY) * Time.fixedDeltaTime;
        _movePosition.x = _currentPositionX;
        _playerRB.MovePosition(_movePosition);
        HandleGravity();
    }

    private void HandleGravity()
    {
        
        bool isFalling = _velocityY < 0.0f;

        if(!IsGrounded())
        {
            _currentFallDownTime += Time.deltaTime;
            if(_currentFallDownTime < fallCoolDownTime)
            {
                return;
            }
            else
            {
                _currentFallDownTime = 0f;
            }
            Debug.Log("Player is not grounded");
            float currentVelocityY = _velocityY;
            float currentGravity = _gravity;
            
            if(isFalling)
            {
                playerAnimator.SetBool("IsFalling",true);
                currentGravity *= fallMultiplier;
            }
            float newVelocityY = currentVelocityY + (currentGravity * Time.fixedDeltaTime);
            float nextVelocityY = (currentVelocityY + newVelocityY) * 0.5f;
            _velocityY = nextVelocityY;

        }
        else
        {
            _velocityY = 0f;
        }
    }

    public void Jump()
    {
        if(IsGrounded())
        {
            Debug.Log("Jumping");
            playerAnimator.SetTrigger("Jump");
            _velocityY += _initialJumpVelocity;
        }      
    }

    
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position,groundCheckRadius,groundMask.value);
    }
}
