using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
public struct InstanceCreteEvent
{
    public List<GameObject> Instances;
    public ObjectName name;

    public InstanceCreteEvent(List<GameObject> Instances, ObjectName name)
    {
        this.Instances = Instances;
        this.name = name;   
    }
}


