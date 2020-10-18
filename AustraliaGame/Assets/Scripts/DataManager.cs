using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager
{
    public static void SaveHighscore(int _highScore)
    {
        BinaryFormatter _bf = new BinaryFormatter();
        FileStream _file = File.Create(Application.persistentDataPath + "/HighScoreData.dat");

        HighscoreData _data = new HighscoreData();
        _data.HighScore = _highScore;

        _bf.Serialize(_file, _data);
        _file.Close();
    }

    public static int LoadHighscore()
    {
        if (File.Exists(Application.persistentDataPath + "/HighScoreData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/HighScoreData.dat", FileMode.Open);
            HighscoreData data = (HighscoreData)bf.Deserialize(file);
            file.Close();

            return data.HighScore;
        }
        else
        {
            return 0;
        }
    }
}

[System.Serializable]
public class HighscoreData
{
    public int HighScore;
}
