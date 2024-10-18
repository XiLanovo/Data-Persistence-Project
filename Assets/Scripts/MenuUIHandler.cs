using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json; // 引入 Newtonsoft.Json


public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField inputField;

    public static MenuUIHandler Instance;

    public string name;

    void Start() { }

    void Update() { }

    public void StartNew()
    {
        InputName();
        SaveName();
        SceneManager.LoadScene("main");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void Awake()
    {
        Instance = this;
    }

    public void InputName()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            Instance.name = inputField.text;
        }
    }

    private void SaveName()
    {
        User newUser = new User { name = name, maxScore = 0 };

        // 检查 Json 文件夹是否存在，如果不存在则创建
        string jsonFolderPath = Path.Combine(Application.dataPath, "Json");
        if (!Directory.Exists(jsonFolderPath))
        {
            Directory.CreateDirectory(jsonFolderPath);
        }

        // 检查 user.json 文件是否存在，如果不存在则创建
        string jsonFilePath = Path.Combine(jsonFolderPath, "user.json");
        if (!File.Exists(jsonFilePath))
        {
            File.Create(jsonFilePath).Close();
            File.WriteAllText(jsonFilePath, "[]");
        }

        // 读取现有的用户列表
        List<User> users = LoadUsers(jsonFilePath);

        // 检查是否已经存在同名的用户
        bool nameExists = users.Exists(u => u.name == newUser.name);
        if (!nameExists)
        {
            // 如果不存在，则添加新用户
            users.Add(newUser);

            // 将用户列表写回文件
            File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(users, Formatting.Indented));
        }
    }

    private List<User> LoadUsers(string jsonFilePath)
    {
        // 尝试读取文件内容
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            // 如果文件不为空，则反序列化为用户列表
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<List<User>>(json);
            }
        }

        // 如果文件不存在或为空，则返回一个新的用户列表
        return new List<User>();
    }

    private void LoadAndPrintUsers(string jsonFilePath)
    {
        List<User> users = LoadUsers(jsonFilePath);
        foreach (User user in users)
        {
            Debug.Log($"Name: {user.name}, MaxScore: {user.maxScore}");
        }
    }

    [System.Serializable]
    public class User
    {
        public string name;
        public int maxScore;
    }
}