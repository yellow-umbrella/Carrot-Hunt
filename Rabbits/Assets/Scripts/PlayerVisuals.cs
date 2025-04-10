using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerVisuals : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    private const string RUNNING_BOOL = "IsRunning";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void LateUpdate()
    {
        if (playerController.IsHidden)
        {
            spriteRenderer.color = Color.gray;
        } else
        {
            spriteRenderer.color = Color.white;
        }

        if (playerController.VelocityNormalized.x > 0)
        {
            spriteRenderer.flipX = false;
        } else if (playerController.VelocityNormalized.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (playerController.VelocityNormalized.sqrMagnitude > 0)
        {
            animator.SetBool(RUNNING_BOOL, true);
        } else
        {
            animator.SetBool(RUNNING_BOOL, false);
        }
    }
}
