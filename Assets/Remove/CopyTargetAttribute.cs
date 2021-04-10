using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTargetAttribute : MonoBehaviour
{
    public enum Attributes { YZPosition, YZPositionAndAllRotation, PositionAndRotation};
    public Attributes attribute = Attributes.YZPosition;
    public Transform target;
    public Vector3 position;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(attribute == Attributes.YZPosition)
        {
            position = target.position;
            position.x = transform.position.x;
            transform.position = position;
        }
        if (attribute == Attributes.YZPositionAndAllRotation)
        {
            position = target.position;
            position.x = transform.position.x;
            transform.position = position;
            transform.rotation = target.rotation;
        }
        if(attribute == Attributes.PositionAndRotation)
        {
            transform.position = target.transform.position;
            transform.rotation = target.transform.rotation;
        }
    }
}
