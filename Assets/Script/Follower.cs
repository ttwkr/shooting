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
    
    
    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch()
    {
        // 따라갈 위치를 계속 갱신
        followPos = parents.position;
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
