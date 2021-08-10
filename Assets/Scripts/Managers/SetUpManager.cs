using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;

public class SetUpManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject tilePrefab;
    public int runners = 20;
    public int initialTileAmount = 20;
    public List<GameObject> houses;

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
        houses.ForEach((house) =>
        {
            var newHouse = GameObjectConversionUtility.ConvertGameObjectHierarchy(house, settings);
            GameDataManager.instance.houses.Add(newHouse);
        });

        //Spawn Initial amount of tiles to make user feel confortable
        GameDataManager.instance.tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        GameDataManager.instance.tileZPos = Mathf.FloorToInt(initialTileAmount / 2) * GameDataManager.instance.tileSize.z * -1;
        float initialPos = GameDataManager.instance.tileZPos - GameDataManager.instance.tileSize.z / 2;
        GameDataManager.instance.rightHousePos = initialPos;
        GameDataManager.instance.leftHousePos = initialPos;
        for (int i = 0; i < initialTileAmount; i++)
        {
            var entity = manager.Instantiate(tileEntity);
            var position = new float3(0, 0, GameDataManager.instance.tileZPos);
            manager.SetComponentData(entity, new Translation { Value = position });
            manager.SetComponentData(entity, new Rotation { Value = quaternion.identity });

            var maxHousePos = GameDataManager.instance.tileZPos + GameDataManager.instance.tileSize.z;
            while (GameDataManager.instance.leftHousePos < maxHousePos)
            {
                AddHouses(ref GameDataManager.instance.leftHousePos, manager, -1);
            }

            while (GameDataManager.instance.rightHousePos < maxHousePos)
            {
                AddHouses(ref GameDataManager.instance.rightHousePos, manager, 1);
            }

            GameDataManager.instance.tileZPos += GameDataManager.instance.tileSize.z;
        }

        //Spawn Initial amount of characters
        var characterSize = characterPrefab.GetComponent<MeshRenderer>().bounds.size;
        for (int i = 0; i < runners; i++)
        {
            var entity = manager.Instantiate(characterEntity);
            var position = new float3(UnityEngine.Random.Range(-4f, 4f), 1, UnityEngine.Random.Range(characterSize.x, 20f));
            var speed = UnityEngine.Random.Range(2f, 5f);
            if (i == 0)
            {
                position = new float3(0, 1, 0);
                speed = 5.2f;
            }

            manager.SetComponentData(entity, new Translation { Value = position });
            manager.SetComponentData(entity, new Rotation { Value = quaternion.identity });
            manager.SetComponentData(entity, new CharaterData { movementSpeed = speed, rotationSpeed = 2, isAlive = true });
            if (i == 0)
            {
                var follower = FindObjectOfType<EntityFollower>();
                if (follower != null)
                {
                    follower.SetEntityToFollow(entity);
                }
            }
        }
    }

    void AddHouses(ref float position, EntityManager manager, float direction)
    {
        var randomIndex = UnityEngine.Random.Range(0, GameDataManager.instance.houses.Count);
        var entityToAdd = GameDataManager.instance.houses[randomIndex];
        var entity = manager.Instantiate(entityToAdd);
        var entitySize = houses[randomIndex].GetComponent<MeshRenderer>().bounds.size;
        var newPos = new float3(direction * ((GameDataManager.instance.tileSize.x / 2) + (entitySize.x / 2)), entitySize.y / 2, position + entitySize.z / 2);
        manager.SetComponentData(entity, new Translation { Value = newPos });
        position += entitySize.z;
    }

    private void OnDestroy()
    {
        _blobAssetStore.Dispose();
    }
}
