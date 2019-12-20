using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    public SceneController controller;

    private void Start()
    {
        TimeAttack.currentState = TimeAttack.Load();
    }

    public void SetDifficulty(int difficulty)
    {
        if (TimeAttack.currentState == null)
            TimeAttack.currentState = new TimeAttack();
        TimeAttack.currentState.difficulty = difficulty;
        TimeAttack.Save(TimeAttack.currentState);
    }

    public void SetTime(int time)
    {
        if (TimeAttack.currentState == null)
            TimeAttack.currentState = new TimeAttack();
        TimeAttack.currentState.time = time;
        TimeAttack.Save(TimeAttack.currentState);
    }

    public void StartChallenge()
    {
        if (TimeAttack.currentState == null)
            TimeAttack.currentState = new TimeAttack();

        TimeAttack.currentState.id = TimeAttack.currentState.difficulty * 1000 + TimeAttack.currentState.time + 100000;
        TimeAttack.currentState.clearedCount = 0;
        TimeAttack.currentState.totalTime = 0;
        TimeAttack.currentState.totalCoin = 0;
        TimeAttack.currentState.totalMiss = 0;
        TimeAttack.currentState.seed = new System.Random().Next();

        GameStats.Reset();
        Next();
    }

    public void Next()
    {
        if (TimeAttack.currentState == null)
            return;

        if (GameStats.currentStats.cleared)
        {
            TimeAttack.currentState.totalCoin += GameStats.currentStats.coin;
            TimeAttack.currentState.totalTime += GameStats.currentStats.time;
            TimeAttack.currentState.totalMiss += GameStats.currentStats.miss;
        }
        GameStats.Reset();

        var rnd = new System.Random(TimeAttack.currentState.seed++);
        int sizebase = 4;
        int sizeex = TimeAttack.currentState.difficulty * 8 + TimeAttack.currentState.clearedCount;
        FloorBehaviour.nextSettings = TimeAttack.currentState.currentStage =
            new FloorSettings(TimeAttack.currentState.id, TimeAttack.currentState.seed, new Vector2Int(sizebase + rnd.Next(0, sizeex), sizebase + rnd.Next(0, sizeex)));

        TimeAttack.Save(TimeAttack.currentState);

        controller.Scene("GameScene");
    }

    public bool HasPausedData()
    {
        return TimeAttack.currentState != null && TimeAttack.currentState.currentStage != null;
    }

    public void ContinuePause()
    {
        FloorBehaviour.nextSettings = TimeAttack.currentState.currentStage;
        controller.Scene("GameScene");
    }
}
