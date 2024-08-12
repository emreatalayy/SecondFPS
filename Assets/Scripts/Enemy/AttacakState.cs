using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AttacakState : BaseState
{
    private float moveTimer;
    private float shootTimer;
    private float losePlayerTimer;
    public void Shoot()
    {
        Transform gunbarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);
        Vector3 shootDirection =( enemy.Player.transform.position - gunbarrel.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f,3f),Vector3.up)*shootDirection * 40;
        Debug.Log("Shoot");
        shootTimer=0;
    }
    public override void Enter()
    {
    }
    public override void Exit()
    {
    }
    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer=0;
            moveTimer+=Time.deltaTime;
            shootTimer+=Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
            if(shootTimer>enemy.fireRate)
            {
                Shoot();
                
            }
            if(moveTimer>Random.Range(3,7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer=0;
            }
            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer>8)
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
