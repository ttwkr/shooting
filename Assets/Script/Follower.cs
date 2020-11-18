using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float currShotDelay;
    public float maxShotDelay;
    public ObjectManager objectManager;
    
    // 따라다니게 하기위한 변수
    public Vector3 followPos;
    public int followDelay;
    public Transform parents;
    public Queue<Vector3> parentsPos;


    private void Awake()
    {
        parentsPos = new Queue<Vector3>();
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch()
    {
        // 플레이어가 멈춰있을때 플레이어랑 겹치는거 방지
        // 큐에 플레이어의 위치가 없으면 큐에 넣는다
        if (!parentsPos.Contains(parents.position))
        {
            // 큐에 넣는다
            parentsPos.Enqueue(parents.position);    
        }
        
        // 따라갈 위치를 계속 갱신
        if (parentsPos.Count > followDelay)
        {
            // 큐에 일정 데이터가 채워지면 그때부터 반환
            followPos = parentsPos.Dequeue();
        }
        else if (parentsPos.Count < followDelay)
        {
            // 큐킈 수가 프레임(딜레이)보다 낮으면 플레이어의 위치
            // 어디까지나 임시방편
            // 처음 시작할때 위치를 초기화하기 위해
            followPos = parents.position;
        }

    }

    void Follow()
    {
        // 따라다니는 놈의 위치
        transform.position = followPos;
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
        
        GameObject bullet = objectManager.MakeObj("followerBullet");
        bullet.transform.position = transform.position;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
           


        currShotDelay = 0;
    }
    
    void Reload()
    {
        currShotDelay += Time.deltaTime;
    }
}
