using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float shootTimer;
    private float losePlayerTimer;
    private float originalSpeed;  // Botun orijinal hızı için değişken

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
        // Orijinal hız değerini sakla
        originalSpeed = enemy.Agent.speed;

        // Koşma animasyonunu başlat ve hızı arttır
        enemy.Animator.SetBool("IsRunning", true);
        enemy.Agent.speed = 2f;  // Hızı 3'e çıkar
    }

    public override void Exit()
    {
        // Animasyonları sıfırla ve hızı orijinal haline döndür
        enemy.Animator.SetBool("IsRunning", false);
        enemy.Agent.speed = originalSpeed;  // Hızı orijinal değere döndür
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            shootTimer += Time.deltaTime;

            enemy.transform.LookAt(enemy.Player.transform);
            enemy.Agent.SetDestination(enemy.Player.transform.position);

            if (shootTimer > enemy.fireRate)
            {
                Shoot();
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
