using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum Dir : short
{
    x,
    y
}
[Serializable]
public struct UI
{
    public RectTransform changeUI;
    public Image fadeUI;
    public TextMeshProUGUI fadeText;
    public Dir dir;
    public Vector2 inAndOut;
    public float time;
    public bool setActive;
    public float fadeFloat;
}
public class UIManager : SingleTon<UIManager>
{
    [Header("UIs")]
    public UI[] mainUI;
    public UI[] playUI;
    public UI[] shopUI;
    public UI[] endingUI;
    public UI[] settingUI;
    public GameObject block;
    public GameObject freeBlock;

    [Header("Game")]
    public GameObject gamePauseUI;
    public TextMeshProUGUI endingText;
    public TextMeshProUGUI leftTimeText;
    public TextMeshProUGUI[] earnMoneyText;
    public TextMeshProUGUI[] gameScoreText;
    public TextMeshProUGUI[] pointText;
    public TextMeshProUGUI[] bestTexts;
    public TextMeshProUGUI countDown;
    public TextMeshProUGUI newRecordText;

    [Header("Shopping")]
    public ShopBuyWindow upgradePanel;
    public ShopBuyWindow ingameUpgradePanel;
    public GameObject resetButton;
    bool option = false;

    [Header("Items")]
    public Transform[] itemParents;
    public GameObject itemPref;

    [Header("Money")]
    public TextMeshProUGUI[] moneyTexts;

    [Header("Hp")]
    public TextMeshProUGUI hpText;
    public Image hpBar;

    public IEnumerator CountDown()
    {
        for(int i = 5; i > 0; i--)
        {
            BoomString(i.ToString(), countDown);
            yield return new WaitForSeconds(1f);
        }
    }
    public void NewRecord()
    {
        BoomString("신기록", newRecordText, 1f);
    }
    private void BoomString(string text, TextMeshProUGUI textMesh, float delay = 0)
    {
        textMesh.transform.localScale = Vector3.zero;
        textMesh.gameObject.SetActive(true);
        textMesh.text = text;
        textMesh.transform.DOScale(Vector3.one, 0.49f).OnComplete(() => {

            textMesh.transform.DOScale(Vector3.zero, 0.49f).SetDelay(delay).OnComplete(() => {
                textMesh.gameObject.SetActive(false);
            });
        });
    }
    public void SetLeftTime(float second) => leftTimeText.text = $"{second.ToString("0")}초";
    public void MainUIIn() => In(mainUI);
    public void MainUIOut() => Out(mainUI);

    public void PlayUIIn() => In(playUI);
    public void PlayUIOut() => Out(playUI);

    public void ShopUIIn(bool withKey = false)
    {
        if (GameManager.Instance.isGamePlaying)
            resetButton.SetActive(false);
        else
            resetButton.SetActive(true);

        if (withKey == false || (withKey && GameManager.Instance.isGamePlaying)){
            option = true;
            In(shopUI);
        }
    }
    public void ShopUIOut()
    {
        if (gamePauseUI.activeSelf == false)
            Time.timeScale = 1f;
        option = false;
        Out(shopUI);
    }

    public void EndingUIIn(bool isClear)
    {
        endingText.text = isClear ? "클리어" : "실패";
        In(endingUI);
    }
    public void EndingUIOut()
    {
        Out(endingUI);
    }

    public void SettingUIIn()
    {
        option = true;
        In(settingUI);
    }
    public void SettingUIOut()
    {
        option = false;
        Out(settingUI);

    }
    public void TogglePause()
    {
        if (GameManager.Instance.isGamePlaying && !option)
        {
            SoundManager.Instance.PlayAudio(Clips.Button);
            gamePauseUI.SetActive(!gamePauseUI.activeSelf);
        }
    }



