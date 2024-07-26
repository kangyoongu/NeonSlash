using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{

    [HideInInspector] public ItemSO itemSO;
    int _currentLevel = 0;
    public int GetLevel() => _currentLevel;

    private Outline _outline;
    private Image _icon;
    private TextMeshProUGUI _nameText;
    private Image[] _level;
    private Color _color;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickThisItem);
    }


    public void Init(ItemSO itemSO)
    {
        this.itemSO = itemSO;

        Transform levelParent = transform.GetChild(2);
        _level = new Image[levelParent.childCount];

        // 반복문을 통해 자식들을 배열에 저장
        for (int i = 0; i < levelParent.childCount; i++)
        {
            _level[i] = levelParent.GetChild(i).GetComponent<Image>();
            if(i >= this.itemSO.maxLevel)
                _level[i].gameObject.SetActive(false);
        }

        _outline = GetComponent<Outline>();
        _icon = transform.GetChild(0).GetComponent<Image>();
        _nameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        _color = ItemManager.Instance.colors[(int)this.itemSO.itemCategory];
        _outline.effectColor = _color;
        _icon.sprite = this.itemSO.icon;
        _nameText.text = this.itemSO.itemName;
        SetLevel(_currentLevel);
    }

    public int GetUsedMoney()
    {
        int usedMoney = 0;
        while(_currentLevel > 0)
        {
            usedMoney += itemSO.firstCost + (itemSO.addCost * (_currentLevel - 1));
            _currentLevel--;
        }
        return usedMoney;
    }

    public void SetLevel(int level)
    {
        _currentLevel = level;
        for (int i = 0; i < itemSO.maxLevel; i++)
        {
            if (i < _currentLevel)
                _level[i].color = ItemManager.Instance.levelOnColor;
            else
                _level[i].color = new Color32(214, 212, 203, 255);
        }
    }
    private void OnClickThisItem()
    {
        if (GameManager.Instance.isGamePlaying)
            UIManager.Instance.ingameUpgradePanel.BuyWindowEnable(_currentLevel, _color, itemSO);
        else
            UIManager.Instance.upgradePanel.BuyWindowEnable(_color, this);
    }
    public int GetPrice()
    {
        return itemSO.firstCost + (itemSO.addCost * _currentLevel);
    }
}
