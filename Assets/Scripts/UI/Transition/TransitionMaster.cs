using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMaster : MonoBehaviour
{
    public static TransitionMaster Instance;

    public Transform transitionSpawn;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayTransition(UITransition transition)
    {
        Instantiate(transition, transitionSpawn);
    }

    public bool IsTransitioning { get { return transitionSpawn.childCount > 0; } }

}
