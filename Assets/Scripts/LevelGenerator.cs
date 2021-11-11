using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelGenerator : MonoBehaviour
{

    string path;

    [Range(0, 18)]
    public int difficulty = 1;

    void Awake()
    {
        /*PlayerPrefs.DeleteAll ();
		PlayerPrefs.Save ();*/
        if (Application.isEditor)
            path = Application.dataPath + "/Resources/Levels/";
    }

    public void GenerateLevel()
    {
        string levelName = (LevelCount() + 1).ToString(), level = "";
        level += GenerateLevelString();
        Save(levelName, level);
    }

    string GenerateLevelString()
    {
        string s = "";
        bool[,] block = new bool[7, 7];
        List<int> directions = new List<int>();
        int[] xf = new int[5];

        int[] x = new int[5];
        x[0] = Mathf.Clamp(Random.Range(Mathf.FloorToInt(difficulty / 3f), Mathf.FloorToInt(difficulty)) + Random.Range(1, 3), 0, 9);
        x[1] = Mathf.Clamp(Random.Range(Mathf.FloorToInt(difficulty / 3f), Mathf.FloorToInt(difficulty / 2f)) + Random.Range(0, 2), 0, 9);
        x[2] = Mathf.Clamp(Random.Range(Mathf.FloorToInt(difficulty / 3f), Mathf.FloorToInt(difficulty / 2f)) + Random.Range(0, 2), 0, 9);
        x[3] = Mathf.Clamp(Random.Range(Mathf.FloorToInt(difficulty / 3f), Mathf.FloorToInt(difficulty / 2f)) + Random.Range(1, 3), 0, 9);
        x[4] = Mathf.Clamp(Random.Range(Mathf.FloorToInt(difficulty / 3f), Mathf.FloorToInt(difficulty / 2f)) + Random.Range(2, 4), 0, 9);
        for (int len = 3; len <= 7; len++)
        {
            for (int i = 0; i < x[len - 3]; i++)
            {
                int startX = Random.Range(0, 7), startY = Random.Range(0, 7);
                directions.Clear();
                directions = DetermineDirections(len, startX, startY);
                if (directions.Count > 0)
                {
                    int direction = directions[Random.Range(0, directions.Count)];
                    if (direction == 1)
                    {
                        for (int j = 0; j < len; j++)
                            block[startX, startY - j] = !block[startX, startY - j];
                    }
                    else if (direction == 2)
                    {
                        for (int j = 0; j < len; j++)
                            block[startX + j, startY] = !block[startX + j, startY];
                    }
                    else if (direction == 3)
                    {
                        for (int j = 0; j < len; j++)
                            block[startX, startY + j] = !block[startX, startY + j];
                    }
                    else if (direction == 4)
                    {
                        for (int j = 0; j < len; j++)
                            block[startX - j, startY] = !block[startX - j, startY];
                    }
                    xf[len - 3]++;
                }
            }
        }

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (i == 6 && j == 6)
                    break;
                if (!block[i, j])
                    s += "1 ";
                else
                    s += "0 ";
            }
        }
        if (!block[6, 6])
            s += "1\n";
        else
            s += "0\n";
        s += xf[4] + " " + xf[3] + " " + xf[2] + " " + xf[1] + " " + xf[0];
        return s;
    }

    List<int> DetermineDirections(int len, int startX, int startY)
    {
        List<int> directions = new List<int>();
        if (startX + len <= 7)
            directions.Add(2);
        if (startX - len >= 0)
            directions.Add(4);
        if (startY + len <= 7)
            directions.Add(3);
        if (startY - len >= 0)
            directions.Add(1);
        return directions;
    }

    void Save(string filename, string content)
    {
        File.WriteAllText(path + filename + ".txt", content);
    }

    public int LevelCount()
    {
        return Directory.GetFiles(path, "*.txt").Length;
    }
}
