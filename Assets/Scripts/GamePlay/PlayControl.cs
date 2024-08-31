using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现对于玩家角色运动的控制，主要判断鼠标输入、进行角色移动以及动画控制、移动完成之后改变事件中心（EventDeal）里角色的位置值，下一步交给事件中心处理。
/// </summary>

//在场景之外的时候要通知EventDeal要过场景了，然后EventDeal把这个脚本禁用。
public class PlayControl : MonoBehaviour
{

    public static PlayControl Instance { get;  set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 如果希望这个实例在场景切换时保持存在
        }
        else
        {
            Destroy(gameObject); // 如果已经有一个实例存在，销毁当前对象
        }
    }

    private bool isMoving = false;
    Vector3 target;
    public float moveSpeed;
    //这里用元组而不是List，因为该类中的playerState只会整体赋值。
    public (int, int) playerState;

    public float changeTime = 2f;
    public bool isChanging = false;

    public List<float> rate = new List<float>() { 0.4f, 0.55f, 0.7f, 0.85f, 1f };

    private Vector3 changedTarget;

    private Animator animator;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //如果处于移动状态
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, changedTarget, moveSpeed * Time.deltaTime);

            // 检测是否到达目标位置
            if (Vector3.Distance(transform.position, changedTarget) < 0.1f)
            {
                isMoving = false; // 停止移动
                //接下来开始调用EventDeal的方法进行当前格子属性变化的结算
                playerState = EventDeal.Instance.changeState(EventDeal.Instance.grid.WorldToCell(target));
                isChanging = true;
                animator.SetBool("isMove", false);
            }
        }

        //如果处于比例变换状态
        if (isChanging && playerState.Item2 - 1 >= 0 && playerState.Item2 - 1 < rate.Count && playerState.Item1 - 1 >= 0 && playerState.Item1 - 1 < rate.Count)
        {

            Vector3 scale = new Vector3(rate[playerState.Item2 - 1], rate[playerState.Item1 - 1], 1.0f);

            transform.localScale = Vector3.SmoothDamp(transform.localScale, scale, ref velocity, 0.1f);


            Vector3 scale1 = new Vector3(rate[PlayControl.Instance.playerState.Item2 - 1], rate[PlayControl.Instance.playerState.Item1 - 1], 1.0f);
            PlayPanel.Instance.imageCha.transform.localScale = scale1;
            Vector3 end = new Vector3(rate[EventDeal.Instance.end.Item2 - 1], rate[EventDeal.Instance.end.Item1 - 1], 1.0f);
            PlayPanel.Instance.imageImp.transform.localScale = end;
            if (Vector3.Distance(transform.localScale, scale) < 0.01f)
            {
                isChanging = false;

            }

        }

        //如果检测到了鼠标左键输入
        if (Input.GetMouseButtonDown(0))
        {
            // 将鼠标点击的屏幕坐标转换为世界坐标
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //拿世界坐标传给EventDeal，然后来看看能走还是不能走。能走就进行移动，并通知角色playperformance做行走动画
            (bool, int,Vector3) move = EventDeal.Instance.MoveDetermine(worldPos);

            if (move.Item1)
            {
                isMoving = true;
                //在这里根据Item2来控制动画
                if(move.Item2 == 0 || move.Item2 == 3)
                {
                    animator.SetBool("isMove", true);
                    animator.SetBool("isLeft", true);
                }
                if (move.Item2 == 1 || move.Item2 == 2)
                {
                    animator.SetBool("isMove", true);
                    animator.SetBool("isLeft", false);
                }

                //在这里根据Item3设置移动目标
                target = move.Item3;
                changedTarget = new Vector3(move.Item3.x, move.Item3.y + 2, move.Item3.z);

            }
            else//鼠标指针显示叉
            {
                //在这里控制动画
            }

        }







    }


}
