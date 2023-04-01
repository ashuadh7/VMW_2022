using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public List<AudioSource> narrationTracks;
    private AudioManager audioManager;
    void Start()
    {
        audioManager = (AudioManager)GameObject.FindObjectOfType(typeof(AudioManager));
        narrationTracks[audioManager.narrationIndex].Play();
        Debug.Log("Narration: " + audioManager.narrationIndex);
    }
}
