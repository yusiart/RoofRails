using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] private GameObject _sparkTemplate;
    [SerializeField] private float _sparkSpeed;
    
    private GameObject _spark;
    
    private void OnCollisionEnter(Collision other)
    {
        _spark = Instantiate(_sparkTemplate, transform.position, Quaternion.identity);
    }

    private void OnCollisionExit(Collision other)
    {
        Destroy(_spark);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Player stave))
        {
            Vector3 target = new Vector3(_spark.transform.position.x, stave.transform.position.y, _spark.transform.position.z);
            _spark.transform.position = target;
        }
    }
}
