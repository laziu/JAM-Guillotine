using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeHelper : MonoBehaviour
{
    public string prevScene, nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Head" || collision.tag == "Body")
            SceneManager.LoadScene("Scenes/" + nextScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
            SceneManager.LoadScene("Scenes/" + prevScene);
        else if (Input.GetKeyDown(KeyCode.F8))
            SceneManager.LoadScene("Scenes/" + nextScene);
        else if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Keypad0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
