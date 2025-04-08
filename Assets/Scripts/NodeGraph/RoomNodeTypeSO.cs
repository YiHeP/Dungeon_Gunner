using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RoomNodeType",menuName ="Scriptable Objects/room/房间类型")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName;

    public bool displayInNodeGraphEditor = true;

    public bool isCorridor;

    public bool isCorridorNS;

    public bool isCorridorEW;

    public bool isEntrance;

    public bool isBossRoom;

    public bool isNone;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif
    #endregion
}
