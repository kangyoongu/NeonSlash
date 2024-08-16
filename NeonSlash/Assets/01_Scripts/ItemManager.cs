using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : SingleTon<ItemManager>
{
    GameData _gameData;
    [SerializeField] private List<ItemSO> _items;
    public Dictionary<ItemCategory, List<ItemController>> categoryCtrls = new();

    private List<ItemController> _totalList;
    public Color levelOnColor;
    public Color disableColor;
    public Color pointColor;
    public Color[] colors;

    public Action<PlayerStat> OnUpgradePlayer;
    public Action<SkillStat> OnUpgradeSkill;
    public Action<CrystalStat> OnUpgradeCrystal;
    public Action OnResetPlayer;
    public Action OnResetSkill;
    public Action OnResetCrystal;
    private void Start()
    {
        LoadGameData();
    }
    private void LoadGameData()
    {
        _gameData = JsonManager.Instance._gameData;
        categoryCtrls[ItemCategory.Player] = new List<ItemController>();
        categoryCtrls[ItemCategory.Enemy] = new List<ItemController>();
        categoryCtrls[ItemCategory.Skill] = new List<ItemController>();
        categoryCtrls[ItemCategory.Crsytal] = new List<ItemController>();
        _totalList = new List<ItemController>();
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
                        level = _gameData.playerLevel[categoryCtrls[ItemCategory.Player].IndexOf(itemCtrl)];
                        break;
                    case ItemCategory.Enemy:
                        {
                            int index = categoryCtrls[ItemCategory.Enemy].IndexOf(itemCtrl);
                            level = Core.IsBitSet(_gameData.enemyBool, index) ? 1 : 0;

                            if (index == 0 && Tutorial.Instance) {
                                Tutorial.Instance.enemyUp = itemCtrl.GetComponent<RectTransform>();
                                itemCtrl.GetComponent<Button>().onClick.AddListener(Tutorial.Instance.OnClickEnemyUp);
                            }
                            break;
                        }
                    case ItemCategory.Skill:
                        level = _gameData.SkillLevel[categoryCtrls[ItemCategory.Skill].IndexOf(itemCtrl)];
                        break;
                    case ItemCategory.Crsytal:
                        level = _gameData.crystalLevel[categoryCtrls[ItemCategory.Crsytal].IndexOf(itemCtrl)];
                        break;
                }
            }
            LevelUpAction(item.itemCategory, itemCtrl.GetLevel(), level, itemCtrl);
            itemCtrl.SetLevel(level, 0);
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

    public void OnClickBuy(ItemController currentItem, bool save = true)
    {
        ItemSO.UpgradeInfo upgradeInfo = currentItem.itemSO.upgradePerLevel[currentItem.GetTotalLevel()];

        SoundManager.Instance.PlayAudio(Clips.Upgrade);
        switch (currentItem.itemSO.itemCategory)
        {
            case ItemCategory.Player:
                OnUpgradePlayer?.Invoke(upgradeInfo.player.changeStat);
                if(save) _gameData.playerLevel[categoryCtrls[ItemCategory.Player].IndexOf(currentItem)]++;
                break;

            case ItemCategory.Enemy:
                EnemyManager.Instance.UnlockEnemy(upgradeInfo.enemy.unlockEnemy);
                if (save) Core.SetBit(ref _gameData.enemyBool, categoryCtrls[ItemCategory.Enemy].IndexOf(currentItem), true);
                break;

            case ItemCategory.Skill:
                OnUpgradeSkill?.Invoke(upgradeInfo.skill.changeStat);
                if (save) _gameData.SkillLevel[categoryCtrls[ItemCategory.Skill].IndexOf(currentItem)]++;
                break;

            case ItemCategory.Crsytal:
                OnUpgradeCrystal?.Invoke(upgradeInfo.crystal.changeStat);
                if (save) _gameData.crystalLevel[categoryCtrls[ItemCategory.Crsytal].IndexOf(currentItem)]++;
                break;
        }
        if (save) JsonManager.Instance.SaveGameData();

        if(save) 
            currentItem.SetLevel(currentItem.GetLevel() + 1, currentItem.GetPoint());
        else 
            currentItem.SetLevel(currentItem.GetLevel(), currentItem.GetPoint() + 1);
    }

    public void OnClickReset(int resetPrice)
    {
        int total = GetTotalUsedMoney();
        if (GameManager.Instance.Money >= resetPrice && total > 0)
        {
            SoundManager.Instance.PlayAudio(Clips.Button);
            //스킬들의 SO Reset;
            OnResetPlayer?.Invoke();
            OnResetSkill?.Invoke();
            OnResetCrystal?.Invoke();
            EnemyManager.Instance.ResetUnlockData();
            JsonManager.Instance.ResetGameData();
            _gameData = JsonManager.Instance._gameData;
            //돈 계산
            GameManager.Instance.Money += total - resetPrice;
            foreach (ItemController item in _totalList)
            {
                item.SetLevel(0, 0);
            }

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

    public void SetLevelColor(Image[] level, ItemController item)
    {
        for (int i = 0; i < item.itemSO.maxLevel; i++)
        {
            if (i < item.GetLevel())
                level[i].color = levelOnColor;
            else if (i < item.GetTotalLevel())
                level[i].color = pointColor;
            else
                level[i].color = disableColor;
        }
    }
}
