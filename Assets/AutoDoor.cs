using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [SerializeField] Transform doorLeft, doorRight;
    private bool moveDoor, closeDoor;
    private float current, target;
    private float startPos = 0f;
    private float endPos = 1.5f;

    [SerializeField] AnimationCurve curve;
    private float doorAnimSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDoor) {
            current = Mathf.MoveTowards(current, target, doorAnimSpeed * Time.deltaTime);
            doorRight.localPosition = new Vector3(Mathf.Lerp(startPos, endPos, curve.Evaluate(current)), doorRight.localPosition.y, doorRight.localPosition.z);
            doorLeft.localPosition = new Vector3(-Mathf.Lerp(startPos, endPos, curve.Evaluate(current)), doorRight.localPosition.y, doorRight.localPosition.z);
            if (current == target) 
                moveDoor = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            moveDoor = true;
            current = 0f;
            target = 1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            moveDoor = true;
            current = 1f;
            target = 0f;

        }
    }
}
