using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyFade : MonoBehaviour
{
    Fade fade;

    static MyFade instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fade = GetComponent<Fade>();
    }

    public void Fadeout(string scene)
    {
        fade.FadeIn(1, () =>
        {
            SceneManager.LoadScene(scene);
            fade.FadeOut(1, () => {
            });
        });
    }

    public static MyFade Get()
    {
        return instance;
    }
}
