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

    [SerializeField] public int[] isLighting;
    

    public void Awake()
    {
        isLighting = new int[] { -1, -1, -1, -1, -1 };//light用

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