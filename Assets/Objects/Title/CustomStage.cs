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
        System.Random rnd = new System.Random();

        bool randomized = false;

        int width, height;
        if (!int.TryParse(widthText.text, out width))
        {
            width = rnd.Next(27) + 3;
            widthText.text = width.ToString();
            randomized = true;
        }
        if (!int.TryParse(heightText.text, out height))
        {
            height = rnd.Next(27) + 3;
            heightText.text = height.ToString();
            randomized = true;
        }

        int seed;
        if (!int.TryParse(seedText.text, out seed))
            if (!string.IsNullOrEmpty(seedText.text))
                seed = seedText.GetHashCode();
            else
            {
                seed = rnd.Next();
                seedText.text = seed.ToString();
                randomized = true;
            }

        if (!randomized)
        {
            FloorBehaviour.nextSettings = new FloorSettings(-1, seed, new Vector2Int(width, height));
            controller.SceneAndAddLast();
        }
    }
}