    private void In(UI[] lst)
    {
        block.SetActive(true);
        float max = 0;
        for (int i = 0; i < lst.Length; i++)
        {
            if (max < lst[i].time) max = lst[i].time;
            if (lst[i].changeUI != null)
            {
                if (lst[i].setActive) lst[i].changeUI.gameObject.SetActive(true);
                if (lst[i].dir == Dir.y) lst[i].changeUI.DOAnchorPosY(lst[i].inAndOut.x, lst[i].time).SetEase(Ease.Linear).SetUpdate(true);
                else lst[i].changeUI.DOAnchorPosX(lst[i].inAndOut.x, lst[i].time).SetEase(Ease.Linear).SetUpdate(true);
            }
            else if (lst[i].fadeUI != null)
            {
                if (lst[i].setActive) lst[i].fadeUI.gameObject.SetActive(true);
                lst[i].fadeUI.DOFade(lst[i].fadeFloat / 255f, lst[i].time).SetEase(Ease.Linear).SetUpdate(true);
            }
            else
            {
                if (lst[i].setActive) lst[i].changeUI.gameObject.SetActive(true);
                lst[i].fadeText.DOFade(lst[i].fadeFloat / 255f, lst[i].time).SetEase(Ease.Linear).SetUpdate(true);
            }
        }
        StartCoroutine(BlockTime(max));
    }

    private void Out(UI[] lst)
    {
        block.SetActive(true);
        float max = 0;
        for (int i = 0; i < lst.Length; i++)
        {
            if (max < lst[i].time) max = lst[i].time;
            int index = i;
            if (lst[i].changeUI != null)
            {
                if (lst[i].dir == Dir.y)
                {
                    lst[i].changeUI.DOAnchorPosY(lst[i].inAndOut.y, lst[i].time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                    {
                        if (lst[index].setActive) lst[index].changeUI.gameObject.SetActive(false);
                    });
                }
                else
                {
                    lst[i].changeUI.DOAnchorPosX(lst[i].inAndOut.y, lst[i].time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                    {
                        if (lst[index].setActive) lst[index].changeUI.gameObject.SetActive(false);
                    });
                }
            }
            else if (lst[i].fadeUI != null)
            {
                lst[i].fadeUI.DOFade(0, lst[i].time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    if (lst[index].setActive) lst[index].fadeUI.gameObject.SetActive(false);
                });
            }
            else
            {
                lst[i].fadeText.DOFade(0, lst[i].time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    if (lst[index].setActive) lst[index].changeUI.gameObject.SetActive(false);
                });
            }
        }
        StartCoroutine(BlockTime(max));
    }
    IEnumerator BlockTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        block.SetActive(false);
    }

    public ItemController MakeItem(ItemSO item)
    {
        GameObject copy = Instantiate(itemPref.gameObject, itemParents[(int)item.itemCategory]);
        ItemController itemCtrl = copy.GetComponent<ItemController>();
        itemCtrl.Init(item);
        return itemCtrl;
    }

    public void SetMoneyText(int money)
    {
        foreach(TextMeshProUGUI text in moneyTexts)
        {
            text.text = $"골드 : {money}G";
        }
    }
    public void SetBestText(int score)
    {
        foreach (TextMeshProUGUI text in bestTexts)
        {
            text.text = $"최고 점수 : {score}";
        }
    }

    public void SetEarnMoney(int value)
    {
        foreach (TextMeshProUGUI text in earnMoneyText)
        {
            text.text = $"골드 : {value}G";
        }
    }
    public void SetGameScore(int value)
    {
        foreach (TextMeshProUGUI text in gameScoreText)
        {
            text.text = $"점수 : {value}";
        }
    }
    public void SetPointText(int value)
    {
        if (GameManager.Instance.isGamePlaying)
        {
            foreach (TextMeshProUGUI text in pointText)
            {
                text.text = $"포인트 : {value}";
            }
        }
    }

    public void SetHP(float currentHp, int maxHp)
    {
        float hpPercent = currentHp / maxHp;
        hpBar.fillAmount = hpPercent;
        hpText.text = $"{currentHp.ToString("0")} / {maxHp}";
    }
}
