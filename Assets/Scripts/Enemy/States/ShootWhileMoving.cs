using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWhileMovingState : BaseState
{
    private float shootTimer;

    public void Shoot()
    {
        Transform gunbarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);
        Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;
        Debug.Log("Shoot");
        shootTimer = 0; // Ateş etme döngüsünü yeniden başlatır
    }

    public override void Enter()
    {
        enemy.Animator.SetBool("IsWalking", true); // Yürüme animasyonu başlar
        enemy.Animator.SetBool("IsRunning", false); // Koşma animasyonu durur
        enemy.Animator.SetBool("IsShootingWhileWalking", false); // Başlangıçta false yap, sonra true olacak
        shootTimer = 0; // Timer'ı sıfırla
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("IsWalking", false); // Yürüme animasyonu durur
        enemy.Animator.SetBool("IsShootingWhileWalking", false); // Yürürken ateş etme animasyonu durur
    }

   public override void Perform()
{
    if (enemy.CanSeePlayer())
    {
        shootTimer += Time.deltaTime;

        // Düşman oyuncuya doğru bakar ve ona doğru hareket eder
        enemy.transform.LookAt(enemy.Player.transform);
        enemy.Agent.SetDestination(enemy.Player.transform.position);

        if (shootTimer > enemy.fireRate)
        {
            enemy.Animator.SetBool("IsShootingWhileWalking", true); // Ateş etme zamanı geldiğinde bu değeri true yap
            Shoot(); // Ateş et
        }
        else
        {
            // Eğer ateş zamanı değilse animasyonu false yapmadan önce durumu kontrol et
            if (enemy.Animator.GetBool("IsShootingWhileWalking"))
            {
                enemy.Animator.SetBool("IsShootingWhileWalking", false);
            }
        }

        enemy.LastKnownPos = enemy.Player.transform.position;

        // Belirli bir süre sonra tekrar attack state'e geçer
        if (shootTimer > 5.0f) // Bu süreyi ihtiyacınıza göre ayarlayın
        {
            stateMachine.ChangeState(new AttackState());
        }
    }
    else
    {
        // Oyuncu kaybolduysa SearchState'e geç
        stateMachine.ChangeState(new SearchState());
    }
}

}
