using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int playerHealth = 3;
    [SerializeField] GameObject[] heart;

    Animator animator;
    Rigidbody2D myrigidbody2D;

    void Start()
    {
        animator = GetComponent<Animator>();
        myrigidbody2D = GetComponent<Rigidbody2D>();
        playerHealth = heart.Length;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        flipSprite();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        myrigidbody2D.velocity = new Vector2(moveInput * moveSpeed, myrigidbody2D.velocity.y);
        if (moveInput == 0)
        {
            animator.SetBool("Walk", false);
        }
        else 
        {
            animator.SetBool("Walk", true);
        }
    }

    void flipSprite()
    {
        bool sprite = Mathf.Abs(myrigidbody2D.velocity.x) > Mathf.Epsilon;
        if (sprite)
        {
            transform.localScale = new Vector2(Mathf.Sin(myrigidbody2D.velocity.x), 1f);
        }
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
            moveSpeed = 0f;
            animator.SetBool("Dead", true);
        }
        Destroy(heart[playerHealth].gameObject);
    }
}
