using System;
using System.Collections.Generic;
using UnityEngine;

using LitJson;


[System.Serializable]//7月17日
public class JaggedArrayContainer
{

    public string name;
    public SongInfo songInfo;
    public int maxBlock;
    public float bpm;
    public int offset;
    public List<float> bpms;
    public List<float> speeds;
    public SplitData bgChanges;
    public NotesLevel notes;
    public List<List<float>> splitBpms;
    public List<List<float>> splitSpeeds;

}
[System.Serializable]
public class SongInfo
{
    public string title;
    public string subtitle;
    public string artist;
    public string description;
    public string credit;
    public string music;
    public string banner;
    public string bg;
    public string illust;
    public string sampleStart;
    public string sampleLength;
    public string offset;
    public string baseBpm;
    public string cmod;
    public string luaScript;
    // 他に必要なプロパティを追加することができます
}
[System.Serializable]
public class SplitData
{
    public List<List<float>> splitBpms;
    public List<List<float>> splitSpeeds;
}


[System.Serializable]
public class NotesLevel
{
    public Notegpt[] hard;
    public Notegpt[] normal;
    public Notegpt[] easy;
}



[System.Serializable]
public class Notegpt
{
    public int type;
    public int num;
    public int block;
    public int lpb;
    public Notegpt[] notes;
}

public class NoteInfo
{
    public int lane;
    public float notestime;
}
public class QuadInfo
{
    public float starttime;
    public float endtime;
    public int startlane;
    public int endlane;
    public GameObject Quad;
}

[System.Serializable]
public class DataLists//ロングノーツ関連
{
    
    public List<float>[] LongSMNT;//始点と中間点(帯の始点)の時間が入ったレーンごとの配列
    public List<QuadInfo>[] QuadA;
    public List<NoteInfo>[] LongMNT;//中間点と終点(帯の終点)の時間が入ったレーン(中間点，終点の)ごとの配列 notestimeは中間点，終点の時間，laneは帯の始点のレーン
    public List<float>[] StartL;//始点のノーツタイム
    public List<GameObject>[] StartObj;//始点のオブジェクト
    public List<GameObject>[] MEObj;//中間点，終点のオブジェクト
}



public class NotesManager : MonoBehaviour
{
    public string jsonFilePath;//7月17日
    public GManager gManager;
    public int noteNum;//総ノーツ数
    private string songName;

    

    private List<int> N = new List<int>();//インデックスを分けた物をいれる
    private List<int> L = new List<int>();
    private List<int> S = new List<int>();
    private List<int> F = new List<int>();
    private List<int> SL = new List<int>();

    private List<int> Flick = new List<int>();//フリックとスライドロングを分ける
    private List<int> Srlong = new List<int>();

    public List<float>[] NoteTime;//ノーマルノーツの時間　レーンごと
    public List<float>[] SrideTime;//スライドノーツの時間　レーンごと
    public List<float>[] FlickTime;//フリックノーツの時間　レーンごと

    public List<GameObject>[] NoteObject;//ノーマルノーツのオブジェクト　レーンごと
    public List<GameObject>[] SrideObject;//スライドノーツのオブジェクト　レーンごと
    public List<GameObject>[] FlickObject;//フリックノーツのオブジェクト　レーンごと


    [SerializeField] private float NotesSpeed;
    [SerializeField] private GameObject noteObj;

   
    [SerializeField] private GameObject SridenoteObj;//スライドノーツのオブジェクト

   
    [SerializeField] private GameObject FlObj;//フリックノーツのオブジェクト

    [SerializeField] private GameObject SampleLong;//ロングノーツの始点

    public DataLists dataLists;//ロングノーツのデータ

    [SerializeField] private float tapLag = 0;//オフセット調整

    void OnEnable()
    {
        NotesSpeed = gManager.noteSpeed;
        noteNum = 0;
        songName = "Grievous Lady(Arcaea)";
        gManager.Level = 0;
        Load(songName, gManager.Level);

    }

    private int[] type1;



    [SerializeField] JaggedArrayContainer container;

    private Notegpt[] GetLevel(JaggedArrayContainer container, int Level)
    {
        switch(Level)
        {
            case 0:
                return container.notes.hard;
            case 1:
                return container.notes.normal;
            case 2:
                return container.notes.easy;
            default:
                return null;
 
        }
    }

