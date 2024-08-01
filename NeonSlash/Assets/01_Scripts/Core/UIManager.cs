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

    [Header("Game")]
    public GameObject gamePauseUI;
    public TextMeshProUGUI endingText;
    public TextMeshProUGUI leftTimeText;
    public void SetLeftTime(float second) => leftTimeText.text = $"{second.ToString("0")}초";
    public TextMeshProUGUI[] earnMoneyText;
    public TextMeshProUGUI[] gameScoreText;

    public TextMeshProUGUI[] bestTexts;

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

    public void MainUIIn() => In(mainUI);
    public void MainUIOut() => Out(mainUI);

    public void PlayUIIn() => In(playUI);
    public void PlayUIOut() => Out(playUI);

    public void ShopUIIn()
    {
        option = true;
        if (GameManager.Instance.isGamePlaying)
            resetButton.SetActive(false);
        else
            resetButton.SetActive(true);
        In(shopUI);
    }
    public void ShopUIOut()
    {
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

    public void TogglePause()
    {
        if (GameManager.Instance.isGamePlaying && !option)
        {
            gamePauseUI.SetActive(!gamePauseUI.activeSelf);
        }
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

    public void SetHP(int currentHp, int maxHp)
    {
        float hpPercent = (float)currentHp / maxHp;
        hpBar.fillAmount = hpPercent;
        hpText.text = $"{currentHp} / {maxHp}";
    }
}
