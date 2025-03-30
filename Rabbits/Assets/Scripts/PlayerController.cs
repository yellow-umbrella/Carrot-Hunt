using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    private const string GRASS_TAG = "Grass";
    [SerializeField] private float moveSpeed;
    public bool IsHidden { get; private set; } = false;

    private GameInput input;
    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;

    private Vector2 velocityNormalized;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        input = new GameInput();
        input.Player.Enable();
    }

    private void Update()
    {
        velocityNormalized = input.Player.Movement.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        playerRigidbody.velocity = velocityNormalized * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GRASS_TAG)
        {
            IsHidden = true;
            Debug.Log("Rabbit is hidden now");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GRASS_TAG)
        {
            IsHidden = false;
            Debug.Log("Rabbit is not hidden");
        }
    }
}
