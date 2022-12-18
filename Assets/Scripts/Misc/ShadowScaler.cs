using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScaler : MonoBehaviour
{
    [SerializeField] private Transform _shadowOwner;
    [SerializeField] private float _maxHeight = 1.5f;

    private void Update()
    {
        transform.localScale = Vector3.one * Mathf.Clamp01(1f - (Vector2.Distance(new Vector2(0f, _shadowOwner.position.y), new Vector2(0f, transform.position.y)) / _maxHeight));
    }
}
