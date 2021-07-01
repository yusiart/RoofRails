using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishPlane : MonoBehaviour
{
    private Animator _animator;

    private const string OnTouched = "OnTouched";
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.Finished();
            FindObjectOfType<PlayerMover>().Finished();
            _animator.SetTrigger(OnTouched);
        }
    }
}
