using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int wayPointIndex;
    public float waitTimer;
    private float originalSpeed;
    private float originalAnimSpeed;
    public float transitionSpeed = 2f; // Hız geçişi için kullanılacak hız çarpanı

    public override void Enter()
    {
        if (enemy == null || enemy.Animator == null)
        {
            return;
        }

        originalSpeed = enemy.Agent.speed;
        originalAnimSpeed = enemy.Animator.speed;

        enemy.Animator.SetBool("IsWalking", true);
        enemy.Animator.SetBool("IsRunning", false);
        enemy.Animator.SetBool("IsSearching", false);
    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("IsWalking", false);
        enemy.Agent.speed = originalSpeed;
        enemy.Animator.speed = originalAnimSpeed;
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f) // Hedefe ulaştıysa
        {
            waitTimer += Time.deltaTime;
            SmoothStop(); // Hızı ve animasyonu yumuşatarak durdur
            
            if (waitTimer > 3) // 3 saniyeden fazla beklediyse
            {
                if (wayPointIndex < enemy.path.waypoints.Count - 1)
                    wayPointIndex++;
                else
                    wayPointIndex = 0;

                enemy.Agent.SetDestination(enemy.path.waypoints[wayPointIndex].position);
                waitTimer = 0;
                SmoothStart(); // Hızı ve animasyonu yumuşatarak tekrar başlat
            }
        }
        else
        {
            SmoothStart(); // Hedefe giderken hızı ve animasyonu yumuşatarak başlat
        }
    }

    private void SmoothStop()
    {
        enemy.Agent.speed = Mathf.Lerp(enemy.Agent.speed, 0, Time.deltaTime * transitionSpeed);
        enemy.Animator.speed = Mathf.Lerp(enemy.Animator.speed, 0, Time.deltaTime * transitionSpeed);
    }

    private void SmoothStart()
    {
        enemy.Agent.speed = Mathf.Lerp(enemy.Agent.speed, originalSpeed, Time.deltaTime * transitionSpeed);
        enemy.Animator.speed = Mathf.Lerp(enemy.Animator.speed, originalAnimSpeed, Time.deltaTime * transitionSpeed);
    }
}
