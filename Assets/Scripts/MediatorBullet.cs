using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediatorBullet : MonoBehaviour
{
    //public GameObject DamagePopup;

    private float Damage { get; set; }

    private float defaultDamage = 10.0f;
    private float strongDamage = 20.0f;
    private float superDamage = 30.0f;
    private float speed = 50.0f;
    private bool canGo = false;
    private Vector3 relativeDirection;
    private Transform relativeTransform;
    private Vector3 lastPos;
    private float timeOfLife = 0.0f;
    private float maxTimeOfLife = 1.5f;

    public void SetDirectionRelateToTransform(Transform relationTransform)
    {
        relativeTransform = relationTransform;
        relativeDirection = new Vector3(relationTransform.forward.x, relationTransform.forward.y + 0.1f, relationTransform.forward.z);
    }

    public void SetDamageMode(BulletDamageMode damageMode = BulletDamageMode.Normal)
    {
        if (damageMode == BulletDamageMode.Normal)
        {
            Damage = defaultDamage;
        }
        else if (damageMode == BulletDamageMode.Strong)
        {
            Damage = strongDamage;
        }
        else if (damageMode == BulletDamageMode.Super)
        {
            Damage = superDamage;
        }
    }

    public void Go()
    {
        canGo = true;
    }

    private void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (canGo)
        {
            timeOfLife += Time.deltaTime;
            if (timeOfLife > maxTimeOfLife)
            {
                Destroy(gameObject);
            }
            transform.Translate(relativeDirection * speed * Time.deltaTime);

            /*RaycastHit hit;
            if (Physics.Linecast(lastPos, transform.position, out hit))
            {
                if (hit.transform.tag != "Player")
                {
                    if (hit.transform.tag == "BulletResponsive")
                    {
                        var def = hit.transform.gameObject.GetComponent<Defeatable>();
                        def.GetDamage(Damage);
                        Debug.Log("Health: " + def.Health);

                        var rep = hit.transform.gameObject.GetComponent<IRepulsive>();
                        rep.Repulse(transform, speed);
                    }
                    Destroy(gameObject);
                }
            }*/

            lastPos = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Bullet"))
        {
            if (other.gameObject.CompareTag("BulletResponsive"))
            {
                var def = other.gameObject.GetComponent<Defeatable>();
                if (def != null)
                {
                    def.GetDamage(Damage, transform, relativeTransform);
                }

                var rep = other.gameObject.GetComponent<IRepulsive>();
                if (rep != null)
                {
                    rep.Repulse(relativeDirection, speed);
                }
            }
            
            Destroy(gameObject);
        }
    }
}

public enum BulletDamageMode
{
    Normal, Strong, Super
}
