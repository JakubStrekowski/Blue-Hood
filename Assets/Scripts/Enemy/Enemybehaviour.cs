using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyBehaviourStates
{
    Idle,
    MoveLeft,
    MoveRight,
    Attack
}

public class Enemybehaviour : MonoBehaviour
{
    [SerializeField]
    [Range(1, 10)]
    private int health = 3;

    [SerializeField]
    [Range(1, 10)]
    private float moveSpeed = 3f;

    [SerializeField]
    private Transform characterPresenter;

    [HideInInspector]
    public bool playerInSight;

    Rigidbody2D _rb;
    Patrolling _patrollingModule;
    Animator _animator;

    EEnemyBehaviourStates _currentState;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _patrollingModule = GetComponent<Patrolling>();
        _animator = characterPresenter.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalVelocity = _rb.velocity.x;

        if(playerInSight)
        {
            _currentState = EEnemyBehaviourStates.Attack;
        }
        else
        {
            if (_patrollingModule.isPatrollingActive())
            {
                if (_patrollingModule.GetNextWaypoint().x < transform.position.x)
                {
                    _currentState = EEnemyBehaviourStates.MoveLeft;
                }
                if(_patrollingModule.GetNextWaypoint().x > transform.position.x)
                {
                    _currentState = EEnemyBehaviourStates.MoveRight;
                }

                if(_patrollingModule.isWaypointReached())
                {
                    _patrollingModule.GoToNextWaypoint();
                }
            }
            else
            {
                _currentState = EEnemyBehaviourStates.Idle;
            }
        }

        switch (_currentState)
        {
            case EEnemyBehaviourStates.Idle:
                {
                    horizontalVelocity = 0;
                    break;
                }
            case EEnemyBehaviourStates.MoveLeft:
                {
                    characterPresenter.localScale = new Vector3(-1, characterPresenter.localScale.y, characterPresenter.localScale.z);
                    horizontalVelocity = -moveSpeed;
                    break;
                }
            case EEnemyBehaviourStates.MoveRight:
                {
                    characterPresenter.localScale = new Vector3(1, characterPresenter.localScale.y, characterPresenter.localScale.z);
                    horizontalVelocity = moveSpeed;
                    break;
                }
            case EEnemyBehaviourStates.Attack:
                {
                    horizontalVelocity = 0;
                    break;
                }

            default:
                break;
        };


        _rb.velocity = new Vector2(horizontalVelocity, _rb.velocity.y);

        _animator.SetBool("isFacingPlayer", playerInSight);
        _animator.SetFloat("RunSpeed", Mathf.Abs(horizontalVelocity));
    }
}
