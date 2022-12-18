using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisSorter : MonoBehaviour
{
    [SerializeField] private Transform _mainTransform = default;        // R�f�rence au Transform du GameObject parent principal

    private Transform _transform;                                       // R�f�rence au Transform de l'enfant (Sprite)

    public Transform MainTransform { get => _mainTransform; set => _mainTransform = value; }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        // On r�cup�re la position du transform
        Vector3 pos = _transform.position;
        // On d�finit son Z comme �tant �gal au Y du mainTransform
        pos.z = _mainTransform.position.y;
        // On red�finit la position avec le nouveau Z
        _transform.position = pos;
    }
}