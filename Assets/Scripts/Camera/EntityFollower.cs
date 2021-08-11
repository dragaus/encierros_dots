using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class EntityFollower : MonoBehaviour
{
    SetUpManager setUpManager;
    Entity entityToFollow;

    public void SetEntityToFollow(Entity entity)
    {
        entityToFollow = entity;
    }

    public void SetSetUpManager(SetUpManager manager)
    {
        setUpManager = manager;
    }

    // Update is called once per frame
    void Update()
    {
        if (entityToFollow != Entity.Null)
        {
            try
            {
                transform.position = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(entityToFollow).Value;
                GameDataManager.instance.characterMostBehind = transform.position.z;
                if (GameDataManager.instance.GetNextSpawnPosition() <= transform.position.z)
                {
                    GameDataManager.instance.UpdateSpawnPosition();
                    setUpManager.AddTile();
                }
            }
            catch 
            {
                entityToFollow = Entity.Null;
            }
        }
    }
}
