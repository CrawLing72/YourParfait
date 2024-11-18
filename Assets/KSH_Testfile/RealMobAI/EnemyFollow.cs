using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private enum State
    {
        Idle,
        MoveToPlayer,
        MoveToSpawner,
        Attack,
        Die
    }

    private State currentState;

    Animator animator;
    [SerializeField]
    public float speed;
    public float lineOfSite;
    public float attackRange;//여기서 총
    public float fireRate = 1f;//공격 delay라고 생각하자
    private float nextFireTime;
    public GameObject bullet;
    public GameObject bulletParent;
    [SerializeField] 
    int playerHealth = 3;

    Rigidbody2D myrigidbody2D;
    public Transform Spawner;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        myrigidbody2D = GetComponent<Rigidbody2D>();
        Spawner = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //lineOfSite = GetComponent<>
        //animator = GetComponent<Animator>();
        //animator.SetBool("Idle", true);
        currentState = State.Idle;  
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        float distanceFromSpawner = Vector2.Distance(Spawner.transform.position, transform.position);

        //if ((distanceFromPlayer < lineOfSite) && (distanceFromPlayer> attackRange))
        //{
       
        //animator.SetBool("Walk", true);
        //}
        //else if ((distanceFromPlayer <= attackRange) && (nextFireTime < Time.time))
        //{
        //     animator.SetBool("Attack", true);
        //}
        //else
        //{
        //   animator.SetBool("Idle", true);
        //}
        switch (currentState)
        {
            case State.Idle:
                if ((distanceFromPlayer < lineOfSite) && (distanceFromPlayer > attackRange))
                    ChangeState(State.MoveToPlayer);//따라가기
                else if (playerHealth <= 0)//죽기
                    ChangeState(State.Die);
                else if (distanceFromPlayer <= attackRange)//공격하기
                    ChangeState(State.Attack);
                UpdateIdle(distanceFromPlayer, distanceFromSpawner);//idle에서 공격하거나,죽거나,따라가거나  
                break;
            case State.MoveToPlayer:
                if (distanceFromSpawner > lineOfSite)
                    ChangeState(State.MoveToSpawner);//돌아가기
                else if (playerHealth <= 0)
                    ChangeState(State.Die);//죽기
                else if (distanceFromPlayer <= attackRange)
                    ChangeState(State.Attack);
                UpdateMoveToPlayer(distanceFromPlayer, distanceFromSpawner);//공격하거나,돌아가던가,죽거나
                break;
            case State.Attack://돌아가거나,따라가던가,죽거나
                if (distanceFromSpawner > lineOfSite)
                    ChangeState(State.MoveToSpawner);//돌아가기
                else if (playerHealth <= 0)
                    ChangeState(State.Die);//죽기
                else if (distanceFromPlayer > attackRange)
                    ChangeState(State.MoveToPlayer);
                UpdateAttack(distanceFromPlayer, distanceFromSpawner);
                break;
            case State.Die://끝
                UpdateDie(distanceFromPlayer, distanceFromSpawner);
                break;
            case State.MoveToSpawner:
                if (distanceFromSpawner < 1)
                    ChangeState(State.Idle);
                UpdateMoveToSpawner(distanceFromPlayer, distanceFromSpawner);
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }

        private void ChangeState(State newState)
    {
        currentState = newState;
    }

    private void UpdateIdle(float DFP, float DFS)
    {
        myrigidbody2D.velocity = new Vector2(0, 0);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Walk", false);
        anim.SetBool("Dead", false);
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", true);
        anim.Play("Idle");
    }
    private void UpdateMoveToPlayer(float DFP, float DFS)
    {
        if(DFP > attackRange)
        {
            Vector2 moveDir = (player.position - transform.position).normalized * speed;
            myrigidbody2D.velocity = new Vector2(moveDir.x, moveDir.y);
        }
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Walk", true);
        anim.SetBool("Idle", false);
        anim.SetBool("Dead", false);
        anim.SetBool("Attack", false);

        anim.Play("Walk");
    }
    private void UpdateAttack(float DFP, float DFS)
    {
        myrigidbody2D.velocity = new Vector2(0, 0);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Dead", false);
        anim.Play("Attack");
    }
    private void UpdateDie(float DFP, float DFS)
    {
        myrigidbody2D.velocity = new Vector2(0, 0);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Attack", false);
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Die", true);
        anim.Play("Die");
    }

    private void UpdateMoveToSpawner(float DFP, float DFS)
    {
      // if(DFS > 1){
            Vector2 moveDir = (Spawner.position - transform.position).normalized * speed;
            myrigidbody2D.velocity = new Vector2(moveDir.x, moveDir.y);
            Animator anim = GetComponent<Animator>();
            anim.SetBool("Attack", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Die", false);
            anim.SetBool("Walk", true);
            anim.Play("Walk");
      //  }
      ///  else
      //  {
      // //     ChangeState(State.Idle);
       // }
       

    }






    private void OnTriggerEnter2D(Collider2D other)
    {
        playerHealth--;
        if (playerHealth > 0)
        {
            animator.SetBool("Dead", false);
        }
        else
        {
            speed = 0f;
            animator.SetBool("Dead", true);
        }
    }

   



    //Animator animator;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    animator = GetComponent<Animator>();
    //}

    //// Update is called once per frame
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if(other.gameObject.CompareTag("Player"))
    //    {
    //        animator.SetTrigger("Attack");
    //    }
    //}
}
