using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public List<AudioSource> musicTracks;
    private AudioManager audioManager;
    private int currentTrack = 1;
    void Start()
    {
        audioManager = (AudioManager)GameObject.FindObjectOfType(typeof(AudioManager));
        musicTracks[audioManager.musicIndex].Play();
        Debug.Log("Music: " + audioManager.musicIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            updateAudio();
        }
    }
    private void updateAudio()
    {
        foreach (var audioClip in musicTracks)
        {
            audioClip.Stop();
        }
        currentTrack += 1;
        if (currentTrack > musicTracks.Count)
        {
            currentTrack = 1;
        }

        musicTracks[currentTrack - 1].Play();
        musicTracks[currentTrack - 1].loop = true;
    }
}
