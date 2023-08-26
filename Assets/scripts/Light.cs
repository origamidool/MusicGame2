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

    
    

    [SerializeField] private float Speed = 3;
    [SerializeField] private int num = 0;
    private Renderer rend;
    private float alfa = 0;
    void Start()
    {
        rend = GetComponent<Renderer>();
       
    }
    void Update()
    {

        if (!(rend.material.color.a <= 0))
        {
            rend.material.color = new Color(rend.material.color.r, rend.material.color.r, rend.material.color.r, alfa);
        }



        

        alfa -= Speed * Time.deltaTime;

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
                            
                            if (num == 1)
                            {
                                colorChange(id, true);

                                break;

                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag1))
                        {
                            if (num == 2)
                            {
                                colorChange(id, true);

                                break;
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag2))
                        {
                            if (num == 3)
                            {
                                colorChange(id, true);

                                break;
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag3))
                        {
                            if (num == 4)
                            {
                                colorChange(id, true);

                                break;
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag4))
                        {
                            if (num == 5)
                            {
                               
                                    colorChange(id, true);
                                   
                                    break;
                                
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag5))
                        {
                            if (num == 6)
                            {
                                colorChange(id, true);

                                break;
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag6))//右端
                        {
                            if (num == 7)
                            {
                                colorChange(id,true);

                                break;
                            }
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

                            if (num == 1)
                            {
                                colorChange(id, false);

                                break;

                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag1))
                        {
                            if (num == 2)
                            {
                                colorChange(id, false);

                                break;
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag2))
                        {
                            if (num == 3)
                            {
                                colorChange(id, false);

                                break;
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag3))
                        {
                            if (num == 4)
                            {
                                colorChange(id, false);

                                break;
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag4))
                        {
                            if (num == 5)
                            {

                                colorChange(id, false);

                                break;

                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag5))
                        {
                            if (num == 6)
                            {
                                colorChange(id, false);

                                break;
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag6))//右端
                        {
                            if (num == 7)
                            {
                                colorChange(id, false);

                                break;
                            }
                        }
                    }

                }
            }

        }
    }

    public void colorChange(int id, bool begin)
    {
        if(!begin)
        {
            if (GManager.instance.isLighting[id] == num) return;
        }
        GManager.instance.isLighting[id] = num;
        alfa = 0.3f;
        rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, alfa);
    }

    
    
}
