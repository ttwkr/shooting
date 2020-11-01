using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float healthy;
    public Sprite[] sprites;
    
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        
        rigid.velocity = Vector2.down * speed;
    }

    void OnHit(int damage)
    {
        healthy -= damage;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (healthy <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            PlayerBullet playerBullet = collision.gameObject.GetComponent<PlayerBullet>();
            OnHit(playerBullet.damage);
            
            Destroy(collision.gameObject);
        }
    }
}
