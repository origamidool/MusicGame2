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
   
    public float lastTouchtime = -1.0f;

    [SerializeField] AudioClip longhitSound;
    [SerializeField] private GameObject[] MessageObj;//プレイヤーに判定を伝えるゲームオブジェクト

  
    public Camera raycastCamera;//メインカメラ




    Dictionary<int, QuadInfo> touchStart = new Dictionary<int, QuadInfo>()
    {
        {0, null },
        {2, null },
        {3, null },
        {4, null },
        {5, null },
        
    };
    
    Dictionary<int, bool> isTouching = new Dictionary<int, bool>()
    {
        {0,false },
        {1,false },
        {2,false },
        {3,false },
        {4,false },
    };


    void Update()
    {
        if (!gManager.Start) return;//ゲームスタート

      
        /*
        StartJudge();
        
        MiddleJudge(isHolding);
        
        EndJudge();
        */
        SearchMiss();

        ProcessInput();
        
        
    }

    public void ProcessInput()
    {
        

        Ray Hray;
        PointerEventData HpointerEventData;
        List<RaycastResult> Hresults;

        for (int id = 0; id < Input.touchCount; id++)
        {
            Touch touch = Input.GetTouch(id);

            if (touch.phase == TouchPhase.Began)
            {
                isTouching[id] = true;

                Ray ray = raycastCamera.ScreenPointToRay(touch.position);

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touch.position;

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
                        else if (result.gameObject.CompareTag(light.cubeTag1))
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
            if(touch.phase == TouchPhase.Ended)
            {
                isTouching[id] = false;

                Ray ray = raycastCamera.ScreenPointToRay(touch.position);

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touch.position;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (var result in results)
                {
                    if (result.gameObject != null)
                    {
                        if (result.gameObject.CompareTag(light.cubeTag0))//0レーン
                        {
                            Check(0,id);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag1))//1レーン
                        {
                            Check(1,id);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag2))//2レーン
                        {
                            Check(2,id);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag3))//3レーン
                        {
                            Check(3,id);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag4))//4レーン
                        {
                            Check(4,id);

                        }
                        else if (result.gameObject.CompareTag(light.cubeTag5))//5レーン
                        {
                            Check(5,id);
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag6))//6レーン
                        {
                            Check(6,id);
                        }
                    }
                }
            }
            if (isTouching[id])
            {
                Hray = raycastCamera.ScreenPointToRay(touch.position);

                HpointerEventData = new PointerEventData(EventSystem.current);
                HpointerEventData.position = touch.position;

                Hresults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(HpointerEventData, Hresults);

               

                foreach (var result in Hresults)
                {
                    if (result.gameObject != null)//当たったオブジェクトがnullじゃなかったら
                    {
                        if (result.gameObject.CompareTag(light.cubeTag0))//0レーン
                        {
                            CheckLayer(0, id);
                            CheckLaneHit(0,id);
                           
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag1))//1レーン
                        {
                            CheckLayer(1, id);
                            CheckLaneHit(1,id);
                            
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag2))//2レーン
                        {
                            CheckLayer(2, id);
                            CheckLaneHit(2,id);
                           
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag3))//3レーン
                        {
                            CheckLayer(3, id);
                            CheckLaneHit(3,id);
                           
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag4))//4レーン
                        {
                            CheckLayer(4, id);
                            CheckLaneHit(4,id);
                            

                        }
                        else if (result.gameObject.CompareTag(light.cubeTag5))//5レーン
                        {
                            CheckLayer(5, id);
                            CheckLaneHit(5,id);
                            
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag6))//6レーン
                        {
                            CheckLayer(6, id);
                            CheckLaneHit(6,id);
                            

                        }
                    }
                }
            }
        }
    }

    public void SearchMiss()
    {
        for (int i = 0; i < notesManager.dataLists.StartL.Length; i++)
        {
            if (notesManager.dataLists.StartL[i].Count > 0 && Time.time > notesManager.dataLists.StartL[i][0] + 0.15f + gManager.StartTime)
            {
                StartMiss(i);
            }
        }

        for (int i = 0; i < notesManager.dataLists.LongMNT.Length; i++)//全てのレーンでミス判定のものがないか探す
        {
            if (notesManager.dataLists.LongMNT[i].Count > 0 && Time.time > notesManager.dataLists.LongMNT[i][0].notestime + 0.15f + gManager.StartTime)
            {
                HandleMiss(i);
            }
            for (int p = 0; p < notesManager.dataLists.LongSMNT[i].Count; p++)
            {
                if (notesManager.dataLists.LongSMNT[i].Count > 0 && Time.time > notesManager.dataLists.LongSMNT[i][p] + 0.15f + gManager.StartTime)
                {
                    notesManager.dataLists.QuadA[i].RemoveAt(p);
                    notesManager.dataLists.LongSMNT[i].RemoveAt(p);
                }
            }

        }
        for (int i = 0; i < notesManager.dataLists.LongSMNT.Length; i++)
        {
            for(int j = 0; j < notesManager.dataLists.LongSMNT[i].Count; j++)
            {
                if (notesManager.dataLists.LongSMNT[i].Count > 0 && Time.time > notesManager.dataLists.LongSMNT[i][j] + 0.15f + gManager.StartTime)
                {
                    notesManager.dataLists.QuadA[i][j].Quad.SetActive(false);
                    
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

    public void HandleMiss(int LaneIndex)
    {
        message(3);

        RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
        gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入

        
        MEdeleteData(LaneIndex);
        Debug.Log("Miss");
        gManager.miss++;
        gManager.combo = 0;
        
        //ミス
    }

    public void CheckLayer(int laneIndex, int id)
    {
        if (notesManager.dataLists.QuadA[laneIndex].Count < 1) return;

        for (int i = 0; i < notesManager.dataLists.QuadA[laneIndex].Count; i++)
        {
            if (notesManager.dataLists.QuadA[laneIndex][i].Quad.layer == 0)
            {
                if (notesManager.dataLists.LongSMNT[laneIndex].Count < 1) return;
                ChangeLayer(GetABS(Time.time - (notesManager.dataLists.LongSMNT[laneIndex][i] + GManager.instance.StartTime)), laneIndex, i, id);
                return;
            }
        }

    }

    public void ChangeLayer(float timeLag, int laneindex, int i, int id)//帯を見切れるようにする
    {
        if (timeLag <= 0.15)
        {
            notesManager.dataLists.QuadA[laneindex][i].Quad.layer = 3;
            touchStart[id] = notesManager.dataLists.QuadA[laneindex][i];
            if (touchStart[id] != null) Debug.Log("触れたQuadの終点" + touchStart[id].endlane);

        }
    }
    public void JCheck(int laneindex)//押された 始点
    {
        if (notesManager.dataLists.StartL[laneindex].Count < 1) return;

        Judgement(GetABS(Time.time - (notesManager.dataLists.StartL[laneindex][0] + GManager.instance.StartTime)), laneindex);//押された時間と，叩くべき時間(以下Ptim)

    }

    public void Check(int lane,int id)//指を離す
    {
        if (!touchStart.ContainsKey(key: id)) return;
        if (touchStart[id] == null) return;
        if (notesManager.dataLists.LongMNT[touchStart[id].endlane].Count < 1) return;//要素がなかったら帰る
       
        MEjudgement(GetABS(Time.time - (notesManager.dataLists.LongMNT[touchStart[id].endlane][0].notestime + GManager.instance.StartTime)), lane,id);//laneは帯の終点のレーン
       
    }


    public void CheckLaneHit(int laneIndex,int id)//指が触れている時
    {
        if (notesManager.dataLists.LongMNT[laneIndex].Count > 0)
        {
            
            if (0.0250 >= GetABS(Time.time - (notesManager.dataLists.LongMNT[laneIndex][0].notestime + GManager.instance.StartTime)))//誤差を考慮 Ptimまで指が触れていたらperfectd
            {
                MEjudgement(0, laneIndex, id);//laneIndexは帯の終点のレーン
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

    public void MEjudgement(float timeLag, int Index,int id)
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
            DeleteQuad(notesManager.dataLists.LongMNT[Index][0].lane, notesManager.dataLists.LongMNT[Index][0].notestime);
            MEdeleteData(touchStart[id].endlane);
            
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
            DeleteQuad(notesManager.dataLists.LongMNT[Index][0].lane, notesManager.dataLists.LongMNT[Index][0].notestime);
            MEdeleteData(touchStart[id].endlane);
            
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
           
            DeleteQuad(notesManager.dataLists.LongMNT[Index][0].lane, notesManager.dataLists.LongMNT[Index][0].notestime);
            MEdeleteData(touchStart[id].endlane);
            
            return;
                
        }

        if(Time.time - GManager.instance.StartTime > touchStart[id].starttime)
        {
            InTheMiddle(notesManager.dataLists.LongMNT[touchStart[id].endlane][0].notestime, id);
            message(3);

            RawScore = (gManager.perfect + gManager.great * 0.65f) / notesManager.noteNum * 900000 + Jjudge.MC / notesManager.noteNum * 100000;
            gManager.ratioScore = (float)Math.Round((float)RawScore, 0, MidpointRounding.AwayFromZero);//小数点以下を四捨五入


            MEdeleteData(touchStart[id].endlane);
            Debug.Log("Miss");
            gManager.miss++;
            gManager.combo = 0;

            //ミス
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

    public void deleteData(int laneindex)//すでにたたいたノーツを削除する関数
    {
        notesManager.dataLists.StartL[laneindex].RemoveAt(0);
        

        gManager.score = (int)gManager.ratioScore;
        //↑new
        Jjudge.comboText.text = gManager.combo.ToString();//new!
        Jjudge.scoreText.text = gManager.score.ToString();//new!
    }

    public void MEdeleteData(int LaneIndex)
    {
        if (notesManager.dataLists.LongMNT[LaneIndex].Count < 1) return;

        notesManager.dataLists.LongMNT[LaneIndex].RemoveAt(0);
       
       
    }

 

    
    public void DeleteQuad(int LaneIndex, float endtime)//帯の始点すら触られなかったらその帯を削除　
    {
        for (int i = 0; i < notesManager.dataLists.QuadA[LaneIndex].Count; i++)
        {
            if(endtime == notesManager.dataLists.QuadA[LaneIndex][i].endtime)
            {
                notesManager.dataLists.QuadA[LaneIndex][i].Quad.SetActive(false);
                notesManager.dataLists.QuadA[LaneIndex].RemoveAt(i);
                notesManager.dataLists.LongSMNT[LaneIndex].RemoveAt(i);
            }
        }

    }
    
    
    public void InTheMiddle(float endtime,int id)
    {
        for (int i = 0; i < notesManager.dataLists.QuadA[touchStart[id].startlane].Count; i++)
        {
            int startlane = touchStart[id].startlane;
            if (touchStart[id].starttime == notesManager.dataLists.QuadA[startlane][i].starttime && touchStart[id].endlane == notesManager.dataLists.QuadA[startlane][i].endlane && touchStart[id].endtime == notesManager.dataLists.QuadA[startlane][i].endtime)
            {
                notesManager.dataLists.QuadA[startlane][i].Quad.SetActive(false);
                notesManager.dataLists.QuadA[startlane].RemoveAt(i);
                notesManager.dataLists.LongSMNT[startlane].RemoveAt(i);
            }
        }

    }
    

    void message(int judge)//判定を表示する
    {
        Jjudge.MsObj.Add(Instantiate(MessageObj[judge], new Vector3(0, 2.530983f, 1.5f), Quaternion.Euler(45, 0, 0)));
    }
}
