using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{

    private Animator _animator;

    private Vector3 _startPos, _endPos;
    private Vector3 _slashVector;
    public bool isAttacking;
    public int attackDirection = 0;
    

    private float _originalSensitivity, _slowSensitivity;


    void Awake()
    {
        _animator = GameObject.FindWithTag("Sword").GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _originalSensitivity = PlayerController.sensitivity;
        _slowSensitivity = PlayerController.sensitivity * 5f;
        // Debug.Log(Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {

            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            // PlayerController.sensitivity = _slowSensitivity;

            _startPos = new Vector3(Screen.width/2, Screen.height/2, 0);
            Cursor.lockState = CursorLockMode.Confined;
            PlayerController.cameraMoveable = false;
        }

        if (Input.GetMouseButtonUp(0)) {

            _endPos = Input.mousePosition;
            _slashVector = _endPos - _startPos;
            // Debug.Log(_slashVector);

            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
            // PlayerController.sensitivity = _originalSensitivity;

            Cursor.lockState = CursorLockMode.Locked;
            PlayerController.cameraMoveable = true;

            Slash();
        }
    }

    void Slash()
    {
        if (Mathf.Sqrt(Mathf.Pow(_slashVector.x, 2) + Mathf.Pow(_slashVector.y, 2)) < 5) {
            _animator.SetTrigger("LSlash");
            StartCoroutine(isSlash25());
            return;
        }
            
        float angle = Mathf.Atan(_slashVector.y / _slashVector.x) * Mathf.Rad2Deg;
        if (_slashVector.x < 0) angle = 180 + angle;
        if (_slashVector.x > 0 && _slashVector.y < 0) angle = 360 + angle;

        // Debug.Log(angle);
        
        if ((0 < angle && angle < 22.5f) || (337.5f < angle && angle < 360)) {
            _animator.SetTrigger("RSlash");
            attackDirection = 1;
        }
        if (22.5f < angle && angle < 67.5f) {
            _animator.SetTrigger("URSlash");
            attackDirection = 2;
        }
        if (67.5f < angle && angle < 112.5f) {
            _animator.SetTrigger("USlash");
            attackDirection = 0;
        }
        if (112.5f < angle && angle < 157.5f) {
            _animator.SetTrigger("ULSlash");
            attackDirection = -2;
        }
        if (157.5f < angle && angle < 202.5f) {
            _animator.SetTrigger("LSlash");
            attackDirection = -1;
        }
        if (202.5f < angle && angle < 247.5f) {
            _animator.SetTrigger("DLSlash");
            attackDirection = -1;
        }
        if (247.5f < angle && angle < 292.5f) {
            _animator.SetTrigger("DSlash");
            attackDirection = 0;
        }
        if (292.5f < angle && angle < 337.5f) {
            _animator.SetTrigger("DRSlash");
            attackDirection = 1;
        }
            

        StartCoroutine(isSlash25());
    }

    IEnumerator isSlash25()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }
}