    private void Load(string SongName, int Level)
    {
        jsonFilePath = "Assets/Resources/" + SongName + ".json";
        string jsonString = System.IO.File.ReadAllText(jsonFilePath);
        container = JsonUtility.FromJson<JaggedArrayContainer>(jsonString);

        


        int[] notestype;
        

      

        for (int i = 0; i < GetLevel(container, Level).Length; i++)
        {

            notestype = new int[GetLevel(container, Level).Length];
            notestype[i] = GetLevel(container, Level)[i].type;
            int target = 1;
            int targetL = 2;
            int targetS = 3;
            int targetF = 4;


            int num = Array.IndexOf(notestype, target);

            int numL = Array.IndexOf(notestype, targetL);

            int numS = Array.IndexOf(notestype, targetS);

            int numFF = Array.IndexOf(notestype, targetF);





            while (num >= 0)
            {

                N.Add(num);
                if (num + 1 < notestype.Length)
                {
                    num = Array.IndexOf(notestype, target, num + 1);

                }

                else
                {
                    break;
                }
            }
            while (numL >= 0)
            {
                L.Add(numL);

                if (numL + 1 < notestype.Length)
                {
                    numL = Array.IndexOf(notestype, targetL, numL + 1);
                }
                else
                {
                    break;
                }

            }
            while (numS >= 0)
            {
                S.Add(numS);

                if (numS + 1 < notestype.Length)
                {
                    numS = Array.IndexOf(notestype, targetS, numS + 1);
                }
                else
                {
                    break;
                }

            }

            while (numFF >= 0)
            {
                F.Add(numFF);

                if (numFF + 1 < notestype.Length)
                {
                    numFF = Array.IndexOf(notestype, targetF, numFF + 1);


                }
                else
                {
                    break;
                }

            }




        }

        dataLists.LongSMNT = new List<float>[7];
        dataLists.QuadA = new List<QuadInfo>[7];//new!
        dataLists.LongMNT = new List<NoteInfo>[7];
        dataLists.StartObj = new List<GameObject>[7];
        dataLists.MEObj = new List<GameObject>[7];
        dataLists.StartL = new List<float>[7];

        NoteTime = new List<float>[7];
        SrideTime = new List<float>[7];
        FlickTime = new List<float>[7];

        NoteObject = new List<GameObject>[7];
        SrideObject = new List<GameObject>[7];
        FlickObject = new List<GameObject>[7];

        for (int i = 0; i < 7; i++)
        {
            dataLists.LongSMNT[i] = new List<float>();
            dataLists.QuadA[i] = new List<QuadInfo>();
            dataLists.LongMNT[i] = new List<NoteInfo>();
            dataLists.StartObj[i] = new List<GameObject>();
            dataLists.MEObj[i] = new List<GameObject>();
            dataLists.StartL[i] = new List<float>();

            NoteTime[i] = new List<float>();
            SrideTime[i] = new List<float>();
            FlickTime[i] = new List<float>();

            NoteObject[i] = new List<GameObject>();
            SrideObject[i] = new List<GameObject>();
            FlickObject[i] = new List<GameObject>();
        }




        for (int u = 0; u < N.Count; u++) //タップノーツ
        {

            float Nkankaku = 60 / (container.bpm * (float)GetLevel(container, Level)[N[u]].lpb);
            float NbeatSec = Nkankaku * (float)GetLevel(container, Level)[N[u]].lpb;
            float Ntime = Nkankaku * GetLevel(container, Level)[N[u]].num + container.offset / 44100 + tapLag / 100 + gManager.grace;

            NoteTime[GetLevel(container, Level)[N[u]].block].Add(Ntime);


            float Nz = Ntime * gManager.noteSpeed;

            NoteObject[GetLevel(container, Level)[N[u]].block].Add(Instantiate(noteObj, new Vector3(GetLevel(container, Level)[N[u]].block - 3, 0.55f, Nz), Quaternion.identity));

            if (u == 0)//総ノーツ数を求める為
            {
                noteNum += N.Count;
            }


        }

        for (int a = 0; a < L.Count; a++)//ロングノーツ
        {
            //始点を生成

            float Samplekankaku = 60 / (container.bpm * (float)GetLevel(container, Level)[L[a]].lpb);
            float SampleLtime = Samplekankaku * GetLevel(container, Level)[L[a]].num + container.offset / 44100 + tapLag / 100 + gManager.grace;


            dataLists.StartL[GetLevel(container, Level)[L[a]].block].Add(SampleLtime);

            dataLists.LongSMNT[GetLevel(container, Level)[L[a]].block].Add(SampleLtime);//帯レイヤー変更判定用 始点の時間をいれる


            float Sample_z = (SampleLtime) * gManager.noteSpeed;
            dataLists.StartObj[GetLevel(container, Level)[L[a]].block].Add(Instantiate(SampleLong, new Vector3(GetLevel(container, Level)[L[a]].block - 3, 0.60f, Sample_z), Quaternion.identity));

            Vector3[] upperVec3 = new Vector3[2];
            upperVec3[0] = new Vector3(GetLevel(container, Level)[L[a]].block - 3.5f, 0.549f, Sample_z);
            upperVec3[1] = new Vector3(GetLevel(container, Level)[L[a]].block - 2.5f, 0.549f, Sample_z);

            if (a == 0)
            {
                noteNum += L.Count;
            }

            noteNum += GetLevel(container, Level)[L[a]].notes.Length;




            int index = -1;

            for (int i = 0; i < GetLevel(container, Level)[L[a]].notes.Length; i++)//中間点，終点を生成
            {
                index += 1;
                Vector3[] lowerVec3 = new Vector3[2];
                

                float Middlekankaku = 60 / (container.bpm * (float)GetLevel(container, Level)[L[a]].notes[i].lpb);
                float Middletime = Middlekankaku * GetLevel(container, Level)[L[a]].notes[i].num + container.offset / 44100 + tapLag / 100 + gManager.grace;

                float[] Middletimes = new float[GetLevel(container, Level)[L[a]].notes.Length];
                Middletimes[i] = Middletime;

                float Middle_z = (Middletime) * gManager.noteSpeed;

                dataLists.MEObj[GetLevel(container, Level)[L[a]].notes[i].block].Add(Instantiate(SampleLong, new Vector3(GetLevel(container, Level)[L[a]].notes[i].block - 3, 0.55f, Middle_z), Quaternion.identity));

                if (L[a] == 31)
                {
                    if(i == 0)  Debug.Log("31 ?=" + dataLists.MEObj[3]);
                }
                lowerVec3[0] = new Vector3(GetLevel(container, Level)[L[a]].notes[i].block - 3.5f, 0.549f, Middle_z);//帯の左端
                lowerVec3[1] = new Vector3(GetLevel(container, Level)[L[a]].notes[i].block - 2.5f, 0.549f, Middle_z);//帯の右端

                Vector3[] lineVerticesVec3 = new Vector3[4];

                lineVerticesVec3[0] = upperVec3[0];
                lineVerticesVec3[1] = upperVec3[1];
                lineVerticesVec3[2] = lowerVec3[0];
                lineVerticesVec3[3] = lowerVec3[1];


                GameObject lineObj = GameObject.CreatePrimitive(PrimitiveType.Quad);

                lineObj.AddComponent<Notes>();//Quadにスクリプトを追加



                if (i == 0)
                {
                    dataLists.LongMNT[GetLevel(container, Level)[L[a]].notes[i].block].Add(new NoteInfo { notestime = Middletime, lane = GetLevel(container, Level)[L[a]].block });//中間点のレーンの1番目に1番目の中間点の時間，始点のレーンをいれる
                    dataLists.QuadA[GetLevel(container, Level)[L[a]].block].Add(new QuadInfo { Quad = lineObj, starttime = SampleLtime, endtime = Middletime, startlane = GetLevel(container, Level)[L[a]].block, endlane = GetLevel(container, Level)[L[a]].notes[i].block });
                }
                if (i < GetLevel(container, Level)[L[a]].notes.Length - 1)
                {
                    dataLists.LongSMNT[GetLevel(container, Level)[L[a]].notes[i].block].Add(Middletime);//中間点の時間をいれる
                }
                if (i > 0)
                {
                    dataLists.LongMNT[GetLevel(container, Level)[L[a]].notes[i].block].Add(new NoteInfo { notestime = Middletime, lane = GetLevel(container, Level)[L[a]].notes[i - 1].block });//中間点のレーンの2番目以降に中間点の時間，1つ前の中間点のレーンをいれる

                    dataLists.QuadA[GetLevel(container, Level)[L[a]].notes[i - 1].block].Add(new QuadInfo { Quad = lineObj, starttime = Middletimes[i - 1], endtime = Middletime, startlane = GetLevel(container, Level)[L[a]].notes[i - 1].block, endlane = GetLevel(container, Level)[L[a]].notes[i].block });



                }//これにより，帯の終点がすぎた時，判定された時に帯の始点のレーンを取得できる(正しい帯を指定できる)




                Mesh mesh = new Mesh();
                mesh.SetVertices(lineVerticesVec3);

                int[] triangles = new int[6];
                triangles[0] = 0;
                triangles[1] = 2;
                triangles[2] = 1;
                triangles[3] = 2;
                triangles[4] = 3;
                triangles[5] = 1;
                mesh.SetTriangles(triangles, 0);

                lineObj.GetComponent<MeshFilter>().mesh = mesh;//メッシュを渡す

                upperVec3[0] = new Vector3(GetLevel(container, Level)[L[a]].notes[i].block - 3.5f, 0.549f, Middle_z);
                upperVec3[1] = new Vector3(GetLevel(container, Level)[L[a]].notes[i].block - 2.5f, 0.549f, Middle_z);

              
            }

        }



        for (int s = 0; s < S.Count; s++) //スライドノーツ
        {

            float Skankaku = 60 / (container.bpm * (float)GetLevel(container, Level)[S[s]].lpb);
            float SbeatSec = Skankaku * (float)GetLevel(container, Level)[S[s]].lpb;
            float Stime = Skankaku * GetLevel(container, Level)[S[s]].num + container.offset / 44100 + tapLag / 100 + gManager.grace;

            SrideTime[GetLevel(container, Level)[S[s]].block].Add(Stime);


            float Sz = Stime * gManager.noteSpeed;


            SrideObject[GetLevel(container, Level)[S[s]].block].Add(Instantiate(SridenoteObj, new Vector3(GetLevel(container, Level)[S[s]].block - 3, 0.55f, Sz), Quaternion.identity));

            if (s == 0)
            {
                noteNum += S.Count;
            }
        }

        for (int f = 0; f < F.Count; f++)
        {
            int flick = 31;
            int srlong = 32;
            int[] f_or_sl = new int[F.Count];
            f_or_sl[f] = (int)GetLevel(container, Level)[F[f]].notes[0].lpb;

            int numf = Array.IndexOf(f_or_sl, flick);
            int numsrl = Array.IndexOf(f_or_sl, srlong);

            while (numf >= 0)
            {
                Flick.Add(numf);

                if (numf + 1 < f_or_sl.Length)
                {
                    numf = Array.IndexOf(f_or_sl, flick, numf + 1);


                }
                else
                {
                    break;
                }


            }
            while (numsrl >= 0)
            {
                Srlong.Add(numsrl);

                if (numsrl + 1 < f_or_sl.Length)
                {
                    numsrl = Array.IndexOf(f_or_sl, srlong, numsrl + 1);


                }
                else
                {
                    break;
                }

            }




        }

        for (int ff = 0; ff < Flick.Count; ff++)
        {
            float Fkankaku = 60 / (container.bpm * (float)GetLevel(container, Level)[F[Flick[ff]]].lpb);
            float FbeatSec = Fkankaku * (float)GetLevel(container, Level)[F[Flick[ff]]].lpb;
            float Ftime = Fkankaku * GetLevel(container, Level)[F[Flick[ff]]].num + container.offset / 44100 + tapLag / 100 + gManager.grace;

            FlickTime[GetLevel(container, Level)[F[Flick[ff]]].block].Add(Ftime);


            float Fz = Ftime * gManager.noteSpeed;

            FlickObject[GetLevel(container, Level)[F[Flick[ff]]].block].Add(Instantiate(FlObj, new Vector3(GetLevel(container, Level)[F[Flick[ff]]].block - 3, 0.55f, Fz), Quaternion.identity));

            if (ff == 0)
            {
                noteNum += F.Count;
            }

        }


    }
}    

    