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




        if (Input.GetMouseButtonDown(0))//タップ
        {
            Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (var result in results)
            {
                if (result.gameObject != null && result.gameObject.CompareTag(cubeTag0))
                {
                    if (num == 1)
                    {
                        colorChange();
                        break;
                    }
                    
                }
                if (result.gameObject != null && result.gameObject.CompareTag(cubeTag1))
                {
                    if (num == 2)
                    {
                        colorChange();
                        break;
                    }
                }
                if (result.gameObject != null && result.gameObject.CompareTag(cubeTag2))
                {
                    if (num == 3)
                    {
                        colorChange();
                        break;
                    }
                }
                if (result.gameObject != null && result.gameObject.CompareTag(cubeTag3))
                {
                    if (num == 4)
                    {
                        colorChange();
                        break;
                    }
                }
                if (result.gameObject != null && result.gameObject.CompareTag(cubeTag4))
                {
                    if (num == 5)
                    {
                        colorChange();
                        break;
                    }
                }
                if (result.gameObject != null && result.gameObject.CompareTag(cubeTag5))
                {
                    if (num == 6)
                    {
                        colorChange();
                        break;
                    }
                }
                if (result.gameObject != null && result.gameObject.CompareTag(cubeTag6))
                {
                    if (num == 7)
                    {
                        colorChange();
                        break;
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



    
}
