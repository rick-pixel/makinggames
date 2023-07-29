
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float patrolSpeed = 2.0f, chaseSpeed = 5.0f, detectionDistance = 10.0f, attackDistance = 1.5f, attackCooldown = 2.0f;
    public int attackDamage = 10;
    public Transform[] patrolPoints;
    public float pauseTime = 1.0f;

    private Transform player;
    private bool isChasing, isMoving = true;
    private float lastAttackTime, timeSinceLastMove;
    private int currentPatrolPointIndex;

    void Start() => transform.position = patrolPoints[currentPatrolPointIndex].position;

    void Update()
    {
        if (isChasing)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        if (isMoving)
        {
            transform.LookAt(patrolPoints[currentPatrolPointIndex]);
            transform.position += transform.forward * patrolSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position) < 0.1f)
            {
                isMoving = false;
                timeSinceLastMove = 0.0f;
            }
        }
        else
        {
            timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove > pauseTime)
            {
                isMoving = true;
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
            }
        }
    }

    void ChasePlayer()
    {
        float distanceToPlayer = player ? Vector3.Distance(transform.position, player.position) : 0;
        if (player && distanceToPlayer <= detectionDistance)
        {
            transform.LookAt(player);
            if (distanceToPlayer > attackDistance)
                transform.position += transform.forward * chaseSpeed * Time.deltaTime;
            if (distanceToPlayer < attackDistance && Time.time - lastAttackTime > attackCooldown)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
                lastAttackTime = Time.time;
            }
        }
        else isChasing = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            player = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            player = null;
        }
    }
}




