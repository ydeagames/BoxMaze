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
        text.text = $"時間	: {((int)stats.time / 60).ToString("00")}:{((int)stats.time).ToString("00")}\nミス	: {stats.miss}\nコイン	: {stats.coin}";
    }
}
