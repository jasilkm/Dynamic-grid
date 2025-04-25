using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MessagePop : MonoBehaviour
{

    [SerializeField] private TMP_Text infoTxt;
    [SerializeField] private Button okBtn;
    public static MessagePop Instance;


    private void Awake()
    {
        okBtn.onClick.AddListener(Hide);
    }

    public void ShowMessage(string message)
    {
        Show();
        infoTxt.text = message;
    }

    private void Hide()
    {
        UIManager.Instance.ShowLevelSelection();
        this.gameObject.SetActive(false);
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

}
