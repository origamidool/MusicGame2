using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Light : MonoBehaviour
{

    [SerializeField] public Camera raycastCamera;


   

    public string cubeTag0 = "Cube";
    public string cubeTag1 = "Cube2";
    public string cubeTag2 = "Cube3";
    public string cubeTag3 = "Cube4";
    public string cubeTag4 = "Cube5";
    public string cubeTag5 = "Cube6";
    public string cubeTag6 = "Cube7";


    [SerializeField] private GameObject[] Lights = new GameObject[7];

    [SerializeField] private float Speed = 3;
    [SerializeField] private int num = 0;
    [SerializeField] public int[] isLighting;

    private Renderer[] rend = new Renderer[7];
    private float[] alfa = new float[] {0,0,0,0,0,0,0};
    void Start()
    {
        isLighting = new int[] { -1, -1, -1, -1, -1, -1, -1};//light用
        
        

        for(int i = 0; i < 7; i++)
        {
            rend[i] = Lights[i].GetComponent<Renderer>();
        }
    }
    void Update()
    {

        for (int i = 0; i < 7; i++)
        {
            if (!(rend[i].material.color.a <= 0))
            {
                rend[i].material.color = new Color(rend[i].material.color.r, rend[i].material.color.r, rend[i].material.color.r, alfa[i]);
            }
            alfa[i] -= Speed * Time.deltaTime;
        }

        

        ProcessInput();

    }
    public void ProcessInput()
    {
        if (Input.touchCount <= 0) return;

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
                        if (result.gameObject.CompareTag(cubeTag0))//左端のレーン 格レーンについたタグ(canvasのbutton（透明）についてる)に当たったら
                        {


                            colorChange(id, true, 0);

                            break;


                        }
                        else if (result.gameObject.CompareTag(cubeTag1))
                        {

                            colorChange(id, true, 1);

                            break;

                        }
                        else if (result.gameObject.CompareTag(cubeTag2))
                        {

                            colorChange(id, true, 2);

                            break;

                        }
                        else if (result.gameObject.CompareTag(cubeTag3))
                        {

                            colorChange(id, true, 3);

                            break;
                        }
                    
                        else if (result.gameObject.CompareTag(cubeTag4))
                        {
                       

                            colorChange(id, true, 4);

                            break;

                        
                        }
                        else if (result.gameObject.CompareTag(cubeTag5))
                        {
                        
                            colorChange(id, true, 5);

                            break;
                        
                        }
                        else if (result.gameObject.CompareTag(cubeTag6))//右端
                        {
                       
                            colorChange(id, true, 6);

                            break;
                        
                        }
                    }

                }
            }
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
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
                        if (result.gameObject.CompareTag(cubeTag0))//左端のレーン 格レーンについたタグ(canvasのbutton（透明）についてる)に当たったら
                        {

                           
                                colorChange(id, false,0);

                                break;

                            
                        }
                        else if (result.gameObject.CompareTag(cubeTag1))
                        {
                            
                                colorChange(id, false,1);

                                break;
                            
                        }
                        else if (result.gameObject.CompareTag(cubeTag2))
                        {
                            
                                colorChange(id, false,2);

                                break;
                            
                        }
                        else if (result.gameObject.CompareTag(cubeTag3))
                        {
                            
                                colorChange(id, false,3);

                                break;
                            
                        }
                        else if (result.gameObject.CompareTag(cubeTag4))
                        {
                            

                                colorChange(id, false,4);

                                break;

                            
                        }
                        else if (result.gameObject.CompareTag(cubeTag5))
                        {
                           
                                colorChange(id, false,5);

                                break;
                            
                        }
                        else if (result.gameObject.CompareTag(cubeTag6))//右端
                        {
                            
                                colorChange(id, false,6);

                                break;
                            
                        }
                    }

                }
            }

        }
    }

    public void colorChange(int id, bool begin,int laneindex)
    {
        if(!begin)
        {
            if (isLighting[id] == laneindex) return;
        }
           
        
        isLighting[id] = laneindex;
        alfa[laneindex] = 0.3f;
        rend[laneindex].material.color = new Color(rend[laneindex].material.color.r, rend[laneindex].material.color.g, rend[laneindex].material.color.b, alfa[laneindex]);
    }
   

}
