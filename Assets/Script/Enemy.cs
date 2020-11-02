using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public string enemySize;
    public int enemyScore;
    public float speed;
    public float healthy;
    public Sprite[] sprites;
    public GameObject player;
    
    public float currShotDelay;
    public float maxShotDelay;
    public GameObject EnemyBulletA;
    public GameObject EnemyBulletB;

    SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Fire();
        Reload();
    }

    void OnHit(int damage)
    {
        healthy -= damage;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (healthy <= 0)
        {
            PlayerPlane playerLogic = player.GetComponent<PlayerPlane>();
            playerLogic.score += enemyScore;
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
    
    void Fire()
    {
        if (currShotDelay < maxShotDelay)
        {
            return;
        }

        if (enemySize == "S")
        {
            GameObject enemyBullet = Instantiate(EnemyBulletA, transform.position, transform.rotation);
            Rigidbody2D rigid = enemyBullet.GetComponent<Rigidbody2D>();
            
            // 플레이어에게 쏴야한다
            // 목표물 방향 = 플레이어의 위치 - 적의 위치
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);
        }
        else if (enemySize == "L")
        {
            GameObject enemyBulletR = Instantiate(EnemyBulletB, transform.position + Vector3.right * 0.4f, transform.rotation);
            GameObject enemyBulletL = Instantiate(EnemyBulletB, transform.position + Vector3.left * 0.4f, transform.rotation);
            
            Rigidbody2D rigidR = enemyBulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = enemyBulletL.GetComponent<Rigidbody2D>();
            
            // 플레이어에게 쏴야한다
            // 목표물 방향 = 플레이어의 위치 - 적의 위치
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.4f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.4f);
            
            rigidR.AddForce(dirVecR.normalized * 10, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 10, ForceMode2D.Impulse);
        }

        currShotDelay = 0;
    }

    void Reload()
    {
        currShotDelay += Time.deltaTime;
    }
}
