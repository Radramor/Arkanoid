using UnityEngine;
using System.IO;
using System;

public class Load : MonoBehaviour
{
    static public void LoadGame()
    {
        Save.data = JsonUtility.FromJson<Data>(File.ReadAllText(Application.streamingAssetsPath + "/save.json"));
    }
}
