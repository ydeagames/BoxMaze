using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour
{
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var state = TimeAttack.currentState;
        if (state != null)
        {
            var dif = new string[] { "かんたん", "ふつう", "難しい", "鬼" };
            var time = state.time * 60f - (GameStats.currentStats.time + state.totalTime);
            text.text =
                  $"難易度	: {dif[state.difficulty % 4]}\n"
                + $"コース	: {state.time}分コース\n"
                + $"残り	: {((int)time / 60).ToString("00")}:{((int)time % 60).ToString("00")}\n"
                + $"現在	: {state.clearedCount+1}ステージ目\n"
                + $"ミス	: {state.totalMiss}\n"
                + $"コイン	: {state.totalCoin}";
        }
        else
        {
            var stats = GameStats.currentStats;
            var star = (stats.coin <= 0 ? "☆☆☆" : (stats.coin == 1 ? "★☆☆" : (stats.coin == 2 ? "★★☆" : "★★★")));
            text.text =
                  $"ステージ {((FloorBehaviour.currentSettings == null) ? "" : FloorBehaviour.currentSettings.id.ToString("D2"))}\n"
                + $"時間	: {((int)stats.time / 60).ToString("00")}:{((int)stats.time % 60).ToString("00")}\n"
                + $"ミス	: {stats.miss}\n"
                + $"コイン	: {star}";
        }
    }
}
