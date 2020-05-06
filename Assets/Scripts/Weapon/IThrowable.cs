using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IThrowable : MonoBehaviour
{
    public Collider col1, col2;

    public abstract void StartThrowingPreparation();
}
