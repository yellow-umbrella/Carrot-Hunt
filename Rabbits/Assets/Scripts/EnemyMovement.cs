using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float patrolSpeed;
    [SerializeField] private PatrolPath patrolPath;

    private NavMeshAgent agent;
    private int currentInd = 0;
    private bool isChasing = false;
    private PlayerController playerController;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = patrolSpeed;
    }

    private void Start()
    {
        transform.position = patrolPath.waypoints[currentInd].position;
    }

    private void Update()
    {
        Rotate();
        if (isChasing)
        {
            if (playerController.IsHidden)
            {
                isChasing = false;
                agent.SetDestination(patrolPath.waypoints[currentInd].position);
                return;
            }
            if (Vector2.Distance(transform.position, playerController.transform.position) <= .1f)
            {
                GameManager.Instance.GameOver();
            }
            agent.SetDestination(playerController.transform.position);
        } else if (Vector2.Distance(transform.position,patrolPath.waypoints[currentInd].position) <= .01f)
        {
            currentInd = (currentInd + 1) % patrolPath.waypoints.Length;
            agent.SetDestination(patrolPath.waypoints[currentInd].position);
        }
        
    }
    public void OnPlayerDetected(PlayerController playerController)
    {
        isChasing = true;
        this.playerController = playerController;
    }

    private void Rotate()
    {
        if (agent.velocity.sqrMagnitude > .01f)
        {
            Vector2 direction = agent.velocity.normalized;
            float angle = Vector2.SignedAngle(transform.right, direction);
            transform.Rotate(Vector3.forward, angle);
        }
    }
}
