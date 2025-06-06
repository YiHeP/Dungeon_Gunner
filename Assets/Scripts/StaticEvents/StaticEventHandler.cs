using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StaticEventHandler
{
    public static event Action<RoomChangedEventArgs> OnRoomChanged;

    public static void CallRoomChangedEvent(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedEventArgs(){ room = room });
    }

    public static event Action<RoomEnemiesDefeatedArgs> OnRoomEnemiesDefeated;//�Ƿ񽫹������

    public static void CallRoomEnemiesDefeatedEvent(Room room)
    {
        OnRoomEnemiesDefeated?.Invoke(new RoomEnemiesDefeatedArgs() { room = room });
    }

    public static event Action<PointScoredArgs> OnPointScored;

    public static void CallPointsScoredEvent(int points)
    {
        OnPointScored?.Invoke(new PointScoredArgs() {  points = points });
    }

    public static event Action<ScoreChangedArgs> OnScoreChanged;

    public static void CallScoreChangedEvent(long score,int multiplier)
    {
        OnScoreChanged?.Invoke(new ScoreChangedArgs() { score = score , multiplier = multiplier});
    }

    public static event Action<MultiplierArgs> OnMultiplier;

    public static void CallMultiplierEvent(bool multiplier)
    {
        OnMultiplier?.Invoke(new MultiplierArgs() {  multiplier = multiplier });
    }
}

public class RoomChangedEventArgs : EventArgs
{
    public Room room;
}

public class RoomEnemiesDefeatedArgs:EventArgs
{
    public Room room;
}

public class PointScoredArgs:EventArgs
{
    public int points;
}

public class ScoreChangedArgs:EventArgs
{
    public long score;
    public int multiplier;
}

public class MultiplierArgs:EventArgs
{
    public bool multiplier;
}
