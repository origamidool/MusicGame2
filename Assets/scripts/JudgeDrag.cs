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

       
    }
}
