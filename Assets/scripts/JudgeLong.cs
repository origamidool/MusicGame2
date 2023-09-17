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

    private bool cL = false;

  
    public Camera raycastCamera;//メインカメラ

    CalculateScore calculate = new CalculateScore();

    float Ptim;
    float Gtim;
    float Btim;

    Dictionary<int, QuadInfo> touchStart = new Dictionary<int, QuadInfo>()
    {
        {0, null },
        {1, null },
        {2, null },
        {3, null },
        {4, null },
        {5, null },
        {6, null },
        {7, null },
        {8, null },
        {9, null },
        {10, null },
    };
    
   
    Dictionary<int, bool> isAdded = new Dictionary<int, bool>()
    {
        {0,false },
        {1,false },
        {2,false },
        {3,false },
        {4,false },
        {6,false },
        {7,false },
        {8,false },
        {9,false },
        {10,false },
    };

    void Start()
    {
        Ptim = gManager.Ptim;
        Gtim = gManager.Gtim;
        Btim = gManager.Btim;
    }

    void Update()
    {
        if (!gManager.Start) return;//ゲームスタート
        


        SearchMiss();

        ProcessInput();

        AutoPlay();
    }
    private void AutoPlay()
    {
        if (!gManager.AutoPlay) return;
        if (!cL)
        {
            cL = true;
            for(int i = 0; i < 7; i++)
            {
                foreach (QuadInfo Quad in notesManager.dataLists.QuadA[i])
                {
                    Quad.Quad.layer = 3;
                }

            }

        }

        for (int i = 0; i < 7; i++)
        {
            
            
            if (notesManager.dataLists.StartL[i].Count >= 1)//始点
            {
                if (0.020000f >= GetABS(Time.time - (notesManager.dataLists.StartL[i][0] + GManager.instance.StartTime)))
                {
                    Judgement(0, i);
                }
            }
            for(int j = 0; j < notesManager.dataLists.LongMNT[i].Count; j++)
            {
                AutoMiddle(i,j);
            }
            
        }
    }
    private void AutoMiddle(int i,int j)
    {
        if (notesManager.dataLists.LongMNT[i].Count < 1) return; //中間点，終点
        else if (0.00000f > Time.time - (notesManager.dataLists.LongMNT[i][j].notestime + GManager.instance.StartTime)) return;
        else if (0.03f >= Time.time - (notesManager.dataLists.LongMNT[i][j].notestime + GManager.instance.StartTime))
        {
            GetComponent<AudioSource>().PlayOneShot(longhitSound);
            Debug.Log("Perfect");


            message(0);
            gManager.perfect++;
            gManager.combo++;

            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算


            MEdeleteData(i,j);
        }
    }

    private void ProcessInput()
    {
        if (gManager.AutoPlay) return;

        Ray Hray;
        PointerEventData HpointerEventData;
        List<RaycastResult> Hresults;

        for (int id = 0; id < Input.touchCount; id++)
        {
            Touch touch = Input.GetTouch(id);

            if (touch.phase == TouchPhase.Began)
            {
                

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
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag1))
                        {
                            JCheck(1);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag2))
                        {
                            JCheck(2);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag3))
                        {
                            JCheck(3);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag4))
                        {
                            JCheck(4);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag5))
                        {
                            JCheck(5);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag6))//右端
                        {
                            JCheck(6);
                            break;
                        }
                    }

                }
            }
            if(touch.phase == TouchPhase.Ended)
            {
                

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
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag1))//1レーン
                        {
                            Check(1,id);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag2))//2レーン
                        {
                            Check(2,id);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag3))//3レーン
                        {
                            Check(3,id);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag4))//4レーン
                        {
                            Check(4,id);
                            break;

                        }
                        else if (result.gameObject.CompareTag(light.cubeTag5))//5レーン
                        {
                            Check(5,id);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag6))//6レーン
                        {
                            Check(6,id);
                            break;
                        }
                    }
                }
            }
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
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
                            break;
                           
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag1))//1レーン
                        {
                            CheckLayer(1, id);
                            CheckLaneHit(1,id);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag2))//2レーン
                        {
                            CheckLayer(2, id);
                            CheckLaneHit(2,id);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag3))//3レーン
                        {
                            CheckLayer(3, id);
                            CheckLaneHit(3,id);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag4))//4レーン
                        {
                            CheckLayer(4, id);
                            CheckLaneHit(4,id);
                            break;

                        }
                        else if (result.gameObject.CompareTag(light.cubeTag5))//5レーン
                        {
                            CheckLayer(5, id);
                            CheckLaneHit(5,id);
                            break;
                        }
                        else if (result.gameObject.CompareTag(light.cubeTag6))//6レーン
                        {
                            CheckLayer(6, id);
                            CheckLaneHit(6,id);
                            break;

                        }
                    }
                }
            }
        }
    }

    public void SearchMiss()
    {
        if (gManager.AutoPlay) return;
        for (int i = 0; i < notesManager.dataLists.StartL.Length; i++)//始点のミスを探す
        {
            if (notesManager.dataLists.StartL[i].Count > 0 && Time.time > notesManager.dataLists.StartL[i][0] + Btim + gManager.StartTime)
            {
                StartMiss(i);
            }
        }

        for (int i = 0; i < notesManager.dataLists.LongMNT.Length; i++)//全てのレーンでミス判定のものがないか探す 中間,終
        {
            for (int j = 0; j < notesManager.dataLists.LongMNT[i].Count; j++)
            {
                if (Time.time > notesManager.dataLists.LongMNT[i][j].notestime + Btim + gManager.StartTime)
                {
                    HandleMiss(i, j);
                }
            }
           
            
        }
        for (int i = 0; i < notesManager.dataLists.LongSMNT.Length; i++)
        {
            for(int j = 0; j < notesManager.dataLists.LongSMNT[i].Count; j++)
            {
                if(Time.time > notesManager.dataLists.LongSMNT[i][j] + Btim + gManager.StartTime)
                {
                    notesManager.dataLists.QuadA[i][j].Quad.SetActive(false);
                    HandleMiss(notesManager.dataLists.QuadA[i][0].endlane,j);
                    DeleteQuadData(i,j);
                }
            }
            
        }
    }
    

  



    public void StartMiss(int laneindex)//始点のミス
    {
        message(3);
        
        gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//小数点以下を四捨五入
        deleteData(laneindex);
        Debug.Log("Miss");
        gManager.miss++;
        gManager.combo = 0;
        //ミス
      
    }

    public void HandleMiss(int LaneIndex,int j)//中間点，終点のミス
    {
        message(3);

        
        gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//小数点以下を四捨五入

        
        MEdeleteData(LaneIndex,j);
        Debug.Log("Miss");
        gManager.miss++;
        gManager.combo = 0;
       
        //ミス
    }

    public void CheckLayer(int laneIndex, int id)//帯のレイヤー変更
    {
        if (notesManager.dataLists.QuadA[laneIndex].Count < 1) return;
        if (notesManager.dataLists.LongSMNT[laneIndex].Count < 1) return;

        for(int j = 0; j < notesManager.dataLists.LongSMNT[laneIndex].Count; j++)
        {
            ChangeLayer(Time.time - (notesManager.dataLists.LongSMNT[laneIndex][j] + GManager.instance.StartTime), laneIndex, id,j);
        }
        
           
    }

    public void ChangeLayer(float timeLag, int laneindex, int id,int j)//帯を見切れるようにする
    {
        if (timeLag > Btim) return;
        if (timeLag < 0.00f) return;
        isAdded[id] = true;
        if (touchStart.ContainsKey(key: id))
        {
            if (touchStart[id] != null)
            {
                if (GetABS(Time.time - (touchStart[id].endtime + gManager.StartTime)) <= 0.050f) isAdded[id] = false;
            
            }
        }

        if (isAdded[id])
        {
            touchStart[id] = notesManager.dataLists.QuadA[laneindex][j];
            touchStart[id].Quad.layer = 3;
            DeleteQuadData(laneindex,j);
            
        }
    }
    public void JCheck(int laneindex)//押された 始点
    {
        if (notesManager.dataLists.StartL[laneindex].Count < 1) return;

        Judgement(GetABS(Time.time - (notesManager.dataLists.StartL[laneindex][0] + GManager.instance.StartTime)), laneindex);//押された時間と，叩くべき時間(以下Ptim)

    }

    public void Check(int lane,int id)//指を離す
    {
        if (!touchStart.ContainsKey(key: id)) return;//キーがあるか
        if (touchStart[id] == null) return;//nullではないか
        if (notesManager.dataLists.LongMNT[touchStart[id].endlane].Count < 1) return;//要素がなかったら帰る
        if (notesManager.dataLists.LongMNT[lane].Count < 1) return;//要素がなかったら帰る

        for(int j = 0; j < notesManager.dataLists.LongMNT[lane].Count; j++)
        {
            MEjudgement(Time.time - (notesManager.dataLists.LongMNT[touchStart[id].endlane][j].notestime + GManager.instance.StartTime), lane, id,j);//laneは帯の終点のレーン

        }

    }


    public void CheckLaneHit(int laneIndex,int id)//指が触れている時
    {
        if (notesManager.dataLists.LongMNT[laneIndex].Count == 0) return;//要素がなかったら帰る
        for(int j = 0; j < notesManager.dataLists.LongMNT[laneIndex].Count; j++)
        {
            if (0.000f <= Time.time - (notesManager.dataLists.LongMNT[laneIndex][j].notestime + GManager.instance.StartTime))
            {
                if (Ptim >= Time.time - (notesManager.dataLists.LongMNT[laneIndex][j].notestime + GManager.instance.StartTime))//誤差を考慮 Ptimまで指が触れていたらperfectd
                {
                    GetComponent<AudioSource>().PlayOneShot(longhitSound);
                    Debug.Log("Perfect");
                    message(0);
                    gManager.perfect++;
                    gManager.combo++;
                    if (gManager.MC < gManager.combo)//MaxComboを設定
                    {
                        gManager.MC = gManager.combo;
                    }

                    gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算


                    MEdeleteData(laneIndex, j);
                }
            }
        }

       
    }
    
    

    public void Judgement(float timeLag, int laneindex)
    {
        if (timeLag > Btim) return;
        GetComponent<AudioSource>().PlayOneShot(longhitSound);
        if (timeLag <= Ptim)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.1秒以下だったら
        {
            Debug.Log("Perfect");
            message(0);
            
            
           

            gManager.perfect++;
            gManager.combo++;
            deleteData(laneindex);
            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
            
            return;
        }
        
        if (timeLag <= Gtim)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.15秒以下だったら
        {
            Debug.Log("Great");
            message(1);
          

            gManager.great++;
            gManager.combo++;
            deleteData(laneindex);
            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算

            return;
        }
           
        if (timeLag <= Btim)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.2秒以下だったら
        {
            Debug.Log("Bad");
            message(2);
            
           
            

            gManager.bad++;
            gManager.combo = 0;
            deleteData(laneindex);
            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
            return;
        }
            
        
    }

    public void MEjudgement(float timeLag, int Index,int id,int j)
    {
        if (timeLag < 0.000f) return;
        if (timeLag <= Btim)
        {
            GetComponent<AudioSource>().PlayOneShot(longhitSound);
        }
        
        if (timeLag <= Ptim)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.05秒以下だったら
        {
            Debug.Log("Perfect");
            message(0);

            gManager.perfect++;
            gManager.combo++;
            
            MEdeleteData(Index,j);
            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
            return;
        }
        
        if (timeLag <= Gtim)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.10秒以下だったら
        {
            Debug.Log("Great");
            message(1);
         
           

            gManager.great++;
            gManager.combo++;
            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
            MEdeleteData(Index,j);
            
            return;
        }
           
        if (timeLag <= Btim)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.15秒以下だったら
        {
            Debug.Log("Bad");
            message(2);
            
            

            gManager.bad++;
            gManager.combo = 0;
            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
            MEdeleteData(Index,j);
            
            return;
                
        }
       

        if(Time.time - GManager.instance.StartTime > touchStart[id].starttime)//正常に動作
        {
            InTheMiddle(id);
            message(3);

            DeleteME(touchStart[id].endlane,id);
            Debug.Log("Miss");
            gManager.miss++;
            gManager.combo = 0;
            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }
            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算
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
        notesManager.dataLists.StartObj[laneindex][0].SetActive(false);
        notesManager.dataLists.StartObj[laneindex].RemoveAt(0);
        notesManager.dataLists.StartL[laneindex].RemoveAt(0);
        

       
    }

    public void MEdeleteData(int LaneIndex,int j)
    {
        if (notesManager.dataLists.LongMNT[LaneIndex].Count < 1) return;
        if (notesManager.dataLists.MEObj[LaneIndex].Count < 1) return;
        notesManager.dataLists.MEObj[LaneIndex][j].SetActive(false);
       
        notesManager.dataLists.MEObj[LaneIndex].RemoveAt(j);

        notesManager.dataLists.LongMNT[LaneIndex].RemoveAt(j);

    }
    public void DeleteME(int laneindex,int id)
    {
        float endtime = touchStart[id].endtime;
        for(int i = 0; i < notesManager.dataLists.LongMNT[laneindex].Count; i++)
        {
            if (notesManager.dataLists.LongMNT[laneindex][i].notestime != endtime) continue;
            notesManager.dataLists.MEObj[laneindex][i].SetActive(false);
            notesManager.dataLists.MEObj[laneindex].RemoveAt(i);
            notesManager.dataLists.LongMNT[laneindex].RemoveAt(i);

           
            break;
        }
    }
 

    
   
    public void DeleteQuadData(int laneindex,int j)
    {
        if (notesManager.dataLists.LongSMNT[laneindex].Count < 1) return;
       

        notesManager.dataLists.QuadA[laneindex].RemoveAt(j);
        notesManager.dataLists.LongSMNT[laneindex].RemoveAt(j);

      
    }
    
    
    public void InTheMiddle(int id)
    {
        touchStart[id].Quad.SetActive(false);
    }
    

    void message(int judge)//判定を表示する
    {
        Jjudge.MsObj.Add(Instantiate(MessageObj[judge], new Vector3(0, 2.530983f, 1.5f), Quaternion.Euler(45, 0, 0)));
    }
}
