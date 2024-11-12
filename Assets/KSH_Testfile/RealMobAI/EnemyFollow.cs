using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
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

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myrigidbody2D = GetComponent<Rigidbody2D>();
        //lineOfSite = GetComponent<>
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);

    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        //if ((distanceFromPlayer < lineOfSite) && (distanceFromPlayer> attackRange))
        //{
        Vector2 moveDir = (player.position - transform.position).normalized * speed;
        myrigidbody2D.velocity = new Vector2(moveDir.x, moveDir.y);
        animator.SetBool("Walk", true);
        //}
        //else if ((distanceFromPlayer <= attackRange) && (nextFireTime < Time.time))
        //{
        //     animator.SetBool("Attack", true);
        //}
        //else
        //{
        //   animator.SetBool("Idle", true);
        //}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, attackRange);

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
