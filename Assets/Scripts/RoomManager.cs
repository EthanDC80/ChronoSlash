using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomPrefabs = new GameObject[5];

    private RoomData[] roomData;

    public GameObject roomJointPrefab;

    private GameObject currentRoom;
    private RoomData currentRoomData;

    private GameObject prevExit, prevExit2;

    private PlayerController playerController;

    [SerializeField] private GameObject enemyPrefab;

    void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

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
        // prevRoom2 = prevRoom1;
        // prevRoom1 = currentRoom;
        // if (prevRoom2 != null) Destroy(prevRoom2);



        int roomIndex = Random.Range(1,roomPrefabs.Length);
        currentRoom = Instantiate(roomPrefabs[roomIndex]);
        currentRoomData = currentRoom.GetComponent<RoomData>();

        // switch (corridor.transform.parent.rotation.eulerAngles.y){
        //     case 0:
        //         currentRoom.transform.rotation = Quaternion.Euler(0, 90, 0);
        //         break;
        //     case 90:
        //         currentRoom.transform.rotation = Quaternion.Euler(0, 180, 0);
        //         break;
        //     case 180:
        //         currentRoom.transform.rotation = Quaternion.Euler(0, 270, 0);
        //         break;
        //     case 270:
        //         currentRoom.transform.rotation = Quaternion.Euler(0, 0, 0);
        //         break;
        // }

        switch (corridor.transform.parent.rotation.eulerAngles.y){
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

        // Debug.Log(corridor.transform.parent.name + " " + corridor.transform.parent.rotation.eulerAngles);
        // Debug.Log(currentRoom.name + " " + currentRoom.transform.rotation.eulerAngles);

        // currentRoom.transform.rotation = Quaternion.Euler(0, corridor.transform.rotation.eulerAngles.y, 0);

        currentRoom.transform.position = corridor.transform.position - corridor.transform.up*1.5f;

        if (currentRoomData.hasEnemy) {
            Instantiate(enemyPrefab, currentRoomData.enemySpawnLocation[Random.Range(0,currentRoomData.enemySpawnLocation.Length)].transform.position, Quaternion.Euler(0,180,0));
        }

        GenerateExits();

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
        
            // switch (Mathf.RoundToInt(exit.transform.rotation.eulerAngles.y)){
            //     case 0:
            //         instanceExit.transform.rotation = Quaternion.Euler(0, 180, 0);
            //         break;
            //     case 90:
            //         instanceExit.transform.rotation = Quaternion.Euler(0, 270, 0);
            //         break;
            //     case 180:
            //         instanceExit.transform.rotation = Quaternion.Euler(0, 0, 0);
            //         break;
            //     case 270:
            //         instanceExit.transform.rotation = Quaternion.Euler(0, 90, 0);
            //         break;
            // }

            switch (Mathf.RoundToInt(exit.transform.rotation.eulerAngles.y)){
                case 0:
                    instanceExit.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case 90:
                    instanceExit.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 180:
                    instanceExit.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case 270:
                    instanceExit.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }

            instanceExit.transform.position = exit.transform.position - exit.transform.right*1.5f - exit.transform.up*1.5f;
            instanceExit.transform.parent = currentRoom.transform;

            // Debug.Log(exit.name + " " + exit.transform.rotation.eulerAngles);
            // Debug.Log(instanceExit.name + " " + instanceExit.transform.rotation.eulerAngles);
        }
    }

    public void DespawnRoom(GameObject thisExit)
    {
        if (prevExit != null) {
            prevExit.GetComponentInChildren<AutoDoor>().locked = true;
            prevExit.transform.parent = null;
        }

        //*Lock Doors
        foreach(Transform exit in thisExit.transform.parent) {
            if(exit.CompareTag("CorrObj")) {
                Debug.Log("Found A Corridor");
                if (exit.gameObject != thisExit) {
                    Debug.Log("Locking not this");
                    exit.gameObject.GetComponentInChildren<AutoDoor>().locked = true;
                    // Debug.Log(exit.gameObject.GetComponentInChildren<AutoDoor>().locked);
                } else {
                    if (prevExit2 != null) Destroy(prevExit2);
                    if (prevExit != null) prevExit2 = prevExit;
                    prevExit = exit.gameObject;
                }
            }
        }

        //*Delete Rooms
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room")) {
            if (room != playerController.currentRoom && room != thisExit.transform.parent.gameObject)
                Destroy(room);
        }
        // foreach (GameObject exit in prevRoomExits) {
        //     Debug.Log("a");
            
        // }
    }
}
