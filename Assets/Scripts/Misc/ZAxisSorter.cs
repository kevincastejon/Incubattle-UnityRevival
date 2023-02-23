using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ZAxisSorter : MonoBehaviour
{
    [SerializeField] private Transform _mainTransform = default;        // R�f�rence au Transform du GameObject parent principal
    [SerializeField] private bool _forceFrontRef;

    public Transform MainTransform { get => _mainTransform; set => _mainTransform = value; }


    private void Update()
    {
        if (_mainTransform == null)
        {
            return;
        }
        // On r�cup�re la position du transform
        Vector3 pos = transform.position;
        // On d�finit son Z comme �tant �gal au Y du mainTransform
        pos.z = _mainTransform.position.y - (_forceFrontRef ? 0.001f : 0f);
        // On red�finit la position avec le nouveau Z
        transform.position = pos;
    }
}