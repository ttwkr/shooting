using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    public string enemySize;
    public int enemyScore;
    public float speed;
    public float healthy;
    public int patternIndex;
    public int currPatternCount; // 현재 패턴 횟수
    public int[] maxPatternCount; // 각 패턴별 반복 횟수
    public int bulletCountA;
    public int bulletCountB;
    public float currShotDelay;
    public float maxShotDelay;
    public GameObject player;
    public ObjectManager objectManager;
    public GameManager gameManager;
    private Animator anim;
    public bool isDead;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        Invoke("Stop", 2);
    }

    void Stop()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        // isDead = false;
        Invoke("Think", 2);
    }

    void Think()
    {
        if (isDead)
        {
            return;
        }
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        currPatternCount = 0; // 현재 패턴 횟수 초기화
        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void PatternLogic(string methodName, float time)
    {
        // 패턴별 정해져 있는 횟수만큼 실행하고 다음 패턴으로 실행
        currPatternCount++;

        if (currPatternCount < maxPatternCount[patternIndex])
        {
            Invoke(methodName, time);
        }
        else
        {
            Invoke("Think", 2);
        }
    }

    void FireFoward()
    {
        Debug.Log("앞으로 발사");

        GameObject bulletRR = objectManager.MakeObj("bossBulletB");
        GameObject bulletR = objectManager.MakeObj("bossBulletB");
        GameObject bulletL = objectManager.MakeObj("bossBulletB");
        GameObject bulletLL = objectManager.MakeObj("bossBulletB");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        PatternLogic("FireFoward", 2f);
    }

    void FireShot()
    {
        Debug.Log("플레이어방향으로 샷건");
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = objectManager.MakeObj("bossBulletB");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 4, ForceMode2D.Impulse);
        }

        PatternLogic("FireShot", 0.5f);
    }

    void FireArc()
    {
        Debug.Log("부채모양으로 발사");
        GameObject bullet = objectManager.MakeObj("bossBulletA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 8 * currPatternCount / maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        PatternLogic("FireArc", 0.15f);
    }

    void FireAround()
    {
        int bulletCount = currPatternCount % 2 == 0 ? bulletCountA : bulletCountB; 
        Debug.Log("보스 중심으로 원 형태로 발사");
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = objectManager.MakeObj("bossBulletA");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount),
                                        Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
            
            Vector3 rotVec = Vector3.forward * 360 * i / bulletCount + Vector3.forward*90;
            bullet.transform.Rotate(rotVec);
        }

        PatternLogic("FireAround", 0.7f);
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
            gameManager.CallExplosion(transform.position, "boss");
            transform.rotation = Quaternion.identity;
            gameManager.EndStage();
            // isDead = true;
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

    void Reload()
    {
        currShotDelay += Time.deltaTime;
    }
}