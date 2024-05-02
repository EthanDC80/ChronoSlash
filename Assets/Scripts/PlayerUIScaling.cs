using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIScaling : MonoBehaviour {

    public GameObject parent;
    void Update() {
        if (parent.GetComponent<PlayerController>().crouched)
            transform.localScale = new Vector3(transform.localScale.x, 2, transform.localScale.z);
        else
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
    }
}
