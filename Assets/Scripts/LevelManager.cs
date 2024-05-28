using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionSpeed = 1f;

    public void LoadTitle()
    {
        StartCoroutine(LoadLevel("Title"));
    }

    public void LoadGame()
    {
        StartCoroutine(LoadLevel("Demo"));
    }

    IEnumerator LoadLevel(string levelName)
    {
        _transition.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(_transitionSpeed);

        SceneManager.LoadScene(levelName);
    }
}
