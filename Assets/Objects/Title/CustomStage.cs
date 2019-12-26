using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomStage : MonoBehaviour
{
    public SceneController controller;
    public InputField widthText;
    public InputField heightText;
    public InputField seedText;

    public void StartCustom()
    {
        var rnd = new System.Random();

        var randomized = false;

        if (!int.TryParse(widthText.text, out var width))
        {
            width = rnd.Next(27) + 3;
            widthText.text = width.ToString();
            randomized = true;
        }
        else if (width > 40)
        {
            width = 40;
            widthText.text = width.ToString();
            randomized = true;
        }
        if (!int.TryParse(heightText.text, out var height))
        {
            height = rnd.Next(27) + 3;
            heightText.text = height.ToString();
            randomized = true;
        }
        else if (height > 40)
        {
            height = 40;
            heightText.text = height.ToString();
            randomized = true;
        }

        if (!int.TryParse(seedText.text, out var seed))
            if (!string.IsNullOrEmpty(seedText.text))
                seed = seedText.GetHashCode();
            else
            {
                seed = rnd.Next();
                seedText.text = seed.ToString();
                randomized = true;
            }

        if (randomized)
            return;

        FloorBehaviour.nextSettings = new FloorSettings(-1, seed, new Vector2Int(width, height));
        controller.SceneAndAddLast();
    }
}
