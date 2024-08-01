using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SettingData
{
    public float _SFX;
    public float _BGM;
    public int _resolution;
    public int _fullScreen;
}

[Serializable]
public class GameData
{
    public List<int> playerLevel;
    public int enemyBool;
    public List<int> SkillLevel;
    public List<int> crystalLevel;
}
public class JsonManager : SingleTon<JsonManager>
{
    [HideInInspector] public GameData _gameData;
    string _fileName;
    string _settingFile;

    private SettingData _settingData;

    //������Ƽ�� �ܺο��� �����ϸ� �ٷ� ����
    public float SFX { get { return _settingData._SFX; } set { _settingData._SFX = value; SaveData(_settingData, _settingFile); } }
    public float BGM { get { return _settingData._BGM; } set { _settingData._BGM = value; SaveData(_settingData, _settingFile); } }
    public int Resolution { get { return _settingData._resolution; } set { _settingData._resolution = value; SaveData(_settingData, _settingFile); } }
    public int FullScreen { get { return _settingData._fullScreen; } set { _settingData._fullScreen = value; SaveData(_settingData, _settingFile); } }

    [HideInInspector] public bool dataIsNull = false;

    private void Awake()
    {
        _fileName = Path.Combine(Application.persistentDataPath + "/ PlayData.json");
        _settingFile = Path.Combine(Application.persistentDataPath + "/ SettingData.json");
        if (File.Exists(_fileName))
            _gameData = LoadData<GameData>(_fileName);
        else
        {
            dataIsNull = true;
            ResetGameData();
        }

        if (File.Exists(_settingFile))
            _settingData = LoadData<SettingData>(_settingFile);
        else
            ResetSettingData();
    }

    public void ResetGameData()
    {
        _gameData = new GameData { playerLevel = new List<int>(new int[10]), enemyBool = 0, SkillLevel = new List<int>(new int[10]), crystalLevel = new List<int>(new int[10]) };
        SaveData(_gameData, _fileName);
    }
    public void ResetSettingData()
    {
        _settingData = new SettingData { _resolution = Screen.resolutions.Length - 1, _fullScreen = 0, _BGM = 1.401642f, _SFX = 1.401642f };
        SaveData(_settingData, _settingFile);
    }

    public void SaveGameData() => SaveData(_gameData, _fileName);

    public void SaveData<T>(T data, string fileName) where T : class
    {
        if (File.Exists(fileName))
            File.Delete(fileName);

        string json = JsonUtility.ToJson(data);

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        string encodedJson = Convert.ToBase64String(bytes);

        File.WriteAllText(fileName, encodedJson);
    }


    public T LoadData<T>(string fileName) where T : class
    {
        if (File.Exists(fileName))
        {
            string jsonFromFile = File.ReadAllText(fileName);

            byte[] bytes = Convert.FromBase64String(jsonFromFile);
            string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);

            return JsonUtility.FromJson<T>(decodedJson);
        }
        return null;
    }
}
