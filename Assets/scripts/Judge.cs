using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;


public class CalculateScore
{
    public int PhiScore(int P, int G, int TN, int MC)
    {
        float RawScore = (P + G * 0.65f) / TN * 900000 + MC / TN * 100000;
        int OrganizedScore = (int)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);
        return OrganizedScore;
    }
}

public class Judge : MonoBehaviour
{
    CalculateScore calculate = new CalculateScore();
    public GManager gManager;

    public Camera raycastCamera;

   
    public float RawScore;

    //変数の宣言
    [SerializeField] private GameObject[] MessageObj;//プレイヤーに判定を伝えるゲームオブジェクト
    [SerializeField] NotesManager notesManager;//スクリプト「notesManager」を入れる変数

    [SerializeField] public TextMeshProUGUI comboText;
    [SerializeField] public TextMeshProUGUI scoreText;

    AudioSource audio;
    [SerializeField] AudioClip hitSound;

    public List<GameObject> MsObj = new List<GameObject>();

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public Light light;

    void Update()
    {
        gManager.score = (int)gManager.ratioScore;
        //↑new
        comboText.text = gManager.combo.ToString();//new!
        scoreText.text = gManager.score.ToString();//new!
        if (!gManager.Start) return;

        for (int i = 0; i < notesManager.NoteTime.Length; i++)//miss
        {
            if (notesManager.NoteTime[i].Count > 0 && Time.time > notesManager.NoteTime[i][0] + 0.15f + gManager.StartTime)
            {
                HandleMiss(i);
            }
        }

        if (MsObj.Count > 1)//メッセージが重ならないように削除
        {
            Destroy(MsObj[0]);
            MsObj.RemoveAt(0);
        }

        AutoPlay();
    }
    private void AutoPlay()
    {
        if (!gManager.AutoPlay) return;

        for (int i = 0; i < 7; i++)
        {
            if (notesManager.NoteTime[i].Count < 1) continue;
                
            if (0.015000f >= GetABS(Time.time - (notesManager.NoteTime[i][0] + GManager.instance.StartTime)))
            {
                Judgement(0, i);
            }
        }
    }

    private void HandleMiss(int lane)
    {
        message(3);
        
        gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
        deleteData(lane);
        Debug.Log("Miss");
        gManager.miss++;
        gManager.combo = 0;
    }

    private void ProcessInput()
    {
        for (int id = 0; id < Input.touchCount; id++)
        {
            Touch touch = Input.GetTouch(id);

            if (touch.phase != TouchPhase.Began) continue;
            


                Ray ray = raycastCamera.ScreenPointToRay(touch.position);

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touch.position;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (var result in results)
                {
                    if (result.gameObject == null) continue;//当たったオブジェクトがnullじゃなかったら
                    
                        if (result.gameObject.CompareTag(light.cubeTag0))//左端のレーン 格レーンについたタグ(canvasのbutton（透明）についてる)に当たったら
                        {
                            Check(0);//レーンが正しいか判断する関数
                            break; 
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag1))
                        {
                            Check(1);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag2))
                        {
                            Check(2);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag3))
                        {
                            Check(3);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag4))
                        {
                            Check(4);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag5))
                        {
                            Check(5);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag6))//右端
                        {
                            Check(6);
                            break;
                        }
                    

                }
            
        }
    }
    public void Check(int lane)
    {
        if (notesManager.NoteTime[lane].Count < 1) return;

        Judgement(GetABS(Time.time - (notesManager.NoteTime[lane][0] + GManager.instance.StartTime)), lane);

    }

    public void Judgement(float timeLag, int lane)
    {
        if (timeLag >= 0.15) return;
        audio.PlayOneShot(hitSound);
        if (timeLag <= 0.05)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.1秒以下だったら
        {
            Debug.Log("Perfect");
            message(0);
            
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
            gManager.perfect++;
            gManager.combo++;
            deleteData(lane);

            if(gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }

            return;
        }
       
        if (timeLag <= 0.10)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.15秒以下だったら
        {
            Debug.Log("Great");
            message(1);
           
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
            gManager.great++;
            gManager.combo++;
            deleteData(lane);

            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }

            return;
        }
           
        if (timeLag <= 0.15)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.2秒以下だったら
        {
            Debug.Log("Bad");
            message(2);
            
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
            gManager.bad++;
            gManager.combo = 0;
            deleteData(lane);

            

            return;
        }
            
        
    }
    public float GetABS(float num)//引数の絶対値を返す関数
    {
        if (num >= 0)
        {
            return num;
        }
        else
        {
            return -num;
        }
    }
    public void deleteData(int lane)//すでにたたいたノーツを削除する関数
    {
        notesManager.NoteTime[lane].RemoveAt(0);
        
       
        
    }

    void message(int judge)//判定を表示する
    {
        MsObj.Add(Instantiate(MessageObj[judge], new Vector3(0, 2.530983f, -1.5f), Quaternion.Euler(45, 0, 0)));
    }
}