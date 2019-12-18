using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomStage : MonoBehaviour
{
    public SceneController controller;
    public Text widthText;
    public Text heightText;
    public Text seedText;

    public void StartCustom()
    {
        System.Random rnd = new System.Random();
        int.TryParse(widthText.text, out int width);
        int.TryParse(heightText.text, out int height);

        int seed;
        if (!int.TryParse(seedText.text, out seed))
            seed = rnd.Next();
        //FloorBehaviour.nextSettings = new FloorSettings(id, id, new Vector2Int(, sizebase + rnd.Next(0, sizeex)));
        //Scene("GameScene");
    }
}
