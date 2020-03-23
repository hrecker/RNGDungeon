using UnityEngine;

public class PlayerStatus
{
    public static Vector3 MapPosition { get; set; }
    public static int MaxHealth { get; set; }
    public static int Health { get; set; }
    public static string SelectedStance { get; set; }
    public static bool Initialized { get; set; }

    public static void InitializeIfNecessary()
    {
        if (!Initialized)
        {
            Restart();
        }
    }

    public static void Restart()
    {
        MaxHealth = 10;
        Health = MaxHealth;
        MapPosition = CurrentLevel.GetPlayerStartingPosition();
        SelectedStance = "Neutral";
        Initialized = true;
    }
}
