using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTriggerZone : MonoBehaviour
{
   [SerializeField] private bool _isDamaging;
   [SerializeField] private float _damage;

   private void OnTriggerStay(Collider col)
   {
      if(col.tag == "Player")
         col.SendMessage((_isDamaging)?"TakeDamage":"HealDamage",Time.deltaTime * _damage);
   }
}
