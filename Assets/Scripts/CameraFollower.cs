using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private Vector3 _offset;

    private Player _player;
    private Vector3 _nextPosition;
    private Stave _stave;
    private bool _isDistantly;
    private readonly Vector3 _distanceToChange = new Vector3(0, 5, -7);
    
    private void Awake()
    {
        _stave = FindObjectOfType<Stave>();
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();

        Quaternion target = Quaternion.Euler(15, 0, 0);
        transform.rotation = target;
    }

    private void OnEnable()
    {
        _stave.SizeChanged += OnStaveSizeChanged;
    }

    private void OnDisable()
    {
        _stave.SizeChanged -= OnStaveSizeChanged;
    }

    private void FixedUpdate()
    {
        _nextPosition = _player.transform.position + _offset;
        transform.position = Vector3.Lerp(transform.position, _nextPosition, _delay * Time.fixedDeltaTime);
    }
    
    private void OnStaveSizeChanged(float staveSize)
    {
        if (staveSize > 6f && !_isDistantly)
        {
            ChangeDistance(_distanceToChange);
        }
        else if (staveSize < 6f && _isDistantly)
        {
            ChangeDistance(-_distanceToChange);
        }
    }

    private void ChangeDistance(Vector3 distanceToChange)
    {
        _offset += distanceToChange;
        _isDistantly = !_isDistantly;
    }
}