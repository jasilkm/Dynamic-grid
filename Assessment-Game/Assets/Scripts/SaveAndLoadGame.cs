using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
public static class SaveAndLoadGame 
{
    private static string SavePath => Application.persistentDataPath + "/level01.json";


    public static void SaveLevelData(List<CardView> cardViews,GameoverInfo gameoverInfo)
    {
        int totalCards = 0;
        LevelData levelData = new LevelData();


        // if it flipped card count  is 1 we need to  update it as 0
        int flippedCount = cardViews.Count(item => item.isFlipped);

        if (flippedCount == 1)
        {
            var itemToReset = cardViews.FirstOrDefault(item => item.isFlipped);
            if (itemToReset != null)
            {
                itemToReset.isFlipped = false;
            }
        }


        foreach (var item in cardViews)
        {
            LevelDataInfo levelDataInfo = new LevelDataInfo();
            levelDataInfo.cardId = item.cardData.cardID;
            levelDataInfo.isFlipped = item.isFlipped;
            levelDataInfo.isSelected = item.isMatched;
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
    public bool isSelected;
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


