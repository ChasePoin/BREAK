using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;



public class GameManager : MonoBehaviour
{
    // map of players to scores
    public Dictionary<int, int> players = new Dictionary<int, int>();
    private PlayerInputManager pim;
    int currentPlayers = 0;
    public GameObject playerPrefab;
    public int nextPlayerId = 1;
    static public GameManager gm;
    void Awake()
    {
        gm = this;
        pim = this.GetComponent<PlayerInputManager>();
        DontDestroyOnLoad(gameObject);
        SpawnPlayers(true);
    }

    private void SpawnPlayers(bool start)
    {
        foreach (Transform childTransform in transform)
        {
            SpawnPoint sp = childTransform.gameObject.GetComponent<SpawnPoint>();
            if (!sp.haveISpawnedSomebody)
            {
                float random = Random.value;
                if (random > .25f) continue;
                GameObject player = Instantiate(playerPrefab, childTransform);
                PlayerController ppfb = player.GetComponent<PlayerController>();
                if (start)
                {
                    int culledPlayerLayer = LayerMask.NameToLayer($"Player{nextPlayerId}");
                    if (culledPlayerLayer != -1)
                    {
                        int layerMaskToHide = 1 << culledPlayerLayer;
                        ppfb.playerCamera.cullingMask &= ~layerMaskToHide;
                        ppfb.playerMesh.gameObject.layer = culledPlayerLayer;
                    }
                    ppfb.playerId = nextPlayerId;
                    players[ppfb.playerId] = 0;
                    nextPlayerId++;
                    Debug.Log(ppfb.playerId + ": " + players[ppfb.playerId]);
                }
                else
                {
                    ppfb.playerId = nextPlayerId;
                    nextPlayerId++;
                    Debug.Log("recreating " + ppfb.playerId + ": " + players[ppfb.playerId]);
                }
                sp.haveISpawnedSomebody = true;
                currentPlayers++;
                Debug.Log("Current Players: " + currentPlayers);
                if (currentPlayers >= pim.maxPlayerCount) return;
            }
        }
        if (currentPlayers != pim.maxPlayerCount) SpawnPlayers(start);
    }
    // need to reset players so we can spawn them in each new scene
    public void ResetPlayers()
    {
        foreach (Transform childTransform in transform)
        {
            SpawnPoint sp = childTransform.gameObject.GetComponent<SpawnPoint>();
            sp.haveISpawnedSomebody = false;

            foreach (Transform gc in childTransform)
            {
                Destroy(gc.gameObject);
                currentPlayers--;
            }
        }
        nextPlayerId = 1;
        SpawnPlayers(false);
    }

}
