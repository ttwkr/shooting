using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed;
    public Transform[] sprites;
    float viewHeight;
    public int startIndex;
    public int endIndex;


    void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }

    void Update()
    {
        Vector3 curPosition = transform.position;
        Vector3 nextPosition = Vector3.down * speed * Time.deltaTime;
        transform.position = curPosition + nextPosition;
        
        // 무한 스크롤
        if (sprites[endIndex].position.y < viewHeight*(-1))
        {
            // 뒤에있는 배경 포지션값
            Vector3 backSprite = sprites[startIndex].localPosition;
            
            // 앞에있는 배경 포지션값
            Vector3 frontSprite = sprites[endIndex].localPosition;

            sprites[endIndex].transform.localPosition = backSprite + Vector3.up * viewHeight;

            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave - 1 == -1) ? sprites.Length -1 : startIndexSave - 1;
        }
    }
}
