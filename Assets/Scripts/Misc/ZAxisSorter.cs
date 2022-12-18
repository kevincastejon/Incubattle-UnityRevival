using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisSorter : MonoBehaviour
{
    [SerializeField] private Transform _mainTransform = default;        // Référence au Transform du GameObject parent principal

    private Transform _transform;                                       // Référence au Transform de l'enfant (Sprite)

    public Transform MainTransform { get => _mainTransform; set => _mainTransform = value; }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        // On récupère la position du transform
        Vector3 pos = _transform.position;
        // On définit son Z comme étant égal au Y du mainTransform
        pos.z = _mainTransform.position.y;
        // On redéfinit la position avec le nouveau Z
        _transform.position = pos;
    }
}