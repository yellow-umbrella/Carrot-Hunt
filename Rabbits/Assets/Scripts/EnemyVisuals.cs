using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemyMovement enemyMovement;

    private const string RUNNING_BOOL = "IsRunning";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }

    private void LateUpdate()
    {
        if (enemyMovement.CurrentDirrection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (enemyMovement.CurrentDirrection.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (enemyMovement.CurrentDirrection.sqrMagnitude > 0)
        {
            animator.SetBool(RUNNING_BOOL, true);
        }
        else
        {
            animator.SetBool(RUNNING_BOOL, false);
        }
    }
}
