using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Boss : MonoBehaviour
{
    
    public string enemySize;
    public int enemyScore;
    public float speed;
    public float healthy;
    public GameObject player;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void OnBossHit(int damage)
    {
        healthy -= damage;
        anim.SetTrigger("OnHit");

        if (healthy <= 0)
        {
            PlayerPlane playerLogic = player.GetComponent<PlayerPlane>();
            playerLogic.score += enemyScore;
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
    }
    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            PlayerBullet playerBullet = collision.gameObject.GetComponent<PlayerBullet>();
            OnBossHit(playerBullet.damage);
            
            collision.gameObject.SetActive(false);
        }
    }
}
