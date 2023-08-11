using System;
using System.Collections.Generic;

using UnityEngine;

using LitJson;


[System.Serializable]//7月17日
public class JaggedArrayContainer
{
    public float bpm;
    public int offset;
    public Notegpt[] notes;
}

[System.Serializable]//7/21
public class Notegpt
{
    public int type;
    public int num;
    public int block;
    public int lpb;
    public Notegpt[] notes;
}



public class NotesManager : MonoBehaviour
{
    public string jsonFilePath;//7月17日


    public static NotesManager instance = null;

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




    public Vector3[][] startPos;


    public GManager gManager;

    


    public int noteNum;//総ノーツ数
    private string songName;

    public List<int> LaneNum = new List<int>();
    public List<int> NoteType = new List<int>();
    public List<float> NotesTime = new List<float>();
    public List<GameObject> NotesObj = new List<GameObject>();

    public List<float> LongNT = new List<float>();
    public List<int> LongLN = new List<int>();
    [SerializeField] public List<float> LongAllNT = new List<float>();
    [SerializeField] public List<int> LongAllLN = new List<int>();

    public List<int> N = new List<int>();
    public List<int> L = new List<int>();
    public List<int> S = new List<int>();
    public List<int> F = new List<int>();
    public List<int> SL = new List<int>();
    


    public List<float> NormalNT = new List<float>();
    public List<int> NormalLN = new List<int>();

    public List<float> FlickNT = new List<float>();
    public List<int> FlickLN = new List<int>();


    public List<GameObject> LongSObj = new List<GameObject>();
    public List<GameObject> LongObj = new List<GameObject>();
    public List<GameObject> LongEObj = new List<GameObject>();

    public List<GameObject> SLongSObj = new List<GameObject>();
    public List<GameObject> SLongObj = new List<GameObject>();
    public List<GameObject> SLongEObj = new List<GameObject>();

    public List<GameObject> FlickObjlist = new List<GameObject>();





    public List<float> SrideNT = new List<float>();
    public List<int> SrideLN = new List<int>();

    public List<float> LongLNT = new List<float>();

    public List<float> LNT = new List<float>();
    public List<float> LLNT = new List<float>();


    public List<float> SLongNT = new List<float>();
    

    public List<int> Flick = new List<int>();//7/20使用中を確認
    public List<int> Srlong = new List<int>();//7/20使用中を確認







    [SerializeField] private float NotesSpeed;
    [SerializeField] private GameObject noteObj;

   
    [SerializeField] private GameObject SridenoteObj;//7/20使用中を確認

   
    [SerializeField] private GameObject FlObj;//7/20使用を確認

    [SerializeField] private GameObject SampleLong;//7/18火
    public List<float> SampleNT = new List<float>();
    [SerializeField] public List<int> SampleLN = new List<int>();
    [SerializeField] public List<GameObject> SampleObj = new List<GameObject>();

    public List<GameObject> MandEObj = new List<GameObject>();//中間点と終点のオブジェクト
  


    public List<GameObject> SrideObj = new List<GameObject>();



    public List<float>[] LongSMNT { get; private set; }
    [SerializeField] public List<GameObject>[] QuadA { get; private set; }
    public List<float>[] LongMNT { get; private set; }


    [SerializeField] private float tapLag = 0;

    void OnEnable()
    {
        NotesSpeed = gManager.noteSpeed;
        noteNum = 0;
        songName = "Grievous Lady(Arcaea)";
        Load(songName);

    }

