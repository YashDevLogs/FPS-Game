using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public float Health { get; set; }

    public virtual void TakeDamage(float damageAmt)
    { 
        Health -= damageAmt;
    }
}
