using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class EnemyController : MonoBehaviour {

    private new Transform transform;
    private new Rigidbody rigidbody;
    private Transform playerTransform;


    private float moveSpeed = 10;
    private Vector3 moveDirection;

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
    }

    private void Move()
    {
        transform.LookAt(playerTransform);
        rigidbody.AddForce(transform.forward * moveSpeed, ForceMode.Force);
    }
}
