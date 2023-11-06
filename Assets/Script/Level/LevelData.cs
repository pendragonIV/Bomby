using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private List<Level> levels = new List<Level>();

    public Level GetLevelAt(int index)
    {
        return levels[index];
    }

    public int GetLevelTotalObj(int index)
    {
        int total = 0;
        levels[index].objects.ForEach(obj =>
        {
            total += obj.quantity;
        });

        return total;
    }

    public List<Level> GetLevels()
    {
        return levels;
    }

    public void SetLevelData(int levelIndex, bool isPlayable, bool isCompleted, int achivement)
    {
        levels[levelIndex].isPlayable = isPlayable;
        levels[levelIndex].isCompleted = isCompleted;
        levels[levelIndex].achivement = achivement;
    }

    #region Save and Load
    public void SaveDataJSON()
    {
        string content = JsonHelper.ToJson(levels.ToArray(), true);
        WriteFile(content);
    }

    public void LoadDataJSON()
    {
        string content = ReadFile();
        if (content != null)
        {
            levels = new List<Level>(JsonHelper.FromJson<Level>(content).ToList());
        }
    }

    private void WriteFile(string content)
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/Levels.json", FileMode.Create);

        using (StreamWriter writer = new StreamWriter(file))
        {
            writer.Write(content);
        }
    }

    private string ReadFile()
    {
        if (File.Exists(Application.persistentDataPath + "/Levels.json"))
        {
            FileStream file = new FileStream(Application.persistentDataPath + "/Levels.json", FileMode.Open);

            using (StreamReader reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }
        else
        {
            return null;
        }
    }
    #endregion
}

[Serializable]
public class Level
{
    public List<LevelObject> objects;
    public List<BlockingObject> blockingObjects;
    public Vector2 nestPosition;
    public bool isCompleted;
    public bool isPlayable;
    public int achivement;
}
[Serializable]
public class LevelObject
{
    public GameObject spawnObject;
    public int quantity;

    public LevelObject(GameObject spawnObject, int quantity)
    {
        this.spawnObject = spawnObject;
        this.quantity = quantity;
    }
}
[Serializable]
public class BlockingObject
{
    public GameObject blockingObject;
    public Vector3Int cellPos;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}


