using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;

    public GameObject parent;
    PlayerController controller;
    void Start() {
        controller = parent.GetComponent<PlayerController>();
    }

    void Update() {
        if (start) {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking() {
        float elapsedTime = 0f;
        Vector3 startPosition = parent.transform.position + new Vector3(0, (controller.crouched == true ? 0f : 0.5f), 0);

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            startPosition = parent.transform.position + new Vector3(0, (controller.crouched == true ? 0f : 0.5f), 0);
            yield return null;
        }

        transform.position = startPosition;
    }

    public void setHeight() {
        transform.position = parent.transform.position + new Vector3(0, (controller.crouched == true ? 0f : 0.5f), 0);
    }
}
