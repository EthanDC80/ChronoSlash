using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [Header("Room")] 
    //1 for small, 2 for big. Unique 2 number room ID. E.g. 201 for big room, number 01. 000 for spawn room.
    public string roomID;

    public int roomWidth; //x axis
    public int roomLength; //z axis
    public int roomHeight = 2; //y axis

    public int numberExits; //number of exits. excludes the entrance. minimum 1, maximum 3

    public GameObject entrance; //gameobject at entrance
    public GameObject[] exit = new GameObject[1]; //gameobject at exit, put as many as specified in numberExits
}
