using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Light : MonoBehaviour
{

    [SerializeField] public Camera raycastCamera;


    private bool[][] ischange = new bool[5][];





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
        for(int i = 0; i < 5; i++)
        {
            ischange[i] = new bool[] { false, false, false, false, false, false, false };

        }
    }
    void Update()
    {

        if (!(rend.material.color.a <= 0))
        {
            rend.material.color = new Color(rend.material.color.r, rend.material.color.r, rend.material.color.r, alfa);
        }



        if (num == 1)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                colorChange();
            }
        }

        if (num == 2)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                colorChange();
            }
        }

        if (num == 3)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                colorChange();
            }
        }

        if (num == 4)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                colorChange();
            }
        }

        if (num == 5)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                colorChange();
            }
        }

        if (num == 6)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                colorChange();
            }
        }

        if (num == 7)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                colorChange();
            }
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
                                if (!ischange[id][0])
                                {
                                    colorChange();
                                    isChange(id, 0);
                                    break;
                                }
                                
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag1))
                        {
                            if (num == 2)
                            {
                                if (!ischange[id][1])
                                {
                                    colorChange();
                                    isChange(id, 1);
                                    break;
                                }
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag2))
                        {
                            if (num == 3)
                            {
                                if (!ischange[id][2])
                                {
                                    colorChange();
                                    isChange(id, 2);
                                    break;
                                }
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag3))
                        {
                            if (num == 4)
                            {
                                if (!ischange[id][3])
                                {
                                    colorChange();
                                    isChange(id, 3);
                                    break;
                                }
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag4))
                        {
                            if (num == 5)
                            {
                                if (!ischange[id][4])
                                {
                                    colorChange();
                                    isChange(id, 4);
                                    break;
                                }
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag5))
                        {
                            if (num == 6)
                            {
                                if (!ischange[id][5])
                                {
                                    colorChange();
                                    isChange(id, 5);
                                    break;
                                }
                            }
                        }
                        else if (result.gameObject.CompareTag(cubeTag6))//右端
                        {
                            if (num == 7)
                            {
                                if (!ischange[id][6])
                                {
                                    colorChange();
                                    isChange(id, 6);
                                    break;
                                }
                            }
                        }
                    }

                }
            }

        }
    }

    public void colorChange()
    {
        alfa = 0.3f;
        rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, alfa);
    }

    public void isChange(int id,int laneindex)
    {
        ischange[id][laneindex] = true;
        for(int i = 0; i < 7; i++)
        {
            if (i != laneindex) ischange[id][i] = false;

        }
    }

    
}
