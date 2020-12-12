using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [SerializeField] string _level;

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(_level);
    }
}
