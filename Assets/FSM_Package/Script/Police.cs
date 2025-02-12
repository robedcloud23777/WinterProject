using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : MonoBehaviour
{
    internal StateMachine<Police> stateMachine;

    [SerializeField] internal float speed;
    [SerializeField] internal Player plr;
    internal Animator animator;
    internal SpriteRenderer spriteRenderer;
    

    public void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stateMachine = new StateMachine<Police>();
        stateMachine.Setup(this);
        stateMachine.AddState("Idle", new PoliceIdle(), StateType.Default);
        stateMachine.AddState("Chase", new PoliceChase(), StateType.None);
        stateMachine.AddState("Wander", new PoliceWander(), StateType.None);
    }

    internal float tps = 60;
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

internal class PoliceIdle : State<Police>
{
    float curTime = 0;
    public override void Enter(Police origin)
    {
        origin.animator.SetBool("IsWalking", false);
        curTime = 0;
    }
    public override void Tick(Police origin)
    {
        if (Vector2.Distance(origin.plr.transform.position, origin.transform.position) < 20)
        {
            origin.stateMachine.ChangeState("Chase");
            return;
        }

        curTime += 1f / origin.tps;
        if(curTime > 5)
        {
            origin.stateMachine.ChangeState("Wander");
            return;
        }
    }
    public override void Execute(Police origin) { }
    public override void Exit(Police origin) { }
}
internal class PoliceChase : State<Police>
{
    public override void Enter(Police origin)
    {
        origin.animator.SetBool("IsWalking", true);
        origin.speed *= 1.5f;
    }

    public override void Exit(Police origin) 
        => origin.speed /= 1.5f;

    Vector3 dir;
    public override void Execute(Police origin)
    {
        dir = (origin.plr.transform.position - origin.transform.position).normalized
            * origin.speed * Time.deltaTime;

        origin.transform.position += dir;

        if (dir.x > 0) origin.spriteRenderer.flipX = false;
        else if (dir.x < 0) origin.spriteRenderer.flipX = true;

        if (Vector2.Distance(origin.plr.transform.position, origin.transform.position) >= 35)
        {
            origin.stateMachine.ChangeState("Idle");
            return;
        }

        if (Vector2.Distance(origin.plr.transform.position, origin.transform.position) < 5)
        {
            Debug.Log("GameOver");
            Time.timeScale = 0f;
            return;
        }
    }

    public override void Tick(Police origin) { }
}

internal class PoliceWander : State<Police>
{
    Vector3 target, dir;
    public override void Enter(Police origin)
    {
        origin.animator.SetBool("IsWalking", true);
        origin.speed *= 0.7f;

        target = origin.transform.position + new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f));
    }

    public override void Exit(Police origin)
        => origin.speed /= 0.7f;


    public override void Execute(Police origin)
    {
        dir = (target - origin.transform.position).normalized
            * origin.speed * Time.deltaTime;
        origin.transform.position += dir;

        if (dir.x > 0) origin.spriteRenderer.flipX = false;
        else if (dir.x < 0) origin.spriteRenderer.flipX = true;

        if (Vector2.Distance(target, origin.transform.position) < 5f)
        {
            origin.stateMachine.ChangeState("Idle");
            return;
        }
        if(Vector2.Distance(origin.plr.transform.position, origin.transform.position) < 20)
        {
            origin.stateMachine.ChangeState("Chase");
            return;
        }
    }

    public override void Tick(Police origin) { }
}

