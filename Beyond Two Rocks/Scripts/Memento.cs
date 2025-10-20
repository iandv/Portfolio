using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memento<TSnapshot> 
{   
    public List<TSnapshot> snapshots = new List<TSnapshot>();

    public void Record(TSnapshot snapshot) 
    {
        snapshots.Add(snapshot);
    }

    public TSnapshot Remember() 
    {
        var snapshot = snapshots[snapshots.Count - 1];      
        snapshots.RemoveAt(snapshots.Count - 1);
        return snapshot;
    }

    public bool CanRemember() 
    {
        return snapshots.Count > 0;
    }    
}