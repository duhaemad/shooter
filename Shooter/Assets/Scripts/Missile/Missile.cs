using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject explosion;
    
    void LaunchMissle(Vector3 tarPos )
    {
        Invoke("SetActive", 0.9f);
        LeanTween.move(gameObject, tarPos, 1.6f).setEase(LeanTweenType.easeInBack).setOnComplete(Explode);
    }
   void SetActive()
    {
        GetComponent<Collider>().enabled = true;


    }
    void Explode()
    {
       //nstantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider t)
    {
        if (t.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            if (t.tag == "Box")
            {
                t.GetComponent<Rigidbody>().AddExplosionForce(2000f, transform.position, 6f);
            }else if (t.tag == ("Enemy"))
            {
               t.SendMessage("Damage");
            
            
            }
            Explode();
          //Destroy(gameObject);

        }
    }
    
}
