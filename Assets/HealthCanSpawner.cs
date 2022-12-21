using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCanSpawner : MonoBehaviour
{
    [SerializeField] private Collectible[] _cans;
    private Breakable _vendingMachine;
    private void Awake()
    {
        _vendingMachine = GetComponent<Breakable>();
    }
    public void SpawnCan()
    {
        if (_vendingMachine.Health % 2 == 0)
        {
            Instantiate(_cans[Random.Range(0, _cans.Length)], transform.position, Quaternion.identity);
        }
    }
}
