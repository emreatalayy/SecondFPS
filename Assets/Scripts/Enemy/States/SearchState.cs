using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;
    private float rotateTimer;
    private bool isRotating;

    public override void Enter()
    {
        Debug.Log("Entering SearchState");
        searchTimer = 0; 
        moveTimer = 0;
        rotateTimer = 0;
        isRotating = true; 

        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.Agent.isStopped = false; 
        enemy.Animator.SetBool("IsWalking", true);
        enemy.Animator.SetBool("IsRunning", false);
        enemy.Animator.SetBool("IsSearching", true);

        Debug.Log("Initial Destination: " + enemy.LastKnownPos);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            Debug.Log("Player detected, switching to AttackState");
            stateMachine.ChangeState(new AttackState());
            return;
        }

        if (isRotating)
        {
            rotateTimer += Time.deltaTime;
            enemy.Agent.isStopped = true; 
            enemy.transform.Rotate(0, 360 * Time.deltaTime / 5, 0); 
            if (rotateTimer > 5)
            {
                Debug.Log("Rotation complete, resuming movement");
                isRotating = false;
                enemy.Agent.isStopped = false; 
                enemy.Agent.SetDestination(enemy.LastKnownPos); 
            }
        }
        else if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(3, 5))
            {
                Vector3 randomDestination = enemy.transform.position + (Random.insideUnitSphere * 10);
                enemy.Agent.SetDestination(randomDestination);
                moveTimer = 0;
                Debug.Log("Moving to new random destination: " + randomDestination);
            }

            if (searchTimer > 10)
            {
                Debug.Log("Search time exceeded, switching to PatrolState");
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("IsSearching", false);
    }
}
