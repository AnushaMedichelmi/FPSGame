using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent agent;
    Animator animator;
    public GameObject target;
    public float stoppingDistance;
    public enum STATE { IDLE, CHASE, ATTACK, DEATH }
    public STATE state = STATE.IDLE;
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.IDLE:
                TurnOffAllAnim();
                if (NearPlayer())
                {
                    state = STATE.CHASE;
                }
                break;
            case STATE.CHASE:
                TurnOffAllAnim();
                animator.SetBool("isRunning", true);
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 3f;
                print("running");
                if (DistanceToPlayer() <= 4f)
                {
                    state = STATE.ATTACK;
                }


                if (DistanceToPlayer() > 20f)
                {
                    state = STATE.IDLE;
                }
                break;
            case STATE.ATTACK:
                TurnOffAllAnim();
                animator.SetBool("isAttacking", true);
                if (DistanceToPlayer() > 4f)
                {
                    state = STATE.IDLE;

                }
                break;
            case STATE.DEATH:
                TurnOffAllAnim();
                animator.SetBool("isDead", true);
                break;
            default:
                break;
        }

    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(target.transform.position, agent.transform.position);
    }

    public bool NearPlayer()
    {
        if (DistanceToPlayer() < 20f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void EnemyDead()
    {

        state = STATE.DEATH;
    }
    public void TurnOffAllAnim()
    {
        animator.SetBool("isDead", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
    }
}

