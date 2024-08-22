using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float shootTimer;
    private float losePlayerTimer;
    private float originalSpeed;  // to store the original speed of the enemy
    private float stoppingDistance = 2f;  // Mesafe tanımlandı, düşman bu mesafede duracak
    private float minimumShootingDistance = 10f; // Minimum mesafe tanımlandı, bu mesafeden itibaren ateş edecek

    public void Shoot()
    {
        Transform gunbarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);
        Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;
        Debug.Log("Shoot");
        
        shootTimer = 0;
    }

    public override void Enter()
    {
        originalSpeed = enemy.Agent.speed;

        enemy.Agent.isStopped = false;  
        enemy.Animator.SetBool("IsRunning", true);
        enemy.Agent.speed = 3f;  
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("IsRunning", false);
        enemy.Agent.speed = originalSpeed; 
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            shootTimer += Time.deltaTime;

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);

            if (distanceToPlayer > stoppingDistance)
            {
                enemy.Agent.isStopped = false;
                enemy.Agent.SetDestination(enemy.Player.transform.position);

                if (distanceToPlayer <= minimumShootingDistance)
                {
                    // Minimum mesafe içindeyse ve henüz stoppingDistance'a ulaşmadıysa, hareket ederken ateş et
                    if (shootTimer > enemy.fireRate)
                    {
                        Shoot();
                    }
                }
            }
            else
            {
                // stoppingDistance içindeyse, durarak ateş et
                enemy.Agent.isStopped = true;

                if (shootTimer > enemy.fireRate)
                {
                    Shoot();
                }
            }

            enemy.transform.LookAt(enemy.Player.transform);
            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 3)
            {
                stateMachine.ChangeState(new SearchState());
            }
            else
            {
                enemy.Agent.SetDestination(enemy.LastKnownPos); 
            }
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
