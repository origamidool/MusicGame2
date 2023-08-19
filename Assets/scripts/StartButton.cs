using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public MusicManager musicManager;
    [SerializeField] Button btn;

    public void Update()
    {
        btn.onClick.AddListener(musicManager.GameStart);
    }


}
