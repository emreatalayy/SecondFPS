using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision)
   {
    Transform hitTransform = collision.transform;
    if (hitTransform.CompareTag("Player"))
    {
        Debug.Log("Player Hit");
        hitTransform.GetComponent<PlayerHealth>().TakeDamage(10);
    }
    Destroy(gameObject); 
      
   }
}
