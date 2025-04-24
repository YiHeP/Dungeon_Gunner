using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SetActiveWeaponEvent : MonoBehaviour
{
    public event Action<SetActiveWeaponEvent, SetActivrWeaponArgs> OnSetActivrWeapon;

    public void CallSetActiveWeaponEvent(Weapon weapon)
    {
        OnSetActivrWeapon?.Invoke(this,new SetActivrWeaponArgs() { weapon = weapon });
    }
}

public class SetActivrWeaponArgs : EventArgs
{
    public Weapon weapon;
}

