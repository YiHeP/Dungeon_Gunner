using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnableObject<T>
{
    private struct chanceBoundaries
    {
        public T spawnableObject;
        public int lowBoundaryValue;
        public int highBoundaryValue;
    }

    private int ratioValueTotal = 0;

    private List<chanceBoundaries> chanceBoundariesList = new List<chanceBoundaries>();
    private List<SpawnableObjectByLevel<T>> spawnableObjectByLevelLsit;

    public RandomSpawnableObject(List<SpawnableObjectByLevel<T>> spawnableObjectByLevelList)
    {
        this.spawnableObjectByLevelLsit = spawnableObjectByLevelList;
    }

    public T GetItem()
    {
        int upperBoundary = -1;
        ratioValueTotal = 0;
        chanceBoundariesList.Clear();
        T spawnableObject = default(T);

        foreach(SpawnableObjectByLevel<T> spawnableObjectByLevel in spawnableObjectByLevelLsit)
        {
            if(spawnableObjectByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                foreach(SpawnableObjectRatio<T> spawnableObjectRatio in spawnableObjectByLevel.spawnableObjectRatiosList)
                {
                    int lowBoundary = upperBoundary + 1;
                    upperBoundary = lowBoundary + spawnableObjectRatio.ratio - 1;
                    ratioValueTotal += spawnableObjectRatio.ratio;

                    chanceBoundariesList.Add(new chanceBoundaries()
                    {
                        spawnableObject = spawnableObjectRatio.dungeonObject,
                        lowBoundaryValue = lowBoundary,
                        highBoundaryValue = upperBoundary,
                    });
                }
            }
        }

        if(chanceBoundariesList.Count == 0)
        {
            return default(T);
        }
        int lookUpvalue = Random.Range(0, ratioValueTotal);

        foreach(chanceBoundaries spawnChance in chanceBoundariesList)
        {
            if(lookUpvalue >= spawnChance.lowBoundaryValue && lookUpvalue <= spawnChance.highBoundaryValue)
            {
                spawnableObject = spawnChance.spawnableObject;
                break;
            }
        }

        return spawnableObject;
    }
}
