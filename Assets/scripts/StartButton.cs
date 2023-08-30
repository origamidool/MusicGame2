using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public MusicManager musicManager;
    [SerializeField] Button btn;
    bool isstart = false;
    public void Update()
    {
        if (isstart) return;
        btn.onClick.AddListener(musicManager.GameStart);
        isstart = true;
    }


}
