using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float TimeForDestruction;
    void Start()
    {
        StartCoroutine(DestroySelf(TimeForDestruction));
    }

    private IEnumerator DestroySelf(float timeForDestruction)
    {
      yield return new WaitForSeconds(timeForDestruction);
        Destroy(gameObject);
    }

}
