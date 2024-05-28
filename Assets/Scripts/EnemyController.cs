using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private new Transform transform;
    private new Rigidbody rigidbody;
    private Animator animator;
    private Transform playerTransform;
    private SwordSlash swordSlash;
    private PlayerController playerController;
    private FOVControl FOV;

    private float moveSpeed = 5;
    private Vector3 moveDirection;

    public bool isAttacking;
    private bool isActive;

    private NavMeshAgent agent;
    Vector3 lastSeen;

    public bool stopped;
    public bool killed;

    public float timeSearch;
    public float timeElapsed;

    public bool lost;

    // Start is called before the first frame update
    void Start() 
    {
        transform = gameObject.GetComponent<Transform>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        swordSlash = GameObject.FindWithTag("Player").GetComponent<SwordSlash>();
        agent = GetComponent<NavMeshAgent>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        FOV = GetComponent<FOVControl>();
        
        isActive = true;
        stopped = false;
        killed = false;

        timeSearch = 5f;

        lastSeen = transform.position + new Vector3(Random.Range(-9f, 9f), Random.Range(0, 9f), Random.Range(-9f, 9f));
    }

    // Update is called once per frame
    void Update() 
    {
        if (!killed) {
            if (!stopped)
                Move();
            else if (!killed) {
                agent.speed = 0f;
                Vector3 rotation = Quaternion.LookRotation(playerTransform.position - transform.position).eulerAngles;
                rotation.x = 0f;
                rotation.z = 0f;
                transform.rotation = Quaternion.Euler(rotation);
            }
            Attack();
        }
    }

    private void Move()
    {
        Vector3 direction = new Vector3(transform.position.x - lastSeen.x, 0, transform.position.z - lastSeen.z);
        if (FOV.canSeePlayer) {
            lastSeen = playerTransform.position;
            lost = false;
        }
        else if (!lost && direction.magnitude < 0.1f) {
            lost = true;
		}
        else if (lost && (direction.magnitude < 0.1f || timeElapsed >= timeSearch)) {
            lastSeen = transform.position + new Vector3(Random.Range(-9f, 9f), Random.Range(0, 9f), Random.Range(-9f, 9f));
            timeElapsed = 0;
        }
        timeElapsed += Time.deltaTime;
        if (!isAttacking) {
            agent.speed = 10f;
            agent.destination = lastSeen;
        }
        
    }

    private void Attack()
    {
        if (!isActive) return;
        if (stopped && !isAttacking) {
            isAttacking = true;
            StartCoroutine(SlashWarmup());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (swordSlash.isAttacking && other.gameObject.CompareTag("SwordCollider")) {

            // Debug.Log("Attacked");
            isActive = false;
            //rigidbody.freezeRotation = false;
            stopped = true;
            killed = true;
            switch (swordSlash.attackDirection) {
                case 0:
                    //rigidbody.AddForceAtPosition(transform.forward * -100f + transform.up*-50f, transform.position+Vector3.up*30f, ForceMode.Impulse);
                    rigidbody.AddForce(transform.forward * -100f + transform.up * -50f, ForceMode.Impulse);
                    break;
                case -1:
                    //rigidbody.AddForceAtPosition(transform.right * 50f, transform.position+Vector3.up*30f, ForceMode.Impulse);
                    rigidbody.AddForce(transform.right * 50f, ForceMode.Impulse);
                    break;
                case -2:
                    //rigidbody.AddForceAtPosition(transform.right * 50f + transform.up*-30f, transform.position+Vector3.up*30f, ForceMode.Impulse);
                    rigidbody.AddForce(transform.right * 50f + transform.up * -30f, ForceMode.Impulse);
                    break;
                case 1:
                    //rigidbody.AddForceAtPosition(transform.right * -50f, transform.position+Vector3.up*30f, ForceMode.Impulse);
                    rigidbody.AddForce(transform.right * -50f, ForceMode.Impulse);
                    break;
                case 2:
                    //rigidbody.AddForceAtPosition(transform.right * -50f + transform.up*-30f, transform.position+Vector3.up*30f, ForceMode.Impulse);
                    rigidbody.AddForce(transform.right * -50f + transform.up * -30f, ForceMode.Impulse);
                    break;
            }
            playerController.enemyDestroyed = true;
            StartCoroutine(DespawnAfter3());
            // rigidbody.AddForceAtPosition(transform.right * 10f, transform.position+Vector3.up*3f, ForceMode.Impulse);
        }
    }

    // oid OnCollisionEnter(Collision other) {
    //     if (isAttacking && other.gameObject.tag == "Enemy") {
    //         other.rigidbody.AddForceAtPosition();
    //         thingToPush.position+Vector3.up*0.5f
    //     }
    // }

    IEnumerator SlashWarmup()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Slash");
        StartCoroutine(SlashCooldown());
    }

    IEnumerator SlashCooldown()
    {
        yield return new WaitForSeconds(3);
        isAttacking = false;
    }

    IEnumerator DespawnAfter3()
    {
        yield return new WaitForSeconds(3);
        playerController.enemyDestroyed = true;
        Destroy(gameObject);
    }
}
