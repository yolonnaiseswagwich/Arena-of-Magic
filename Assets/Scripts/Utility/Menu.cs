using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height * 0.4f), "DeathMatch"))
        {
            SceneManager.LoadScene("DM Lobby");
        }
        if (GUI.Button(new Rect(0, Screen.height * 0.4f, Screen.width, Screen.height * 0.4f), "Rounds"))
        {
            SceneManager.LoadScene("R Lobby");
        }
        if (GUI.Button(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.2f), "Quit"))
        {
            Application.Quit();
        }
    }
}
