using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public int id;

    public void OnClicked()
    {
        GetComponent<SceneController>().StartGame(id);
    }

    public void UpdateDisplay()
    {
        transform.Find("Title").GetComponent<Text>().text = $"{id+1}";
        var stats = GameStats.Load(id);
        transform.Find("Score").GetComponent<Text>().text = !stats.cleared ? "" : (stats.coin <= 0 ? "☆☆☆" : (stats.coin == 1 ? "★☆☆" : (stats.coin == 2 ? "★★☆" : "★★★")));
    }
}
