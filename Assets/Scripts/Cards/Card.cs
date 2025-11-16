using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum CardTypes
{
    Ball,
    Terrain,
    Player
}

public enum Rarity
{
    Common,
    Rare,
    Epic
}

public class Card : MonoBehaviour
{
    public int CardID;
    public string CardName;
    public CardTypes Type;
    public Rarity Rarity;
    public Sprite CardSprite;

    public virtual void UseCard(Ball ballToApplyTo = null, PlayerController playerToApplyTo = null)
    {
        string what = ballToApplyTo == null ? "player" : "ball";
        Debug.Log("Applying " + CardName + " to " + what);
    }
}
