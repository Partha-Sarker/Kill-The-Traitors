using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitTarget : MonoBehaviour
{
    public Transform orbitAround;
    public float testAngle = 0;
    public Vector3 initialTargetPosition;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        //initialTargetPosition = orbitAround.localPosition;
        //distance = Vector2.Distance(
        //    new Vector2(transform.position.y, transform.position.z), 
        //    new Vector2(initialTargetPosition.y, initialTargetPosition.z)
        //    );
        //Orbit(testAngle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Orbit(float angle) 
    {
        transform.localPosition = new Vector3(
            initialTargetPosition.x, 
            initialTargetPosition.y + distance * Mathf.Sin(angle), 
            initialTargetPosition.z + distance * Mathf.Cos(angle)
        );
    }
}
