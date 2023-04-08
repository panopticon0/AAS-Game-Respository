using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;



public class IntroTimelineActive : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject playerObject;
    public GameObject playerCamera;
    public GameObject FPVCamera;
    public GameObject canvas;

    private void Awake()
    {
        playerObject.SetActive(false);
        playerCamera.SetActive(false);
        canvas.SetActive(false);
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;

    }

    private void Director_Stopped(PlayableDirector obj)
    {
        playerObject.SetActive(true);
        playerCamera.SetActive(true);
        canvas.SetActive(true);
        Destroy(gameObject);
        Destroy(FPVCamera);
    }

    private void Director_Played(PlayableDirector obj)
    {
        playerObject.SetActive(false);
        playerCamera.SetActive(false);
        canvas.SetActive(false);
    }

    public void StartTimeline()
    {
        director.Play();
    }


}