    public int[] type1;

    

    
    private void Load(string SongName)
    {
        jsonFilePath = "Assets/Resources/" + SongName + ".json";
        string jsonString = System.IO.File.ReadAllText(jsonFilePath);
        JaggedArrayContainer container = JsonUtility.FromJson<JaggedArrayContainer>(jsonString);
        

       

       
        gManager.maxScore = noteNum * 5;//new!!

        
        int[] notestype;

        for (int i = 0; i < container.notes.Length; i++)
        {

            notestype = new int[container.notes.Length];
            notestype[i] = container.notes[i].type;
            int target = 1;
            int targetL = 2;
            int targetS = 3;
            int targetF = 4;
            int targetSL = 5;//6/17から不要になった

            int num = Array.IndexOf(notestype, target);

            int numL = Array.IndexOf(notestype, targetL);

            int numS = Array.IndexOf(notestype, targetS);

            int numFF = Array.IndexOf(notestype, targetF);

            int numSL = Array.IndexOf(notestype, targetSL);



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
            while (numSL >= 0)
            {
                SL.Add(numSL);

                if (numSL + 1 < notestype.Length)
                {
                    numSL = Array.IndexOf(notestype, targetSL, numSL + 1);


                }
                else
                {
                    break;
                }

            }

           

        }

        LongSMNT = new List<float>[7];
        QuadA = new List<GameObject>[7];//new!
        LongMNT = new List<float>[7];
        for (int i = 0; i < 7; i++)
        {
            LongSMNT[i] = new List<float>();
            QuadA[i] = new List<GameObject>();
            LongMNT[i] = new List<float>();
        }




        for (int u = 0; u < N.Count; u++) //タップノーツ
        {

            float Nkankaku = 60 / (container.bpm * (float)container.notes[N[u]].lpb);
            float NbeatSec = Nkankaku * (float)container.notes[N[u]].lpb;
            float Ntime = Nkankaku * container.notes[N[u]].num + container.offset / 44100 + tapLag / 100;

            NormalNT.Add(Ntime);
            NormalLN.Add(container.notes[N[u]].block);
            float Nz = NormalNT[u] * gManager.noteSpeed;

            NotesObj.Add(Instantiate(noteObj, new Vector3(container.notes[N[u]].block - 3, 0.55f, Nz), Quaternion.identity));

            if(u == 0)//総ノーツ数を求める為
            {
                noteNum += N.Count;
            }

            
        }

        for (int a = 0; a < L.Count; a++)
        {
            float Samplekankaku = 60 / (container.bpm * (float)container.notes[L[a]].lpb);
            float SampleLtime = Samplekankaku * container.notes[L[a]].num + container.offset / 44100 + tapLag / 100;

            SampleNT.Add(SampleLtime);
            SampleLN.Add(container.notes[L[a]].block);
            LongSMNT[container.notes[L[a]].block].Add(SampleLtime);

            float Sample_z = SampleNT[L[a]] * gManager.noteSpeed;
            SampleObj.Add(Instantiate(SampleLong, new Vector3(container.notes[L[a]].block - 3, 0.55f, Sample_z), Quaternion.identity));

            Vector3[] upperVec3 = new Vector3[2];
            upperVec3[0] = new Vector3(container.notes[L[a]].block - 3.5f, 0.55f, Sample_z);
            upperVec3[1] = new Vector3(container.notes[L[a]].block - 2.5f, 0.55f, Sample_z);

            if(a == 0)
            {
                noteNum += L.Count;
            }

            noteNum += container.notes[L[a]].notes.Length;

            int index = -1;

            foreach (Notegpt notesData in container.notes[L[a]].notes)
            {
                index += 1;
                Vector3[] lowerVec3 = new Vector3[2];


                float Middlekankaku = 60 / (container.bpm * (float)notesData.lpb);
                float Middletime = Middlekankaku * notesData.num + container.offset / 44100 + tapLag / 100;
               
                LongMNT[container.notes[L[a]].block].Add(Middletime);
                

                float Middle_z = Middletime * gManager.noteSpeed;
                MandEObj.Add(Instantiate(SampleLong, new Vector3(notesData.block - 3, 0.55f, Middle_z), Quaternion.identity));

                lowerVec3[0] = new Vector3(notesData.block - 3.5f, 0.55f, Middle_z);
                lowerVec3[1] = new Vector3(notesData.block - 2.5f, 0.55f, Middle_z);

                Vector3[] lineVerticesVec3 = new Vector3[4];

                lineVerticesVec3[0] = upperVec3[0];
                lineVerticesVec3[1] = upperVec3[1];
                lineVerticesVec3[2] = lowerVec3[0];
                lineVerticesVec3[3] = lowerVec3[1];

                GameObject lineObj = GameObject.CreatePrimitive(PrimitiveType.Quad);

                lineObj.AddComponent<LongNotes>();//Quadにスクリプトを追加

               

                if(index == 0)
                {
                    QuadA[container.notes[L[a]].block].Add(lineObj);
                }
                if (index < container.notes[L[a]].notes.Length - 1)
                {
                    LongSMNT[notesData.block].Add(Middletime);
                    QuadA[notesData.block].Add(lineObj);
                }
                


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

                lineObj.GetComponent<MeshFilter>().mesh = mesh;

                upperVec3[0] = new Vector3(notesData.block - 3.5f, 0.55f, Middle_z);
                upperVec3[1] = new Vector3(notesData.block - 2.5f, 0.55f, Middle_z);

            }

        }



        for (int s = 0; s < S.Count; s++) //スライドノーツ
        {

            float Skankaku = 60 / (container.bpm * (float)container.notes[S[s]].lpb);
            float SbeatSec = Skankaku * (float)container.notes[S[s]].lpb;
            float Stime = Skankaku * container.notes[S[s]].num + container.offset / 44100 + tapLag / 100;
            SrideNT.Add(Stime);
            SrideLN.Add(container.notes[S[s]].block);
            float Sz = SrideNT[s] * gManager.noteSpeed;


            SrideObj.Add(Instantiate(SridenoteObj, new Vector3(container.notes[S[s]].block - 3, 0.55f, Sz), Quaternion.identity));

            if(s == 0)
            {
                noteNum += S.Count;
            }
        }

        for (int f = 0; f < F.Count; f++)
        {
            int flick = 1;
            int srlong = 6;
            int[] f_or_sl = new int[F.Count];
            f_or_sl[f] = (int)container.notes[F[f]].notes[0].block;

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

        for(int ff = 0; ff < Flick.Count; ff++)
        {
            float Fkankaku = 60 / (container.bpm * (float)container.notes[F[Flick[ff]]].lpb);
            float FbeatSec = Fkankaku * (float)container.notes[F[Flick[ff]]].lpb;
            float Ftime = Fkankaku * container.notes[F[Flick[ff]]].num + container.offset / 44100 + tapLag / 100;

            FlickNT.Add(Ftime);
            FlickLN.Add(container.notes[F[Flick[ff]]].block);
            float Fz = FlickNT[ff] * gManager.noteSpeed;

            FlickObjlist.Add(Instantiate(FlObj, new Vector3(container.notes[F[Flick[ff]]].block - 3, 0.55f, Fz), Quaternion.identity));

            if(ff == 0)
            {
                noteNum += F.Count;
            }

        }

        
    }
} 
    

    
    

        
      

    






