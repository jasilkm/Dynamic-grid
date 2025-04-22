using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cardID;
}