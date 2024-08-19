using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int wayPointIndex ;
    public float waitTimer;
    public override void Enter()
    {
        if (enemy == null)
        {
            Debug.LogError("PatrolState'te enemy nesnesi null.");
            return;
        }
        
        if (enemy.Animator == null)
        {
            Debug.LogError("Enemy nesnesinde Animator bileşeni eksik.");
            return;
        }
        
        enemy.Animator.SetBool("IsWalking", true);
        enemy.Animator.SetBool("IsRunning", false);
        enemy.Animator.SetBool("IsSearching", false);
    }

    public override void Perform()
    {
        PatrolCycle();
        if(enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }
    public override void Exit()
    {
            enemy.Animator.SetBool("IsWalking", false);
    }

    public void PatrolCycle()
    {
        if(enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer>3)
            {
            if(wayPointIndex<enemy.path.waypoints.Count-1)
                wayPointIndex++;
            else
                wayPointIndex = 0;
            
            enemy.Agent.SetDestination(enemy.path.waypoints[wayPointIndex].position);
            waitTimer = 0;
            }
        }
    }
}
