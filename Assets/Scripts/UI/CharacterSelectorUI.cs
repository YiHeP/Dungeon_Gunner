using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class CharacterSelectorUI : MonoBehaviour
{
    #region Tooltip
    [Tooltip("填入玩家选择的物体")]
    #endregion
    [SerializeField] private Transform characterSelector;

    #region Tooltip
    [Tooltip("填入玩家姓名的输入组件")]
    #endregion
    [SerializeField] private TMP_InputField playerNameInput;
    private List<PlayerDetailsSO> playerDetailsList;
    private GameObject playerSelectionPrefab;
    private CurrentPlayerSO currentPlayer;
    private List<GameObject> playerCharacterGameObjectList = new List<GameObject>();
    private Coroutine coroutine;
    [SerializeField] private int selectedPlayerIndex = 0;
    private float offset = 4f;

    private void Awake()
    {
        playerSelectionPrefab = GameResources.Instance.playerSelectionPrefab;
        playerDetailsList = GameResources.Instance.playerDetailsList;
        currentPlayer = GameResources.Instance.currentPlayer;
    }

    private void Start()
    {
        for(int i = 0; i < playerDetailsList.Count; i++)
        {
            GameObject playerSelectionObjects = Instantiate(playerSelectionPrefab, characterSelector);
            playerCharacterGameObjectList.Add(playerSelectionObjects);
            playerSelectionObjects.transform.localPosition = new Vector3((offset * i),0f,0f);
            PopulatePlayerDetails(playerSelectionObjects.GetComponent<PlayerSelectionUI>(), playerDetailsList[i]);
        }

        playerNameInput.text = currentPlayer.playerName;

        currentPlayer.playerDetails = playerDetailsList[selectedPlayerIndex];
    }

    private void PopulatePlayerDetails(PlayerSelectionUI playerSelectionUI,PlayerDetailsSO playerDetails)
    {
        playerSelectionUI.playerHandSpriteRenderer.sprite = playerDetails.PlayerHandSprite;
        playerSelectionUI.playerHandNoWeaponSpriteRenderer.sprite = playerDetails.PlayerHandSprite;
        playerSelectionUI.playerWeaponSpriteRenderer.sprite = playerDetails.staringWeapon.weaponSprite;
        playerSelectionUI.animator.runtimeAnimatorController = playerDetails.runtimeAnimatorController;
    }

    public void NextCharacter()
    {
        if (selectedPlayerIndex >= playerDetailsList.Count - 1)
            return;
        selectedPlayerIndex++;

        currentPlayer.playerDetails = playerDetailsList[selectedPlayerIndex];

        MoveToSelectedCharacter(selectedPlayerIndex);

    }

    public void PreviousCharacter()
    {
        if (selectedPlayerIndex == 0)
            return;
        selectedPlayerIndex--;

        currentPlayer.playerDetails = playerDetailsList[selectedPlayerIndex];

        MoveToSelectedCharacter(selectedPlayerIndex);
    }

    private void MoveToSelectedCharacter(int index)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(MoveToSelectedCharacterRoutine(index));
    }

    private IEnumerator MoveToSelectedCharacterRoutine(int index)
    {
        float currentLocalXPosition = characterSelector.localPosition.x;
        float targetLocalXPostion = index * offset * characterSelector.localScale.x * -1f;

        while(Mathf.Abs(currentLocalXPosition - targetLocalXPostion) > 0.01f)
        {
            currentLocalXPosition = Mathf.Lerp(currentLocalXPosition, targetLocalXPostion,Time.deltaTime * 10f);
            characterSelector.localPosition = new Vector3(currentLocalXPosition, characterSelector.localPosition.y, 0f);

            yield return null;
        }

        characterSelector.localPosition = new Vector3(targetLocalXPostion, characterSelector.localPosition.y, 0f);
    }

    public void UpdatePlayerName()
    {
        currentPlayer.playerName = playerNameInput.text;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(characterSelector), characterSelector);
        HelpUtilities.ValidateCheckNullValues(this, nameof(playerNameInput), playerNameInput);
    }
#endif
    #endregion
}
