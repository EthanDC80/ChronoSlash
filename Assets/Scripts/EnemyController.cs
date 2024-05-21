using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class EnemyController : MonoBehaviour {

    private new Transform transform;
    private new Rigidbody rigidbody;
    private Transform playerTransform;


    private float moveSpeed = 5;
    private Vector3 moveDirection;

    private bool isAttacking;

    // Start is called before the first frame update
    void Start() 
    {
        transform = gameObject.GetComponent<Transform>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() 
    {
        Move();
        Attack();
    }

    private void Move()
    {
        if (!isAttacking) {
            transform.LookAt(playerTransform);
            Vector3 direction = new Vector3(playerTransform.position.x - transform.position.x, 0, playerTransform.position.z - transform.position.z);
                

            rigidbody.AddForce(direction * moveSpeed, ForceMode.Force);
            Debug.Log(rigidbody.velocity.magnitude);

            Vector3 flatVel = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rigidbody.velocity = new Vector3(limitedVel.x, rigidbody.velocity.y, limitedVel.z);
            }
        }
    }

    private void Attack()
    {
        Vector3 toPlayer = new Vector3(playerTransform.position.x - transform.position.x, 0, playerTransform.position.z - transform.position.z);
        if (toPlayer.magnitude < 2) {
            isAttacking = true;
        }

        if (isAttacking) {
            StartCoroutine(wait3sec());
        }
    }

    IEnumerator wait3sec()
    {
        yield return new WaitForSeconds(2);
        isAttacking = false;
    }
}
