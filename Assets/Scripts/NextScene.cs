
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour
{
    public string nextSceneName;
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with: " + other.name);
        if (other.tag == "Player")
        {
            Debug.Log("Change scene!");
            SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        }
    }
}