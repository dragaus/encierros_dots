using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class EntityFollower : MonoBehaviour
{
    Entity entityToFollow;
    public void SetEntityToFollow(Entity entity)
    {
        entityToFollow = entity;
    }

    // Update is called once per frame
    void Update()
    {
        if (entityToFollow != Entity.Null)
        {
            try
            {
                transform.position = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(entityToFollow).Value;
            }
            catch 
            {
                entityToFollow = Entity.Null;
            }
        }
    }
}
