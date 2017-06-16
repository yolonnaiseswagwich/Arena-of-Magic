using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NetworkDeleter : MonoBehaviour {
    void OnGUI() {
        if (SceneManager.GetActiveScene().name != "DeathMatch" && SceneManager.GetActiveScene().name != "Rounds")
        if (GUI.Button(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.2f), "Quit"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene("Menu");
        }
    }
}
