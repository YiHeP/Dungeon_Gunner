using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeTypeList",menuName ="Scriptable Objects/room/房间节点类型列表")]
public class RoomNodeTypeListSO : ScriptableObject
{
    public List<RoomNodeTypeSO> list;

    #region Validation
#if UNITY_EDITOR
    public void OnValidate()
    {
        HelpUtilities.ValidateCheckEnumerableValues(this, nameof(list), list);
    }
#endif
    #endregion
}
