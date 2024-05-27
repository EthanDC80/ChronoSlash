using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorMethod : MonoBehaviour
{
    RoomManager roomManager;

    [SerializeField] GameObject corridorObject;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = GameObject.FindWithTag("RoomManager").GetComponent<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("locking");
            roomManager.DespawnRoom(corridorObject);
            Destroy(gameObject);
        }
    }
}
