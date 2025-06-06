using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    void InitialiseAmmo(AmmoDetailSO ammoDetails,float aimAngle,float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector,
        bool overrideAmmoMovement = false);

    GameObject GetGameObject();
}
