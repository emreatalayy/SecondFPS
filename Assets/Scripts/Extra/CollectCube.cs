using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCube : Interactable
{
    public GameObject particle;
  
    protected override void Interact()
    {
        Destroy(gameObject);
        Instantiate(particle, transform.position, Quaternion.identity);
    }
}
