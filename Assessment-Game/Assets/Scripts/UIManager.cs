using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    [SerializeField] private HudController hudController;
    [SerializeField] private GameOverController gameOverController;
    [SerializeField] private PauseController pauseController;



    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }



    public void ShowGameOverScreen(GameoverInfo gameoverInfo)
    {
        gameOverController.ShowGameover(gameoverInfo);
    }

    public GameoverInfo GetGameoverInfo()
    {

        return hudController.GetGameoverInfo();

    }

    public void ShowPauseScreen()
    {
        pauseController.ShowPauseScreen();
    }


    /// <summary>
    /// Updating Hud score
    /// </summary>
    /// <param name="score"></param>

    public void SetScore(int score)
    {
        hudController.UpdateScrore(score);
    }
    public void SetBonusScore(int bonus)
    {
        hudController.UpdateBonus(bonus);
    }




}
