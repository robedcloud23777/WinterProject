using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    internal StateMachine<Player> stateMachine;

    [SerializeField] internal int speed;
    internal Animator animator;
    internal SpriteRenderer spriteRenderer;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stateMachine = new StateMachine<Player>();
        stateMachine.Setup(this);
        stateMachine.AddState("Idle", new PlayerIdle(), StateType.Default);
        stateMachine.AddState("Walk", new PlayerWalk(), StateType.None);
    }

    const float tps = 60;
    float curTime = 0;
    public void Update()
    {
        stateMachine.Execute(); 

        curTime += Time.deltaTime;
        if(curTime > 1f / tps)
        {
            curTime = 0;
            stateMachine.Tick();
        }
    }
}

internal class PlayerIdle : State<Player>
{
    public override void Enter(Player origin)
        => origin.animator.SetBool("IsWalking", false);
    
    public override void Exit(Player origin) { }
    public override void Tick(Player origin) { }
    public override void Execute(Player origin)
    {
        if (Input.GetAxisRaw("Horizontal") != 0
            || Input.GetAxisRaw("Vertical") != 0)
            origin.stateMachine.ChangeState("Walk");
    }
}


internal class PlayerWalk : State<Player>
{
    public override void Enter(Player origin) 
        => origin.animator.SetBool("IsWalking", true);

    float x, y;
    public override void Execute(Player origin)
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if(x ==0 && y == 0)
        {
            origin.stateMachine.ChangeState("Idle");
            return;
        }

        if (x > 0) origin.spriteRenderer.flipX = false;
        else if (x < 0) origin.spriteRenderer.flipX = true;

        origin.transform.position
            += new Vector3(x, y).normalized * origin.speed * Time.deltaTime;
    }
    public override void Exit(Player origin) { }
    public override void Tick(Player origin) { }
}