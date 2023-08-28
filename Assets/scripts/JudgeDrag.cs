using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeDrag : MonoBehaviour
{

    public NotesManager notesManager;
    public Light Llight;
    public Judge judge;

    public bool isHolding = false;

    void Update()
    {
        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (notesManager.SrideNT.Count - 1 > -1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHolding = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isHolding = false;
            }

            if(isHolding)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {

                }
            }
        }
    }
}
