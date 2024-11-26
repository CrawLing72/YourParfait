using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class MinionsAIBlue : NetworkBehaviour, IAttack
{

    private enum State
    {
        Idle,
        MoveToResource,
        MoveToSpawner
    }

    private State currentState;
    Animator animator;

    [SerializeField]
    public float speed;
    public float StopRange;
    public float WaitingTime;
    public float SearchRange;

    [Networked]
    public float HP { get; set; }

    Rigidbody2D myrigidbody2D;
    public Transform Spawner;
    public Transform Resource;
    float Timer;
    private SpriteRenderer minionSpriteRenderer;
    public Slider HPBar;



    // Start is called before the first frame update
    void Start()
    {
        myrigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        Timer = 0;
        minionSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //lineOfSite = GetComponent<>
        //animator = GetComponent<Animator>();
        //animator.SetBool("Idle", true);
        currentState = State.MoveToResource;
        HP = 100f;
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {

        if(HP < 0f)
        {
            NetworkManager.Instance.runner.Despawn(Object);
            GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
            gameState.BlueScore_Minions -= 1;
        }

        float distanceFromResource = Vector2.Distance(Resource.transform.position, transform.position);
        float distanceFromSpawner = Vector2.Distance(Spawner.transform.position, transform.position);

        switch (currentState)
        {
            case State.MoveToResource:
                if (distanceFromResource < SearchRange)//멈추고 2초동안 대기(자원캐기)
                    ChangeState(State.Idle);
                MoveToResource();
                break;
            case State.MoveToSpawner:   
                if(distanceFromSpawner < SearchRange) 
                    ChangeState(State.Idle);
                MoveToSpawner();
                break;
            case State.Idle:
                Timer += NetworkManager.Instance.runner.DeltaTime;
                if (WaitingTime < Timer)
                {
                    Timer = 0;
                    if(distanceFromResource > distanceFromSpawner)
                        ChangeState(State.MoveToResource);
                    else
                        ChangeState(State.MoveToSpawner);
                }
                Idle();
                break;


        }
    }

    public void FixedUpdate()
    {
        HPBar.value = HP / 100f;
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
    }

    private void MoveToResource()
    {
        //Vector3 dir = Resource.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        //Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, speed);
        //transform.rotation = rotation;
        Rpc_SetDirection(true);
        gameObject.transform.Translate((Resource.position - transform.position).normalized * speed * NetworkManager.Instance.runner.DeltaTime);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Run", true);
        anim.SetBool("Idle", false);
        

        anim.Play("Run");
    }
    private void MoveToSpawner()
    {
        //Vector3 dir = Spawner.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        //Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, speed);
        //transform.rotation = rotation;
        Rpc_SetDirection(false);
        gameObject.transform.Translate((Spawner.position - transform.position).normalized * speed * NetworkManager.Instance.runner.DeltaTime);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Run", true);
        anim.SetBool("Idle", false);

        anim.Play("Run");
    }
    private void Idle()
    {
        myrigidbody2D.velocity = new Vector2(0, 0);

        Animator anim = GetComponent<Animator>();
        anim.SetBool("Run", false);
        anim.SetBool("Idle", true);

        anim.Play("Idle");
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_Damage(float damage)
    {
        HP -= damage;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_SetDirection(bool _dir)
    {
        minionSpriteRenderer.flipX = _dir;
    }
}

  

