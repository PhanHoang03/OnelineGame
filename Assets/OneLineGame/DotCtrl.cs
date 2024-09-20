using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCtrl : MonoBehaviour
{
    [SerializeField] private int pass;
    public int Pass { get => pass; set => pass = value; }
}
