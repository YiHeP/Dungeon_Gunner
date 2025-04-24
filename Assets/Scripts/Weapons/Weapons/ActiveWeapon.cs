using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
    #region Tooltip
    [Tooltip("ÎäÆ÷¾«Áé")]
    #endregion
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

    #region Tooltip
    [Tooltip("ÎäÆ÷µÄÅö×²Ìå")]
    #endregion
    [SerializeField] private PolygonCollider2D weaponPolygonCollider2D;

    #region Tooltip
    [Tooltip("ÎäÆ÷Éä»÷Î»ÖÃ")]
    #endregion
    [SerializeField] private Transform weaponShootPositionTransform;

    #region Tooltip
    [Tooltip("ÎäÆ÷Ó°ÏìÎ»ÖÃ")]
    #endregion
    [SerializeField] private Transform weaponEffectPositionTransform;

    private SetActiveWeaponEvent weaponEvent;
    private Weapon currentWeapon;

    private void Awake()
    {
        weaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    private void OnEnable()
    {
        weaponEvent.OnSetActivrWeapon += weaponEvent_OnSetActivrWeapon;
    }

    private void OnDisable()
    {
        weaponEvent.OnSetActivrWeapon -= weaponEvent_OnSetActivrWeapon;
    }

    private void weaponEvent_OnSetActivrWeapon(SetActiveWeaponEvent setActiveWeaponEvent,SetActivrWeaponArgs setActivrWeaponArgs)
    {
        SetWeapon(setActivrWeaponArgs.weapon);
    }

    private void SetWeapon(Weapon weapon)
    {
        currentWeapon = weapon;

        weaponSpriteRenderer.sprite = currentWeapon.weaponsDetails.weaponSprite;

        //ÈÃÅö×²Æ÷ÊÊÓÃÓÚÎäÆ÷¾«Áé
        if(weaponPolygonCollider2D != null && weaponSpriteRenderer.sprite != null)
        {
            List<UnityEngine.Vector2> spritePhysicsShapePointsList = new List<UnityEngine.Vector2>();
            weaponSpriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList);
            weaponPolygonCollider2D.points = spritePhysicsShapePointsList.ToArray();

        }
        weaponShootPositionTransform.localPosition = currentWeapon.weaponsDetails.weaponShootPosition;
    }

    public AmmoDetailSO GetCurrentAmmo()
    {
        return currentWeapon.weaponsDetails.weaponCurrentAmmo;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public UnityEngine.Vector3 GetShootPosition()
    {
        return weaponShootPositionTransform.position;
    }

    public UnityEngine.Vector3 GetEffectPosition()
    {
        return weaponEffectPositionTransform.position;
    }

    public void RemoveCurrentWeapon()
    {
        currentWeapon = null;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(weaponSpriteRenderer), weaponSpriteRenderer);
        HelpUtilities.ValidateCheckNullValues(this, nameof(weaponPolygonCollider2D), weaponPolygonCollider2D);
        HelpUtilities.ValidateCheckNullValues(this, nameof(weaponShootPositionTransform), weaponShootPositionTransform);
        HelpUtilities.ValidateCheckNullValues(this, nameof(weaponEffectPositionTransform), weaponEffectPositionTransform);
    }
#endif
    #endregion
}
