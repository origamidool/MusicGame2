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
    public float rayInterval = 0.02f;//難病おきに飛ばすか
    public float lastTouchtime = -1.0f;

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

        if (Input.GetMouseButtonDown(0))//指が触れている間ずっと
        {
            isHolding = true;


        }
        else if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;

        }

        StartJudge();
        MiddleJudge(isHolding);
        EndJudge();

        for (int i = 0; i < notesManager.StartL.Length; i++)
        {
            if (notesManager.StartL[i].Count > 0 && Time.time > notesManager.StartL[i][0] + 0.2f + gManager.StartTime)
            {
                StartMiss(i);
            }
        }

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
                        else if(result.gameObject.CompareTag(light.cubeTag1))
                        {
                            JCheck(1);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag2))
                        {
                            JCheck(2);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag3))
                        {
                            JCheck(3);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag4))
                        {
                            JCheck(4);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag5))
                        {
                            JCheck(5);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag6))//右端
                        {
                            JCheck(6);
                        }
                    }

                }


            }
            
    }

    public void EndJudge()
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
    public void MiddleJudge(bool isHolding)
    {
        Ray Hray;
        PointerEventData HpointerEventData;
        List<RaycastResult> Hresults;//スタート時に作成

        

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
                        CheckLayer(0);
                    }
                    if (result.gameObject.CompareTag(light.cubeTag1))//1レーン
                    {
                        CheckLaneHit(1);
                        CheckLayer(1);
                    }
                    if (result.gameObject.CompareTag(light.cubeTag2))//2レーン
                    {
                        CheckLaneHit(2);
                        CheckLayer(2);
                    }
                    if (result.gameObject.CompareTag(light.cubeTag3))//3レーン
                    {
                        CheckLaneHit(3);
                        CheckLayer(3);
                    }
                    if (result.gameObject.CompareTag(light.cubeTag4))//4レーン
                    {
                        CheckLaneHit(4);
                        CheckLayer(4);

                    }
                    if (result.gameObject.CompareTag(light.cubeTag5))//5レーン
                    {
                        CheckLaneHit(5);
                        CheckLayer(5);
                    }
                    if (result.gameObject.CompareTag(light.cubeTag6))//6レーン
                    {
                        CheckLaneHit(6);
                        CheckLayer(6);

                    }
                }
                
            }
        }
    }



    public void StartMiss(int laneindex)
    {
        message(3);
        RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
        gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
        deleteData(laneindex);
        Debug.Log("Miss");
        gManager.miss++;
        gManager.combo = 0;
        //ミス
    }

    public void HandleMiss(int LaneIndex, int Qlane)
    {
        message(3);

        RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
        gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入

        notesManager.LongSMNT[Qlane].RemoveAt(0);
        MEdeleteData(LaneIndex);
        Debug.Log("Miss");
        gManager.miss++;
        gManager.combo = 0;
        
        //ミス
    }


    public void JCheck(int laneindex)//押された 始点
    {
        if (notesManager.StartL[laneindex].Count < 1) return;

        Judgement(GetABS(Time.time - (notesManager.StartL[laneindex][0] + GManager.instance.StartTime)), laneindex);//押された時間と，叩くべき時間(以下Ptim)

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

    }
    public void CheckLayer(int laneIndex)
    {
        if (notesManager.QuadA[laneIndex].Count < 1) return;

        for (int i = 0; i < notesManager.QuadA[laneIndex].Count; i++)
        {
            if (notesManager.QuadA[laneIndex][i].Quad.layer == 0)
            {
                if (notesManager.LongSMNT[laneIndex].Count < 1) return;
                ChangeLayer(GetABS(Time.time - (notesManager.LongSMNT[laneIndex][i] + GManager.instance.StartTime)), laneIndex, i);
                return;
            }
        }
        
    }
    

    public void Judgement(float timeLag, int laneindex)
    {
        if (timeLag > 0.15f) return;
        GetComponent<AudioSource>().PlayOneShot(longhitSound);
        if (timeLag <= 0.05)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.1秒以下だったら
        {
            Debug.Log("Perfect");
            message(0);
            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入
            
            gManager.perfect++;
            gManager.combo++;
            deleteData(laneindex);
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
            deleteData(laneindex);
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
            deleteData(laneindex);
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
            DeleteQuad(notesManager.LongMNT[Index][0].lane, notesManager.LongMNT[Index][0].notestime);
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
            DeleteQuad(notesManager.LongMNT[Index][0].lane, notesManager.LongMNT[Index][0].notestime);
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
            DeleteQuad(notesManager.LongMNT[Index][0].lane, notesManager.LongMNT[Index][0].notestime);
            MEdeleteData(Index);
            
            return;
                
        }
       


    }
    public void ChangeLayer(float timeLag , int laneindex , int i)//帯を見切れるようにする
    {
        if(timeLag <= 0.15)
        {
            notesManager.QuadA[laneindex][i].Quad.layer = 3;
            notesManager.LongSMNT[laneindex].RemoveAt(i);
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

    public void deleteData(int laneindex)//すでにたたいたノーツを削除する関数
    {
        notesManager.StartL[laneindex].RemoveAt(0);
        

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

 

    
    public void DeleteQuad(int LaneIndex, float endtime)//帯の始点すら触られなかったらその帯を削除　始点は別で削除される
    {
        for (int i = 0; i < notesManager.QuadA[LaneIndex].Count; i++)
        {
            if(endtime == notesManager.QuadA[LaneIndex][i].endtime)
            {
                notesManager.QuadA[LaneIndex][i].Quad.SetActive(false);
                notesManager.QuadA[LaneIndex].RemoveAt(i);
                
            }
        }

    }

    void message(int judge)//判定を表示する
    {
        Jjudge.MsObj.Add(Instantiate(MessageObj[judge], new Vector3(0, 2.530983f, 1.5f), Quaternion.Euler(45, 0, 0)));
    }
}
