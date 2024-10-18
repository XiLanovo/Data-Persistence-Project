using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public GameObject BackToMenuButton;
    public Text ShowBestScoreWithName;

    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;

    private string jsonFilePath;
    private User currentUser;

    void Start()
    {
        string jsonFolderPath = Path.Combine(Application.dataPath, "Json");
        jsonFilePath = Path.Combine(jsonFolderPath, "user.json");

        LoadAndPrintUsers(jsonFilePath);

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // 检查当前得分是否超过最高得分
        if (currentUser != null && m_Points > currentUser.maxScore)
        {
            currentUser.maxScore = m_Points;
            ShowBestScoreWithName.text = $"{currentUser.name}'s Best Score : {currentUser.maxScore}";
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        BackToMenuButton.SetActive(true);

        // 保存最高得分
        SaveUserScore(jsonFilePath);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private List<User> LoadUsers(string jsonFilePath)
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<List<User>>(json);
            }
        }
        return new List<User>();
    }

    private void LoadAndPrintUsers(string jsonFilePath)
    {
        List<User> users = LoadUsers(jsonFilePath);
        foreach (User user in users)
        {
            if (user.name == MenuUIHandler.Instance.name)
            {
                currentUser = user;
                ShowBestScoreWithName.text = $"{user.name}'s Best Score : {user.maxScore}";
                Debug.Log($"Name: {user.name}, MaxScore: {user.maxScore}");
                return;
            }
        }
    }

    private void SaveUserScore(string jsonFilePath)
    {
        List<User> users = LoadUsers(jsonFilePath);
        foreach (User user in users)
        {
            if (user.name == currentUser.name)
            {
                user.maxScore = currentUser.maxScore;
                break;
            }
        }
        string json = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(jsonFilePath, json);
    }

    [System.Serializable]
    public class User
    {
        public string name;
        public int maxScore;
    }
}