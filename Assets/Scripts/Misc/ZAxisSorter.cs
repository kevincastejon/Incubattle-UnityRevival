using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ZAxisSorter : MonoBehaviour
{
    [SerializeField] private Transform _mainTransform = default;        // Référence au Transform du GameObject parent principal
    [SerializeField] private bool _forceFrontRef;

    public Transform MainTransform { get => _mainTransform; set => _mainTransform = value; }


    private void Update()
    {
        if (_mainTransform == null)
        {
            return;
        }
        // On récupère la position du transform
        Vector3 pos = transform.position;
        // On définit son Z comme étant égal au Y du mainTransform
        pos.z = _mainTransform.position.y - (_forceFrontRef ? 0.001f : 0f);
        // On redéfinit la position avec le nouveau Z
        transform.position = pos;
    }
}