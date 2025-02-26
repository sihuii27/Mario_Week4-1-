using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class World1 : MonoBehaviour
{
    void Start()
    {
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("Main-Menu", LoadSceneMode.Single);
    }
}
