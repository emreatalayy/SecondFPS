using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float shootTimer;
    private float losePlayerTimer;

    public void Shoot()
    {
        Transform gunbarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);
        Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;
        Debug.Log("Shoot");
        
        //enemy.Animator.SetBool("IsShootingWhileWalking", false);
        shootTimer = 0;
    }

    public override void Enter()
    {
        // Koşma animasyonunu başlat
        enemy.Animator.SetBool("IsRunning", true);
        enemy.Animator.SetBool("IsShootingWhileWalking", false);
        enemy.Animator.SetBool("IsWalking", false);
    }

    public override void Exit()
    {
        // Animasyonları sıfırla
        enemy.Animator.SetBool("IsRunning", false);
        enemy.Animator.SetBool("IsShootingWhileWalking", false);
        enemy.Animator.SetBool("IsWalking", false);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            shootTimer += Time.deltaTime;

            // Düşman oyuncuya doğru bakar
            enemy.transform.LookAt(enemy.Player.transform);

            // Düşman oyuncuyu takip eder
            enemy.Agent.SetDestination(enemy.Player.transform.position);

        if (shootTimer > enemy.fireRate)
        {
            // Önce animasyonları durdur
            enemy.Animator.SetBool("IsRunning", false);
            enemy.Animator.SetBool("IsWalking", true);
            
            // Sonra ateş etme animasyonunu başlat
            enemy.Animator.SetBool("IsShootingWhileWalking", true);
              Debug.Log("IsShootingWhileWalking set to true: " + enemy.Animator.GetBool("IsShootingWhileWalking"));
            Shoot();
        }
        else
        {
            // Ateş etme bitince tekrar koşmaya başla
            enemy.Animator.SetBool("IsShootingWhileWalking", false);
            
            // Eğer ateş etme animasyonu kapalıysa, koşma animasyonuna dön
            if (!enemy.Animator.GetBool("IsShootingWhileWalking"))
            {
                enemy.Animator.SetBool("IsRunning", true);
                enemy.Animator.SetBool("IsWalking", false);
            }
        }


            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                stateMachine.ChangeState(new SearchState());
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
