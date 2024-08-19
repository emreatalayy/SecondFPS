using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public Animator Animator { get; private set; }
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 lastKnownPos;
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }

    public Path path;
    public GameObject debugSphere;
    [Header("Sight Values")]
    public float sightDistance=20f;
    public float fieldOfView=85f;
    public float eyeHeight;
    [Header("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f,10f)]
    public float fireRate;
    [SerializeField] 
    private string currentState;
    void Start()
    {
        stateMachine=GetComponent<StateMachine>();
        agent=GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player=GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        currentState=stateMachine.activeState.ToString();
        debugSphere.transform.position=lastKnownPos;
    }
    public bool CanSeePlayer()
    {
       if(player != null)
       {
          if(Vector3.Distance(transform.position,player.transform.position)<sightDistance)
          {
              Vector3 targetdirection= player.transform.position - transform.position- (Vector3.up * eyeHeight);
              float angleToPlayer=Vector3.Angle(targetdirection,transform.forward);
              if(angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
              {
                 Ray ray =new Ray(transform.position+( Vector3.up * eyeHeight), targetdirection);
                 RaycastHit hitInfo = new RaycastHit();
                 if(Physics.Raycast(ray, out hitInfo, sightDistance))
                 {
                        if(hitInfo.transform.gameObject== player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                 }
                
              }
          }
          
       }
       return false;
    }
}
