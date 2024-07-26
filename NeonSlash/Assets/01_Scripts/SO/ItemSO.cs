using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory
{
    Player = 0,
    Enemy,
    Skill,
    Crsytal
}

[CreateAssetMenu(fileName = "Item", menuName = "SO/Item")]
public class ItemSO : ScriptableObject
{
    public ItemCategory itemCategory;
    public Sprite icon;
    [TextArea]
    public string dialog;
    public string itemName;
    public int maxLevel;
    public int firstCost;
    public int addCost;

    public UpgradeSO upgradeSO;
}
