using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomStatsText : MonoBehaviour
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
        var stats = TimeAttack.currentState;
        if (stats == null)
            return;
        var dif = new string[] { "かんたん", "ふつう", "難しい", "鬼" };
        text.text =
              $"難易度			: {dif[stats.difficulty % 4]} - {stats.time}分コース\n"
            + $"残り時間		: {TimeAttack.currentState?.time - (TimeAttack.currentState?.totalTime + stats.time)}分\n"
            + $"クリアステージ	: {stats.clearedCount}\n"
            + $"ミス			: {stats.totalMiss}\n"
            + $"コイン			: {stats.totalCoin}";
    }
}
