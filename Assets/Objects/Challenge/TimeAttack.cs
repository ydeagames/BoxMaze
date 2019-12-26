using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeAttack
{
    public int id;
    public int time;
    public int difficulty;
    public int clearedCount;
    public int seed;
    public float totalTime;
    public int totalCoin;
    public int totalMiss;
    public FloorSettings currentStage;

    public static TimeAttack currentState = null;
    public static void Reset()
    {
        currentState = null;
    }

    public TimeAttack()
    {
    }

    public TimeAttack(TimeAttack stats)
    {
        this.id = stats.id;
        this.time = stats.time;
        this.difficulty = stats.difficulty;
        this.clearedCount = stats.clearedCount;
        this.seed = stats.seed;
        this.totalTime = stats.totalTime;
        this.totalCoin = stats.totalCoin;
        this.totalMiss = stats.totalMiss;
        this.currentStage = stats.currentStage;
    }

    public static void Save(TimeAttack stats)
    {
        if (stats != null)
        {
            PlayerPrefs.SetInt($"challenge.id", stats.id);
            PlayerPrefs.SetInt($"challenge.time", stats.time);
            PlayerPrefs.SetInt($"challenge.difficulty", stats.difficulty);
            PlayerPrefs.SetInt($"challenge.cleared", stats.clearedCount);
            PlayerPrefs.SetInt($"challenge.seed", stats.seed);
            PlayerPrefs.SetFloat($"challenge.totalTime", stats.totalTime);
            PlayerPrefs.SetInt($"challenge.totalCoin", stats.totalCoin);
            PlayerPrefs.SetInt($"challenge.totalMiss", stats.totalMiss);

            if (stats.currentStage != null)
            {
                PlayerPrefs.SetInt($"challenge.stage.id", stats.currentStage.id);
                PlayerPrefs.SetInt($"challenge.stage.size.x", stats.currentStage.size.x);
                PlayerPrefs.SetInt($"challenge.stage.size.y", stats.currentStage.size.y);
                PlayerPrefs.SetInt($"challenge.seed", stats.currentStage.seed);
                PlayerPrefs.SetInt($"challenge.spawn.x", stats.currentStage.spawn.x);
                PlayerPrefs.SetInt($"challenge.spawn.y", stats.currentStage.spawn.y);
            }
            else
            {
                PlayerPrefs.DeleteKey($"challenge.stage.id");
            }
        }
        else
        {
            PlayerPrefs.DeleteKey($"challenge.id");
        }
        PlayerPrefs.Save();
    }

    public static TimeAttack Load()
    {
        if (PlayerPrefs.HasKey($"challenge.id"))
        {
            var stats = new TimeAttack();

            stats.id = PlayerPrefs.GetInt($"challenge.id");
            stats.time = PlayerPrefs.GetInt($"challenge.time");
            stats.difficulty = PlayerPrefs.GetInt($"challenge.difficulty");
            stats.clearedCount = PlayerPrefs.GetInt($"challenge.cleared");
            stats.seed = PlayerPrefs.GetInt($"challenge.seed");
            stats.totalTime = PlayerPrefs.GetFloat($"challenge.totalTime");
            stats.totalCoin = PlayerPrefs.GetInt($"challenge.totalCoin");
            stats.totalMiss = PlayerPrefs.GetInt($"challenge.totalMiss");

            if (PlayerPrefs.HasKey($"challenge.stage.id"))
            {
                var id = PlayerPrefs.GetInt($"challenge.stage.id");
                var seed = PlayerPrefs.GetInt($"challenge.seed");
                var size = new Vector2Int(PlayerPrefs.GetInt($"challenge.stage.size.x"), PlayerPrefs.GetInt($"challenge.stage.size.y"));
                stats.currentStage = new FloorSettings(id, seed, size)
                {
                    spawn = new Vector2Int(PlayerPrefs.GetInt($"challenge.spawn.x"), PlayerPrefs.GetInt($"challenge.spawn.y"))
                };
            }

            return stats;
        }
        else
        {
            return null;
        }
    }
}
