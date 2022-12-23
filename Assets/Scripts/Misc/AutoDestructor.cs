using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestructor : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
