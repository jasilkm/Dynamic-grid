
using System;
public static class GameEvents
{
    public static event Action OnQuitPressed;
    public static event Action<int,int> OnLevelSelected;
    public static event Action OnLoadLevelFromPersistance;


    // GameQuit Event fro Quit menu
    public static void RaiseQuitPressed()
    {
        OnQuitPressed?.Invoke();
    }

    // level Selection
    public static void RaiseOnLevelSelected(int totalCards,int currentLevel)
    {
        OnLevelSelected?.Invoke(totalCards, currentLevel);
    }

    public static void RaiseOnLoadLevelFromPersistance()
    {
        OnLoadLevelFromPersistance?.Invoke();
    }

}
