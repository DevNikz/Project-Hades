using UnityEngine;

[System.Serializable]
public class PlayerStats {
    public int Runs;
    public int DepthLevel;
    public int[] AugmentID;
    public int Wins;
    public int Deaths;
    public int hasPlayed;
    public PlayerStats()
    {
        Runs = 0;
        DepthLevel = 0;
        Wins = 0;
        Deaths = 0;
        hasPlayed = 0;
    }

    public PlayerStats(SaveManager player)
    {
        Runs = player.CurrentStats.Runs;
        DepthLevel = player.CurrentStats.DepthLevel;
        Wins = player.CurrentStats.Wins;
        Deaths = player.CurrentStats.Deaths;
        hasPlayed = player.CurrentStats.hasPlayed;
    }
}