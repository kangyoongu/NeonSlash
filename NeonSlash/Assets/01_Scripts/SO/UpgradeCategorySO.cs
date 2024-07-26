using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeCategory : ScriptableObject
{

}


[CreateAssetMenu(fileName = "UpgradePlayer", menuName = "SO/Category/Player")]
public class UpgradePlayerSO : UpgradeCategory
{

}


[CreateAssetMenu(fileName = "UpgradeEnemy", menuName = "SO/Category/Enemy")]
public class UpgradeEnemySO : UpgradeCategory
{

}

[CreateAssetMenu(fileName = "UpgradeSkill", menuName = "SO/Category/Skill")]
public class UpgradeSkillSO : UpgradeCategory
{

}

[CreateAssetMenu(fileName = "UpgradeCrystal", menuName = "SO/Category/Crystal")]
public class UpgradeCrystalSO : UpgradeCategory
{

}