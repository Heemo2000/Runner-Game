using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : GenericSingleton<GameManager>
{

    [SerializeField]private Canvas scoreCanvas;
    [SerializeField]private TMP_Text scoreText;
    [SerializeField]private Canvas gameOverCanvas;
    [SerializeField]private TMP_Text gameOverScoreText;
    private int _score = 0;

    public Action OnScoreIncrease;
    public Action OnGameOver;

    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(true);
        OnScoreIncrease += IncreaseScore;
        OnScoreIncrease += UpdateScoreText;
        OnGameOver += DisableScoreCanvas;
        OnGameOver += ShowGameOverScreen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IncreaseScore()
    {
        _score++;
    }

    private void UpdateScoreText()
    {
        scoreText.text = _score.ToString();
    }

    private void ShowGameOverScreen()
    {
        gameOverCanvas.gameObject.SetActive(true);
        gameOverScoreText.text = _score.ToString();
    }

    private void DisableScoreCanvas()
    {
        scoreCanvas.gameObject.SetActive(false);
    }

    private void OnDestroy() 
    {
        OnScoreIncrease -= IncreaseScore;
        OnScoreIncrease -= UpdateScoreText;    
        OnGameOver -= DisableScoreCanvas;
        OnGameOver -= ShowGameOverScreen;
    }
}
