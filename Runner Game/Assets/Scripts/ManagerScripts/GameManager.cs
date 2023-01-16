using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : GenericSingleton<GameManager>
{

    private Canvas _scoreCanvas;
    private TMP_Text _scoreText;
    
    private Canvas _gameOverCanvas;
    private TMP_Text _gameOverScoreText;

    private ScreenUIManager _uiManager;
    private int _score = 0;

    private bool _isGameEnded;

    public UnityEvent OnGameStart;
    public UnityEvent OnScoreIncrease;
    public UnityEvent OnGameOver;

    public bool IsGameEnded { get => _isGameEnded; }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initialize();
    }


    private void Initialize()
    {
        Debug.Log("Initializing");
        
        OnGameStart.RemoveAllListeners();
        OnScoreIncrease.RemoveAllListeners();
        OnGameOver.RemoveAllListeners();
        
        _scoreCanvas = GameObject.Find("Score Canvas").GetComponent<Canvas>();
        _scoreText = GameObject.Find("Score Text").GetComponent<TMP_Text>();
        
        _gameOverCanvas = GameObject.Find("Game Over Canvas").GetComponent<Canvas>();
        _gameOverScoreText = GameObject.Find("Game Over Score Text").GetComponent<TMP_Text>();

        _uiManager = GameObject.Find("ScreenUIManager").GetComponent<ScreenUIManager>();

        _scoreCanvas.gameObject.SetActive(true);
        
        OnGameStart.AddListener(()=>_uiManager.Open(_scoreCanvas));
        OnGameOver.AddListener(()=> _uiManager.Open(_gameOverCanvas));

        OnScoreIncrease.AddListener(IncreaseScore);
        OnScoreIncrease.AddListener(UpdateScoreText);
        OnGameOver.AddListener(SetGameEndedTrue);

        SoundManager.Instance?.PlayMusic();

        _isGameEnded = false;
        _score = 0;
        OnGameStart?.Invoke();
    }

    
    private void IncreaseScore()
    {
        _score++;
    }

    private void UpdateScoreText()
    {
        _scoreText.text = _score.ToString();
    }

    private void ShowGameOverScreen()
    {
        _gameOverScoreText.text = _score.ToString();
    }

    private void SetGameEndedTrue()
    {
        _isGameEnded = true;
    }

    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        Initialize();
    }
    private void OnDestroy() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnGameStart.RemoveAllListeners();
        OnScoreIncrease.RemoveAllListeners();
        OnGameOver.RemoveAllListeners();
    }
}
