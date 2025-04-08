using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;
    [SerializeField] private Transform cursorTarget;

    private void Awake()
    {
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start()
    {
        SetCinemachingTargetGroup();
    }

    private void SetCinemachingTargetGroup()
    {
        CinemachineTargetGroup.Target cinemachineTargetGroupTarget_Player = new CinemachineTargetGroup.Target { weight = 1f,
        radius = 2.5f, target = GameManager.Instance.GetPlayer().transform};

        CinemachineTargetGroup.Target cinemachineTargetGroupTarget_Cursor = new CinemachineTargetGroup.Target
        {
            weight = 1f,
            radius = 1f,
            target = cursorTarget
        };

        CinemachineTargetGroup.Target[] cinemachineTargetGroupTargetArray = new CinemachineTargetGroup.Target[] { cinemachineTargetGroupTarget_Player, cinemachineTargetGroupTarget_Cursor };
        cinemachineTargetGroup.m_Targets = cinemachineTargetGroupTargetArray;
    }

    private void Update()
    {
        cursorTarget.position = HelpUtilities.GetMouseWorldPosition();
    }

}
