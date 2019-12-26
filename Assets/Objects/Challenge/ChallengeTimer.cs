using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeTimer : MonoBehaviour
{
    public Challenge challenge;
    bool once;

    // Update is called once per frame
    void Update()
    {
        if (TimeAttack.currentState == null)
            return;

        var stats = GameStats.currentStats;
        if (TimeAttack.currentState.time * 60 <= TimeAttack.currentState.totalTime + stats.time)
            if (!once)
            {
                once = true;
                if (challenge != null)
                {
                    challenge.Finished();
                }
            }
    }
}
