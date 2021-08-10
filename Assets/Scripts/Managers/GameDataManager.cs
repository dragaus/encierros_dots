using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    [HideInInspector]
    public Vector3 tileSize;
    [HideInInspector]
    public float tileZPos;
    [HideInInspector]
    public float leftHousePos;
    [HideInInspector]
    public float rightHousePos;
    [HideInInspector]
    public List<Entity> houses = new List<Entity>();
    [HideInInspector]
    public float characterMostBehaind;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }


}
