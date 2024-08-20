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

    [Header("Pathfinding")]
    public Path path;
    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight = 1.7f;

    [Header("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate = 1f;

    [SerializeField]
    private string currentState;


    void Start()
    {
        Animator = GetComponent<Animator>();
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            lastKnownPos = player.transform.position;
        }
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
          

            if (distanceToPlayer < sightDistance)
            {
                Vector3 targetDirection = (player.transform.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

               

                if (angleToPlayer <= fieldOfView / 2) // Görüş açısını ikiye bölerek kullanıyoruz
                {
                    Vector3 rayOrigin = transform.position + (Vector3.up * eyeHeight);
                    Ray ray = new Ray(rayOrigin, targetDirection);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red); 
                        if (hitInfo.transform.gameObject == player)
                        {
                           
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
