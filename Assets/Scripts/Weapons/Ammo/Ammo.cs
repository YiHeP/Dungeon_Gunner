using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    #region Tooltip
    [Tooltip("ÓÃ×Ó¹ì¼£äÖÈ¾Æ÷½øÐÐÌî³ä")]
    #endregion
    [SerializeField] private TrailRenderer trailRenderer;

    private float ammoRange = 0f;//×Óµ¯´æÔÚµÄ·¶Î§
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private SpriteRenderer spriteRenderer;
    private AmmoDetailSO ammoDetails;
    private float ammoChargeTimer;
    private bool isAmmoMaterialSet = false;
    private bool overrideAmmoMovement;
    private bool isCollider = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(ammoChargeTimer > 0)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }
        else if(!isAmmoMaterialSet)
        {
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;
        transform.position += distanceVector;
        ammoRange -= distanceVector.magnitude;

        if(ammoRange < 0f)
        {
            if(ammoDetails.isPlayerAmmo)
            {
                StaticEventHandler.CallMultiplierEvent(false);
            }

            DisableAmmo();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollider) return;

        DealDamage(collision);

        AmmoHitEffect();

        DisableAmmo();
    }

    private void DealDamage(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        bool enemtHit = false;

        if(health != null)
        {
            isCollider = true;

            health.TakeDamage(ammoDetails.ammoDamage);

            if(health.enemy !=  null)
            {
                enemtHit = true;
            }
        }

        if(ammoDetails.isPlayerAmmo)
        {
            if(enemtHit)
            {
                StaticEventHandler.CallMultiplierEvent(true);
            }
            else
            {
                StaticEventHandler.CallMultiplierEvent(false);
            }
        }
    }

    public void AmmoHitEffect()
    {
        if(ammoDetails.ammoHitEffect != null && ammoDetails.ammoHitEffect.ammoHitEffectPrefab != null)
        {
            AmmoHitEffect ammoHitEffect = (AmmoHitEffect)PoolManager.Instance.ReuseComponent
                (ammoDetails.ammoHitEffect.ammoHitEffectPrefab,transform.position,Quaternion.identity);

            ammoHitEffect.SetHitEffect(ammoDetails.ammoHitEffect);

            ammoHitEffect.gameObject.SetActive(true);
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void InitialiseAmmo(AmmoDetailSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false)
    {
        #region ×Óµ¯
        this.ammoDetails = ammoDetails;
        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);
        spriteRenderer.sprite = ammoDetails.ammoSprite;
        isCollider = false;

        if(ammoDetails.ammoChargeTime > 0f)
        {
            ammoChargeTimer = ammoDetails.ammoChargeTime;
            SetAmmoMaterial(ammoDetails.ammoChargeMaterial);
            isAmmoMaterialSet = false;
        }
        else
        {
            ammoChargeTimer = 0f;
            SetAmmoMaterial(ammoDetails.ammoMaterial); 
            isAmmoMaterialSet = true;
        }
        ammoRange = ammoDetails.ammoRange;

        this.ammoSpeed = ammoSpeed;

        this.overrideAmmoMovement = overrideAmmoMovement;

        gameObject.SetActive(true);

        #endregion

        #region ºÛ¼£
        if(ammoDetails.isAmmoTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = ammoDetails.ammoTrailMaterial;
            trailRenderer.startWidth = ammoDetails.ammoTrailStartWidth;
            trailRenderer.endWidth = ammoDetails.ammoTrailEndtWidth;
            trailRenderer.time = ammoDetails.ammoTrailTime;
        }
        else
        {
            trailRenderer.emitting = false;
            trailRenderer.gameObject.SetActive(false);
        }

        #endregion
    }

    private void SetFireDirection(AmmoDetailSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);
        int spreadToggle = Random.Range(0, 2) * 2 - 1;
        if(weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else
        {
            fireDirectionAngle = weaponAimAngle;
        }

        fireDirectionAngle += spreadToggle * randomSpread;

        transform.eulerAngles = new Vector3(0, 0,fireDirectionAngle);

        fireDirectionVector = HelpUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);

    }

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    private void SetAmmoMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    #region
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(trailRenderer), trailRenderer);  
    }
#endif
    #endregion

}
