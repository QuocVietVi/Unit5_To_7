using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SaveData
{
    public string name;
    public int highScore;
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Button startBtn, exitBtn;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI nameInputTxt;
    public GameObject menuPanel;
    private SaveData saveData;

    public SaveData SaveData { get => saveData; set => saveData = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        startBtn.onClick.AddListener(StartGame);
        exitBtn.onClick.AddListener(Exit);
        
        GetPlayerData();
        scoreTxt.text = "Best Score: " + SaveData.name + ":" + SaveData.highScore;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
        menuPanel.SetActive(false);
    }

    public void Exit()
    {
        //if UNITY_EDITOR
        //else
        Application.Quit(); 

    }



    public void SaveInfo<T>(T data)
    {
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public T LoadData<T>()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
        
    }

    public void GetPlayerData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            SaveData = LoadData<SaveData>();
            Debug.Log("Loaded User Data.");
        }
        else
        {
            SaveData = new SaveData();
            SaveInfo(SaveData);
            Debug.Log("Creating New User Data");
        }
    }

}
