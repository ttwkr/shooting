using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Invoke("Disable",2f);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    public void StartExplosion(string type)
    {
        anim.SetTrigger("OnExplosion");
        if (type == "boss")
        {
            transform.localScale = Vector3.one * 3f;
        }
        else
        {
            transform.localScale = Vector3.one * 1f;
        }
    }
}
