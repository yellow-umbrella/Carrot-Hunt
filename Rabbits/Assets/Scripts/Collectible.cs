using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private void Start()
    {
        GameManager.Instance.OnGameReset += Reset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == PLAYER_TAG)
        {
            gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        gameObject.SetActive(true);
    }
}
