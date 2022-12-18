using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedRandomPicker
{
    [SerializeField] private WeightedObject[] _objects;

    public WeightedObject[] Objects { get => _objects; set => _objects = value; }

    public int PickIndex()
    {
        float random = Random.Range(0, _objects[_objects.Length - 1].SummedWeight);
        for (int i = 0; i < _objects.Length; i++)
        {
            if (random <= _objects[i].SummedWeight)
            {
                return i;
            }
        }
        return -1;
    }
    public Object PickObject()
    {
        float random = Random.Range(0, _objects[_objects.Length - 1].SummedWeight);
        for (int i = 0; i < _objects.Length; i++)
        {
            if (random <= _objects[i].SummedWeight)
            {
                return _objects[i].Object;
            }
        }
        return null;
    }
    public string PickName()
    {
        float random = Random.Range(0, _objects[_objects.Length - 1].SummedWeight);
        for (int i = 0; i < _objects.Length; i++)
        {
            if (random <= _objects[i].SummedWeight)
            {
                return _objects[i].Name;
            }
        }
        return null;
    }
}
