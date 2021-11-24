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
    }

    #endregion

    [Header("Save Data Settings")]
    [SerializeField] string fileName = "data.json";
    [SerializeField] int highscoreCount = 1;
    string filePath;

    SaveData currentData;

    private void Start()
    {
        filePath = Application.dataPath + "/" + fileName;

        Load();
    }

    public SaveData CurrentSaveData
    {
        get { return currentData; }
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
        currentData = new SaveData(highscoreCount);
        Save();
    }
}

[System.Serializable]
public class SaveData
{
    float[] highscores;

    public float[] Highscores
    {
        get { return highscores; }
        set
        {
            highscores = value;
            SaveDataManager.instance.Save();
        }
    }

    public SaveData(int highscoreCount = 1)
    {
        highscores = new float[highscoreCount];
    }

    public void SetHighscores(float[] scores)
    {
        scores.CopyTo(highscores, 0);
        SaveDataManager.instance.Save();
    }
}