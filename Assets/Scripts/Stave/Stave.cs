using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CrystalCollector))]
public class Stave : MonoBehaviour
{
    [SerializeField] private GameObject _smallPartTemplate;
    [SerializeField] private float _lenghToCut; 
    [SerializeField] private float _moveCenterSpeed;
    
    private Player _player;
    private Vector3 _stavePosition;
    private Vector3 _smallPartPos;
    private Vector3 _largePartPos;
    private float _cutPoint;
    private float _differenceValue;
    private float _largePartScale;
    private float _smallPartScale;
    private float _smallPartXPos;
    private float _largePartXPos;
    private bool _isInCenter = true;
    private bool _onOneBar;
    private Rigidbody _rigidbody;

    public event UnityAction<float> SizeChanged;
    
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        _player.FireStepped += OnStepOnFire;
        _player.BonusGoted += OnBonusGot;
    }

    private void OnDisable()
    {
        _player.FireStepped -= OnStepOnFire;
        _player.BonusGoted -= OnBonusGot;
    }

    private void Update()
    {
        if (!_isInCenter)
        {
            Vector3 target = new Vector3(_player.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, target, _moveCenterSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.position.x - _player.transform.position.x) < 0.03f)
            {
                _isInCenter = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Saw saw))
        {
            if (SetStavesPositionsAndLocalScale(saw))
            {
                ChangeCurrentStavePos(_largePartPos, _largePartScale);
                CreateSmallPartStave(_smallPartPos, _smallPartScale);

                StartCoroutine("StartMoveCenter");
            }
        }

        if (other.gameObject.TryGetComponent(out Bar bar))
        {
            _player.FreezeYPos();
        }
        
        if (other.gameObject.TryGetComponent(out FallingChecker fallingChecker))
        {
            Falling();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bar bar))
        {
            _player.DefrostYPos();
        }
    }

    private IEnumerator StartMoveCenter()
    {
        yield return new WaitForSeconds(0.8f);

        _isInCenter = false;
    }

    private bool SetStavesPositionsAndLocalScale(Saw saw)
    {
        _cutPoint = saw.transform.position.x;
        _differenceValue = Mathf.Abs(transform.position.x - _cutPoint);
        
        _largePartScale = transform.localScale.y / 2 + _differenceValue / 2;
        _smallPartScale = transform.localScale.y - _largePartScale;

        if (_smallPartScale < 0)
        {
            return false;
        }

        if (transform.position.x < saw.transform.position.x)
        {
            _smallPartXPos = _cutPoint  + _smallPartScale;
            _largePartXPos = _cutPoint - _largePartScale;
        }
        else
        {
            _smallPartXPos = _cutPoint - _smallPartScale;
            _largePartXPos = _cutPoint + _largePartScale;
        }
        
        _smallPartPos = GetStavePartPosition(_smallPartXPos);
        _largePartPos = GetStavePartPosition(_largePartXPos);
        
        return true;
    }

    private void ChangeCurrentStavePos(Vector3 largePartPos, float largePartScale)
    {
        transform.localScale = new Vector3(transform.localScale.x, largePartScale, transform.localScale.z);
        transform.position = largePartPos;
        
        SizeChanged?.Invoke(transform.localScale.y);
    }

    private void CreateSmallPartStave(Vector3 smallPartPos, float smallPartScale)
    {
        Quaternion rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
        GameObject smallPartStave = Instantiate(_smallPartTemplate, smallPartPos, rotation);
        smallPartStave.transform.localScale = new Vector3(transform.localScale.x, smallPartScale, transform.localScale.z);
        
        Destroy(smallPartStave, 0.8f);
    }

    private void OnStepOnFire()
    {
        if (transform.localScale.y < _lenghToCut)
            return;
        
        Vector3 lenghtToCut = new Vector3(0, _lenghToCut,0);

        float partsSize = _lenghToCut / 2;

        float leftXPos = transform.position.x - transform.localScale.y + partsSize / 2;
        float rightXPos = transform.position.x + transform.localScale.y - partsSize / 2;
        
        Vector3 rightPos = GetStavePartPosition(rightXPos);
        Vector3 leftPos = GetStavePartPosition(leftXPos);

        CreateSmallPartStave(leftPos, partsSize);
        CreateSmallPartStave(rightPos, partsSize);

        transform.localScale -= lenghtToCut;
        SizeChanged?.Invoke(transform.localScale.y);
    }

    private Vector3 GetStavePartPosition(float xPosition)
    {
       return new Vector3(xPosition, transform.position.y, transform.position.z);
    }

    private void OnBonusGot(float bonusLenght)
    {
        if (bonusLenght > 0)
        {
            transform.localScale += new Vector3(0f, bonusLenght, 0.0f);
            SizeChanged?.Invoke(transform.localScale.y);
        }
    }

    public void CheckForSecondBar()
    {
        _onOneBar = !_onOneBar;
        
        StartCoroutine("CheckForBarPositions");
    }

    private IEnumerator CheckForBarPositions()
    {
        yield return  new WaitForSeconds(0.15f);

        if (_onOneBar)
        {
            Falling();
        }
    }

    private void Falling()
    {
        _rigidbody.useGravity = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _player.Falling();
    }
}