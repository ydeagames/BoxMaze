using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelector : MonoBehaviour
{
    public GameObject prefab;
    GameObject[,] buttons;
    int page = 0;

    // Start is called before the first frame update
    void Start()
    {
        buttons = new GameObject[4, 4];
        for (int iy = 0; iy < 4; iy++)
        {
            for (int ix = 0; ix < 4; ix++)
            {
                var obj = buttons[ix, iy] = Instantiate(prefab, transform);
                var recttrans = obj.GetComponent<RectTransform>();
                recttrans.localScale = Vector3.one * .8f;
                recttrans.pivot = new Vector2(-(ix - 1), (iy - 1)) * 1.2f;
            }
        }
        ShowPage(page);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextPage()
    {
        if (page < 3)
            ShowPage(++page);
    }

    public void PrevPage()
    {
        if (page > 0)
            ShowPage(--page);
    }

    public void ShowPage(int page)
    {
        for (int iy = 0; iy < 4; iy++)
        {
            for (int ix = 0; ix < 4; ix++)
            {
                var obj = buttons[ix, iy];
                var stage = obj.GetComponent<Stage>();
                stage.id = page * 16 + iy * 4 + ix;
                stage.UpdateDisplay();
            }
        }
    }
}
