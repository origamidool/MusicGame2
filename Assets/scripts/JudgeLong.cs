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
   
    public Camera raycastCamera;//メインカメラ

    void Start()
    {
        isHolding = false;
    }

    void Update()
    {
        if (!gManager.Start) return;//ゲームスタート

        StartJudge();
        MiddleJudge();
        EndJudge(isHolding);


        for(int i = 0; i < notesManager.LongMNT.Length; i++)//全てのレーンでミス判定のものがないか探す
        {
            if (notesManager.LongMNT[i].Count > 0 && Time.time > notesManager.LongMNT[i][0].notestime + 0.2f + gManager.StartTime)
            {
                HandleMiss(i, notesManager.LongMNT[i][0].lane);
            }
        }

    }

    public void StartJudge()
    {
        if (notesManager.SampleNT.Count > 0)//ノーツがあるなら
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
            if (notesManager.SampleNT.Count > 0)
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
    }

    public void MiddleJudge()
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
    }
    public void EndJudge(bool isHolding)
    {
        Ray Hray;
        PointerEventData HpointerEventData;
        List<RaycastResult> Hresults;//スタート時に作成

        if (Input.GetMouseButtonDown(0))//指が触れている間ずっと
        {
            isHolding = true;


        }
        else if (Input.GetMouseButtonUp(0))
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
    }



    public void HandleMiss(int LaneIndex, int Qlane)
    {
        message(3);

        RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
        gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入

        MEdeleteData(LaneIndex);
        Debug.Log("Miss");
        gManager.miss++;
        gManager.combo = 0;
        RemoveQuad(Qlane);//帯の始点のレーンを入れて正しい帯を削除
        //ミス
    }


    public void JCheck(int lane)//押された 始点
    {
        if (notesManager.SampleLN[0] == lane)//レーンがあってる
        {
           
            Judgement(GetABS(Time.time - (notesManager.SampleNT[0] + GManager.instance.StartTime)));//押された時間と，叩くべき時間(以下Ptim)
        }
        else
        {
           
               
                if (notesManager.SampleLN[1] == lane)//同時押しに対応(2点タップまで)
                {
                    
                    Judgement(GetABS(Time.time - (notesManager.SampleNT[1] + GManager.instance.StartTime)));
                }
            
        }
    }

    public void Check(int lane)//指を離す
    {
        if (notesManager.LongMNT[lane].Count < 1) return;//要素がなかったら帰る
       
        MEjudgement(GetABS(Time.time - (notesManager.LongMNT[lane][0].notestime + GManager.instance.StartTime)), lane);//laneは帯の終点のレーン
       
    }


    public void CheckLaneHit(int laneIndex)//指が触れている時
    {
        if (notesManager.LongMNT[laneIndex].Count > 0)
        {
            
            if (0.0250 >= GetABS(Time.time - (notesManager.LongMNT[laneIndex][0].notestime + GManager.instance.StartTime)))//誤差を考慮 Ptimまで指が触れていたらperfectd
            {
                MEjudgement(0, laneIndex);//laneIndexは帯の終点のレーン
                

               
            }
            
        }

        if (notesManager.LongMNT[laneIndex].Count < 1) return;
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
        if (timeLag <= 0.15)
        {
            GetComponent<AudioSource>().PlayOneShot(longhitSound);
        }
        
        if (timeLag <= 0.05)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.05秒以下だったら
        {
            Debug.Log("Perfect");
            message(0);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
           
            gManager.perfect++;
            gManager.combo++;
            RemoveQuad(notesManager.LongMNT[Index][0].lane);
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
            RemoveQuad(notesManager.LongMNT[Index][0].lane);
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
            RemoveQuad(notesManager.LongMNT[Index][0].lane);
            MEdeleteData(Index);
            
            return;
                
        }
       


    }
    public void ChangeLayer(float timeLag ,int laneindex)//帯を見切れるようにする
    {
        if (notesManager.QuadA[laneindex].Count < 1) return;//無くても動作するはず

        if(timeLag <= 0.15)
        {
            notesManager.QuadA[laneindex][0].layer = 3;
            notesManager.LongSMNT[laneindex].RemoveAt(0);
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
        if (notesManager.LongMNT[LaneIndex].Count < 1) return;

        notesManager.LongMNT[LaneIndex].RemoveAt(0);
       
       
    }

 

    public void RemoveQuad(int LaneIndex)
    {
        if (notesManager.QuadA[LaneIndex].Count > 0)
        {
            notesManager.QuadA[LaneIndex].RemoveAt(0);
        }
    }

    void message(int judge)//判定を表示する
    {
        Jjudge.MsObj.Add(Instantiate(MessageObj[judge], new Vector3(0, 2.530983f, 1.5f), Quaternion.Euler(45, 0, 0)));
    }
}
