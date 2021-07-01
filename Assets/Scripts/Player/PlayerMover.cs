using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
   [SerializeField] private float _speed;
   [SerializeField] private float _turningSpeed;
   [SerializeField] private float _accelerationSpeed;

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
   
   public void Accelerate()
   {
      _speed += _accelerationSpeed;
   }
   
   public void Braking()
   {
      _speed -= _accelerationSpeed;
   }

   public void Finished()
   {
      _speed = 0;
      _turningSpeed = 0;
   }
}
