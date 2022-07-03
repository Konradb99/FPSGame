using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static float timeleft = 180f;
    
    public static List<PlayerScore> Players = new List<PlayerScore>();

    public static void NewPlayer(string name)
    {
        Players.Add(new PlayerScore(name));
        Debug.Log($"Nowy gracz: {name}");
    }
}

public class PlayerScore
{
    public PlayerScore(string name)
    {
        Name = name;
        Score = 0;
    }

    public int Score { get; set; }
    public string Name { get; set; }
}
