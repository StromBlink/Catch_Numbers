using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{

    public void onClickPause(string x)
    {
        switch (x)
        {
            case "Resume": Time.timeScale = 1; break;
            case "Pause": Time.timeScale = 0.01f; break;
        }
    }
    public void onClickPrivacyPolicy(string x) => Application.OpenURL(x);
    public void onClickExit() => Application.Quit();
    public void loadScene(int x) => SceneManager.LoadScene(x);
}

