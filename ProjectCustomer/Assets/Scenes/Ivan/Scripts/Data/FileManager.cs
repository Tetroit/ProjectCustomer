
using System.IO;
using UnityEngine;

public static class FileManager
{
    public static T ReadFromFile<T>(string path)
    {
        string fullPath = Path.Combine(Application.persistentDataPath, path);
        T data;
        if (File.Exists(fullPath))
        {
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = JsonUtility.FromJson<T>(reader.ReadToEnd());
                }
            }
            return data;
        }
        else
            return default;
    }
    public static void WriteToFile<T>(string path, T data)
    {
        string dir = Application.persistentDataPath;
        string fullPath = Path.Combine(Application.persistentDataPath, path);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        string json = JsonUtility.ToJson(data, true);

        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(json);
            }
        }
    }
}
