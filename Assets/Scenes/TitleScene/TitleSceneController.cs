using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public TimelineAsset timeline;
    public AudioSource scream;
    public new Transform camera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            playableDirector.Play(timeline);
    }

    public void StartGame()
    {
        playableDirector.Play(timeline);
        scream.Play();
        StartCoroutine(FinishTimeline());
        StartCoroutine(ChangeScene());
    }

    IEnumerator FinishTimeline()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 1000; i++)
        {
            camera.position += new Vector3(.02f * i, .02f * i, -.02f * i);
            yield return i;
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scenes/Stage1");
    }
}
