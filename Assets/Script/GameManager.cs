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
    public float maxSpawnDelay;
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
    }

    void Update()
    {
        currSpawnDelay += Time.deltaTime;
        if (currSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, rangeSpawn);
            currSpawnDelay = 0;
        }

        PlayerPlane playerLogic = player.GetComponent<PlayerPlane>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score); //세자리마다 끊어준다
    }

    void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyObjects.Length);
        int randomPoint = Random.Range(0, 8);

        GameObject enemy = objectManager.MakeObj(enemyObjects[randomEnemyIndex]);
        enemy.transform.position = spawnPoints[randomPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();

        // 발사체를 플레이어한테 주기위해 플레이어 변수를 선언
        // 인스턴스화되지 않은 오브젝트한테 플레이어변수를 주지 않는다
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if (randomPoint == 6 || randomPoint == 8) //오른쪽 스폰
        {
            enemy.transform.Rotate(Vector3.forward * 45); // 바라보는 방향으로 돌림
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }

        else if (randomPoint == 5 || randomPoint == 7) //왼쪽 스폰
        {
            enemy.transform.Rotate(Vector3.back * 45); // 바라보는 방향으로 돌림
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }
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