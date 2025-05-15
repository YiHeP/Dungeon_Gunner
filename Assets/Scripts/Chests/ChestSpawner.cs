using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [System.Serializable]

    private struct RangeByLevel
    {
        public DungeonLevelSO dungeonLevel;
        [Range(0, 100)] public int min;
        [Range(0, 100)] public int max;
    }

    #region Header 宝箱预制体
    [Space(10)]
    [Header("宝箱预制体")]
    #endregion

    #region Tooltip
    [Tooltip("填入宝箱预制体")]
    #endregion
    [SerializeField] private GameObject chestPrefab;

    #region Header 宝箱生成概率
    [Space(10)]
    [Header("宝箱生成概率")]
    #endregion

    #region Tooltip
    [Tooltip("最小概率生成宝箱")]
    #endregion
    [SerializeField][Range(0, 100)] private int chestSpawnChanceMin;

    #region Tooltip
    [Tooltip("最大概率生成宝箱")]
    #endregion
    [SerializeField][Range(0, 100)] private int chestSpawnChanceMax;

    #region Tootip
    [Tooltip("通过地牢关卡覆盖生成几率")]
    #endregion
    [SerializeField] private List<RangeByLevel> rangeByLevelList;

    #region Header 宝箱生成细节
    [Space(10)]
    [Header("宝箱生成细节")]
    #endregion

    [SerializeField] private ChestSpawnEvent chestSpawnEvent;
    [SerializeField] private ChestSpawnPosition chestSpawnPosition;

    #region Tooltip
    [Tooltip("生成物品数量的最小值")]
    #endregion
    [SerializeField][Range(0, 3)] private int numberOfItemsToSpawnMin;

    #region Tooltip
    [Tooltip("生成物品数量的最大值")]
    #endregion
    [SerializeField][Range(0, 3)] private int numberOfItemsToSpawnMax;

    #region Header 宝箱内容细节
    [Space(10)]
    [Header("宝箱内容细节")]
    #endregion

    #region Tooltip
    [Tooltip("武器在每层地牢的生成几率")]
    #endregion
    [SerializeField] private List<SpawnableObjectByLevel<WeaponsDetailsSO>> weaponsSpawnByLevelList;

    #region Tooltip
    [Tooltip("生命值每层生成的范围")]
    #endregion
    [SerializeField] private List<RangeByLevel> healthSpawnByLevelList;

    #region Tooltip
    [Tooltip("子弹每层生成的范围")]
    #endregion
    [SerializeField] private List<RangeByLevel> ammopSpawnByLevelList;

    private bool chestSpawned = false;
    private Room chestRoom;

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;

        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs args)
    {
        if (chestRoom == null)
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;

        if (!chestSpawned && chestSpawnEvent == ChestSpawnEvent.onRoomEntry && chestRoom == args.room)
        {
            SpawnChest();
        }
    }

    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs args)
    {
        if (chestRoom == null)
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;

        if (!chestSpawned && chestSpawnEvent == ChestSpawnEvent.onEnemiesDefeated && chestRoom == args.room)
        {
            SpawnChest();
        }
    }

    private void SpawnChest()
    {
        chestSpawned = true;

        if (!RandomSpawnChest()) return;

        GetItemsToSpawn(out int ammoNum, out int healthNum, out int weaponNum);

        GameObject chestGameObject = Instantiate(chestPrefab, this.transform);

        if (chestSpawnPosition == ChestSpawnPosition.atSpawnerPosition)
        {
            chestGameObject.transform.position = this.transform.position;
        }
        else if (chestSpawnPosition == ChestSpawnPosition.atPlayerPosition)
        {
            Vector3 spawnPosition = HelpUtilities.GetSpawnPositionNearestToPlayer(GameManager.Instance.GetPlayer().transform.position);

            Vector3 variation = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            chestGameObject.transform.position = spawnPosition + variation;
        }

        Chest chest = chestGameObject.GetComponent<Chest>();

        if (chestSpawnEvent == ChestSpawnEvent.onRoomEntry)
        {
            chest.Initialize(false, GetHealthPercentToSpawn(healthNum), GetWeawponDetailsToSpawn(weaponNum), GetAmmoPercentToSpawn(ammoNum));
        }
        else
        {
            chest.Initialize(true, GetHealthPercentToSpawn(healthNum), GetWeawponDetailsToSpawn(weaponNum), GetAmmoPercentToSpawn(ammoNum));
        }
    }

    private bool RandomSpawnChest()
    {
        int chestPercent = Random.Range(chestSpawnChanceMin, chestSpawnChanceMax + 1);

        foreach (RangeByLevel rangeByLevel in rangeByLevelList)
        {
            if (rangeByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                chestPercent = Random.Range(rangeByLevel.min, rangeByLevel.max);
                break;
            }
        }

        int randomPercent = Random.Range(1, 100 + 1);
        if (randomPercent <= chestPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GetItemsToSpawn(out int ammo, out int health, out int weapons)
    {
        ammo = 0;
        health = 0;
        weapons = 0;

        int numberOfItemsToSpawn = Random.Range(numberOfItemsToSpawnMin, numberOfItemsToSpawnMax);

        int choice;

        if (numberOfItemsToSpawn == 1)
        {
            choice = Random.Range(0, 3);
            if (choice == 0)
            {
                weapons++;
                return;
            }
            else if (choice == 1)
            {
                ammo++;
                return;
            }
            else if (choice == 2)
            {
                health++;
                return;
            }
            return;
        }
        else if (numberOfItemsToSpawn == 2)
        {
            choice = Random.Range(0, 3);
            if (choice == 0)
            {
                weapons++;
                ammo++;
                return;
            }
            else if (choice == 1)
            {
                ammo++;
                health++;
                return;
            }
            else if (choice == 2)
            {
                health++;
                weapons++;
                return;
            }
            return;
        }
        else if (numberOfItemsToSpawn >= 3)
        {
            ammo++;
            health++;
            weapons++;
        }
    }

    private int GetHealthPercentToSpawn(int num)
    {
        if (num == 0) return 0;

        foreach (RangeByLevel spawnPercentByLevel in healthSpawnByLevelList)
        {
            if (spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;
    }

    private WeaponsDetailsSO GetWeawponDetailsToSpawn(int num)
    {
        if(num == 0) return null;

        RandomSpawnableObject<WeaponsDetailsSO> weaponRandom = new RandomSpawnableObject<WeaponsDetailsSO>(weaponsSpawnByLevelList);

        WeaponsDetailsSO weaponsDetails = weaponRandom.GetItem();

        return weaponsDetails;
    }

    private int GetAmmoPercentToSpawn(int num)
    {
        if (num == 0) return 0;

        foreach(RangeByLevel spawnPercentByLevel in ammopSpawnByLevelList)
        {
            if(spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(chestPrefab), chestPrefab);
        HelpUtilities.ValidateCheckPositiveRange(this,nameof(chestSpawnChanceMin),chestSpawnChanceMin,nameof(chestSpawnChanceMax),chestSpawnChanceMax,true);
        if(rangeByLevelList != null && rangeByLevelList.Count > 0)
        {
            HelpUtilities.ValidateCheckEnumerableValues(this,nameof(rangeByLevelList),rangeByLevelList);

            foreach(RangeByLevel rangeByLevel in rangeByLevelList)
            {
                HelpUtilities.ValidateCheckNullValues(this,nameof(rangeByLevel.dungeonLevel),rangeByLevel.dungeonLevel);
                HelpUtilities.ValidateCheckPositiveRange(this,nameof(rangeByLevel.min),rangeByLevel.min,nameof(rangeByLevel.max),rangeByLevel.max,true);
            }
        }
        HelpUtilities.ValidateCheckPositiveRange(this,nameof(numberOfItemsToSpawnMin),numberOfItemsToSpawnMin,nameof(numberOfItemsToSpawnMax),
            numberOfItemsToSpawnMax,true);

        if(weaponsSpawnByLevelList != null && weaponsSpawnByLevelList.Count > 0)
        {
            foreach(SpawnableObjectByLevel<WeaponsDetailsSO> weaponDetailsByLevel in weaponsSpawnByLevelList)
            {
                HelpUtilities.ValidateCheckNullValues(this,nameof(weaponDetailsByLevel.dungeonLevel),weaponDetailsByLevel.dungeonLevel);

                foreach(SpawnableObjectRatio<WeaponsDetailsSO> weaponRatio in weaponDetailsByLevel.spawnableObjectRatiosList)
                {
                    HelpUtilities.ValidateCheckNullValues(this, nameof(weaponRatio.dungeonObject),weaponRatio.dungeonObject);
                    HelpUtilities.ValidateCheckPositiveValues(this,nameof(weaponRatio.ratio),weaponRatio.ratio,true);
                }
            }
        }

        if (healthSpawnByLevelList != null && healthSpawnByLevelList.Count > 0)
        {
            HelpUtilities.ValidateCheckEnumerableValues(this,nameof(healthSpawnByLevelList),healthSpawnByLevelList);

            foreach(RangeByLevel rangeByLevel in healthSpawnByLevelList)
            {
                HelpUtilities.ValidateCheckNullValues(this,nameof(rangeByLevel.dungeonLevel),rangeByLevel.dungeonLevel);
                HelpUtilities.ValidateCheckPositiveRange(this, nameof(rangeByLevel.min), rangeByLevel.min, nameof(rangeByLevel.max), rangeByLevel.max,
                    true);
            }
        }

        if (ammopSpawnByLevelList != null && ammopSpawnByLevelList.Count > 0)
        {
            HelpUtilities.ValidateCheckEnumerableValues(this, nameof(ammopSpawnByLevelList), ammopSpawnByLevelList);

            foreach (RangeByLevel rangeByLevel in ammopSpawnByLevelList)
            {
                HelpUtilities.ValidateCheckNullValues(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                HelpUtilities.ValidateCheckPositiveRange(this, nameof(rangeByLevel.min), rangeByLevel.min, nameof(rangeByLevel.max), rangeByLevel.max,
                    true);
            }
        }

    }
#endif
    #endregion
}
