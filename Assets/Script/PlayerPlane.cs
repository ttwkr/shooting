using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlane : MonoBehaviour
{
    public float speed;
    public float currShotDelay;
    public float maxShotDelay;
    public float power;
    public float maxPower;
    public float maxBoom;
    public int boom;
    public int life;
    public int score;
    public Boolean isHit;
    public Boolean isBoomTime;
    public Boolean isRespawnTime;

    public Boolean isTouchTop;
    public Boolean isTouchBottom;
    public Boolean isTouchLeft;
    public Boolean isTouchRight;

    public Sprite[] sprites;
    public GameManager manager;
    public ObjectManager objectManager;
    public GameObject PlayerBulletA;
    public GameObject PlayerBulletB;
    public GameObject BoomEffect;
    public GameObject[] followers;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        FireBoom();
        Reload();
    }

    void Move()
    {
        float horizon = Input.GetAxisRaw("Horizontal"); // x축 방향
        if ((isTouchRight && horizon == 1) || (isTouchLeft && horizon == -1))
        {
            horizon = 0;
        }

        float vertical = Input.GetAxisRaw("Vertical"); // y축 방향
        if ((isTouchTop && vertical == 1) || (isTouchBottom && vertical == -1))
        {
            vertical = 0;
        }

        Vector3 currPosition = transform.position; // 현재위치
        Vector3 nextPosition = new Vector3(horizon, vertical, 0) * speed * Time.deltaTime; //다음 위치

        transform.position = currPosition + nextPosition;

        // 애니메이션
        if (Input.GetButtonDown("Horizontal") ||
            Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int) horizon);
        }
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
        {
            return;
        }

        if (currShotDelay < maxShotDelay)
        {
            return;
        }

        switch (power)
        {
            case 1:
                GameObject bullet = objectManager.MakeObj("playerBulletA");
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 2:
                GameObject bulletR = objectManager.MakeObj("playerBulletA");
                bulletR.transform.position = transform.position + Vector3.right * 0.2f;

                GameObject bulletL = objectManager.MakeObj("playerBulletA");
                bulletL.transform.position = transform.position + Vector3.left * 0.2f;

                Rigidbody2D rigid1 = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigid2 = bulletL.GetComponent<Rigidbody2D>();
                rigid1.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigid2.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            default:
                GameObject bulletRR = objectManager.MakeObj("playerBulletA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.4f;

                GameObject bulletCC = objectManager.MakeObj("playerBulletB");
                bulletCC.transform.position = transform.position;

                GameObject bulletLL = objectManager.MakeObj("playerBulletA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.4f;

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }


        currShotDelay = 0;
    }

    void FireBoom()
    {
        if (!Input.GetButton("Fire2"))
        {
            return;
        }

        if (isBoomTime)
        {
            return;
        }

        if (boom == 0)
        {
            return;
        }

        boom--;
        isBoomTime = true;
        manager.UpdateIcon(manager.boomImages, boom);
        //#1.폭탄 이펙트
        BoomEffect.SetActive(true);

        Invoke("RemoveBoomEffect", 4f);

        //#2.적 제거
        GameObject[] enemyRed = objectManager.GetPool("enemyRed");
        GameObject[] enemyOrange = objectManager.GetPool("enemyOrange");
        GameObject[] enemyYello = objectManager.GetPool("enemyYello");
        GameObject[] enemyGreen = objectManager.GetPool("enemyGreen");
        GameObject[] enemyBlue = objectManager.GetPool("enemyBlue");
        GameObject[] enemyNavy = objectManager.GetPool("enemyNavy");
        GameObject[] enemyPurple = objectManager.GetPool("enemyPurple");
        ObjectForSyntax(enemyRed);
        ObjectForSyntax(enemyOrange);
        ObjectForSyntax(enemyYello);
        ObjectForSyntax(enemyGreen);
        ObjectForSyntax(enemyBlue);
        ObjectForSyntax(enemyNavy);
        ObjectForSyntax(enemyPurple);

        //#3. 총알 제거
        GameObject[] bulletA = objectManager.GetPool("enemyBulletA");
        GameObject[] bulletB = objectManager.GetPool("enemyBulletB");
        for (int i = 0; i < bulletA.Length; i++)
        {
            if (bulletA[i].activeSelf)
            {
                bulletA[i].SetActive(false);
            }
        }

        for (int i = 0; i < bulletB.Length; i++)
        {
            if (bulletB[i].activeSelf)
            {
                bulletB[i].SetActive(false);
            }
        }
    }

    void ObjectForSyntax(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            if (obj[i].activeSelf)
            {
                Enemy enemyLogic = obj[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
    }

    void Reload()
    {
        currShotDelay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 벽에 부딪혔을 때 트리거 작동
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
            }
        }

        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isHit)
            {
                return;
            }

            isHit = true;
            // 피격하면 오브젝트 비활성화
            life--;
            boom = 0;
            manager.UpdateIcon(manager.lifeImage, life);
            manager.UpdateIcon(manager.boomImages, boom);
            if (life == 0)
            {
                manager.GameOver();
            }
            else
            {
                manager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            // Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "Item")
        {
            Items item = collision.gameObject.GetComponent<Items>();

            // 아이템
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;

                case "Power":

                    if (power == maxPower)
                    {
                        score += 500;
                    }
                    else
                    {
                        power++;
                        AddFollower();
                    }

                    break;

                case "Boom":
                    if (boom == maxBoom)
                    {
                        score += 1000;
                    }
                    else
                    {
                        boom++;
                        manager.UpdateIcon(manager.boomImages, boom);
                    }

                    break;
            }

            collision.gameObject.SetActive(false);
        }
    }

    void AddFollower()
    {
        if (power == 4)
        {
            followers[0].SetActive(true);
        }
        else if (power == 5)
        {
            followers[1].SetActive(true);
        }
        else if (power == 6)
        {
            followers[2].SetActive(true);
        }
    }

    void RemoveBoomEffect()
    {
        BoomEffect.SetActive(false);
        isBoomTime = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 벽에 부딪혔을 때 트리거 해제
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
            }
        }
    }
}