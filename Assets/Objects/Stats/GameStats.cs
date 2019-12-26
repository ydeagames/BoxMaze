using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStats
{
    public bool cleared;
    public int coin;
    public float time;
    public int miss;

    public static GameStats currentStats = new GameStats();
    public static void Reset()
    {
        currentStats = new GameStats();
    }

    public GameStats()
    {
    }

    public GameStats(GameStats stats)
    {
        this.cleared = stats.cleared;
        this.coin = stats.coin;
        this.time = stats.time;
        this.miss = stats.miss;
    }

    public static void Save(int id, GameStats stats)
    {
        PlayerPrefs.SetInt($"stats.{id}.cleared", stats.cleared ? 1 : 0);
        PlayerPrefs.SetInt($"stats.{id}.coin", stats.coin);
        PlayerPrefs.SetFloat($"stats.{id}.time", stats.time);
        PlayerPrefs.SetInt($"stats.{id}.miss", stats.miss);
        PlayerPrefs.Save();
    }

    public static GameStats Load(int id)
    {
        var stats = new GameStats();
        stats.cleared = PlayerPrefs.GetInt($"stats.{id}.cleared") != 0;
        stats.coin = PlayerPrefs.GetInt($"stats.{id}.coin");
        stats.time = PlayerPrefs.GetFloat($"stats.{id}.time");
        stats.miss = PlayerPrefs.GetInt($"stats.{id}.miss");
        return stats;
    }
}
