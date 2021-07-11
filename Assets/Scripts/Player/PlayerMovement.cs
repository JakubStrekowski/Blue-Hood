using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    readonly float HORIZONTAL_SPEED_CUTOFF = 0.05f; //absolute velocity below this value will be set to 0 to avoid very slow sliding
    readonly float MAX_HORIZONTAL_SPEED = 15f;
    readonly float JUMP_HELPING_COOLDOWN = 0.05f; // bonus time to jump even after we lost contact with the ground

    readonly float CAMERA_Y_AXIS_ADD = 1.5f;

    [SerializeField]
    private float maxInputSpeed = 5f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float attackMoveSpeedModifier = 0.3f;

    [SerializeField]
    [Range(5.0f, 20.0f)]
    private float dashSpeed = 7f;

    [SerializeField]
    private float jumpSpeed = 10f;

    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float timeToDoubleTap = 0.5f; // time to second tap to dash

    [SerializeField]
    Animator animator;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    Collider2D groundDetector;

    [SerializeField]
    Transform characterPresenter;

    [SerializeField]
    Transform cameraLookPoint;

    //jumping
    LayerMask _layer;
    private bool _isOnGround;
    private bool _hasJumped;
    private float _bonusTimeToJump;

    //dashing
    private float _secondTapCooldown;
    private int _dirTapCounter; // how many taps there were in during _secondTapCooldown sequence


    private void Awake()
    {
        _layer = LayerMask.GetMask(LayerMask.LayerToName(3));
    }

    float CalculateHorizontalSpeed(float moveSpeed)
    {
        float resultSpeed = Mathf.Clamp(rb.velocity.x + moveSpeed, -MAX_HORIZONTAL_SPEED, MAX_HORIZONTAL_SPEED);

        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            if (moveSpeed > 0)
            {
                characterPresenter.localScale = new Vector3(1f, transform.localScale.y, 1f);
            }
            if (moveSpeed < 0)
            {
                characterPresenter.localScale = new Vector3(-1f, transform.localScale.y, 1f);
            }
            if (moveSpeed == 0)
            {
                resultSpeed *= 0.3f;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            resultSpeed = dashSpeed * characterPresenter.localScale.x;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            resultSpeed = resultSpeed * attackMoveSpeedModifier;
        }

        return resultSpeed;
    }

    float CalculateVeritcalSpeed()
    {
        rb.gravityScale = 3;

        if (Input.GetButtonDown("Jump") && _bonusTimeToJump > 0 && !_hasJumped)
        {
            _hasJumped = true;
            _bonusTimeToJump = 0;
            return jumpSpeed;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            rb.gravityScale = 0;
            return 0;
        }
        return rb.velocity.y;
    }

    void DashMovement()
    {
        if (_secondTapCooldown > 0) _secondTapCooldown -= Time.deltaTime;
        if (_secondTapCooldown == 0)
        {
            _dirTapCounter = 0;
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (_dirTapCounter < 1) _dirTapCounter = 1;
                else _dirTapCounter++;
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (_dirTapCounter > 1) _dirTapCounter = -1;
                else _dirTapCounter--;
            }

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
            {
                if (_secondTapCooldown > 0 && (_dirTapCounter > 1 || _dirTapCounter < -1))
                {
                    animator.SetTrigger("RollPressed");
                    _secondTapCooldown = 0;
                    _dirTapCounter = 0;
                }
                else
                {
                    _secondTapCooldown = timeToDoubleTap;
                }
            }
        }
    }

    bool isStateInterruptsMovement()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Injured")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = Input.GetAxis("Horizontal") * maxInputSpeed;
        float horizontalVelocity = rb.velocity.x;
        if (!isStateInterruptsMovement())
        {
            horizontalVelocity = CalculateHorizontalSpeed(moveSpeed);
            DashMovement();
        }
        

        if (_bonusTimeToJump > 0) _bonusTimeToJump -= Time.deltaTime;

        if(groundDetector.IsTouchingLayers(_layer))
        {
            _bonusTimeToJump = JUMP_HELPING_COOLDOWN;
            _isOnGround = true;
            _hasJumped = false;
        }
        else
        {
            if (_isOnGround)
            {
                animator.SetTrigger("StartedMidAir");
            }
            _isOnGround = false;
        }

        float verticalSpeedResult = CalculateVeritcalSpeed();

        if (Mathf.Abs(horizontalVelocity) < HORIZONTAL_SPEED_CUTOFF) horizontalVelocity = 0;

        rb.velocity = (new Vector2(horizontalVelocity, verticalSpeedResult));

        Vector2 newCameraLookPos = (rb.velocity.normalized * 5f);
        cameraLookPoint.localPosition = new Vector3(newCameraLookPos.x, newCameraLookPos.y + CAMERA_Y_AXIS_ADD, 0);

        if (Input.GetButtonDown("Attack"))
        {
            animator.SetTrigger("AttackPressed");
        }

        
        animator.SetBool("OnGround", _isOnGround);
        animator.SetFloat("RunSpeed", Mathf.Abs(moveSpeed) / maxInputSpeed);
        animator.SetFloat("VerticalSpeed", rb.velocity.y);

    }
}
