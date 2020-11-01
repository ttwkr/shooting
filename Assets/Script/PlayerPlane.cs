using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlane : MonoBehaviour
{
    public float speed;
    public Boolean isTouchTop;
    public Boolean isTouchBottom;
    public Boolean isTouchLeft;
    public Boolean isTouchRight;

    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float horizon = Input.GetAxisRaw("Horizontal"); // x축 방향
        if ((isTouchRight && horizon == 1) || (isTouchLeft && horizon == -1))
        {
            horizon = 0;
        }

        float vertical = Input.GetAxisRaw("Vertical"); // y축 방향
        if ((isTouchRight && vertical == 1) || (isTouchLeft && vertical == -1))
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
            anim.SetInteger("Input", (int)horizon);
        } 
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