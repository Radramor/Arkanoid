using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    public static Data data;
    static public void SaveGame()
    {
        
        File.WriteAllText(Application.streamingAssetsPath + "/save.json", JsonUtility.ToJson(data));

    }
}
