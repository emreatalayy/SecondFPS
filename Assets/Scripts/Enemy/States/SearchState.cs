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
        searchTimer = 0; 
        moveTimer = 0;
        rotateTimer = 0;
        isRotating = false;
        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.Animator.SetBool("IsWalking", true);
        enemy.Animator.SetBool("IsRunning", false);
        enemy.Animator.SetBool("IsSearching", true);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
            stateMachine.ChangeState(new AttackState());

        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            // Etrafında dönme işlemi
            if (!isRotating)
            {
                isRotating = true;
                rotateTimer = 0;
            }

            if (isRotating)
            {
                rotateTimer += Time.deltaTime;
                enemy.transform.Rotate(0, 360 * Time.deltaTime / 5, 0); // 5 saniyede tam bir tur

                if (rotateTimer > 5)
                {
                    isRotating = false; // Dönüş tamamlandığında dur
                }
            }

            if (!isRotating && moveTimer > Random.Range(3, 5))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }

            if (searchTimer > 10)
            {
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("IsSearching", false);
    }
}
