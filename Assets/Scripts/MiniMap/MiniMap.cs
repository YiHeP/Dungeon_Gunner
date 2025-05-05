using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[DisallowMultipleComponent]
public class MiniMap : MonoBehaviour
{
    #region Tooltip
    [Tooltip("填入玩家在小地图上的头像")]
    #endregion
    [SerializeField]private GameObject miniMapPlayer;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.GetPlayer().transform;

        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        cinemachineVirtualCamera.Follow = playerTransform;

        SpriteRenderer spriteRenderer = miniMapPlayer.GetComponent<SpriteRenderer>();

        if(spriteRenderer != null )
        {
            spriteRenderer.sprite = GameManager.Instance.GetMiniMapPlayer();
        }
    }

    private void Update()
    {
        if( miniMapPlayer != null && playerTransform != null) 
        {
            miniMapPlayer.transform.position = playerTransform.position;
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(miniMapPlayer),miniMapPlayer);
    }
#endif
    #endregion
}
