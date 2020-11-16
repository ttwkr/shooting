using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.IO;

public class GameManager : MonoBehaviour
{
    public string[] enemyObjects;
    public Transform[] spawnPoints;

    public float currSpawnDelay;
    public float nextSpawnDelay;
    public float rangeSpawn;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImages;
    public GameObject gameOverSet;
    public ObjectManager objectManager;


    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;
    
    private void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjects = new string[]
            {"enemyRed", "enemyOrange", "enemyYello", "enemyGreen", "enemyBlue", "enemyNavy", "enemyPurple"};
        ReadSpawnFile();
    }

    void ReadSpawnFile()
    {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;
        
        TextAsset textFile = Resources.Load("Stage 0") as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();

            if (line == null)
            {
                break;
            }
            
            // 텍스트 한줄씩 반환
            string[] splited_list = line.Split(',');
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(splited_list[0]);
            spawnData.type = splited_list[1];
            spawnData.point = int.Parse(splited_list[2]);
        
            spawnList.Add(spawnData);
        }
        stringReader.Close();

        nextSpawnDelay = spawnList[0].delay;
    }

    void Update()
    {
        currSpawnDelay += Time.deltaTime;
        if (currSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            currSpawnDelay = 0;
        }

        PlayerPlane playerLogic = player.GetComponent<PlayerPlane>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score); //세자리마다 끊어준다
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "enemyRed":
                enemyIndex = 0;
                break;
            case "enemyOrange":
                enemyIndex = 1;
                break;
            case "enemyYello":
                enemyIndex = 2;
                break;
            case "enemyGreen":
                enemyIndex = 3;
                break;
            case "enemyBlue":
                enemyIndex = 4;
                break;
            case "enemyNavy":
                enemyIndex = 5;
                break;
            case "enemyPurple":
                enemyIndex = 6;
                break;
        }

        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = objectManager.MakeObj(enemyObjects[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();

        // 발사체를 플레이어한테 주기위해 플레이어 변수를 선언
        // 인스턴스화되지 않은 오브젝트한테 플레이어변수를 주지 않는다
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if (enemyPoint == 6 || enemyPoint == 8) //오른쪽 스폰
        {
            enemy.transform.Rotate(Vector3.forward * 45); // 바라보는 방향으로 돌림
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }

        else if (enemyPoint == 5 || enemyPoint == 7) //왼쪽 스폰
        {
            enemy.transform.Rotate(Vector3.back * 45); // 바라보는 방향으로 돌림
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }
        
        // 리스폰인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateIcon(Image[] image, int icon)
    {
        for (int i = 0; i < 3; i++)
        {
            // 아이콘 비활성화
            image[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < icon; i++)
        {
            // 아이콘 활성화
            image[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer()
    {
        Invoke("SpawnPlayer", 2f);
    }

    void SpawnPlayer()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        PlayerPlane playerLogic = player.GetComponent<PlayerPlane>();
        playerLogic.isHit = false;
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}