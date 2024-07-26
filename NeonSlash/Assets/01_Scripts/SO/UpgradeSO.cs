using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeStat", menuName = "SO/UpgradeStat")]
public class UpgradeSO : ScriptableObject
{
    public List<UpgradeInfo> upgradePerLevel;
}

[Serializable]
public struct UpgradeInfo
{
    public ItemCategory category;
    public UpgradeCategory so;
}

