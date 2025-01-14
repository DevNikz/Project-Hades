using UnityEngine;

[System.Serializable]
public class PlayerStats {
    public int Runs;
    public int DepthLevel;
    public int[] AugmentID;
    public int Wins;
    public int Deaths;
    public int hasPlayed;

    public PlayerStats(SaveManager player) {
        Runs = player.Runs;
        DepthLevel = player.DepthLevel;
        Wins = player.Wins;
        Deaths = player.Deaths;
        hasPlayed = player.hasPlayed ? 1 : 0;
    }
}