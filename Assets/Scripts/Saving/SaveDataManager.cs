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
    string filePath;

    [SerializeField] SaveData currentData;

    private void Start()
    {
        filePath = Application.dataPath + "/" + fileName;

        LoadData();
    }

    public void SaveData()
    {
        string saveDataString = JsonUtility.ToJson(currentData);
        File.WriteAllText(filePath, saveDataString);
    }
    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string saveDataString = File.ReadAllText(filePath);
            currentData = JsonUtility.FromJson<SaveData>(saveDataString);
            currentData.ClearData();
        }
        else
            InitializeSaveData();     
    }

    public void InitializeSaveData()
    {
        currentData = new SaveData();
        SaveData();
    }
    public float[] Highscores
    {
        get { return currentData.highscores; }
        set 
        { 
            currentData.highscores = value; 
            SaveData(); 
        }
    }
}

[System.Serializable]
public class SaveData
{
    public float[] highscores;

    public SaveData()
    {
        highscores = new float[5];
    }

    public void ClearData()
    {
        highscores = new float[5];
    }
}