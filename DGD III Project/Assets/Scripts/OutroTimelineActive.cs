using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OutroTimelineActive : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject playerObj;
    public GameObject playerCam;
    public GameObject outroCam;
    public GameObject boat;
    public GameObject boatOutro;
    public int plankNum;
    public Player playerScript;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;
        playerScript = playerObj.GetComponent<Player>();
    }

    private void Director_Stopped(PlayableDirector obj)
    {


    }

    private void Director_Played(PlayableDirector obj)
    {
        playerObj.SetActive(false);
        playerCam.SetActive(false);
        outroCam.SetActive(true);
        boat.SetActive(false);
        boatOutro.SetActive(true);

    }

    public void StartTimeline()
    {
        director.Play();
    }
}
