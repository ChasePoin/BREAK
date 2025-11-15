using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;



public class GameManager : MonoBehaviour
{
    // map of players to scores
    [SerializeField]
    private Dictionary<int, int> players = new Dictionary<int, int>();
    private PlayerInputManager pim;
    int currentPlayers = 0;
    public GameObject playerPrefab;
    public int nextPlayerId = 1;
    void Awake()
    {
        pim = this.GetComponent<PlayerInputManager>();
        DontDestroyOnLoad(gameObject);
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        foreach (Transform childTransform in transform)
        {
            SpawnPoint sp = childTransform.gameObject.GetComponent<SpawnPoint>();
            if (!sp.haveISpawnedSomebody)
            {
                GameObject player = Instantiate(playerPrefab, childTransform);
                PlayerController ppfb = player.GetComponent<PlayerController>();
                if (ppfb.playerId == 0)
                {
                    ppfb.playerId = nextPlayerId;
                    players[ppfb.playerId] = 0;
                    nextPlayerId++;
                    Debug.Log(ppfb.playerId + ": " + players[ppfb.playerId]);
                }
                sp.haveISpawnedSomebody = true;
                currentPlayers++;
                Debug.Log("Current Players: " + currentPlayers);
                if (currentPlayers >= pim.maxPlayerCount) return;
            }
        }
    }
    // need to reset players so we can spawn them in each new scene
    public void ResetPlayers()
    {
        currentPlayers = 0;
    }

}
