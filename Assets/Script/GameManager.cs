using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjects;
    public Transform[] spawnPoints;

    public float currSpawnDelay;
    public float maxSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public GameObject gameOverSet;

    void Update()
    {
        currSpawnDelay += Time.deltaTime;
        if (currSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            currSpawnDelay = 0;
        }

        PlayerPlane playerLogic = player.GetComponent<PlayerPlane>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score); //세자리마다 끊어준다
    }

    void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, 3);
        int randomPoint = Random.Range(0, 9);
        GameObject enemy =  Instantiate(
                                enemyObjects[randomEnemy], 
                                spawnPoints[randomPoint].position, 
                                spawnPoints[randomPoint].rotation
                            );

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        
        // 발사체를 플레이어한테 주기위해 플레이어 변수를 선언
        // 인스턴스화되지 않은 오브젝트한테 플레이어변수를 주지 않는다
        enemyLogic.player = player;

        if (randomPoint == 6 || randomPoint == 8) //오른쪽 스폰
        {
            enemy.transform.Rotate(Vector3.forward * 45); // 바라보는 방향으로 돌림
            rigid.velocity = new Vector2(enemyLogic.speed,-1);
        }
        
        else if (randomPoint == 7 || randomPoint == 9) //왼쪽 스폰
        {
            enemy.transform.Rotate(Vector3.back * 45); // 바라보는 방향으로 돌림
            rigid.velocity = new Vector2(enemyLogic.speed*(-1),-1);
        }
        else
        {
            rigid.velocity = new Vector2(0,enemyLogic.speed*(-1));
        }
    }

    public void UpdateLifeIcon(int life)
    {
        for (int i = 0; i < 3; i++)
        {
            // 아이콘 비활성화
            lifeImage[i].color = new Color(1, 1, 1, 0);
        }
        
        for (int i = 0; i < life; i++)
        {
            // 아이콘 활성화
            lifeImage[i].color = new Color(1, 1, 1, 1);
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