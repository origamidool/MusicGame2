using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    [SerializeField] float NoteSpeed = 1;

    

    void Start()
    {
        NoteSpeed = GManager.instance.noteSpeed;
    }
    
    // Update is called once per frame
    void Update()
    {
       
        if (GManager.instance.Start)
        {
            transform.position -= transform.forward * Time.deltaTime * NoteSpeed;
        }
    }
}
