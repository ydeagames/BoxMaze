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
        StartCoroutine(FadeoutCoroutine(scene));
    }

    IEnumerator FadeoutCoroutine(string scene)
    {
        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            fade.fade.Range = Mathf.Clamp(time, 0, 1);
            yield return null;
        }

        fade.fade.Range = 1;
        SceneManager.LoadScene(scene);

        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            fade.fade.Range = Mathf.Clamp(1 - time, 0, 1);
            yield return null;
        }

        fade.fade.Range = 0;
        yield break;
    }

    public static MyFade Get()
    {
        return instance;
    }
}
