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
        criAtomSource = GetComponent<CriAtomSource>();//new!
        played = false;
    }

    public void Update()
    {
        
    }

    public void GameStart()
    {
        gManager.Start = true;
        GManager.instance.StartTime = Time.time;
        criAtomSource.cueName = songName;
        criAtomSource.Play(criAtomSource.cueName);
        played = true;
        
        targetObj.SetActive(false);
    }
}