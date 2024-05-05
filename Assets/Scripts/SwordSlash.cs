using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{

    private Animator _animator;

    private Vector3 _startPos, _endPos;
    private Vector3 _slashVector;
    private bool _isSlash, _createSlash;

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
        Debug.Log(Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            _isSlash = true;

            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            // PlayerController.sensitivity = _slowSensitivity;

            _startPos = new Vector3(Screen.width/2, Screen.height/2, 0);
            Cursor.lockState = CursorLockMode.Confined;
            PlayerController.cameraMoveable = false;
        }

        if (_isSlash) {

        }

        if (Input.GetMouseButtonUp(0)) {
            _isSlash = false;

            _endPos = Input.mousePosition;
            _slashVector = _endPos - _startPos;
            Debug.Log(_slashVector);

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
        float angle = Mathf.Atan(_slashVector.y / _slashVector.x) * Mathf.Rad2Deg;
        if (_slashVector.x < 0) angle = 180 + angle;
        if (_slashVector.x > 0 && _slashVector.y < 0) angle = 360 + angle;

        Debug.Log(angle);
        
        if ((0 < angle && angle < 45f) || (315 < angle && angle < 360))
            _animator.SetTrigger("RSlash");
        if (45 < angle && angle < 135)
            _animator.SetTrigger("USlash");
        if (135 < angle && angle < 225)
            _animator.SetTrigger("LSlash");
        if (225 < angle && angle < 315)
            _animator.SetTrigger("DSlash");
    }
}
