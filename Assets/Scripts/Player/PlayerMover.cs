using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
   [SerializeField] private float _speed;
   [SerializeField] private float _turningSpeed;

   private void FixedUpdate()
   {
      transform.position += Vector3.forward * _speed * Time.fixedDeltaTime;

      if (Input.GetKey(KeyCode.A))
      {
         transform.position += Vector3.left * _turningSpeed * Time.fixedDeltaTime;
      }
      
      if (Input.GetKey(KeyCode.D))
      {
         transform.position += Vector3.right * _turningSpeed * Time.fixedDeltaTime;
      }
   }
}
