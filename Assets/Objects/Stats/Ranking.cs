using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ranking
{
    [System.Serializable]
    public class RankingItem
    {
        public SerializableDateTime date;
        public int clearedCount;
        public int totalCoin;
        public int totalMiss;
    }

    public List<RankingItem> items = new List<RankingItem>();

    public void Add(RankingItem item)
    {
        items.Add(item);
    }

    public static void Save(int id, Ranking ranking)
    {
        if (ranking != null)
        {
            PlayerPrefs.SetString($"ranking.{id}", JsonUtility.ToJson(ranking));
        }
        else
        {
            PlayerPrefs.DeleteKey($"ranking.{id}");
        }
        PlayerPrefs.Save();
    }

    public static Ranking Load(int id)
    {
        if (PlayerPrefs.HasKey($"ranking.{id}"))
        {
            var ranking = JsonUtility.FromJson<Ranking>(PlayerPrefs.GetString($"ranking.{id}"));
            return ranking;
        }
        else
        {
            return null;
        }
    }
}
