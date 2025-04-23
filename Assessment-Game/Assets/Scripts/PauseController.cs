using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PauseController : MonoBehaviour
{

    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _resumeButton;


    void Awake()
    {
        _quitButton.onClick.AddListener(()=> {

            GameEvents.RaiseQuitPressed();
            this.gameObject.SetActive(false);

        });
        _resumeButton.onClick.AddListener(()=> {

            this.gameObject.SetActive(false);
        });
    }

   

    public void ShowPauseScreen()
    {
        this.gameObject.SetActive(true); 
    }



}
