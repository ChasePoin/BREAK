using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;



public class GameManager : MonoBehaviour
{
    // map of players to scores
    public Dictionary<int, int> players = new Dictionary<int, int>();
    public Dictionary<int, bool> aliveStatus = new Dictionary<int, bool>();
    private PlayerInputManager pim;
    int currentPlayers = 0;
    public GameObject playerPrefab;
    public int nextPlayerId = 1;
    public int numRounds = 5;
    public int currentRound = 0;
    static public GameManager gm;
    public string scene = "skybox test";
    public AudioSource backgroundAudioSource;
    public bool isRoundEnding = false;
    [SerializeField]
    private GameObject RoundEndCamera;
    public int maxBalls = 10;
    void Awake()
    {
        if (gm != null) {
            Destroy(gameObject);
            return;
        }
        gm = this;
        pim = this.GetComponent<PlayerInputManager>();
        DontDestroyOnLoad(gameObject);
        Debug.Log("Spawning Players");
        SpawnPlayers(true);
    }

    private void SpawnPlayers(bool start)
    {
        foreach (Transform childTransform in transform)
        {
            SpawnPoint sp = childTransform.gameObject.GetComponent<SpawnPoint>();
            if (!sp.haveISpawnedSomebody && currentPlayers < pim.maxPlayerCount)
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
                    aliveStatus[ppfb.playerId] = true;
                    nextPlayerId++;
                    Debug.Log(ppfb.playerId + ": " + players[ppfb.playerId]);
                }
                else
                {
                    int culledPlayerLayer = LayerMask.NameToLayer($"Player{nextPlayerId}");
                    if (culledPlayerLayer != -1)
                    {
                        int layerMaskToHide = 1 << culledPlayerLayer;
                        ppfb.playerCamera.cullingMask &= ~layerMaskToHide;
                        ppfb.playerMesh.gameObject.layer = culledPlayerLayer;
                    }
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
        isRoundEnding = false;
    }
    // need to reset players so we can spawn them in each new scene
    public void ResetPlayers()
    {
        foreach (Transform childTransform in transform)
        {
            SpawnPoint sp = childTransform.GetComponent<SpawnPoint>();
            sp.haveISpawnedSomebody = false;

            foreach (Transform gc in childTransform)
            {
                Destroy(gc.gameObject);
            }
        }
        List<int> keys = new List<int>(aliveStatus.Keys);
        foreach (int id in keys)
        {
            aliveStatus[id] = true;
        }

        currentPlayers = 0;
        nextPlayerId = 1;
        currentRound += 1;
        SpawnPlayers(false);

    }


    public void FixedUpdate()
    {
        if (isRoundEnding) return;
        var alivePlayers = CheckRemainingPlayers();
        if (alivePlayers.count <= 1) isRoundEnding = true;
        ProcessAliveCount(alivePlayers.count, alivePlayers.ids);

    }

    // leaving as an int in case we want to do special logic based off of players remaining later
    public (int count, List<int> ids) CheckRemainingPlayers()
    {
        int count = 0;
        List<int> ids = new List<int>();
        foreach(var entry in aliveStatus)
        {
            int playerid = entry.Key;
            bool alive = entry.Value;

            if (alive) 
            {
                count += 1;
                ids.Add(playerid);
            }
        }

        return (count, ids);
    }

    public void ProcessAliveCount(int aliveCount, List<int> aliveIds)
    {
        if (aliveCount == 1 && currentRound < numRounds)
        {
            players[aliveIds[0]] += 1;
            StartCoroutine(EndRound());
            return;
        }
        else if (aliveCount == 0 && currentRound < numRounds && !isRoundEnding)
        {
            StartCoroutine(EndRound());
            return;
        }
        else if (aliveCount == 1 && currentRound == numRounds)
        {
            players[aliveIds[0]] += 1;
            StartCoroutine(EndGame());
            return;
        }
        else if (aliveCount == 0 && currentRound == numRounds)
        {
            StartCoroutine(EndGame());
            return;
        }
    }

    public IEnumerator EndRound()
    {
        // need to add logic to show leaderboard
        yield return VictoryCountdown(3);
        RoundEndCamera.SetActive(true);
        backgroundAudioSource.Stop();
        AudioController.PlayClip("scoreboard");
        yield return VictoryCountdown(12);
        ResetPlayers();
        RoundEndCamera.SetActive(false);
        SceneManager.LoadScene(scene);
    }

    public IEnumerator EndGame()
    {
        yield return VictoryCountdown(3);
        RoundEndCamera.SetActive(true);
        backgroundAudioSource.Stop();
        AudioController.PlayClip("scoreboard");
        yield return VictoryCountdown(15);
        RoundEndCamera.SetActive(false);
        SceneManager.LoadScene("StartScene");
    }

    IEnumerator VictoryCountdown(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            Debug.Log(counter);
            yield return new WaitForSeconds(1);
            counter--;
        }
    }


}
