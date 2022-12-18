using UnityEngine;

[System.Serializable]
public class WeightedObject
{
    [SerializeField] private string _name;
    [Min(0f)][SerializeField] private float _weight = 1f;
    [SerializeField] private Object _object;
    [SerializeField] private float _summedWeight;

    public string Name { get => _name; set => _name = value; }
    public float Weight { get => _weight; set => _weight = value; }
    public Object Object { get => _object; set => _object = value; }
    public float SummedWeight { get => _summedWeight; set => _summedWeight = value; }
}
