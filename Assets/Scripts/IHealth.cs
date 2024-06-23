using UnityEngine;

public interface IHealth
{
    public float Health { get; set; }

    public virtual void TakeDamage() { }
}
