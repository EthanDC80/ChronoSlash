using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomPrefabs = new GameObject[5];

    private RoomData[] roomData;

    public GameObject roomJointPrefab;


    // Start is called before the first frame update
    void Start()
    {
        roomData = new RoomData[roomPrefabs.Length];
        // roomData[0] = roomPrefabs[0].GetComponent<RoomData>();
        // foreach (GameObject room in smallRoom) {

        // }
        for (int i = 0; i < roomPrefabs.Length; i++) {
            roomData[i] = roomPrefabs[i].GetComponent<RoomData>();
        }
        // for (int i = 0; i < largeRoomPrefabs.Length; i++) {
        //     roomData[1+roomPrefabs.Length+i] = largeRoomPrefabs[i].GetComponent<RoomData>();
        // }


        Instantiate(roomPrefabs[0]);

        int roomIndex = Random.Range(1,roomData.Length);
        Instantiate(roomPrefabs[roomIndex], new Vector3(0, 0,roomData[0].exit1.transform.position.z + roomData[roomIndex].roomLength/2 + roomJointPrefab.transform.localScale.z), Quaternion.Euler(0,0,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
