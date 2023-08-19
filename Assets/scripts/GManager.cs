using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    

    
    public float ratioScore;//new!!
    public float maxScore;//new!!

    public int songID;
    public float noteSpeed;

    public bool Start;
    public float StartTime;

    public int combo;
    public int score;

    public int perfect;
    public int great;
    public int bad;
    public int miss;

    public float grace = 0.0f;

    public void Awake()
    {
        

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}