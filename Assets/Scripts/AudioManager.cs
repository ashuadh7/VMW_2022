using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public TMP_Dropdown narration;
    public TMP_Dropdown music;
    public TMP_Dropdown scene;
    public int narrationIndex = 0;
    public int musicIndex = 0;
    public int sceneIndex = 0;
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        narration.onValueChanged.AddListener(delegate { NarrationSelected(narration);});
        music.onValueChanged.AddListener(delegate { MusicSelected(music);});
        scene.onValueChanged.AddListener(delegate { SceneSelected(scene);});
    }
    void NarrationSelected(TMP_Dropdown dropdown)
    {
        narrationIndex = dropdown.value;
    }

    void MusicSelected(TMP_Dropdown dropdown)
    {
        musicIndex = dropdown.value;
    }
    void SceneSelected(TMP_Dropdown dropdown)
    {
        sceneIndex = dropdown.value;
    }
}
