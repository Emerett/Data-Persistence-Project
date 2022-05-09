using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public TextMeshProUGUI highScore;
    public TMP_InputField inputName;

    public string playerName;
    public string bestPlayerName = "Name";
    public int bestScore = 0;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoader;
        Debug.Log("OnEnable");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoader;
        Debug.Log("OnDisable");
    }

    private void SceneLoader(Scene scene, LoadSceneMode mode)
    {
        highScore = GameObject.FindGameObjectWithTag("HighScore").GetComponent<TextMeshProUGUI>();

        if (scene.buildIndex == 0)
        {
            inputName = GameObject.FindObjectOfType<TMP_InputField>();
        }

        LoadBestScore();
        DisplayBestScore();
        Debug.Log("OnSceneLoaded: " + scene.name);
    }

    [System.Serializable]
    public class SaveData
    {
        public string bestPlayerName;
        public int bestScore;
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayerName = data.bestPlayerName;
            bestScore = data.bestScore;
        }
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestPlayerName = bestPlayerName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void SavePlayerName()
    {
        playerName = inputName.text;
    }

    public void DisplayBestScore()
    {
        highScore.text = "High Score: " + bestPlayerName + " - " + bestScore;
    }

    public void SetBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            bestPlayerName = playerName;
            SaveBestScore();
        }
            
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
