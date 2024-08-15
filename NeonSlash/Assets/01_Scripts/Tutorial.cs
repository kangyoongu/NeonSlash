using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Kang;
public class Tutorial : SingleTon<Tutorial>
{
    [SerializeField] private RectTransform focus;
    [SerializeField] private TextMeshProUGUI tutText;
    [SerializeField] private GameObject tutRoot;
    [SerializeField] private RectTransform shop;

    [SerializeField] private RectTransform enemyTab;
    [SerializeField] private Kang.TabButton enemyTabButton;
    public RectTransform enemyUp;
    [SerializeField] private RectTransform upgrade;
    [SerializeField] private Button upgradeBtn;

    private Player player;

    [SerializeField] private float textTime = 0.1f;
    [SerializeField] private float textDelay = 0.8f;

    [HideInInspector] public bool endTut = false;
    public Queue<string> texts = new Queue<string>();

    [TextArea]
    [SerializeField] private string[] gameStartText;
    [TextArea]
    [SerializeField] private string[] pointUpgradeDialog;
    [TextArea]
    [SerializeField] private string[] aboutDie;
    [TextArea]
    [SerializeField] private string[] onShop;
    [TextArea]
    [SerializeField] private string[] ending;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        if (!PlayerPrefs.HasKey("Tut"))
        {
            PlayerPrefs.SetInt("Tut", 0);
        }

        if (PlayerPrefs.GetInt("Tut") >= 10)
        {
            tutRoot.SetActive(false);
            gameObject.SetActive(false);
            endTut = true;
        }
        else if(PlayerPrefs.GetInt("Tut") == 5)
        {
            StartCoroutine(Core.FrameDelay(() => SetFocus(shop)));
        }
        else
        {
            PlayerPrefs.SetInt("Tut", 0);
            endTut = false;
        }
    }
    private void Start()
    {
        GameManager.Instance.OnGameStart += GameStart;
    }
    public void GameStart()
    {
        if(PlayerPrefs.GetInt("Tut") < 5)
        {
            foreach (string item in gameStartText)
                texts.Enqueue(item);
            StartQueueString(true);
        }
    }
    public void SetFocus(RectTransform target)
    {
        focus.position = target.position;
        focus.sizeDelta = target.sizeDelta;
        focus.gameObject.SetActive(true);
    }
    public void OnClickShop()
    {
        if (PlayerPrefs.GetInt("Tut") == 5)
        {
            OffFocus();
            UIManager.Instance.freeBlock.SetActive(true);

            foreach (string item in onShop)
                texts.Enqueue(item);
            StartQueueString();
        }
    }
    public void OffFocus()
    {
        focus.gameObject.SetActive(false);
    }
    public void StartQueueString(bool notClear = false)
    {
        StartCoroutine(TypingText(notClear));
    }
    IEnumerator TypingText(bool notClear)
    {
        while (texts.Count > 0)
        {
            tutText.text = "";
            string text = texts.Dequeue();
            foreach (char c in text)
            {
                tutText.text += c;
                yield return new WaitForSecondsRealtime(textTime);
            }
            yield return new WaitForSecondsRealtime(textDelay);
        }
        if (!notClear)
            tutText.text = "";
        PlayerPrefs.SetInt("Tut", PlayerPrefs.GetInt("Tut") + 1);
        if (PlayerPrefs.GetInt("Tut") == 3)
        {
            GameManager.Instance.AddPoint(5);
            SoundManager.Instance.PlayAudio(Clips.PutMoney);
        }
        else if (PlayerPrefs.GetInt("Tut") == 6)
        {
            GameManager.Instance.ApplyMoney(20000);
            SoundManager.Instance.PlayAudio(Clips.PutMoney);

            UIManager.Instance.freeBlock.SetActive(false);
            SetFocus(enemyTab);
            enemyTabButton.OnClick += () =>
            {
                if (PlayerPrefs.GetInt("Tut") == 6)
                    SetFocus(enemyUp);
            };
            upgradeBtn.onClick.AddListener(() =>
            {
                if (PlayerPrefs.GetInt("Tut") == 6)
                {
                    PlayerPrefs.SetInt("Tut", PlayerPrefs.GetInt("Tut") + 1);
                    OffFocus();
                    foreach (string item in ending)
                        texts.Enqueue(item);
                    StartQueueString();
                }
            });
        }
        else if (PlayerPrefs.GetInt("Tut") == 8)
        {
            EndTutorial();
        }
    }
    public void OnClickEnemyUp()
    {
        if (PlayerPrefs.GetInt("Tut") == 6)
            SetFocus(upgrade);
    }
    private void Update()
    {
        print(PlayerPrefs.GetInt("Tut"));
        if (PlayerPrefs.GetInt("Tut") == 3 && Time.timeScale == 1f)
        {
            player.TakeDamage(player.CurrentHp - 1);
            foreach (string item in aboutDie)
                texts.Enqueue(item);
            StartQueueString();
            PlayerPrefs.SetInt("Tut", PlayerPrefs.GetInt("Tut") + 1);
        }
    }
    public void ClickTab()
    {
        if (PlayerPrefs.GetInt("Tut") == 1)
        {
            foreach (string item in pointUpgradeDialog)
                texts.Enqueue(item);
            StartQueueString();
            PlayerPrefs.SetInt("Tut", PlayerPrefs.GetInt("Tut") + 1);
        }
    }
    public void EndTutorial()
    {
        PlayerPrefs.SetInt("Tut", 10);
        tutRoot.SetActive(false);
        endTut = true;
    }
}
