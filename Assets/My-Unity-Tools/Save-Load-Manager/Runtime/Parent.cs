using System;
using UnityEngine;

public abstract class APooledBehaviour : MonoBehaviour, IPooledObject
{
    public virtual void OnGetFromPool() { }
    public virtual void OnReleaseToPool() { }
}

public interface IPooledObject
{
    void OnGetFromPool();
    void OnReleaseToPool();
}

