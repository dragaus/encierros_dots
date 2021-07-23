using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

public class SetUpManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject tilePrefab;
    public int runners = 20;
    public int initialTileAmount = 20;
    BlobAssetStore _blobAssetStore;

    // Start is called before the first frame update
    void Start()
    {
        //Get all settings and prefabs ready for initialization
        _blobAssetStore = new BlobAssetStore();
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
        var characterEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(characterPrefab, settings);
        var tileEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(tilePrefab, settings);

        //Spawn Initial amount of tiles to make user feel confortable
        var tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        float lastPositionZ = Mathf.FloorToInt(initialTileAmount / 2) * tileSize.z * -1;
        for (int i = 0; i < initialTileAmount; i++)
        {
            var entity = manager.Instantiate(tileEntity);
            var position = new float3(0, 0, lastPositionZ);
            manager.SetComponentData(entity, new Translation { Value = position });
            manager.SetComponentData(entity, new Rotation { Value = quaternion.identity });
            lastPositionZ += tileSize.z;
        }

        //Spawn Initial amount of characters
        var characterSize = characterPrefab.GetComponent<MeshRenderer>().bounds.size;
        for (int i = 0; i < runners; i++)
        {
            var entity = manager.Instantiate(characterEntity);
            var position = new float3(UnityEngine.Random.Range(-4f, 4f), 1, UnityEngine.Random.Range(characterSize.x, 20f));
            if (i == 0) position = new float3(0, 1, 0);
            manager.SetComponentData(entity, new Translation { Value = position });
            manager.SetComponentData(entity, new Rotation { Value = quaternion.identity });
            manager.SetComponentData(entity, new CharaterData { movementSpeed = 2, rotationSpeed = 2 });
        }
    }

    private void OnDestroy()
    {
        _blobAssetStore.Dispose();
    }
}
