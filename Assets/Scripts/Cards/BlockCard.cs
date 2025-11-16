using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BlockCard : Card
{
    public float duration = 30f;
    public GameObject shieldPrefab;
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
        playerToApplyTo.BlockBalls(duration);
    }
}
