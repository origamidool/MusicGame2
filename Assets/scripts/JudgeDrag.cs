using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class JudgeDrag : MonoBehaviour
{
    public GManager gManager;
    public NotesManager notesManager;
    public Light Llight;
    public Judge Jjudge;

    
    CalculateScore calculate = new CalculateScore();
    public Camera raycastCamera;//メインカメラ

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
        for (int i = 0; i < 7; i++)
        {
            if (notesManager.SrideTime[i].Count > 0)
            {
                if (0.000f <= Time.time - (notesManager.SrideTime[i][0] + GManager.instance.StartTime)) continue;
                {
                    if (0.01500f >= Time.time - (notesManager.SrideTime[i][0] + GManager.instance.StartTime))
                    {
                        Check(i);
                    }
                }
            }

           
        }
    }
    private void SearchMiss()
    {
        if (gManager.AutoPlay) return;
        for (int i = 0; i < notesManager.SrideTime.Length; i++)
        {
            if (notesManager.SrideTime[i].Count <= 0) continue;
            if (notesManager.SrideTime[i].Count > 0 && Time.time > notesManager.SrideTime[i][0] + 0.15f + gManager.StartTime)
            {
                HandleMiss(i);
            }
        }
    }
    private void HandleMiss(int laneindex)
    {
        message(3);
        gManager.combo = 0;

        gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//小数点以下を四捨五入
        DeleteSride(laneindex);
        Debug.Log("Miss");
        gManager.miss++;
        
        //ミス
    }

    private void ProcessInput()
    {
        if (gManager.AutoPlay) return;
        Ray ray;
        PointerEventData pointerEventData;
        List<RaycastResult> results;

        for (int id = 0; id < Input.touchCount; id++)
        {
            Touch touch = Input.GetTouch(id);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                ray = raycastCamera.ScreenPointToRay(touch.position);

                pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touch.position;

                results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);



                foreach (var result in results)
                {
                    if (result.gameObject != null)//当たったオブジェクトがnullじゃなかったら
                    {
                        if (result.gameObject.CompareTag(GetComponent<Light>().cubeTag0))//0レーン
                        {
                            Check(0);
                            break;
                        }
                        else if (result.gameObject.CompareTag(GetComponent<Light>().cubeTag1))//1レーン
                        {
                            Check(1);
                            break;
                        }
                        else if (result.gameObject.CompareTag(GetComponent<Light>().cubeTag2))//2レーン
                        {
                            Check(2);
                            break;
                        }
                        else if (result.gameObject.CompareTag(GetComponent<Light>().cubeTag3))//3レーン
                        {
                            Check(3);
                            break;
                        }
                        else if (result.gameObject.CompareTag(GetComponent<Light>().cubeTag4))//4レーン
                        {
                            Check(4);
                            break;
                        }
                        else if (result.gameObject.CompareTag(GetComponent<Light>().cubeTag5))//5レーン
                        {
                            Check(5);
                            break;
                        }
                        else if (result.gameObject.CompareTag(GetComponent<Light>().cubeTag6))//6レーン
                        {
                            Check(6);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void Check(int laneindex)
    {
        if (notesManager.SrideTime[laneindex].Count <= 0) return;//要素がなかったら帰る

        if (0.000f > Time.time - (notesManager.SrideTime[laneindex][0] + GManager.instance.StartTime)) return;

        if (0.0500f >= Time.time - (notesManager.SrideTime[laneindex][0] + GManager.instance.StartTime))
        {
            Debug.Log("Perfect");
            message(0);
            gManager.perfect++;
            gManager.combo++;
            if (gManager.MC < gManager.combo)//MaxComboを設定
            {
                gManager.MC = gManager.combo;
            }

            gManager.ratioScore = calculate.PhiScore(gManager.perfect, gManager.great, notesManager.noteNum, gManager.MC);//スコア計算

            
            DeleteSride(laneindex);
            return;
        }
        else if (0.1000f >= Time.time - (notesManager.SrideTime[laneindex][0] + GManager.instance.StartTime))
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

            
            DeleteSride(laneindex);
        }
        else if (0.1500f >= Time.time - (notesManager.SrideTime[laneindex][0] + GManager.instance.StartTime))
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

            
            DeleteSride(laneindex);
            return;
        }

    }
    private void DeleteSride(int laneindex)
    {
        if (notesManager.SrideTime[laneindex].Count <= 0) return;
        notesManager.SrideObject[laneindex][0].SetActive(false);
        notesManager.SrideObject[laneindex].RemoveAt(0);
        notesManager.SrideTime[laneindex].RemoveAt(0);
    }
    void message(int judge)//判定を表示する
    {
        Jjudge.MsObj.Add(Instantiate(Jjudge.MessageObj[judge], new Vector3(0, 2.530983f, 1.5f), Quaternion.Euler(45, 0, 0)));
    }
}
