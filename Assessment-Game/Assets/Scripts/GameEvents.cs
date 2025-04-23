
using System;
public static class GameEvents
{
    public static event Action OnQuitPressed;

    public static void RaiseQuitPressed()
    {
        OnQuitPressed?.Invoke();
    }
}