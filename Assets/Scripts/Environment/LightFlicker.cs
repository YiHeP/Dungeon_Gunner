using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[DisallowMultipleComponent]
public class LightFlicker : MonoBehaviour
{
    private Light2D light2D;
    [SerializeField] private float lightIntensityMin;//��˸ʱ��
    [SerializeField] private float lightIntensityMax;
    [SerializeField] private float lightFlickerTimeMin;//���ʱ��
    [SerializeField] private float lightFlickerTimeMax;
    private float lightFlickerTimer;//��ʱ��

    private void Awake()
    {
        light2D = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        lightFlickerTimer = Random.Range(lightFlickerTimeMin, lightFlickerTimeMax);
    }

    private void Update()
    {
        if (light2D == null)
            return;
        lightFlickerTimer -= Time.deltaTime;

        if(lightFlickerTimer < 0f)
        {
            lightFlickerTimer = Random.Range(lightFlickerTimeMin,lightFlickerTimeMax);

            RandomiseLightIntensity();
        }

    }

    private void RandomiseLightIntensity()
    {
        light2D.intensity = Random.Range(lightIntensityMin, lightIntensityMax);
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckPositiveRange(this, nameof(lightFlickerTimeMin), lightFlickerTimeMin, nameof(lightFlickerTimeMax), lightFlickerTimeMax,
            false);
        HelpUtilities.ValidateCheckPositiveRange(this,nameof(lightIntensityMin),lightIntensityMin, nameof(lightIntensityMax), lightIntensityMax, false);
    }
#endif
    #endregion
}
