using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
public static class SaveAndLoadGame 
{
    private static string SavePath => Application.persistentDataPath + "/level.json";


    public static void SaveLevelData(List<CardView> cardViews,GameoverInfo gameoverInfo)
    {
        int totalCards = 0;
        LevelData levelData = new LevelData();

        foreach (var item in cardViews)
        {
            LevelDataInfo levelDataInfo = new LevelDataInfo();
            levelDataInfo.cardId = item.cardData.cardID;
            levelDataInfo.isFlipped = item.isFlipped;
            levelData.levelDataInfos.Add(levelDataInfo);
            totalCards++;
        }

        levelData.score = gameoverInfo.Score;
        levelData.time = gameoverInfo.Time;
        levelData.totalCards = totalCards;
        levelData.bonus = gameoverInfo.Bonus ;

        string json = JsonConvert.SerializeObject(levelData,Formatting.Indented);

        File.WriteAllText(SavePath, json);

    }


    public static LevelData LoadLevelData()
    {
        if (!File.Exists(SavePath))
        {
            return new LevelData();
        }

        string json = File.ReadAllText(SavePath);
        return JsonConvert.DeserializeObject<LevelData>(json);
    }


}


public class LevelDataInfo
{
    public bool isFlipped;
    public int cardId;
    
}

[System.Serializable]
public class LevelData
{
    public List<LevelDataInfo> levelDataInfos = new List<LevelDataInfo>();
    public int score;
    public int bonus;
    public int time;
    public int totalCards;
    
}


