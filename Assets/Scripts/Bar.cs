using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] private GameObject _sparkTemplate;
    [SerializeField] private ParticleSystem _wind;
    
    private GameObject _spark;
    private Vector3 _windTransform;
    private Quaternion _windRotation;
    
    private void Start()
    {
        _windTransform = new Vector3(transform.position.x, transform.position.y + 13, transform.position.z + 25f); 
        _windRotation = Quaternion.Euler(transform.rotation.x + 90, transform.rotation.y + 45, transform.rotation.z -45);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Stave stave))
        {
            _spark = Instantiate(_sparkTemplate, transform.position, Quaternion.identity);
            stave.GetComponentInParent<PlayerMover>().Accelerate();
            stave.GetComponentInParent<Player>().Sliding();
            Instantiate(_wind, _windTransform, _windRotation, transform);
            stave.CheckForSecondBar();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Stave stave))
        {
            Destroy(_spark);
            stave.GetComponentInParent<PlayerMover>().Braking();
            stave.GetComponentInParent<Player>().StopSliding();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Stave stave))
        {
            Vector3 target = new Vector3(_spark.transform.position.x, _spark.transform.position.y, stave.transform.position.z);
            _spark.transform.position = target;
        }
    }
}
