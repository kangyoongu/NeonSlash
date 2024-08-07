using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public bool isGamePlaying = false;

    float _leftTime = 300f;
    int earnMoney = 0;
    int gameScore = 0;

    public Action OnGameStart;
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
            PlayerPrefs.SetInt("Money", 999999999);

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
            UIManager.Instance.SetLeftTime(_leftTime);
        }
    }

    private void Clear()
    {
        SoundManager.Instance.PlayAudio(Clips.Clear);
        _leftTime = 0;
        isGamePlaying = false;
        Money += earnMoney;
        ApplyScore();
        UIManager.Instance.EndingUIIn(true);
    }

    public void GameStart()
    {
        isGamePlaying = true;
        earnMoney = 0;
        gameScore = 0;
        UIManager.Instance.SetEarnMoney(earnMoney);
        UIManager.Instance.SetGameScore(gameScore);
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
        SceneManager.LoadScene(gameObject.scene.name);
    }
    public void AddEarnMoney(int value)
    {
        earnMoney += value;
        UIManager.Instance.SetEarnMoney(earnMoney);
    }
    public void AddGameScore(int value)
    {
        gameScore += value;
        UIManager.Instance.SetGameScore(gameScore);
    }

    public void ApplyScore()
    {
        if(gameScore >= PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", gameScore);
            UIManager.Instance.SetBestText(gameScore);
        }
    }
}
