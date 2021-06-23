using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction FireStepped;
    public event UnityAction<float> BonusGoted;
    
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
}
