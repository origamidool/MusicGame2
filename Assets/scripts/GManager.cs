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

    public int MC;//MaxCombo

    public float grace = 0.0f;//開始まで

    public bool AutoPlay = false;//オートプレイか通常プレイか
    public int LaneLength = 7;//最大レーン　変わらないと思われるが
    

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