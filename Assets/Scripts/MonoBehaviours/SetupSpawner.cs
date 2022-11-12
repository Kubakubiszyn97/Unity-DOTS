using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SetupSpawner : MonoBehaviour
{
    [SerializeField] GameObject personPrefab;
    [SerializeField] int gridSize;
    [SerializeField] int spread;
    [SerializeField] Vector2 speedRange;
    [SerializeField] Vector2 lifeTimeRange = new Vector2(10, 60);

    private BlobAssetStore blob;

    private void Start()
    {
        blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(personPrefab, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        for(int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                var instance = entityManager.Instantiate(entity);

                float3 position = new float3(x * spread, 0, z * spread);
                entityManager.SetComponentData(instance, new Translation { Value = position });
                entityManager.SetComponentData(instance, new Destination { Value = position });
                float speed = UnityEngine.Random.Range(speedRange.x, speedRange.y);
                entityManager.SetComponentData(instance, new MovementSpeed { Value = speed });
                float lifeTime = UnityEngine.Random.Range(lifeTimeRange.x, lifeTimeRange.y);
                entityManager.SetComponentData(instance, new Lifetime { Value = lifeTime });
            }
        }
    }

    private void OnDestroy()
    {
        blob.Dispose();
    }
}
