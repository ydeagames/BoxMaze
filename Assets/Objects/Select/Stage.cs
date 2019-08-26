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
        FloorBehaviour.nextSettings = new FloorSettings(id, id, new Vector2Int(id / 8 + 4, id / 8 + 4));
        SceneController.LoadScene("GameScene");
    }

    public void UpdateDisplay()
    {
        transform.Find("Title").GetComponent<Text>().text = $"{id}";
        var stats = GameStats.Load(id);
        transform.Find("Score").GetComponent<Text>().text = !stats.cleared ? "" : (stats.coin <= 0 ? "☆☆☆" : (stats.coin == 1 ? "★☆☆" : (stats.coin == 2 ? "★★☆" : "★★★")));
    }
}
