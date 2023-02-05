using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    [SerializeField]
    private float forceAmount;
    void Start()
    {
        rigidbody2D.velocity = Vector3.down * forceAmount;
    }
}
