using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPhysics : MonoBehaviour
{
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rigidbody.velocity.magnitude);
    }

    // private void OnCollisionEnter(Collision other) {
    //     if (isAttacking && other.gameObject.tag == "Enemy") {
    //         other.rigidbody.AddForceAtPosition();
    //         thingToPush.position+Vector3.up*0.5f
    //     }
    // }
}
