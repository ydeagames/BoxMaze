using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
{
    public int Compare(TKey x, TKey y)
    {
        var result = x?.CompareTo(y) ?? 0;
        return result == 0 ? -1 : -result;
    }
}

public class RankingStats : MonoBehaviour
{
    private Ranking ranking;
    private DateTime current;
    public Text textBase;
    public Text textCurrent;
    public Animator flashAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (TimeAttack.currentState == null)
            return;

        ranking = Ranking.Load(TimeAttack.currentState.id);

        if (ranking == null)
            return;

        var items = new SortedList<int, Ranking.RankingItem>(new DuplicateKeyComparer<int>());
        foreach (var item in ranking.items)
        {
            items.Add(item.clearedCount, item);
        }

        if (ranking.items.Count > 0)
        {
            current = ranking.items.Last().date.Value;
            if (items.Count > 0)
            {
                var current = ranking.items.Last().date.Value;
                var top = items.First().Value.date.Value;
                if (current == top)
                    flashAnimator?.SetBool("Enabled", true);
            }
        }

        textBase.text = $"ランキング - {ranking.items.Count}回の記録\n"
                        + string.Join("\n", items.Select(item =>
                            item.Value.date.Value == current
                                ? ""
                                : $"{item.Value.date}        クリア:{item.Value.clearedCount}        ミス:{item.Value.totalMiss}       コイン:{item.Value.totalCoin}"));
        textCurrent.text = "\n"
                           + string.Join("\n", items.Select(item =>
                               item.Value.date.Value != current
                                   ? ""
                                   : $"{item.Value.date}        クリア:{item.Value.clearedCount}        ミス:{item.Value.totalMiss}       コイン:{item.Value.totalCoin}"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
