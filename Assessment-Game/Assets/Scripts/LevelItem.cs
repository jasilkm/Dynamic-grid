using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelItem : MonoBehaviour
{

    

    [SerializeField] private TMP_Text _levelTxt;
    [SerializeField] private Button _levelBtn;
    public int levelID { get;  set; }
    public int totalCards { get;  set; }


    private void Awake()
    {
        _levelBtn.onClick.AddListener(OnLevelBtnClicked);

    }



    private void OnLevelBtnClicked()
    {
        GameEvents.RaiseOnLevelSelected(totalCards, levelID);
    }


    public void UpdateLevelInfo(int id)
    {
        _levelTxt.text = id.ToString();
        levelID = id;
        totalCards = id * 2;
    }

    private void OnDestroy()
    {
        _levelBtn.onClick.RemoveListener(OnLevelBtnClicked);
    }

}
