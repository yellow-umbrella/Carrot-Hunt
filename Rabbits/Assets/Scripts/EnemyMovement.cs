using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public Vector2 CurrentDirrection { get; private set; }
    public bool IsChasing { get; private set; } = false;

    [SerializeField] private float patrolSpeed;
    [SerializeField] private PatrolPath patrolPath;

    private NavMeshAgent agent;
    private int currentInd = 0;
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
        GameManager.Instance.OnGameReset += Reset;
    }

    private void Update()
    {
        if (playerController == null)
        {
            StopChasing();
        }
        if (IsChasing)
        {
            if (playerController.IsHidden)
            {
                StopChasing();
                return;
            }
            agent.SetDestination(playerController.transform.position);
        } else if (Vector2.Distance(transform.position,patrolPath.waypoints[currentInd].position) <= .01f)
        {
            currentInd = (currentInd + 1) % patrolPath.waypoints.Length;
            agent.SetDestination(patrolPath.waypoints[currentInd].position);
        }
        Rotate();
    }

    public void OnPlayerDetected(PlayerController playerController)
    {
        IsChasing = true;
        this.playerController = playerController;
    }

    private void Reset()
    {
        currentInd = 0;
        transform.position = patrolPath.waypoints[currentInd].position;
        StopChasing();
    }

    private void StopChasing()
    {
        IsChasing = false;
        agent.SetDestination(patrolPath.waypoints[currentInd].position);
    }

    private void Rotate()
    {
        if (agent.velocity.sqrMagnitude > .01f)
        {
            Vector2 direction = agent.velocity.normalized;
            CurrentDirrection = direction.normalized;
            float angle = Vector2.SignedAngle(transform.right, direction);
            //transform.Rotate(Vector3.forward, angle);
        }
    }
}
