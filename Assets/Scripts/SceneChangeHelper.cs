using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeHelper : MonoBehaviour
{
    public string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Head" || collision.tag == "Body")
            SceneManager.LoadScene("Scenes/" + nextScene);
    }
}
