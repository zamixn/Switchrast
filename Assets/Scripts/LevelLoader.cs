using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System.Text;
using System.Linq;

public class LevelLoader : Singleton<LevelLoader>
{

    List<string[]> levels;

    void Awake()
    {
        LoadAllLevels();
    }

    void LoadAllLevels()
    {
        TextAsset[] files = Resources.LoadAll("", typeof(TextAsset)).Cast<TextAsset>().ToArray();
        files = files.OrderBy(file => int.Parse(file.name)).ToArray();
        levels = new List<string[]>();
        for (int i = 0; i < files.Length; i++)
            levels.Add(files[i].text.Split('\n'));
    }

    public string[] LoadLevel(int levelNum)
    {

        string[] s = levels[levelNum - 1];
        return s;
    }

    public int LevelCount()
    {
        return levels.Count;
    }

    /*string path;
	void Awake () {
		path = Application.dataPath + "/Resources/Levels/";
	}
	public string[] LoadLevel(int levelNum){
		string[] s = File.ReadAllLines(path + levelNum.ToString() + ".txt");
		return s;
	}*/
}
