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
        isRotating = true; // Dönüşe başlamak için true olarak ayarlandı.
        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.Animator.SetBool("IsWalking", true);
        enemy.Animator.SetBool("IsRunning", false);
        enemy.Animator.SetBool("IsSearching", true);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
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
                isRotating = false; 
                enemy.Agent.isStopped = false; 

                // Dönüş tamamlandıktan sonra tekrar hareket etmeye başlasın
                enemy.Agent.SetDestination(enemy.LastKnownPos); // Son bilinen pozisyona tekrar git
            }
        }
        else if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(3, 5))
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
