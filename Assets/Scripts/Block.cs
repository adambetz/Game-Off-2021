using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public BlockData blockData;
    
    public abstract void Initialize(BlockData data);
    public abstract void SetReachable(bool reachable);
}
