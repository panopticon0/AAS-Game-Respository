using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class OutroTimelineActive : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject playerObj;
    public GameObject playerCam;
    public GameObject outroCam;
    public GameObject boat;
    public GameObject boatOutro;
    public GameObject button;
    [SerializeField] private AudioSource endMusic;

    void Start()
    {

    }

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;
        
    }

    private void Director_Stopped(PlayableDirector obj)
    {
        SceneManager.LoadScene("Main  Menu", LoadSceneMode.Single);

    }

    private void Director_Played(PlayableDirector obj)
    {
        boat.SetActive(false);
        button.SetActive(false);
        playerObj.SetActive(false);
        playerCam.SetActive(false);
        outroCam.SetActive(true);
        boatOutro.SetActive(true);
        endMusic.Play();
    }

    public void StartTimeline()
    {
        director.Play();
    }
}
