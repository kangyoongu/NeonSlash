using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public bool isGamePlaying = false;

    float _leftTime = 300f;
    int _earnMoney = 0;
    int _gameScore = 0;
    public int Point { get; private set; }

    bool _countdown = false;
    bool _bestScore = false;
    public Action OnGameStart = null;
    public int Money
    {
        get => PlayerPrefs.GetInt("Money");
        set
        {
            PlayerPrefs.SetInt("Money", value);
            UIManager.Instance.SetMoneyText(PlayerPrefs.GetInt("Money"));
        }
    
    }
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Money"))
            PlayerPrefs.SetInt("Money", 0);

        if (!PlayerPrefs.HasKey("BestScore"))
            PlayerPrefs.SetInt("BestScore", 0);
    }
    private void Start()
    {
        UIManager.Instance.SetMoneyText(Money);
        UIManager.Instance.SetBestText(PlayerPrefs.GetInt("BestScore"));
    }

    private void Update()
    {
        if (isGamePlaying)
        {
            _leftTime -= Time.deltaTime;
            if(_leftTime <= 0)
            {
                Clear();
            }
            else if(_leftTime <= 5.5f && _countdown == false)
            {
                _countdown = true;
                StartCoroutine(UIManager.Instance.CountDown());
            }
            UIManager.Instance.SetLeftTime(_leftTime);
        }
    }

    private void Clear()
    {
        SoundManager.Instance.PlayAudio(Clips.Clear);
        _leftTime = 0;
        isGamePlaying = false;
        ApplyMoney();
        UIManager.Instance.EndingUIIn(true);
    }


    public void GameStart()
    {
        isGamePlaying = true;
        _earnMoney = 0;
        _gameScore = 0;
        Point = 0;
        UIManager.Instance.SetEarnMoney(_earnMoney);
        UIManager.Instance.SetGameScore(_gameScore);
        UIManager.Instance.SetPointText(Point);
        OnGameStart?.Invoke();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void ReloadScene()
    {
        if(PlayerPrefs.GetInt("Tut") >= 5)
            SceneManager.LoadScene(gameObject.scene.name);
    }
    public void AddEarnMoney(int value)
    {
        _earnMoney += value;
        UIManager.Instance.SetEarnMoney(_earnMoney);
    }
    public void AddGameScore(int value)
    {
        _gameScore += value;
        ApplyScore();
        UIManager.Instance.SetGameScore(_gameScore);
    }
    public void AddPoint(int value)
    {
        Point += value;
        UIManager.Instance.SetPointText(Point);
    }
    public void ApplyMoney()
    {
        Money += _earnMoney;
    }
    public void ApplyMoney(int value)
    {
        Money += value;
    }

    public void ApplyScore()
    {
        if(_gameScore > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", _gameScore);
            UIManager.Instance.SetBestText(_gameScore);
            if (PlayerPrefs.GetInt("BestScore") > 0 && _bestScore == false)
            {
                _bestScore = true;
                UIManager.Instance.NewRecord();
            }
        }
    }
}
