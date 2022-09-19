using UnityEngine;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct PositionData : IComponentData 
{
    public float3 Value;
}
public struct RotationData : IComponentData 
{
    public Quaternion Value;   
}

public struct TargetPositionData : IComponentData
{
    public float3 Value;
}

public struct MoveSpeedData : IComponentData 
{
    public float Value;
}

public struct StateVariable : IComponentData
{
    public bool isMovable;
    public bool isReady;
    public bool isApproaching;
    public bool isAttacking;
    public bool isApproachable;
    public bool isHealing;
    public bool isImmune;
    public bool isDying;
    public bool isSinking;
    public int noAttackers;
    public int maxAttackers;
    public float prevR;
    public int failedR;
    public int critFailedR;
    public float health;
    public float maxHealth;
    public float selfHealFactor;
    public float strength;
    public float defence;
    public int deathCalls;
    public int maxDeathCalls;
    public int sinkCalls;
    public int maxSinkCalls;
    public bool changeMaterial;
    public int nation;
}


[AddComponentMenu("BotEntity")]
public class BotEntity : MonoBehaviour, IConvertGameObjectToEntity
{
    public float moveSpeed;

    public bool isMovable = true;

    public bool isReady = false;
    public bool isApproaching = false;
    public bool isAttacking = false;
    [HideInInspector] public bool isApproachable = true;
    public bool isHealing = false;
    public bool isImmune = false;
    public bool isDying = false;
    public bool isSinking = false;


    public int noAttackers = 0;
    public int maxAttackers = 3;

    [HideInInspector] public float prevR;
    [HideInInspector] public int failedR = 0;
    public int critFailedR = 100;

    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public float selfHealFactor = 10.0f;

    public float strength = 10.0f;
    public float defence = 10.0f;

    [HideInInspector] public int deathCalls = 0;
    public int maxDeathCalls = 5;

    [HideInInspector] public int sinkCalls = 0;
    public int maxSinkCalls = 5;

    [HideInInspector] public bool changeMaterial = true;

    public int nation = 1;


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponents(entity, new ComponentTypes(
          new ComponentType[]
          {
                typeof(PositionData),
                typeof(RotationData),
                typeof(TargetPositionData),
                typeof(MoveSpeedData),
                typeof(StateVariable)
          }));

        dstManager.SetComponentData(entity, new PositionData { Value = transform.position });
        dstManager.SetComponentData(entity, new RotationData { Value = transform.rotation });
        dstManager.SetComponentData(entity, new MoveSpeedData { Value = moveSpeed });
        dstManager.SetComponentData(entity, new StateVariable { 
            isMovable = isMovable,
            isReady = isReady,
            isApproaching = isApproaching,
            isAttacking = isAttacking,
            isApproachable = isApproachable,
            isHealing = isHealing,
            isImmune = isImmune,
            isDying = isDying,
            isSinking = isSinking,
            noAttackers = noAttackers,
            maxAttackers = maxAttackers,
            prevR = prevR,
            failedR = failedR,
            critFailedR = critFailedR,
            health = health,
            maxHealth = maxHealth,
            selfHealFactor = selfHealFactor,
            strength = strength,
            defence = defence,
            deathCalls = deathCalls,
            maxDeathCalls = maxDeathCalls,
            sinkCalls = sinkCalls,
            maxSinkCalls = maxSinkCalls,
            changeMaterial = changeMaterial,
            nation = nation
        });
    }
}

