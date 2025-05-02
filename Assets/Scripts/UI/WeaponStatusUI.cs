using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatusUI : MonoBehaviour
{
    #region Header 物体引用
    [Space(10)]
    [Header("物体引用")]
    #endregion

    #region Tooltip
    [Tooltip("填入武器精灵")]
    #endregion
    [SerializeField] private Image weaponImage;

    #region Tooltip
    [Tooltip("填入自弹药架子对象的坐标")]
    #endregion
    [SerializeField] private Transform ammoHolderTransform;

    #region Tooltip
    [Tooltip("填入重新装填用的字体")]
    #endregion
    [SerializeField] private TextMeshProUGUI reloadText;

    #region Tooltip
    [Tooltip("填入子弹剩余量所用的字体")]
    #endregion
    [SerializeField] private TextMeshProUGUI ammoRemainingText;

    #region Tooltip
    [Tooltip("武器名称字体")]
    #endregion
    [SerializeField] private TextMeshProUGUI weaponNameText;

    #region Tooltip
    [Tooltip("填入重新装载进度栏")]
    #endregion
    [SerializeField] private Transform reloadBar;

    #region Tooltip
    [Tooltip("填入重新装载精灵")]
    #endregion
    [SerializeField] private Image barImage;

    private Player player;
    private List<GameObject> ammoIconList = new List<GameObject>();
    private Coroutine reloadWeaponCoroutine;
    private Coroutine blinkingReloadTextCoroutine;

    private void Awake()
    {
        player = GameManager.Instance.GetPlayer();
    }

    private void OnEnable()
    {
        player.setActiveWeaponEvent.OnSetActivrWeapon += SetActiveWeaponEvent_OnSetActivrWeapon;

        player.weaponFireEvent.OnWeaponFire += WeaponFireEvent_OnWeaponFire;

        player.reloadWeaponEvent.OnReloadWeapon += ReloadWeaponEvent_OnReloadWeapon;

        player.weaponReloadEvent.OnWeaponReloaded += WeaponReloadEvent_OnWeaponReloaded;
    }

    private void OnDisable()
    {
        player.setActiveWeaponEvent.OnSetActivrWeapon -= SetActiveWeaponEvent_OnSetActivrWeapon;

        player.weaponFireEvent.OnWeaponFire -= WeaponFireEvent_OnWeaponFire;

        player.reloadWeaponEvent.OnReloadWeapon -= ReloadWeaponEvent_OnReloadWeapon;

        player.weaponReloadEvent.OnWeaponReloaded -= WeaponReloadEvent_OnWeaponReloaded;
    }

    private void Start()
    {
        SetActiveWeapon(player.activeWeapon.GetCurrentWeapon());
    }

    private void SetActiveWeaponEvent_OnSetActivrWeapon(SetActiveWeaponEvent setActiveWeaponEvent,SetActivrWeaponArgs args)
    {
        SetActiveWeapon(args.weapon);

    }

    private void WeaponFireEvent_OnWeaponFire(WeaponFireEvent weaponFireEvent,WeaponFireArgs args)
    {
        WeaponFire(args.weapon);
    }

    private void ReloadWeaponEvent_OnReloadWeapon(ReloadWeaponEvent reloadWeaponEvent,ReloadWeaponArgs args)
    {
        UpdateWeaponReloadBar(args.weapon);
    }

    private void WeaponReloadEvent_OnWeaponReloaded(WeaponReloadEvent weaponReloadEvent,WeaponReloadArgs args)
    {
        WeaponReloaded(args.weapon);
    }

    private void SetActiveWeapon(Weapon weapon)
    {
        UpdateActiveWeaponImage(weapon.weaponsDetails);
        UpdateActiveWeaponName(weapon);
        UpdateAmmoText(weapon);
        UpdateAmmoLoadedIcons(weapon);

        if(weapon.isWeaponReloading)
        {
            UpdateWeaponReloadBar(weapon);
        }
        else
        {
            ResetWeaponReloadBar();
        }
        UpdateReloadText(weapon); 
    }

    private void WeaponFire(Weapon weapon)
    {
        UpdateAmmoText(weapon);
        UpdateAmmoLoadedIcons(weapon);
        UpdateReloadText(weapon);
    }

    private void UpdateWeaponReloadBar(Weapon weapon)
    {
        if(weapon.weaponsDetails.hasInfiniteClipCapacity)
        {
            return;
        }

        StopReloadWeaponCoroutine();
        UpdateReloadText(weapon);

        reloadWeaponCoroutine = StartCoroutine(UpdateWeaponReloadBarRoutine(weapon));
    }

    private void WeaponReloaded(Weapon weapon)//武器装填中
    {
        if(player.activeWeapon.GetCurrentWeapon() == weapon)
        {
            UpdateReloadText(weapon);
            UpdateAmmoText(weapon);
            UpdateAmmoLoadedIcons(weapon);
            ResetWeaponReloadBar();
        }
    }

    private void UpdateAmmoText(Weapon weapon)
    {
        if(weapon.weaponsDetails.hasInfiniteAmmo)
        {
            ammoRemainingText.text = "无限弹药";
        }
        else
        {
            ammoRemainingText.text = weapon.weaponRemainingAmmo.ToString() + "/" + weapon.weaponsDetails.weaponAmmoCapacity.ToString(); 
        }
    }

    private void UpdateAmmoLoadedIcons(Weapon weapon)
    {
        ClearAmmoLoadedIcons();

        for(int i = 0;i <weapon.weaponClipRemainingAmmo;i++)
        {
            GameObject ammoIcon = Instantiate(GameResources.Instance.ammoIconPrefab, ammoHolderTransform);

            ammoIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, Settings.uiAmmoIconSpacing * i);

            ammoIconList.Add(ammoIcon);
        }
    }

    private void UpdateReloadText(Weapon weapon)
    {
        if((!weapon.weaponsDetails.hasInfiniteClipCapacity) && (weapon.weaponClipRemainingAmmo <= 0 || weapon.isWeaponReloading))
        {
            barImage.color = Color.red;

            StopBlinkingReloadTextCoroutine();

            blinkingReloadTextCoroutine = StartCoroutine(StartBlinkingReloadTextRoutine());
        }
        else
        {
            StopBlinkingReloadText();
        }
    }

    private void ResetWeaponReloadBar()
    {
        StopReloadWeaponCoroutine();

        barImage.color = Color.green;

        reloadBar.transform.localScale = new Vector3(1f,1f,1f);
    }

    private void UpdateActiveWeaponImage(WeaponsDetailsSO weapon)
    {
        weaponImage.sprite = weapon.weaponSprite;
    }

    private void UpdateActiveWeaponName(Weapon weapon)
    {
        weaponNameText.text = "(" + weapon.weaponListPosition + ")" + weapon.weaponsDetails.weaponName;
    }

    private void ClearAmmoLoadedIcons()
    {
        foreach(GameObject ammoIcon in ammoIconList)
        {
            Destroy(ammoIcon);
        }
        ammoIconList.Clear();
    }

    private IEnumerator UpdateWeaponReloadBarRoutine(Weapon weapon)
    {
        barImage.color = Color.red;

        while(weapon.isWeaponReloading)
        {
            float barFill = weapon.weaponReloadTimer / weapon.weaponsDetails.weaponReloadTime;

            reloadBar.transform.localScale = new Vector3(barFill,1f,1f);

            yield return null;
        }
    }

    private void StopReloadWeaponCoroutine()
    {
        if(reloadWeaponCoroutine != null)
        {
            StopCoroutine(reloadWeaponCoroutine);
        }
    }

    private IEnumerator StartBlinkingReloadTextRoutine()
    {
        while(true)
        {
            reloadText.text = "装填中";
            yield return new WaitForSeconds(0.3f);
            reloadText.text = "";
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void StopBlinkingReloadText()
    {
        StopBlinkingReloadTextCoroutine();
        reloadText.text = "";
    }

    private void StopBlinkingReloadTextCoroutine()
    {
        if(blinkingReloadTextCoroutine != null)
        {
            StopCoroutine (blinkingReloadTextCoroutine);
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(weaponImage), weaponImage);
        HelpUtilities.ValidateCheckNullValues(this, nameof(ammoHolderTransform), ammoHolderTransform);
        HelpUtilities.ValidateCheckNullValues(this, nameof(reloadText), reloadText);
        HelpUtilities.ValidateCheckNullValues(this, nameof(ammoRemainingText), ammoRemainingText);
        HelpUtilities.ValidateCheckNullValues(this, nameof(weaponNameText), weaponNameText);
        HelpUtilities.ValidateCheckNullValues(this, nameof(reloadBar), reloadBar);
        HelpUtilities.ValidateCheckNullValues(this, nameof(barImage), barImage);
    }
#endif
    #endregion
}

