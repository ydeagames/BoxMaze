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
        System.Random rnd = new System.Random(id);
        int sizebase = 4;
        int sizeex = (id % 8) * (id / 8) / 2;
        FloorBehaviour.nextSettings = new FloorSettings(id, id, new Vector2Int(sizebase + rnd.Next(0, sizeex), sizebase + rnd.Next(0, sizeex)));
        SceneController.LoadScene("GameScene");
    }

    public void UpdateDisplay()
    {
        transform.Find("Title").GetComponent<Text>().text = $"{id+1}";
        var stats = GameStats.Load(id);
        transform.Find("Score").GetComponent<Text>().text = !stats.cleared ? "" : (stats.coin <= 0 ? "☆☆☆" : (stats.coin == 1 ? "★☆☆" : (stats.coin == 2 ? "★★☆" : "★★★")));
    }
}
