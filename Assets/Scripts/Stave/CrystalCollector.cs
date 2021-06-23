using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCollector : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.TryGetComponent(out Crystal crystal))
      {
         Destroy(crystal.gameObject);
      }
   }
}
