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

    // ボタンをクリックするとBattleSceneに移動します
    public void ButtonClicked()
    {
        Scene("GameScene");
    }

    public void TitleClicked()
    {
        Scene("TitleScene");
    }

    public void SelectOrHowTo()
    {
        if (PlayerPrefs.GetInt("howto.first") != 1)
            HowToClicked();
        else
            Scene("SelectScene");
    }

    public void HowToClicked()
    {
        Scene("HowToScene");
        PlayerPrefs.SetInt("howto.first", 1);
    }

    public void SceneClicked()
    {
        Scene(scene);
    }

    public void Scene(string scene)
    {
        if (audioClip != null)
            AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
        LoadScene(scene);
    }

    public void StartGame(int id)
    {
        System.Random rnd = new System.Random(id);
        int sizebase = 4;
        int sizeex = (id % 8) * (id / 8) / 2;
        FloorBehaviour.nextSettings = new FloorSettings(id, id, new Vector2Int(sizebase + rnd.Next(0, sizeex), sizebase + rnd.Next(0, sizeex)));
        Scene("GameScene");
    }

    public void NextScene()
    {
        if (audioClip != null)
            AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
        StartGame(FloorBehaviour.currentSettings.id + 1);
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