using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCtrl : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _text;
    public int level;

    private void Start()
    {
        this.SetUp();
    }

    private void SetUp()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        Transform source = transform.parent;
        PlayerPrefs.SetInt("Level", index);
        source.Find("Home").gameObject.SetActive(false);
        source.Find("TraceLine").gameObject.SetActive(false);
        this._text.text = SceneManager.GetActiveScene().name;
    }

    public void NewGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Continue()
    {
        int index = PlayerPrefs.GetInt("Level");
        if (index < 1) index = 1;
        SceneManager.LoadSceneAsync(index);
    }
}
