using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ChargeCard : Card
{
    public float speedModifier = 2f;
    public float duration = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void UseCard(Ball ballToApplyTo = null, PlayerController playerToApplyTo = null)
    {
        string what = ballToApplyTo == null ? "player" : "ball";
        Debug.Log("Applying " + CardName + " as " + what);
        playerToApplyTo.ModifySpeed(speedModifier, duration);
    }
}
