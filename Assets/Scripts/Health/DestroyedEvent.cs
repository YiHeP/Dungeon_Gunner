using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent,DestroyedEventArgs> OnDestroyed;

    public void CallDestroyedEvent(bool isPlayerDied)
    {
        OnDestroyed?.Invoke(this,new DestroyedEventArgs() { isPlayDied = isPlayerDied });
    }
}

public class DestroyedEventArgs:EventArgs
{
    public bool isPlayDied;
}
