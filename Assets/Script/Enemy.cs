using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{

    public string enemySize;
    public int enemyScore;
    public float speed;
    public float healthy;
    public Sprite[] sprites;
    public GameObject player;
    public ObjectManager objectManager;
    public string[] itemObject;

    public float currShotDelay;
    public float maxShotDelay;
    public GameObject EnemyBulletA;
    public GameObject EnemyBulletB;

    SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemObject = new string[] {"itemPower", "itemBoom", "itemCoin"};
    }

    private void OnEnable()
    {
        switch (enemySize)
        {
            case "L":
                healthy = 4;
                break;
            case "M":
                healthy = 3;
                break;
            case "S":
                healthy = 2;
                break;
        }
    }

    private void Update()
    {
        Fire();
        Reload();
    }

    public void OnHit(int damage)
    {
        if (healthy <= 0)
        {
            return;
        }
        healthy -= damage;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (healthy <= 0)
        {
            PlayerPlane playerLogic = player.GetComponent<PlayerPlane>();
            int randomItemIndex = Random.Range(0, 3);
            playerLogic.score += enemyScore;
            gameObject.SetActive(false);
            GameObject item = objectManager.MakeObj(itemObject[randomItemIndex]);
            item.transform.position = transform.position;
            transform.rotation = Quaternion.identity;
            
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
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            PlayerBullet playerBullet = collision.gameObject.GetComponent<PlayerBullet>();
            OnHit(playerBullet.damage);
            
            collision.gameObject.SetActive(false);
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
            GameObject enemyBullet = objectManager.MakeObj("enemyBulletA");
            enemyBullet.transform.position = transform.position;
            Rigidbody2D rigid = enemyBullet.GetComponent<Rigidbody2D>();
            
            // 플레이어에게 쏴야한다
            // 목표물 방향 = 플레이어의 위치 - 적의 위치
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);
        }
        else if (enemySize == "L")
        {
            GameObject enemyBulletR = objectManager.MakeObj("enemyBulletB");
            enemyBulletR.transform.position = transform.position + Vector3.right * 0.4f;
                
            GameObject enemyBulletL = objectManager.MakeObj("enemyBulletB");
            enemyBulletL.transform.position = transform.position + Vector3.left * 0.4f;

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
