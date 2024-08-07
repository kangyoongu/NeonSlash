using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingleTon<ItemManager>
{
    [SerializeField] private List<ItemSO> _items;
    public Dictionary<ItemCategory, List<ItemController>> categoryCtrls = new();

    private List<ItemController> _totalList = new List<ItemController>();
    public Color levelOnColor;
    public Color[] colors;

    public Action<PlayerStat> OnUpgradePlayer;
    public Action<SkillStat> OnUpgradeSkill;
    public Action<CrystalStat> OnUpgradeCrystal;
    public Action OnResetPlayer;
    public Action OnResetSkill;
    public Action OnResetCrystal;
    private void Start()
    {
        categoryCtrls[ItemCategory.Player] = new List<ItemController>();
        categoryCtrls[ItemCategory.Enemy] = new List<ItemController>();
        categoryCtrls[ItemCategory.Skill] = new List<ItemController>();
        categoryCtrls[ItemCategory.Crsytal] = new List<ItemController>();
        foreach(ItemSO item in _items)
        {
            ItemController itemCtrl = UIManager.Instance.MakeItem(item);
            categoryCtrls[item.itemCategory].Add(itemCtrl);

            int level = 0;
            if (JsonManager.Instance.dataIsNull == false)
            {
                switch (item.itemCategory)
                {
                    case ItemCategory.Player:
                        level = JsonManager.Instance._gameData.playerLevel[categoryCtrls[ItemCategory.Player].IndexOf(itemCtrl)];
                        break;
                    case ItemCategory.Enemy:
                        level = Core.IsBitSet(JsonManager.Instance._gameData.enemyBool, categoryCtrls[ItemCategory.Enemy].IndexOf(itemCtrl)) ? 1 : 0;
                        break;
                    case ItemCategory.Skill:
                        level = JsonManager.Instance._gameData.SkillLevel[categoryCtrls[ItemCategory.Skill].IndexOf(itemCtrl)];
                        break;
                    case ItemCategory.Crsytal:
                        level = JsonManager.Instance._gameData.crystalLevel[categoryCtrls[ItemCategory.Crsytal].IndexOf(itemCtrl)];
                        break;
                }
            }
            LevelUpAction(item.itemCategory, itemCtrl.GetLevel(), level, itemCtrl);
            itemCtrl.SetLevel(level);
        }
        _totalList.AddRange(categoryCtrls[ItemCategory.Player]);
        _totalList.AddRange(categoryCtrls[ItemCategory.Enemy]);
        _totalList.AddRange(categoryCtrls[ItemCategory.Skill]);
        _totalList.AddRange(categoryCtrls[ItemCategory.Crsytal]);
        EnemyManager.Instance.SetUnlockData();
    }

    private void LevelUpAction(ItemCategory category, int originLevel, int upLevel, ItemController item)
    {
        if (upLevel == 0) return;
        for (int i = originLevel; i < upLevel; i++)
        {
            if (category == ItemCategory.Player)
                OnUpgradePlayer?.Invoke(item.itemSO.upgradePerLevel[i].player.changeStat);
            else if (category == ItemCategory.Skill)
                OnUpgradeSkill?.Invoke(item.itemSO.upgradePerLevel[i].skill.changeStat);
            else if (category == ItemCategory.Crsytal)
                OnUpgradeCrystal?.Invoke(item.itemSO.upgradePerLevel[i].crystal.changeStat);

        }
    }

    public void OnClickBuy(ItemController currentItem)
    {
        SoundManager.Instance.PlayAudio(Clips.Upgrade);
        switch (currentItem.itemSO.itemCategory)
        {
            case ItemCategory.Player:
                OnUpgradePlayer?.Invoke(currentItem.itemSO.upgradePerLevel[currentItem.GetLevel()].player.changeStat);
                JsonManager.Instance._gameData.playerLevel[categoryCtrls[ItemCategory.Player].IndexOf(currentItem)]++;
                break;

            case ItemCategory.Enemy:
                EnemyManager.Instance.UnlockEnemy(currentItem.itemSO.upgradePerLevel[currentItem.GetLevel()].enemy.unlockEnemy);
                Core.SetBit(ref JsonManager.Instance._gameData.enemyBool, categoryCtrls[ItemCategory.Enemy].IndexOf(currentItem), true);
                break;

            case ItemCategory.Skill:
                OnUpgradeSkill?.Invoke(currentItem.itemSO.upgradePerLevel[currentItem.GetLevel()].skill.changeStat);
                JsonManager.Instance._gameData.SkillLevel[categoryCtrls[ItemCategory.Skill].IndexOf(currentItem)]++;
                break;
            case ItemCategory.Crsytal:
                OnUpgradeCrystal?.Invoke(currentItem.itemSO.upgradePerLevel[currentItem.GetLevel()].crystal.changeStat);
                JsonManager.Instance._gameData.crystalLevel[categoryCtrls[ItemCategory.Crsytal].IndexOf(currentItem)]++;
                break;
        }
        JsonManager.Instance.SaveGameData();

        currentItem.SetLevel(currentItem.GetLevel() + 1);
    }

    public void OnClickReset(int resetPrice)
    {
        int total = GetTotalUsedMoney();
        if (GameManager.Instance.Money >= resetPrice && total > 0)
        {
            SoundManager.Instance.PlayAudio(Clips.Button);
            //돈 계산
            GameManager.Instance.Money += total - resetPrice;
            foreach (ItemController item in _totalList)
            {
                item.SetLevel(0);
            }

            //스킬들의 SO Reset;
            OnResetPlayer?.Invoke();
            OnResetSkill?.Invoke();
            OnResetCrystal?.Invoke();
            EnemyManager.Instance.ResetUnlockData();

            JsonManager.Instance.ResetGameData();
        }
        else
        {
            SoundManager.Instance.PlayAudio(Clips.Cancel);
        }
    }
    public int GetTotalUsedMoney()
    {
        int usedMoney = 0;
        foreach (ItemController item in _totalList)
        {
            usedMoney += item.GetUsedMoney();
        }
        return usedMoney;
    }
}
