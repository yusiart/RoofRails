using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusStave : MonoBehaviour
{
    private float _lenght;
    
    private void Start()
    {
        _lenght = transform.localScale.y;
    }

    public float GetLenght()
    {
        return _lenght;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
