using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    public SceneController controller;
    public AudioClip audioFinished;

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

        {
            TimeAttack.currentState.id = TimeAttack.currentState.difficulty * 1000 + TimeAttack.currentState.time + 100000;
            TimeAttack.currentState.totalTime = 0;
            TimeAttack.currentState.totalCoin = 0;
            TimeAttack.currentState.totalMiss = 0;
            TimeAttack.currentState.clearedCount = 0;
            TimeAttack.currentState.seed = new System.Random().Next();
            TimeAttack.Save(TimeAttack.currentState);
        }

        SceneController.lastSelect = "TitleScene";

        GameStats.Reset();
        ApplyChallenge();
    }

    [ContextMenu("Goal")]
    public void Next()
    {
        if (TimeAttack.currentState == null)
            return;

        {
            TimeAttack.currentState.totalCoin += GameStats.currentStats.coin;
            TimeAttack.currentState.totalTime += GameStats.currentStats.time;
            TimeAttack.currentState.totalMiss += GameStats.currentStats.miss;
            TimeAttack.currentState.clearedCount++;
            TimeAttack.currentState.seed++;
            TimeAttack.Save(TimeAttack.currentState);
        }

        GameStats.Reset();
        ApplyChallenge();
    }

    public void ApplyChallenge()
    {
        var rnd = new System.Random(TimeAttack.currentState.seed);
        int sizebase = 4;
        int sizeex = TimeAttack.currentState.difficulty * 8 + TimeAttack.currentState.clearedCount;
        TimeAttack.currentState.currentStage =
            new FloorSettings(TimeAttack.currentState.id, TimeAttack.currentState.seed, new Vector2Int(sizebase + rnd.Next(0, sizeex), sizebase + rnd.Next(0, sizeex)));

        TimeAttack.Save(TimeAttack.currentState);
        ContinuePause();
    }

    [ContextMenu("Finish")]
    public void Finished()
    {
        if (TimeAttack.currentState == null)
            return;

        TimeAttack.currentState.totalTime = TimeAttack.currentState.time;
        GameStats.currentStats.time = 0;

        var ranking = Ranking.Load(TimeAttack.currentState.id) ?? new Ranking();
        ranking.Add(new Ranking.RankingItem()
        {
            clearedCount = TimeAttack.currentState.clearedCount,
            date = DateTime.Now,
            totalMiss = TimeAttack.currentState.totalMiss,
            totalCoin = TimeAttack.currentState.totalCoin,
        });
        Ranking.Save(TimeAttack.currentState.id, ranking);

        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioFinished);
        controller.SceneNoSound("RandomResultScene");
    }

    public bool HasPausedData()
    {
        return TimeAttack.currentState != null && TimeAttack.currentState.currentStage != null;
    }

    public void ContinuePause()
    {
        TimeAttack.currentState = TimeAttack.Load();
        FloorBehaviour.nextSettings = TimeAttack.currentState.currentStage;
        SceneController.lastSelect = "TitleScene";
        controller.Scene("GameScene");
    }
}
