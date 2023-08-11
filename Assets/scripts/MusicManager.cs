using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;

public class MusicManager : MonoBehaviour
{
    public GManager gManager;
    [SerializeField] GameObject targetObj;

    AudioSource audio;
    AudioClip Music;
    CriAtomSource criAtomSource;
    
    string songName;
    bool played;
    
    void Start()
    {
        GManager.instance.Start = false;
        songName = "Grievous Lady(Arcaea)";
        audio = GetComponent<AudioSource>();
        criAtomSource = GetComponent<CriAtomSource>();//new!
        Music = (AudioClip)Resources.Load("Musics/" + songName);
        played = false;
        Debug.Log(Music);
        
    }

    public void Update()
    {
        
    }

    public void GameStart()
    {
        gManager.Start = true;
        GManager.instance.StartTime = Time.time;
        audio.PlayOneShot(Music);
        criAtomSource.cueName = songName;
        criAtomSource.Play(criAtomSource.cueName);
        played = true;
        Debug.Log("はい");
        targetObj.SetActive(false);
    }
}