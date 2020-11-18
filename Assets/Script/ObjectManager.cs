using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyRedPrefab;
    public GameObject enemyOrangePrefab;
    public GameObject enemyYelloPrefab;
    public GameObject enemyBluePrefab;
    public GameObject enemyGreenPrefab;
    public GameObject enemyNavyPrefab;
    public GameObject enemyPurplePrefab;
    public GameObject enemyBulletAPrefab;
    public GameObject enemyBulletBPrefab;
    public GameObject bossBulletAPrefab;
    public GameObject bossBulletBPrefab;
    public GameObject playerBulletAPrefab;
    public GameObject playerBulletBPrefab;
    public GameObject followerBulletPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    public GameObject itemCoinPrefab;


    private GameObject[] enemyRed;
    private GameObject[] enemyOrange;
    private GameObject[] enemyYello;
    private GameObject[] enemyGreen;
    private GameObject[] enemyBlue;
    private GameObject[] enemyNavy;
    private GameObject[] enemyPurple;

    private GameObject[] enemyBulletA;
    private GameObject[] enemyBulletB;
    private GameObject[] bossBulletA;
    private GameObject[] bossBulletB;
    private GameObject[] playerBulletA;
    private GameObject[] playerBulletB;
    private GameObject[] followerBullet;

    private GameObject[] itemPower;
    private GameObject[] itemBoom;
    private GameObject[] itemCoin;

    private GameObject[] targetPool;

    void Awake()
    {
        //한번에 등장할 수를 고려하여 배열길이 할당
        enemyRed = new GameObject[20];
        enemyOrange = new GameObject[20];
        enemyYello = new GameObject[20];
        enemyBlue = new GameObject[20];
        enemyGreen = new GameObject[20];
        enemyNavy = new GameObject[20];
        enemyPurple = new GameObject[20];

        enemyBulletA = new GameObject[200];
        enemyBulletB = new GameObject[200];
        bossBulletA = new GameObject[200];
        bossBulletB = new GameObject[200];
        playerBulletA = new GameObject[200];
        playerBulletB = new GameObject[200];
        followerBullet = new GameObject[200];

        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];
        itemCoin = new GameObject[20];

        Generate();
    }

    void Generate()
    {
        GenerateObject(enemyRed, enemyRedPrefab);
        GenerateObject(enemyBlue, enemyBluePrefab);
        GenerateObject(enemyOrange, enemyOrangePrefab);
        GenerateObject(enemyYello, enemyYelloPrefab);
        GenerateObject(enemyGreen, enemyGreenPrefab);
        GenerateObject(enemyNavy, enemyNavyPrefab);
        GenerateObject(enemyPurple, enemyPurplePrefab);

        GenerateObject(enemyBulletA, enemyBulletAPrefab);
        GenerateObject(enemyBulletB, enemyBulletBPrefab);
        GenerateObject(bossBulletA, bossBulletAPrefab);
        GenerateObject(bossBulletB, bossBulletBPrefab);
        GenerateObject(playerBulletA, playerBulletAPrefab);
        GenerateObject(playerBulletB, playerBulletBPrefab);
        GenerateObject(followerBullet, followerBulletPrefab);

        GenerateObject(itemPower, itemPowerPrefab);
        GenerateObject(itemBoom, itemBoomPrefab);
        GenerateObject(itemCoin, itemCoinPrefab);
    }


    void GenerateObject(GameObject[] gameObj, GameObject preFab)
    {
        //Instantiate로 생성한 인스턴스를 배열에 저장
        for (int i = 0; i < gameObj.Length; i++)
        {
            gameObj[i] = Instantiate(preFab);
            //Instantiate로 생성한 후에 바로 비활성화
            gameObj[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        targetPool = GetPool(type);
        
        for (int i = 0; i < targetPool.Length; i++)
        {
            // 활성화된 obj냐?
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            // 적
            case "enemyRed":
                targetPool = enemyRed;
                break;
            case "enemyOrange":
                targetPool = enemyOrange;
                break;
            case "enemyYello":
                targetPool = enemyYello;
                break;
            case "enemyGreen":
                targetPool = enemyGreen;
                break;
            case "enemyBlue":
                targetPool = enemyBlue;
                break;
            case "enemyNavy":
                targetPool = enemyNavy;
                break;
            case "enemyPurple":
                targetPool = enemyPurple;
                break;
            
            // 플레이어 총알
            case "playerBulletA":
                targetPool = playerBulletA;
                break;
            case "playerBulletB":
                targetPool = playerBulletB;
                break;
            case "followerBullet":
                targetPool = followerBullet;
                break;
            
            // 적 총알
            case "enemyBulletA":
                targetPool = enemyBulletA;
                break;
            case "enemyBulletB":
                targetPool = enemyBulletB;
                break;
            case "bossBulletA":
                targetPool = bossBulletA;
                break;
            case "bossBulletB":
                targetPool = bossBulletB;
                break;
            
            // 아이템
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
        }

        return targetPool;
    }
}