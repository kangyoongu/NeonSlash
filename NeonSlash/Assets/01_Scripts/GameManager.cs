using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public bool isGamePlaying = false;

    public Action OnGameStart;
    private int _money = 999999;
    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            UIManager.Instance.SetMoneyText(_money);
        }
    }
    private void Start()
    {
        UIManager.Instance.SetMoneyText(_money);
    }
    public void GameStart()
    {
        isGamePlaying = true;
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
}
