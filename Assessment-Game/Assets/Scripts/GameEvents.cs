
using System;
public static class GameEvents
{
    public static event Action OnQuitPressed;
    public static event Action<int> OnLevelSelected;

    public static void RaiseQuitPressed()
    {
        OnQuitPressed?.Invoke();
    }

    public static void RaiseOnLevelSelected(int totalCards)
    {
        OnLevelSelected?.Invoke(totalCards);
    }
}