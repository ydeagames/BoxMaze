using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeContinueButton : MonoBehaviour
{
    public Challenge challenge;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(challenge.HasPausedData());
    }
}
