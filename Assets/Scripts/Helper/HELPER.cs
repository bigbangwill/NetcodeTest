using Unity.Multiplayer.Playmode;
using System.Linq;

public static class HELPER
{
    public static string GetPlayerName()
    {
        if (CurrentPlayer.ReadOnlyTags().Contains("Player 1"))
        {
            return "Player 1";
        }
        else if (CurrentPlayer.ReadOnlyTags().Contains("Player 2"))
        {
            return "Player 2";
        }
        else if (CurrentPlayer.ReadOnlyTags().Contains("Player 3"))
        {
            return "Player 3";
        }
        else
        {
            return "WTF";
        }
    }
}