/*using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public string inputName; // 存储从MenuUIHandler传入的名字

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保在场景切换时不会被销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetInputName(string name)
    {
        inputName = name;
    }

    public void LoadDataAndCompare()
    {
        string loadPath = Path.Combine(Application.dataPath, "../json");
        string fileName = "user.json";
        string filePath = Path.Combine(loadPath, fileName);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            List<MenuUIHandler.InputData> dataList = JsonUtility.FromJson<List<MenuUIHandler.InputData>>(jsonData);

            foreach (var data in dataList)
            {
                if (data.name == inputName)
                {
                    Debug.Log($"Name: {data.name}");
                }
                else
                {
                    Debug.Log("None");
                }
            }
        }
    }
}*/