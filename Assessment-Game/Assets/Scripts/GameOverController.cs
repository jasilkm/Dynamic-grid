using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameOverController : MonoBehaviour
{
    public TMP_Text _scoretxt;
    public TMP_Text _bonustxt;
    public Button _closeBtn;
    public Button _nextBtn;
    [SerializeField] private GameObject _gameOverScreen;
    private void Awake()
    {
        _closeBtn.onClick.AddListener(Hide);
        _nextBtn.onClick.AddListener(NextGame);
    }

    public void ShowGameover(GameoverInfo gameoverInfo)
    {

        _gameOverScreen.SetActive(true);


        _scoretxt.text = $"Score : {gameoverInfo.Score}";
        _bonustxt.text = $"Bonus : {gameoverInfo.Bonus}";
    }

    public void NextGame()
    {
        Debug.Log("NextGame");

    }


    public void Hide () => this.gameObject.SetActive(false);
}




public class GameoverInfo
{
    public int Score { get; set; }
    public int Bonus { get; set; }
    public int Time { get; set; }


}
