using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public int health;
    public int healthBar = 10;
    // Start is called before the first frame update
    void Start() {
        health = healthBar;
    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) {
            Destroy(gameObject);
        }
        //Debug.Log(health);
    }

    public void TakeDamage(int damage) {
        health -= damage;
    }
}
