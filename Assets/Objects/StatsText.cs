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
        var stats = GameStats.currentStats;
        var star = !stats.cleared ? "" : (stats.coin <= 0 ? "☆☆☆" : (stats.coin == 1 ? "★☆☆" : (stats.coin == 2 ? "★★☆" : "★★★")));
        text.text =
              $"ステージ {FloorBehaviour.currentSettings.id.ToString("D2")}\n"
            + $"時間	: {((int)stats.time / 60).ToString("00")}:{((int)stats.time % 60).ToString("00")}\n"
            + $"ミス	: {stats.miss}\n"
            + $"コイン	: {star}";
    }
}
