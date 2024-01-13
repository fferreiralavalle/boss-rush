using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public Enemy enemy;


    public void Start()
    {
        enemy.gameObject.SetActive(true);
    }
}
