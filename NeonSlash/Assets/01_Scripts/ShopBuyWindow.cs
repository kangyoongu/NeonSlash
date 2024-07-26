using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyWindow : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Outline _outline;
    [SerializeField] private Image[] _level;

    [Header("Price")]
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Image _priceBackground;
    [SerializeField] private Color _priceOnColor;
    [SerializeField] private Color _priceOffColor;

    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _dialog;

    int currentPrice = 0;
    ItemController currentItem;

    public void BuyWindowEnable(Color outColor, ItemController itemCtrl)
    {
        currentItem = itemCtrl;

        _itemName.text = itemCtrl.itemSO.itemName;
        _icon.sprite = itemCtrl.itemSO.icon;
        _outline.effectColor = outColor;
        _dialog.text = itemCtrl.itemSO.dialog;

        for (int i = 0; i < 5; i++)
        {
            if (i >= itemCtrl.itemSO.maxLevel)
                _level[i].gameObject.SetActive(false);
            else
                _level[i].gameObject.SetActive(true);
        }
        SetLevel();
        gameObject.SetActive(true);
    }
    public void BuyWindowEnable(int currentLevel, Color outColor, ItemSO itemSO)
    {
        _itemName.text = itemSO.itemName;
        _icon.sprite = itemSO.icon;
        _outline.effectColor = outColor;
        _dialog.text = itemSO.dialog;

        for (int i = 0; i < 5; i++)
        {
            if (i < currentLevel)
                _level[i].color = ItemManager.Instance.levelOnColor;

            if (i >= itemSO.maxLevel)
                _level[i].gameObject.SetActive(false);
            else
                _level[i].gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }

    public void OnClickBuy()
    {
        if(currentPrice > GameManager.Instance.Money)
        {
            return;
        }
        if(currentItem.GetLevel() >= currentItem.itemSO.maxLevel)
        {
            return;
        }
        GameManager.Instance.Money -= currentPrice;
        currentItem.SetLevel(currentItem.GetLevel() + 1);
        SetLevel();
    }
    private void SetLevel()
    {
        for (int i = 0; i < _level.Length; i++)
        {
            if(i < currentItem.GetLevel())
                _level[i].color = ItemManager.Instance.levelOnColor;
            else 
                _level[i].color = new Color32(214, 212, 203, 255);
        }
        currentPrice = currentItem.GetPrice();


        if (currentItem.GetLevel() >= currentItem.itemSO.maxLevel)
        {
            _priceBackground.color = _priceOffColor;
            _price.text = "최대레벨입니다";
        }
        else if (currentPrice > GameManager.Instance.Money)
        {
            _priceBackground.color = _priceOffColor;
            _price.text = $"구매 ({currentPrice}원)";
        }
        else
        {
            _priceBackground.color = _priceOnColor;
            _price.text = $"구매 ({currentPrice}원)";
        }
    }
}
