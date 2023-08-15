using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;


public class Judge : MonoBehaviour
{
    public GManager gManager;

    public Camera raycastCamera;

    public int MC;
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
        if (gManager.Start)
        {
            Ray ray;
            PointerEventData pointerEventData;

            if (notesManager.NormalNT.Count - 1 > -1)
            {
                if (Input.GetKeyDown(KeyCode.D))//〇キーが押されたとき
                {
                    Check(0);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    Check(1);
                }

                if (Input.GetKeyDown(KeyCode.G))
                {
                    Check(2);
                }

                if (Input.GetKeyDown(KeyCode.H))
                {
                    Check(3);
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    Check(4);
                }

                if (Input.GetKeyDown(KeyCode.K))
                {
                    Check(5);
                }

                if (Input.GetKeyDown(KeyCode.L))
                {
                    Check(6);
                }

                if (Input.GetMouseButtonDown(0))//タップ
                {
                    ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

                    pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;

                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, results);


                    foreach (var result in results)
                    {
                        if(result.gameObject != null)
                        {
                            if(result.gameObject.CompareTag(light.cubeTag0))
                            {
                                Check(0);
                            }
                            else if (result.gameObject.CompareTag(light.cubeTag1))
                            {
                                Check(1);
                            }
                            else if (result.gameObject.CompareTag(light.cubeTag2))
                            {
                                Check(2);
                            }
                            else if (result.gameObject.CompareTag(light.cubeTag3))
                            {
                                Check(3);
                            }
                            else if (result.gameObject.CompareTag(light.cubeTag4))
                            {
                                Check(4);
                            }
                            else if (result.gameObject.CompareTag(light.cubeTag5))
                            {
                                Check(5);
                            }
                            else if (result.gameObject.CompareTag(light.cubeTag6))
                            {
                                Check(6);
                            }
                        }
                    }

                   
                    
                }


            }





            if (notesManager.NormalNT.Count - 1 > -1)
            {
                if (Time.time > notesManager.NormalNT[0] + 0.2f + gManager.StartTime)//本来ノーツをたたくべき時間から0.2秒たっても入力がなかった場合
                {
                    message(3);
                    RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + MC / notesManager.noteNum * 100000;
                    gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
                    deleteData();
                    Debug.Log("Miss");
                    gManager.miss++;
                    gManager.combo = 0;
                    //ミス
                }
            }

            
        }

        if(MsObj.Count > 1)//メッセージが重ならないように削除
        {
            Destroy(MsObj[0]);
            MsObj.RemoveAt(0);
            
        }
    }


    public void Check(int lane)
    {
        if (notesManager.NormalLN[0] == lane)
        {
            Judgement(GetABS(Time.time - (notesManager.NormalNT[0] + GManager.instance.StartTime)));
        }
        
        else if (notesManager.NormalNT.Count - 1 > 0)
        {
            if (notesManager.NormalLN[1] == lane)
            {
                 Judgement(GetABS(Time.time - (notesManager.NormalNT[1] + GManager.instance.StartTime)));
            }
        }


        
    }

    public void Judgement(float timeLag)
    {
        audio.PlayOneShot(hitSound);
        if (timeLag <= 0.10)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.1秒以下だったら
        {
            Debug.Log("Perfect");
            message(0);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
            gManager.perfect++;
            gManager.combo++;
            deleteData();

            if(MC < gManager.combo)//MaxComboを設定
            {
                MC = gManager.combo;
            }

            return;
        }
       
        if (timeLag <= 0.15)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.15秒以下だったら
        {
            Debug.Log("Great");
            message(1);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
            gManager.great++;
            gManager.combo++;
            deleteData();

            if (MC < gManager.combo)//MaxComboを設定
            {
                MC = gManager.combo;
            }

            return;
        }
           
        if (timeLag <= 0.20)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.2秒以下だったら
        {
            Debug.Log("Bad");
            message(2);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
            gManager.bad++;
            gManager.combo = 0;
            deleteData();

            if (MC < gManager.combo)//MaxComboを設定
            {
                MC = gManager.combo;
            }

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
    public void deleteData()//すでにたたいたノーツを削除する関数
    {
        notesManager.NormalNT.RemoveAt(0);
        notesManager.NormalLN.RemoveAt(0);
       
        gManager.score = (int)gManager.ratioScore;
        //↑new
        comboText.text = gManager.combo.ToString();//new!
        scoreText.text = gManager.score.ToString();//new!
    }

    void message(int judge)//判定を表示する
    {
        MsObj.Add(Instantiate(MessageObj[judge], new Vector3(0, 2.530983f, -1.5f), Quaternion.Euler(45, 0, 0)));
    }
}