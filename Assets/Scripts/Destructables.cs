using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour
{
    public List<Rigidbody> AllParts = new List<Rigidbody> ();

    public void Destruct()
    {
        foreach (Rigidbody part in  AllParts)
        {
            part.isKinematic = false;

        }
    }
}
