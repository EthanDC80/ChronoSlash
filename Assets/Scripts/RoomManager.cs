using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomPrefabs = new GameObject[5];

    private RoomData[] roomData;

    public GameObject roomJointPrefab;

    private GameObject currentRoom;
    private RoomData currentRoomData;

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


        currentRoom = Instantiate(roomPrefabs[0], new Vector3(0,0,0), Quaternion.Euler(0,0,0));
        currentRoomData = currentRoom.GetComponent<RoomData>();

        GenerateExits();

        // int roomIndex = Random.Range(1,roomData.Length);
        // float zPos = roomData[0].exit[0].transform.position.z + roomData[roomIndex].roomLength/2 + roomJointPrefab.transform.localScale.z;
        // nextRoom = Instantiate(roomPrefabs[roomIndex], new Vector3(0, 0, zPos), Quaternion.Euler(0,0,0));
        // nextIndex = roomIndex;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // GenerateRoom();
    }

    public void GenerateRoom(GameObject corridor)
    {
        int roomIndex = Random.Range(1,roomPrefabs.Length);
        currentRoom = Instantiate(roomPrefabs[roomIndex]);

        switch (corridor.transform.rotation.eulerAngles.y){
            case 0:
                currentRoom.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case 90:
                currentRoom.transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case 180:
                currentRoom.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 270:
                currentRoom.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
        }
        currentRoom.transform.position = corridor.transform.position + currentRoom.transform.forward*1.5f;
        // currentRoom.transform.position = corridor.transform.position;
        // currentRoom.transform.rotation = corridor.transform.rotation;



        // GameObject exit = roomData[nextIndex].exit[Random.Range(0, roomData[nextIndex].numberExits)];
        
        // if (exit.transform.localPosition.x > 0) 
        // {
        //     Instantiate(roomJointPrefab, exit.transform.position + new Vector3(1.5f, 0, 0), Quaternion.Euler(0, 90, 0));
        //     float xPos = exit.transform.position.x + roomData[roomIndex].roomWidth/2 + 3;
        //     nextRoom = Instantiate(roomPrefabs[roomIndex], new Vector3(xPos, 0, 0), Quaternion.Euler(0, 90, 0));
        // } else if (exit.transform.localPosition.x < 0) 
        // {
        //     Instantiate(roomJointPrefab, exit.transform.position - new Vector3(1.5f, 0, 0), Quaternion.Euler(0, 90, 0));
        //     float xPos = exit.transform.position.x - roomData[roomIndex].roomWidth/2 - 3;
        //     nextRoom = Instantiate(roomPrefabs[roomIndex], new Vector3(xPos, 0, 0), Quaternion.Euler(0, 90, 0));
        // } else 
        // // if (exit.transform.localPosition.z > 0) 
        // {
        //     Instantiate(roomJointPrefab, exit.transform.position + new Vector3(0, 0, 1.5f), Quaternion.Euler(0, 0, 0));
        //     float zPos = exit.transform.position.z + roomData[roomIndex].roomLength/2 + 3;
        //     nextRoom = Instantiate(roomPrefabs[roomIndex], new Vector3(0, 0, zPos), Quaternion.Euler(0, 0, 0));
        // } 

        // currentRoom = nextRoom;
        // currentIndex = nextIndex;
        // nextIndex = 0;
    }

    void GenerateExits()
    {
        foreach (GameObject exit in currentRoomData.exit) {
            GameObject instanceExit = Instantiate(roomJointPrefab);
            // Debug.Log(exit.name + " " + exit.transform.rotation.eulerAngles);

            switch (exit.transform.rotation.eulerAngles.y){
                case 0:
                    instanceExit.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 90:
                    instanceExit.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case 180:
                    instanceExit.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 270:
                    instanceExit.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
            }

            instanceExit.transform.position = exit.transform.position - instanceExit.transform.forward*1.5f + instanceExit.transform.right*1.5f;
        }
    }
}
