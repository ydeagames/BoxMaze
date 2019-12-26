using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ボタンを使用するためUIとSceneManagerを使用ためSceneManagementを追加
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string scene;
    public AudioClip audioClip;
    public static string lastSelect = "SelectScene";

    // ボタンをクリックするとBattleSceneに移動します
    public void ButtonClicked()
    {
        Scene("GameScene");
    }

    public void TitleClicked()
    {
        Scene("TitleScene");
    }

    public void SelectRandomOrHowTo()
    {
        if (PlayerPrefs.GetInt("howto.first") != 1)
        {
            lastSelect = "RandomSelectScene";
            HowToClicked();
        }
        else
            Scene("RandomSelectScene");
    }

    public void SelectOrHowTo()
    {
        if (PlayerPrefs.GetInt("howto.first") != 1)
        {
            lastSelect = "SelectScene";
            HowToClicked();
        }
        else
            Scene("SelectScene");
    }

    public void HowTo()
    {
        lastSelect = "RandomSelectScene";
        HowToClicked();
    }

    public void HowToClicked()
    {
        Scene("HowToScene");
        PlayerPrefs.SetInt("howto.first", 1);
    }

    public void HowToStart()
    {
        Scene(lastSelect);
    }

    public void SceneClicked()
    {
        Scene(scene);
    }

    public void Back()
    {
        Scene(lastSelect);
    }

    public void SceneAndAddLast()
    {
        lastSelect = SceneManager.GetActiveScene().name;
        Scene(scene);
    }

    public void Scene(string scene)
    {
        if (audioClip != null)
        {
            var source = Camera.main.GetComponent<AudioSource>();
            if (source != null)
                source.PlayOneShot(audioClip);
            else
                AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
        }
        LoadScene(scene);
    }

    public void SceneNoSound(string scene)
    {
        LoadScene(scene);
    }

    public void ClearGame()
    {
        var before = GameStats.Load(FloorBehaviour.currentSettings.Value.id);
        GameStats.currentStats.cleared = true;
        var stats = new GameStats(GameStats.currentStats);
        stats.coin = System.Math.Max(stats.coin, before.coin);
        GameStats.Save(FloorBehaviour.currentSettings.Value.id, stats);
        Scene("ResultScene");
    }

    public void StartGame(int id)
    {
        TimeAttack.Reset();
        TimeAttack.Save(TimeAttack.currentState);

        lastSelect = SceneManager.GetActiveScene().name;
        System.Random rnd = new System.Random(id);
        int sizebase = 4;
        int sizeex = (id % 8) * (id / 8) / 2;
        FloorBehaviour.nextSettings = new FloorSettings(id, id, new Vector2Int(sizebase + rnd.Next(0, sizeex), sizebase + rnd.Next(0, sizeex)));
        Scene("GameScene");
    }

    public void StartGame0(int id)
    {
        TimeAttack.Reset();
        TimeAttack.Save(TimeAttack.currentState);

        System.Random rnd = new System.Random(id);
        int sizebase = 4;
        int sizeex = (id % 8) * (id / 8) / 2;
        FloorBehaviour.nextSettings = new FloorSettings(id, id, new Vector2Int(sizebase + rnd.Next(0, sizeex), sizebase + rnd.Next(0, sizeex)));
        Scene("GameScene");
    }

    public void NextScene()
    {
        if (audioClip != null)
            Camera.main.GetComponent<AudioSource>().PlayOneShot(audioClip);
        lastSelect = "SelectScene";
        StartGame0(FloorBehaviour.currentSettings.Value.id + 1);
    }

    public static void LoadScene(string scene)
    {
        MyFade.Get().Fadeout(scene);
    }

    // ボタンをクリックすると終了
    public void ExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
		Application.OpenURL("http://www.google.com/");
#else
		Application.Quit();
#endif
    }
}