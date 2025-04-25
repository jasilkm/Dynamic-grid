using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameOverController : MonoBehaviour
{

    #region private properties

   [SerializeField] private TMP_Text _scoretxt;
   [SerializeField] private TMP_Text _bonustxt;
   [SerializeField] private Button _closeBtn;
   [SerializeField] private Button _nextBtn;
   [SerializeField] private GameObject _gameOverScreen;
    #endregion

    #region Unity Methods
    void Awake()
    {
        _closeBtn.onClick.AddListener(Hide);
        _nextBtn.onClick.AddListener(NextLevel);
    }
    #endregion


    #region public Methods

    public void ShowGameover(GameoverInfo gameoverInfo)
    {
        _gameOverScreen.SetActive(true);
        _scoretxt.text = $"Score : {gameoverInfo.Score}";
        _bonustxt.text = $"Bonus : {gameoverInfo.Bonus}";
    }
   
    public void NextLevel()
    {
        int level = GameManager.Instance.CurrentLevel;
        int totalCards = level * 2;
        GameEvents.RaiseOnLevelSelected(totalCards, level);
        this.gameObject.SetActive(false);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        UIManager.Instance.ShowLevelSelection();
    }

    #endregion
}




public class GameoverInfo
{
    public int Score { get; set; }
    public int Bonus { get; set; }
    public int Time { get; set; }


}
