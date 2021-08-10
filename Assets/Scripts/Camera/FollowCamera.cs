using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Follow Camera needs a target to follow please set the Traget");
            return;
        }

        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null) return;

        //var diference = target.position - transform.position;
        //Debug.Log(diference);
        var newPos = target.position + offset;

        transform.position = newPos;
    }
}
