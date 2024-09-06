using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    #region Singleton

    public static SaveDataManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        filePath = Path.Combine(Application.persistentDataPath, fileName);
        //filePath = Application.dataPath + "/" + fileName;
        Load();
    }

    #endregion

    [Header("Save Data Settings")]
    [SerializeField] string fileName = "cactus.json";
    string filePath;

    SaveData currentData;

    public SaveData CurrentSaveData
    {
        get 
        { 
            if(currentData == null)
            {
                Initialize();
            }

            return currentData; 
        }
    }

    public void Save()
    {
        string saveDataString = JsonUtility.ToJson(currentData);
        File.WriteAllText(filePath, saveDataString);
    }

    public void Load()
    {
        if (File.Exists(filePath))
        {
            string saveDataString = File.ReadAllText(filePath);
            currentData = JsonUtility.FromJson<SaveData>(saveDataString);
        }
        else
            Initialize();     
    }

    public void Clear()
    {
        currentData = new SaveData();
        Save();
    }

    public void Initialize()
    {
        currentData = new SaveData();
        Save();
    }
}

[System.Serializable]
public class SaveData
{
    [SerializeField]
    float highscore;

    [SerializeField]
    bool muteEffects = false;
    [SerializeField]
    bool muteMusic =false;

    public float Highscore
    {
        get { return highscore; }
        set
        {
            highscore = value;
            SaveDataManager.instance.Save();
        }
    }

    public bool MuteMusic
    {
        get { return muteMusic; }
        set
        {
            muteMusic = value;
            SaveDataManager.instance.Save();
        }
    }

    public bool MuteEffects
    {
        get { return muteEffects; }
        set
        {
            muteEffects = value;
            SaveDataManager.instance.Save();
        }
    }

    public SaveData()
    {
        highscore = 0;

        //SaveDataManager.instance.Save();
    }

    public void SetHighscore(float score)
    {
        highscore = score;
        SaveDataManager.instance.Save();
    }
}