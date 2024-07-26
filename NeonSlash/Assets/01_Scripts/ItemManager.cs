using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingleTon<ItemManager>
{
    [SerializeField] private List<ItemSO> _items;
    private List<ItemController> _itemCtrls = new List<ItemController>();

    public Color levelOnColor;
    public Color[] colors;

    private void Start()
    {
        foreach(ItemSO item in _items)
        {
            _itemCtrls.Add(UIManager.Instance.MakeItem(item));
        }
    }

    public void OnClickReset(int resetPrice)
    {
        GameManager.Instance.Money += GetTotalUsedMoney() - resetPrice;
        foreach(ItemController item in _itemCtrls)
        {
            item.SetLevel(0);
        }
    }
    public int GetTotalUsedMoney()
    {
        int usedMoney = 0;
        foreach (ItemController item in _itemCtrls)
        {
            usedMoney += item.GetUsedMoney();
        }
        return usedMoney;
    }
}
