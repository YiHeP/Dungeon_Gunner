using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponShootEffect : MonoBehaviour
{
    private ParticleSystem shootEffectParticleSystem;

    private void Awake()
    {
        shootEffectParticleSystem = GetComponent<ParticleSystem>();
    }

    public void SetShootEffect(WeaponShootEffectSO weaponShootEffect,float aimAngle)
    {
        SetShootEffectColorGradient(weaponShootEffect.colorGradient);//颜色渐变

        SetShootEffectParticleStartingValue(weaponShootEffect.duration, weaponShootEffect.startParticleSize, weaponShootEffect.startParticleSpeed,
            weaponShootEffect.startLifeTime, weaponShootEffect.effectGravity, weaponShootEffect.maxdParticlesNumber);//初始值

        SetShootEffectParticleEmission(weaponShootEffect.emissionRate, weaponShootEffect.burstParticleNumber);//生成量

        SetEmmitterRotation(aimAngle);//旋转

        SetShootEffectParticleSprite(weaponShootEffect.effectSprite);//精灵

        SetShootEffectVelocityOverLifeTime(weaponShootEffect.velocityOverLifeTimeMin, weaponShootEffect.velocityOverLifeTimeMax);//生命周期
    }

    private void SetShootEffectColorGradient(Gradient gradient)
    {
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = shootEffectParticleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = gradient;
    }

    private void SetShootEffectParticleStartingValue(float duration, float startParticleSize, float startParticleSpeed, float startLifeTime, 
        float effectGravity, int maxdParticlesNumber)
    {
        ParticleSystem.MainModule mainModule = shootEffectParticleSystem.main;
        mainModule.duration = duration;
        mainModule.startSize = startParticleSize;
        mainModule.startSpeed = startParticleSpeed;
        mainModule.startLifetime = startLifeTime;
        mainModule.gravityModifier = effectGravity;
        mainModule.maxParticles = maxdParticlesNumber;
    }

    private void SetShootEffectParticleEmission(int emissionRate,int burstParticleNumber)
    {
        ParticleSystem.EmissionModule emissionModule = shootEffectParticleSystem.emission;

        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, burstParticleNumber);

        emissionModule.SetBurst(0, burst);

        emissionModule.rateOverTime = emissionRate;
    }

    private void SetEmmitterRotation(float angle)
    {
        transform.eulerAngles = new Vector3(0f,0f,angle);
    }

    private void SetShootEffectParticleSprite(Sprite sprite)
    {
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = shootEffectParticleSystem.textureSheetAnimation;
        textureSheetAnimationModule.SetSprite(0,sprite);
    }

    private void SetShootEffectVelocityOverLifeTime(Vector3 minVelocity,Vector3 maxVelocity)
    {
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = shootEffectParticleSystem.velocityOverLifetime;

        ParticleSystem.MinMaxCurve minMaxCurveX = new ParticleSystem.MinMaxCurve();
        minMaxCurveX.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveX.constantMin = minVelocity.x;
        minMaxCurveX.constantMax = maxVelocity.x;
        velocityOverLifetimeModule.x = minMaxCurveX;

        ParticleSystem.MinMaxCurve minMaxCurveY = new ParticleSystem.MinMaxCurve();
        minMaxCurveY.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveY.constantMin = minVelocity.y;
        minMaxCurveY.constantMax = maxVelocity.y;
        velocityOverLifetimeModule.y = minMaxCurveY;

        ParticleSystem.MinMaxCurve minMaxCurveZ = new ParticleSystem.MinMaxCurve();
        minMaxCurveZ.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveZ.constantMin = minVelocity.z;
        minMaxCurveZ.constantMax = maxVelocity.z;
        velocityOverLifetimeModule.z = minMaxCurveZ;

    }
}
