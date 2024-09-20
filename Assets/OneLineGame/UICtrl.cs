using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    [SerializeField] protected GameObject gameClear;

    public void GameClearUI()
    {
        this.gameClear.transform.Find("View").gameObject.SetActive(true);
    }
}
