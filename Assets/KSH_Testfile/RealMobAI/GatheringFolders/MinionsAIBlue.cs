using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MinionsAIBlue : MonoBehaviour
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

    Rigidbody2D myrigidbody2D;
    public Transform Spawner;
    public Transform Resource;
    float Timer;
    private SpriteRenderer minionSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myrigidbody2D = GetComponent<Rigidbody2D>();
        Spawner = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>();
        Resource = GameObject.FindGameObjectWithTag("Resource").GetComponent<Transform>();
        Timer = 0;
        minionSpriteRenderer = GetComponent<SpriteRenderer>();
        //lineOfSite = GetComponent<>
        //animator = GetComponent<Animator>();
        //animator.SetBool("Idle", true);
        currentState = State.MoveToResource;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromResource = Vector2.Distance(Resource.transform.position, transform.position);
        float distanceFromSpawner = Vector2.Distance(Spawner.transform.position, transform.position);

        switch (currentState)
        {
            case State.MoveToResource:
                if (distanceFromResource < 3)//멈추고 2초동안 대기(자원캐기)
                    ChangeState(State.Idle);
                MoveToResource();
                break;
            case State.MoveToSpawner:   
                if(distanceFromSpawner < 3) 
                    ChangeState(State.Idle);
                MoveToSpawner();
                break;
            case State.Idle:
                Timer += Time.deltaTime;
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
        minionSpriteRenderer.flipX = true;
        Vector2 moveDir = (Resource.position - transform.position).normalized * speed;
        myrigidbody2D.velocity = new Vector2(moveDir.x, moveDir.y);
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
        minionSpriteRenderer.flipX = false;
        Vector2 moveDir = (Spawner.position - transform.position).normalized * speed;
        myrigidbody2D.velocity = new Vector2(moveDir.x, moveDir.y);
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
}

  

