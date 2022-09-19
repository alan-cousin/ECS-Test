using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System.ComponentModel;
using Unity.Mathematics;
using Unity.Transforms;
using static UnityEditor.PlayerSettings;

public struct TeamInfo : IComponentData 
{
    public int numerOfBots;
}

[AddComponentMenu("TeamEntity")]
public class TeamSpawnEntity : MonoBehaviour
{
    public int numberOfBots;
    public GameObject Prefab;
    public float size;
    public Vector3 posOffset;
    public string TeamName;

    Terrain ter;

    void Start()
    {
        ter = FindObjectOfType<Terrain>();

        // Start is called before the first frame update
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        for(int i = 0; i < numberOfBots; i++) 
        {
            Quaternion rot = transform.rotation;
            rot = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            Vector2 randPos = UnityEngine.Random.insideUnitCircle * size;

            Vector3 pos = transform.position + new Vector3(randPos.x, 0f, randPos.y) + transform.rotation * posOffset;
            pos = TerrainVector(pos, ter);

            var instance = entityManager.Instantiate(prefab);

            // Place the instantiated entity in a grid with some noise
            //var position = transform.TransformPoint(new float3(x * 1.3F, noise.cnoise(new float2(x, y) * 0.21F) * 2, y * 1.3F));
            //entityManager.SetComponentData(instance, new Translation { Value = pos });
            //entityManager.SetComponentData(instance, new Rotation { Value = rot });
            entityManager.SetComponentData(instance, new PositionData { Value = pos });
            entityManager.SetComponentData(instance, new RotationData { Value = rot });
            entityManager.SetComponentData(instance, new TargetPositionData { Value = new float3(UnityEngine.Random.Range(700, 1100), pos.y, UnityEngine.Random.Range(700, 1100)) });
        }
    }

    Vector3 TerrainVector(Vector3 origin, Terrain ter1)
    {
        if (ter1 == null)
        {
            return origin;
        }

        Vector3 planeVect = new Vector3(origin.x, 0f, origin.z);
        float y = ter1.SampleHeight(planeVect);

        y = y + ter1.transform.position.y;

        Vector3 tv = new Vector3(origin.x, y, origin.z);
        return tv;
    }
}
