using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;




public class JudgeLong : MonoBehaviour
{
    public Judge Jjudge;
    public GManager gManager;
    public NotesManager notesManager;
    public Light light;

    public float RawScore;

    [SerializeField] AudioClip longhitSound;
    [SerializeField] private GameObject[] MessageObj;//プレイヤーに判定を伝えるゲームオブジェクト

    public bool button = false;//多分関係無い
    public bool isHolding = false;
    public int holdIndex;

    public Camera raycastCamera;//メインカメラ

    void Start()
    {
        isHolding = false;
    }

    void Update()
    {
        if(gManager.Start)//ゲームスタート
        {
            holdIndex = 0;

            Ray Hray;
            PointerEventData HpointerEventData;
            List<RaycastResult> Hresults;//スタート時に作成

            if (notesManager.SampleNT.Count - 1 > -1)//ノーツがあるなら
            {
                //ロングノーツの始点を判定
                if (Input.GetMouseButtonDown(0))//タップ
                {
                    Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;

                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, results);

                    foreach (var result in results)
                    {
                        if (result.gameObject != null)//当たったオブジェクトがnullじゃなかったら
                        {
                            if (result.gameObject.CompareTag(light.cubeTag0))//左端のレーン 格レーンについたタグ(canvasのbutton（透明）についてる)に当たったら
                            {
                                JCheck(0);//レーンが正しいか判断する関数
                            }
                            if (result.gameObject.CompareTag(light.cubeTag1))
                            {
                                JCheck(1);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag2))
                            {
                                JCheck(2);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag3))
                            {
                                JCheck(3);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag4))
                            {
                                JCheck(4);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag5))
                            {
                                JCheck(5);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag6))//右端
                            {
                                JCheck(6);
                            }
                        }

                    }


                }
                if (notesManager.SampleNT.Count - 1 > -1)
                {
                    if (Time.time > notesManager.SampleNT[0] + 0.2f + gManager.StartTime)//本来ノーツをたたくべき時間から0.2秒たっても入力がなかった場合
                    {
                        message(3);
                        RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
                        gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
                        deleteData();
                        Debug.Log("Miss");
                        gManager.miss++;
                        gManager.combo = 0;
                        //ミス
                    }
                }


            }//notesManager.SampleNT.Count - 1 > -1

            if (notesManager.MiddleLN.Count > 0)//中間点，終点の判定
            {
                if (Input.GetMouseButtonUp(0))//指を離す
                {
                    Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;

                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, results);

                    foreach (var result in results)
                    {
                        if (result.gameObject != null)
                        {
                            if (result.gameObject.CompareTag(light.cubeTag0))//0レーン
                            {
                                Check(0);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag1))//1レーン
                            {
                                Check(1);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag2))//2レーン
                            {
                                Check(2);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag3))//3レーン
                            {
                                Check(3);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag4))//4レーン
                            {
                                Check(4);
                               
                            }
                            if (result.gameObject.CompareTag(light.cubeTag5))//5レーン
                            {
                                Check(5);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag6))//6レーン
                            {
                                Check(6);
                            }
                        }

                    }





                }//(Input.GetMouseButtonUp(0))//指を離す

                if (notesManager.MiddleNT.Count - 1 > -1)
                {
                    if (Time.time > notesManager.MiddleNT[0] + 0.2f + gManager.StartTime)//本来ノーツをたたくべき時間から0.2秒たっても入力がなかった場合
                    {
                        message(3);
                        RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
                        gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
                        
                        MEdeleteData(0);
                        Debug.Log("Miss");
                        gManager.miss++;
                        gManager.combo = 0;
                        //ミス
                    }
                }

                if (Input.GetMouseButtonDown(0))//指が触れている間ずっと
                {
                    isHolding = true;
                   

                }
                else if(Input.GetMouseButtonUp(0))
                {
                    isHolding = false;
                   
                }

                if (isHolding)//指が触れている
                {
                   
                    Hray = raycastCamera.ScreenPointToRay(Input.mousePosition);

                    HpointerEventData = new PointerEventData(EventSystem.current);
                    HpointerEventData.position = Input.mousePosition;

                    Hresults = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(HpointerEventData, Hresults);

                    foreach (var result in Hresults)
                    {
                        if (result.gameObject != null)
                        {
                            if (result.gameObject.CompareTag(light.cubeTag0))//0レーン
                            {
                                CheckLaneHit(0);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag1))//1レーン
                            {
                                CheckLaneHit(1);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag2))//2レーン
                            {
                                CheckLaneHit(2);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag3))//3レーン
                            {
                                CheckLaneHit(3);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag4))//4レーン
                            {
                                CheckLaneHit(4);

                            }
                            if (result.gameObject.CompareTag(light.cubeTag5))//5レーン
                            {
                                CheckLaneHit(5);
                            }
                            if (result.gameObject.CompareTag(light.cubeTag6))//6レーン
                            {
                                CheckLaneHit(6);
                            }
                        }
                    }
                }

            }//(notesManager.MiddleLN.Count - 1 > -1)//中間点，終点の判定

          
        }
       

    }//Update

    public void JCheck(int lane)//押された 始点
    {
        if (notesManager.SampleLN[0] == lane)//レーンがあってる
        {
           
            Judgement(GetABS(Time.time - (notesManager.SampleNT[0] + GManager.instance.StartTime)));//押された時間と，叩くべき時間(以下Ptim)
        }
        else
        {
            if (notesManager.SampleNT.Count - 1 > 0)//これいらないね
            {
               
                if (notesManager.SampleLN[1] == lane)//同時押しに対応(2点タップまで)
                {
                    
                    Judgement(GetABS(Time.time - (notesManager.SampleNT[1] + GManager.instance.StartTime)));
                }
            }
        }
    }

    public void Check(int lane)//指を離す
    {
        if (notesManager.MiddleLN[0] == lane)
        {
            
            MEjudgement(GetABS(Time.time - (notesManager.MiddleNT[0] + GManager.instance.StartTime)), 0);
        }
        else
        {
            if (notesManager.MiddleNT.Count - 1 > 0)
            {
                if (notesManager.MiddleLN[1] == lane)
                {
                    
                    MEjudgement(GetABS(Time.time - (notesManager.MiddleNT[1] + GManager.instance.StartTime)),0);
                }
            }
        }
    }


    public void CheckLaneHit(int laneIndex)//指が触れている時
    {
        if (notesManager.MiddleLN[0] == laneIndex)
        {
            
            if (0.0250 >= GetABS(Time.time - (notesManager.MiddleNT[0] + GManager.instance.StartTime)))//誤差を考慮 Ptimまで指が触れていたらperfectd
            {
                MEjudgement(0, laneIndex);
               
            }
            
        }
        else
        {


            if (notesManager.MiddleLN[1] == laneIndex) 
            {
                

                if (0.0250 >= GetABS(Time.time - (notesManager.MiddleNT[1] + GManager.instance.StartTime))) 
                {
                    MEjudgement(0, laneIndex);
                        
                }
            }
            
        }

        if(notesManager.LongSMNT[laneIndex].Count > 0)
        {
            ChangeLayer(GetABS(Time.time - (notesManager.LongSMNT[laneIndex][0] + GManager.instance.StartTime)), laneIndex);
        }

       

    }
    

    public void Judgement(float timeLag)
    {
        GetComponent<AudioSource>().PlayOneShot(longhitSound);
        if (timeLag <= 0.05)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.1秒以下だったら
        {
            Debug.Log("Perfect");
            message(0);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
            
            gManager.perfect++;
            gManager.combo++;
            deleteData();
            return;
        }
        
        if (timeLag <= 0.10)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.15秒以下だったら
        {
            Debug.Log("Great");
            message(1);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
           
            gManager.great++;
            gManager.combo++;
            deleteData();
            return;
        }
           
        if (timeLag <= 0.15)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.2秒以下だったら
        {
            Debug.Log("Bad");
            message(2);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
           
            gManager.bad++;
            gManager.combo = 0;
            deleteData();
            return;
        }
            
        
    }

    public void MEjudgement(float timeLag, int Index)
    {
        GetComponent<AudioSource>().PlayOneShot(longhitSound);
        if (timeLag <= 0.05)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.05秒以下だったら
        {
            Debug.Log("Perfect");
            message(0);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
           
            gManager.perfect++;
            gManager.combo++;
            MEdeleteData(Index);
            return;
        }
        
        if (timeLag <= 0.10)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.10秒以下だったら
        {
            Debug.Log("Great");
            message(1);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
           
            gManager.great++;
            gManager.combo++;
            MEdeleteData(Index);
            return;
        }
           
        if (timeLag <= 0.15)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.15秒以下だったら
        {
            Debug.Log("Bad");
            message(2);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
            
            gManager.bad++;
            gManager.combo = 0;
            MEdeleteData(Index);
            return;
                
        }
        //解決次第離すのが早すぎたときの判定を作成


    }
    public void ChangeLayer(float timeLag ,int laneindex)//帯を見切れるようにする
    {
        if (notesManager.LongSMNT[laneindex].Count < 1) return;

        if(timeLag <= 0.15)
        {
            notesManager.QuadA[laneindex][0].layer = 3;
            DeleteCLdata(laneindex);
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
        notesManager.SampleNT.RemoveAt(0);
        notesManager.SampleLN.RemoveAt(0);

        gManager.score = (int)gManager.ratioScore;
        //↑new
        Jjudge.comboText.text = gManager.combo.ToString();//new!
        Jjudge.scoreText.text = gManager.score.ToString();//new!
    }

    public void MEdeleteData(int LaneIndex)
    {
        notesManager.MiddleNT.RemoveAt(0);
        notesManager.MiddleLN.RemoveAt(0);

       
    }

    public void DeleteCLdata(int laneindex)
    {
        notesManager.LongSMNT[laneindex].RemoveAt(0);
        notesManager.QuadA[laneindex].RemoveAt(0);
    }

    void message(int judge)//判定を表示する
    {
        Jjudge.MsObj.Add(Instantiate(MessageObj[judge], new Vector3(0, 2.530983f, 1.5f), Quaternion.Euler(45, 0, 0)));
    }
}
