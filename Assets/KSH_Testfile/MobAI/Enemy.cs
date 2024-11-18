using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnaAttack;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float chaseDistanceThreshold = 3, attackDistanceThreshold = 0.8f;

    [SerializeField]
    private float attackDelay = 1;
    private float passedTime = 1;

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(player.position, transform.position);
        if(distance < chaseDistanceThreshold)
        {
            OnPointerInput?.Invoke(player.position);
            if(distance <= attackDistanceThreshold)
            {
                //attack behavior
                OnMovementInput?.Invoke(Vector2.zero);
                if(passedTime >= attackDelay)
                {
                    passedTime = 0;
                    OnaAttack?.Invoke();
                }
            }
            else
            {
                //chasing the player
                Vector2 direction = player.position - transform.position;  
                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        if(passedTime < attackDelay)
        {
            passedTime = Time.deltaTime;
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
