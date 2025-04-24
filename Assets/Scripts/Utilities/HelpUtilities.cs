using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpUtilities
{
    public static Camera mainCamera;

    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector3 mouseScreenPositon = Input.mousePosition;
        mouseScreenPositon.x = Mathf.Clamp(mouseScreenPositon.x, 0f, Screen.width);
        mouseScreenPositon.y = Mathf.Clamp(mouseScreenPositon.y, 0f, Screen.height);

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPositon);

        mouseWorldPosition.z = 0f;

        return mouseWorldPosition;
    }

    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }

    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle),Mathf.Sin(Mathf.Deg2Rad * angle),0f);
        return directionVector;
    }

    public static AimDirection GetAimDirection(float angleDegrees)
    {
        AimDirection aimDirection;
        if(angleDegrees >= 22f && angleDegrees <= 67f)
        {
            aimDirection = AimDirection.UpRight;
        }
        else if(angleDegrees > 67f && angleDegrees <= 112f)
        {
            aimDirection = AimDirection.Up;
        }
        else if(angleDegrees > 112f && angleDegrees <= 158f)
        {
            aimDirection = AimDirection.UpLeft;
        }
        else if((angleDegrees > 158f && angleDegrees <= 180f) || (angleDegrees > -180f && angleDegrees <= -135f))
        {
            aimDirection = AimDirection.Left;
        }
        else if(angleDegrees > -135f && angleDegrees <= -45f)
        {
            aimDirection = AimDirection.Down;
        }
        else if((angleDegrees > -45f && angleDegrees <= 0f) || (angleDegrees > 0f && angleDegrees < 22f))
        {
            aimDirection = AimDirection.Right;
        }
        else
        {
            aimDirection = AimDirection.Right;
        }
        return aimDirection;
    }

    public static bool ValidateCheckEmptyString(Object thisObject, string fileName, string stringToCheck)
    {
        if(stringToCheck == "")
        {
            Debug.Log(fileName + "是空的并且必须包含一个值在物体" + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    public static bool ValidateCheckNullValues(Object thisObject, string fileName, UnityEngine.Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(fileName + "为空并且包含一个值在" + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log(fieldName + " 为空在物体 " + thisObject.name.ToString());
            return true;
        }

        foreach (var item in enumerableObjectToCheck)
        {

            if (item == null)
            {
                Debug.Log(fieldName + "有空值在 " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fieldName + " 无值在 " + thisObject.name.ToString());
            error = true;
        }

        return error;
    }

    public static bool ValidateCheckPositiveValues(Object thisObject, string filename, int valueToCheck, bool isZeroAllowed)
    {
        bool error = false;
        if(isZeroAllowed)
        {
            if(valueToCheck < 0)
            {
                Debug.Log(filename + "必须包含正值或0在物体:" + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(filename + "必须包含正值=在物体:" + thisObject.name.ToString());
                error = true;
            }
        }
        return error;
    }

    public static bool ValidateCheckPositiveValues(Object thisObject, string filename, float valueToCheck, bool isZeroAllowed)
    {
        bool error = false;
        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(filename + "必须包含正值或0在物体:" + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(filename + "必须包含正值=在物体:" + thisObject.name.ToString());
                error = true;
            }
        }
        return error;
    }

    public static bool ValidateCheckPositiveRange(Object thisObject,string filenameMinMum,float valueToCheckMinMum,string filenameMaxMum,
        float valueToCheckMaxMum,bool isZeroAllowed)
    {
        bool error = false;
        if(valueToCheckMinMum > valueToCheckMaxMum)
        {
            Debug.Log(filenameMinMum + "必须小于或等于" + filenameMaxMum + "在物体" + thisObject.name.ToString());
        }

        if(ValidateCheckPositiveValues(thisObject, filenameMinMum, valueToCheckMinMum, isZeroAllowed)) error = true;
        if (ValidateCheckPositiveValues(thisObject,filenameMaxMum, valueToCheckMaxMum, isZeroAllowed)) error = true;

        return error;
    }

    public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPosition)
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;
        Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0);
        foreach(Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
        {
            Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);
            if(Vector3.Distance(spawnPositionWorld,playerPosition) < Vector3.Distance(nearestSpawnPosition,playerPosition))
            {
                nearestSpawnPosition = spawnPositionWorld;
            }
        }

        return nearestSpawnPosition;
    }
}
