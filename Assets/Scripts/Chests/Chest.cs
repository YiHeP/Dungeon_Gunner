using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MaterializeEffect))]
public class Chest : MonoBehaviour, IUseable
{
    #region Tooltip
    [Tooltip("设置颜色")]
    #endregion
    [ColorUsage(false,true)]
    [SerializeField] private Color materializeColor;

    #region Tooltip
    [Tooltip("设置时间")]
    #endregion
    [SerializeField] private float materializeTime = 3f;

    #region Tooltip
    [Tooltip("设置物品的生成点")]
    #endregion
    [SerializeField] private Transform itemSpawnPoint;

    private int healthPercent;
    private WeaponsDetailsSO weaponDetails;
    private int ammoPercent;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MaterializeEffect materializeEffect;
    private bool isEnable = false;
    private ChestState chestState = ChestState.closed;
    private GameObject chestItemGameObjecet;
    private ChestItem chestItem;
    private TextMeshPro messageTextTMP;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        materializeEffect = GetComponent<MaterializeEffect>();
        messageTextTMP = GetComponent<TextMeshPro>();
    }

    public void Initialize(bool shouldMaterialize, int healthPercent, WeaponsDetailsSO weaponsDetails, int ammoPercent)
    {
        this.healthPercent = healthPercent;
        this.weaponDetails = weaponsDetails;
        this.ammoPercent = ammoPercent;

        if(shouldMaterialize)
        {
            StartCoroutine(MaterializeChest());
        }
        else
        {
            EnableChest();
        }
    }

    private IEnumerator MaterializeChest()
    {
        SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] {spriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance.materializeShader, materializeColor, materializeTime,
            spriteRendererArray, GameResources.Instance.litMaterial));

        EnableChest();
    }

    private void EnableChest()
    {
        isEnable = true;
    }

    public void UseItem()
    {
        if (!isEnable) return;

        switch(chestState)
        {
            case ChestState.closed:
                OpenChest();
                break;
            case ChestState.healthItem:
                CollectHealthItem();
                break;
            case ChestState.weaponItem:
                CollectWeaponItem();
                break;
            case ChestState.ammoItem:
                CollectAmmoItem();
                break;
            case ChestState.empty:
                return;
            default:
                return;
        }
    }

    private void OpenChest()
    {
        animator.SetBool(Settings.use, true);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);

        if(weaponDetails != null)
        {
            if (GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
                weaponDetails = null;
        }

        UpdateChestStatus();
    }

    private void UpdateChestStatus()
    {
        if(healthPercent != 0)
        {
            chestState = ChestState.healthItem;
            InstantiateHealthItem();
        }
        else if(weaponDetails != null) 
        {
            chestState = ChestState.weaponItem;
            InstantiateWeaponItem();
        }
        else if(ammoPercent != 0)
        {
            chestState = ChestState.ammoItem;
            InstantiateAmmoItem();
        }
        else
        {
            chestState= ChestState.empty;
        }
    }
    private void InstantiateItem()
    {
        chestItemGameObjecet = Instantiate(GameResources.Instance.chestItemPerfab, this.transform);

        chestItem = chestItemGameObjecet.GetComponent<ChestItem>();
    }

    private void InstantiateHealthItem()
    {
        InstantiateItem();
        chestItem.Initialize(GameResources.Instance.heartIcon,healthPercent.ToString() + "%", itemSpawnPoint.position,materializeColor);
    }

    private void InstantiateWeaponItem()
    {
        InstantiateItem();
        chestItemGameObjecet.GetComponent<ChestItem>().Initialize(weaponDetails.weaponSprite,weaponDetails.weaponName,itemSpawnPoint.position,materializeColor);
    }

    private void InstantiateAmmoItem()
    {
        InstantiateItem();
        chestItem.Initialize(GameResources.Instance.bulletIcon, ammoPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);
    }

    private void CollectHealthItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        GameManager.Instance.GetPlayer().health.AddHealth(healthPercent);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickUp);

        healthPercent = 0;

        Destroy(chestItemGameObjecet);

        UpdateChestStatus();
    }

    private void CollectWeaponItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        if(!GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
        {
            GameManager.Instance.GetPlayer().AddWeaponToPlayer(weaponDetails);

            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickUp);
        }
        else
        {
            StartCoroutine(DisPlayMessage("武器\n已被\n装备",5f));
        }
        weaponDetails = null;

        Destroy(chestItemGameObjecet);

        UpdateChestStatus();
    }

    private void CollectAmmoItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        Player player = GameManager.Instance.GetPlayer();

        player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(),ammoPercent);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickUp);

        ammoPercent = 0;

        Destroy(chestItemGameObjecet);

        UpdateChestStatus();
    }

    private IEnumerator DisPlayMessage(string message,float messageDisPlayTime)
    {
        messageTextTMP.text = message;

        yield return new WaitForSeconds(messageDisPlayTime);

        messageTextTMP.text = "";
    }
}
