using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel", menuName = "Scriptable Objects/Dungeon/���ιؿ�")]
public class DungeonLevelSO : ScriptableObject
{
    #region Header �ؿ�����ϸ��
    [Space(10)]
    [Header("�ؿ�����ϸ��")]
    #endregion Header �ؿ�����ϸ��

    #region Tooltip
    [Tooltip("�ؿ���")]
    #endregion Tooltip

    public string levelName;

    #region �ؿ��ķ���ģ��
    [Space(10)]
    [Header("�ؿ��ķ���ģ��")]
    #endregion

    public List<RoomTemplateSO> roomTemplateList;

    #region Header �ؿ��ķ���ڵ�ͼ
    [Space(10)]
    [Header("�ؿ��ķ���ڵ�ͼ")]
    #endregion Header �ؿ��ķ���ڵ�ͼ
    #region Tooltip
    [Tooltip("�ؿ��Ӵ˴�����ѡ�񷿼�ڵ�ͼ")]
    #endregion Tooltip

    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this,nameof(levelName),levelName);
        if (HelpUtilities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
            return;
        if (HelpUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
            return;

        bool isEWCorridor = false;
        bool isNSCorridor = false;
        bool isEntrance = false;

        foreach (RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if (roomTemplate == null) 
                return;
            if (roomTemplate.roomNodeType.isCorridorEW)
                isEWCorridor = true;
            if(roomTemplate.roomNodeType.isCorridorNS)
                isNSCorridor = true;
            if(roomTemplate.roomNodeType.isEntrance)
                isEntrance = true;
        }

        if (!isEWCorridor)
        {
            Debug.Log("��" + this.name.ToString() + "�޶�����������");
        }
        if (!isNSCorridor)
        {
            Debug.Log("��" + this.name.ToString() + "���ϱ���������");
        }
        if (!isEntrance)
        {
            Debug.Log("��" + this.name.ToString() + "�����");
        }

        foreach (RoomNodeGraphSO roomNodeGraph in roomNodeGraphList)
        {
            if (roomNodeGraph == null)
                return;
            foreach(RoomNodeSO roomNodeSO in roomNodeGraph.roomNodeList)
            {
                if(roomNodeSO == null) 
                    continue;
                if (roomNodeSO.roomNodeType.isEntrance || roomNodeSO.roomNodeType.isCorridorEW || roomNodeSO.roomNodeType.isCorridorNS ||
                    roomNodeSO.roomNodeType.isCorridor || roomNodeSO.roomNodeType.isEntrance)
                    continue;
                bool isRoomNodeTypeFound = false;

                foreach(RoomTemplateSO roomTemplate in roomTemplateList)
                {
                    if (roomTemplate == null) 
                        continue;
                    if(roomNodeSO.roomNodeType ==  roomTemplate.roomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                }
                if(!isRoomNodeTypeFound)
                {
                    Debug.Log("��"+this.name.ToString()+"��û����"+roomNodeGraph.name.ToString()+"���ҵ����±���¼�ķ���ģ��"+
                        roomNodeSO.roomNodeType.name.ToString());
                }
            }
        }
    }

#endif
    #endregion Validation
}
