using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ボタンを使用するためUIとSceneManagerを使用ためSceneManagementを追加
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // ボタンをクリックするとBattleSceneに移動します
    public void ButtonClicked()
    {
        MyFade.Get().Fadeout("GameScene");
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