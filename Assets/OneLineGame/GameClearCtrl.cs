using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearCtrl : MonoBehaviour
{
    public void NextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(index + 1);
    }
}
