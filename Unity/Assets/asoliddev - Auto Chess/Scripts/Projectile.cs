using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    public float duration;

    private GameObject target;

    private bool isMoving = false;
    
    public void Init(GameObject _target)
    {
        target = _target;
       
        isMoving = true;
    }

    void Update()
    {
      
        if (isMoving)
        {
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }

            Vector3 relativePos = target.transform.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            this.transform.rotation = rotation;


            Vector3 targetPosition = target.transform.position + new Vector3(0, 1, 0);

            float step = speed * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);


            float distance = Vector3.Distance(this.transform.position, targetPosition);

            if (distance < 0.2f)
            {
               // isMoving = false;

                this.transform.parent = target.transform;


                Destroy(this.gameObject, duration);
            }
           
        }
     

    }
}
