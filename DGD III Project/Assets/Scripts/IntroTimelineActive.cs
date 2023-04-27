using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;



public class IntroTimelineActive : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject playerObject;
    public GameObject playerCamera;
    public GameObject FPVCamera;
    public GameObject canvas;
    public GameObject plane;
    public GameObject plane2;


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
        Destroy(plane);
        Destroy(plane2);

        Destroy(FPVCamera);
        Destroy(gameObject);
        
        GameObject[] Spawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject spawner in Spawners) {
            spawner.GetComponent<enemySpawn>().spawn = true;
        }
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
