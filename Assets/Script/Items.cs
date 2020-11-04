using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public string type;
    public float itemSpeed;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * itemSpeed;
    }
}
