using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class IceCard : Card
{
    public GameObject iceWallPrefab;
    public float distanceFromPlayer = 3f;
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
        GameObject newIceWall = Instantiate(iceWallPrefab);
        Vector3 facingDirection = playerToApplyTo.gameObject.transform.right;
        facingDirection.y = 0;
        facingDirection.Normalize();
        newIceWall.transform.forward = facingDirection;
        newIceWall.transform.position = playerToApplyTo.gameObject.transform.position + playerToApplyTo.gameObject.transform.forward * distanceFromPlayer + Vector3.up *1.5f;
    }
}
