using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class leaderboard : MonoBehaviour
{
    public int player_count;
    public TMP_Text t1;
    public TMP_Text t2;
    public TMP_Text t3;
    public TMP_Text t4;
    public TMP_Text t5;
    public TMP_Text t6;
    public TMP_Text t7;
    public TMP_Text t8;
    public Dictionary<int, int> playersdict => GameManager.gm.players;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // testing vars
        t1.text = "";
        t2.text = "";
        t3.text = "";
        t4.text = "";
        t5.text = "";
        t6.text = "";
        t7.text = "";
        t8.text = "";
        
    }

    // Update is called once per frame
    void Update()
    {
        updateLeaderBoard();
    }
    

    public void updateLeaderBoard()
    {
        var sortedByValueDesc = playersdict.OrderByDescending(pair => pair.Value).ToList();
        try
        {
            var pair = sortedByValueDesc[0];
            t1.text = pair.Key.ToString();
            t2.text = pair.Value.ToString();
        }
        catch (Exception ex)
        {
            Debug.Log("AW HELL NAH");
            return;
        }
        try
        {
            var pair = sortedByValueDesc[1];
            t3.text = pair.Key.ToString();
            t4.text = pair.Value.ToString();
        }
        catch (Exception ex)
        {
            Debug.Log("No P2 Found");
            return;
        }
        try
        {
            var pair = sortedByValueDesc[2];
            t5.text = pair.Key.ToString();
            t6.text = pair.Value.ToString();
        }
        catch (Exception ex)
        {
            Debug.Log("No P3 Found");
            return;
        }
try
        {
            var pair = sortedByValueDesc[3];
            t7.text = pair.Key.ToString();
            t8.text = pair.Value.ToString();
        }
        catch (Exception ex)
        {
            Debug.Log("No P4 Found");
            return;
        }
    }
}
