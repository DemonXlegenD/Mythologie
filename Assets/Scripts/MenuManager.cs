using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonStart;
    [SerializeField] private GameObject buttonQuit;
    [SerializeField] private GameObject buttonMusic;
    [SerializeField] private GameObject buttonUnmusic;
    [SerializeField] private GameObject panelQuit;
    [SerializeField] private GameObject skip;
    [SerializeField] private List<GameObject> hideToStart;

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private int videoClipIndex = 0;
    private int videoLoop = 3;
    private bool cinematic = false;
    private bool clicked = false;

    // Noms de fichiers dans Assets/StreamingAssets (lecture par URL, requis pour WebGL)
    [SerializeField] private List<string> videos;
    [SerializeField] private string menuVideoFile = "Main_Menu.mp4";

    public bool isMute = false;

    void Start()
    {
        skip.SetActive(false);
        videoPlayer.url = StreamingUrl(menuVideoFile);
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

    private static string StreamingUrl(string fileName)
    {
        return Application.streamingAssetsPath + "/" + fileName;
    }

    private void Update()
    {
        if (cinematic)
        {
            if (Input.GetMouseButtonDown(0))
            {
                    Debug.Log("Clicked");
                    videoClipIndex++;
                PlayNextVideo();
            }
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (videoClipIndex == videos.Count - 1)
        {
            videoPlayer.isLooping = true;
            videoPlayer.Play();
            videoLoop--;
            if (videoLoop == 0) PlayNextVideo();
        }
        else
        {
            videoClipIndex++;
            PlayNextVideo();
        }
    }

    private void PlayNextVideo()
    {
        if (videoClipIndex < videos.Count)
        {
            if (videoPlayer.isPlaying) videoPlayer.Stop();
            videoPlayer.url = StreamingUrl(videos[videoClipIndex]);
            videoPlayer.Play();
        }
        else
        {
            GameManager.Instance.StartTuto();
        }
        
    }



    public void OnPlayButtonClick()
    {
        skip.SetActive(true);
        cinematic = true;
        videoPlayer.isLooping = false;
        foreach (GameObject gameObject in hideToStart)
        {
            gameObject.SetActive(false);
        }
        videoPlayer.loopPointReached += OnVideoFinished;
        PlayNextVideo();
    }

    public void OnQuitButtonClick()
    {
        ShowPanelQuit();
    }

    public void OnTutoButtonClick()
    {
        GameManager.Instance.ChangeScene("TutoScene");
    }

    public void OnYesButtonClick()
    {
        GameManager.Instance.Quit();
    }

    public void OnNoButtonClick()
    {
        HidePanelQuit();
    }

    public void OnSoundButtonClick()
    {
        if (isMute) GameManager.Instance.UnmuteAudio();
        else GameManager.Instance.MuteAudio();
        isMute = !isMute;
        buttonUnmusic.SetActive(isMute);
        buttonMusic.SetActive(!isMute);
    }


    public void HidePanelQuit()
    {
        videoPlayer.Play();
        buttonQuit.GetComponent<Interactable>().enabled = true;
        buttonStart.GetComponent<Interactable>().enabled = true;
        buttonMusic.GetComponent<Interactable>().enabled = true;
        buttonUnmusic.GetComponent<Interactable>().enabled = true;
        panelQuit.SetActive(false);
    }

    public void ShowPanelQuit()
    {
        videoPlayer.Pause();
        buttonQuit.GetComponent<Interactable>().enabled = false;
        buttonStart.GetComponent<Interactable>().enabled = false;
        buttonMusic.GetComponent<Interactable>().enabled = false;
        buttonUnmusic.GetComponent<Interactable>().enabled = false;
        panelQuit.SetActive(true);
    }
}
