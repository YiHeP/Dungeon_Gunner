using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFireEvent : MonoBehaviour
{
    public Action<WeaponFireEvent, WeaponFireArgs> OnWeaponFire;

    public void CallWeaponFireEvent(Weapon weapon)
    {
        OnWeaponFire?.Invoke(this,new WeaponFireArgs() { weapon = weapon });
    }
}

public class WeaponFireArgs : EventArgs
{
    public Weapon weapon;
}

