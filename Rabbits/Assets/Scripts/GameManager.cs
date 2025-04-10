using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action OnGameReset;

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private Text scoreText;

    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 startingPosition;
    private int score = 0;
    private GameObject currentPlayer;

    private const string CARROT_TAG = "Carrot";
    private const string BURROW_TAG = "Burrow";
    private Dictionary<string, int> points = new Dictionary<string, int>()
    {
        {CARROT_TAG, 1 },
        {BURROW_TAG, 5 },
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartGame()
    {
        score = 0;
        UpdateScore();
        OnGameReset?.Invoke();
        startMenu.SetActive(false);
        endMenu.SetActive(false);
        currentPlayer = Instantiate(player, startingPosition, Quaternion.identity);
    }

    public void EndGame()
    {
        endMenu.SetActive(true);
        Destroy(currentPlayer);
    }

    public void GameOver()
    {
        Debug.Log("Game over");
        EndGame();
    }

    public void AddPoints(string tag)
    {
        if (points.ContainsKey(tag))
            score += points[tag];
        UpdateScore();
        if (tag == BURROW_TAG)
        {
            EndGame();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = $"SCORE: {score}";
    }
}
