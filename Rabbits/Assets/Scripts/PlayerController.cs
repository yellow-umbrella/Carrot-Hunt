using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private const string GRASS_TAG = "Grass";
    private const string ENEMY_TAG = "Enemy";

    [SerializeField] private float moveSpeed;
    public bool IsHidden { get; private set; } = false;
    public Vector2 VelocityNormalized { get; private set; }

    private GameInput input;
    private Rigidbody2D playerRigidbody;
    private int grass_count = 0;


    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        input = new GameInput();
        input.Player.Enable();
    }

    private void Update()
    {
        VelocityNormalized = input.Player.Movement.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        playerRigidbody.velocity = VelocityNormalized * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag) {
            case GRASS_TAG:
                IsHidden = true;
                grass_count++;
                break;
            case ENEMY_TAG:
                GameManager.Instance.GameOver();
                break;
            default:
                GameManager.Instance.AddPoints(collision.tag);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GRASS_TAG)
        {
            grass_count--;
            if (grass_count == 0)
            {
                IsHidden = false;
                Debug.Log("Rabbit is not hidden");
            }
        }
    }
}
