using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _timeToBurnStave;
    
    public event UnityAction FireStepped;
    public event UnityAction<float> BonusGoted;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private float groundRadius = 0.08f;
    private bool _isGrounded;
    private float _timer;
    private StartGame _startGame;

    private const string IsFinished = "IsFinished";
    private const string IsFalling = "IsFalling";
    private const string IsSliding = "IsSliding";
    private const string IsJumping = "IsJumping";
    private const string IsStart = "IsStart";

    private void Awake()
    {
        _startGame = FindObjectOfType<StartGame>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        
        InvokeRepeating("CheckGround",0.5f, 0.1f );
    }

    private void OnEnable()
    {
        _startGame.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _startGame.GameStarted -= OnGameStarted;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BonusStave bonusStave))
        {
            BonusGoted?.Invoke(bonusStave.GetLenght());
            bonusStave.Destroy();
        }

        if (other.gameObject.TryGetComponent(out Fire fire))
        {
            FireStepped?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Fire fire))
        {
            FireStayed();
        }
    }

    private void FireStayed()
    {
        _timer += Time.deltaTime;
        
        if (_timer > _timeToBurnStave)
        {
            _timer = 0;
            FireStepped?.Invoke();
        }
    }

    private void CheckGround()
    {
        if (Physics.CheckSphere(_groundCheck.position, groundRadius, _whatIsGround))
        {
            _isGrounded = true;
        }
  
        if (_isGrounded)
        {
            _animator.SetBool(IsSliding, false);
            _animator.SetBool(IsJumping, false);
        }
    }

    private void OnGameStarted()
    {
        _animator.SetBool(IsStart, true);
    }

    public void FreezeYPos()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        _rigidbody.freezeRotation = true;
    }

    public void DefrostYPos()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.freezeRotation = true;
    }

    public void Sliding()
    {
        _isGrounded = false;
        _animator.SetBool(IsJumping, false);
        _animator.SetBool(IsSliding, true);
    }

    public void StopSliding()
    {
        _isGrounded = false;
        _animator.SetBool(IsSliding, false);
        _animator.SetBool(IsJumping, true);
    }

    public void Finished()
    {
        _animator.SetTrigger(IsFinished);
    }

    public void Falling()
    {
        _animator.SetTrigger(IsFalling);
    }
}
