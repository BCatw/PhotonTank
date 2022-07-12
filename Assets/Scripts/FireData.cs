using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireData
{
    public Transform fireTransform;
    public float fireForce;

    public FireData(Transform transform, float force)
    {
        fireTransform = transform;
        fireForce = force;
    }
}
