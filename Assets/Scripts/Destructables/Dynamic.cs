using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Dynamic : NetworkBehaviour
{
    [SyncVar]
    public Vector3 Force;
    [SyncVar]
    public float SlideMultiplier = 0;
    [SyncVar]
    public float FallTimer = 0.75f;
    [SyncVar]
    public bool Falling = false;
    [SyncVar]
    public float currentHealth = 100;
    public void TakeDamage(Vector3 Postion, float Damage)
    {
        if (!isServer)
        {
            return;
        }
        Force += Postion * Damage * (1.0f + SlideMultiplier) * 0.125f;
        SlideMultiplier += Damage * 0.0075f;
        currentHealth -= Damage * 0.1f;
        //do knockback
        if (currentHealth <= 0)
        {
            RpcDestroy();
        }
    }
    void Update()
    {

        if (isServer)
        {
            RaycastHit hit = new RaycastHit();
            if (!Falling && Physics.Raycast(gameObject.transform.position + new Vector3(0, 10, 0), Vector3.down, out hit, Mathf.Infinity, (1 << 8) + (1 << 13))) ;
            {
                hit.point = new Vector3(hit.point.x, 0, hit.point.z);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.layer == 13)
                    {
                        FallTimer -= Time.deltaTime;
                        if (FallTimer <= 0)
                        {
                            Falling = true;
                            // GetComponent<CapsuleCollider>().enabled = false;
                            FallTimer = 0.75f;
                        }
                    }
                    else
                    {
                        FallTimer += Time.deltaTime;
                        if (FallTimer >= 0.75f)
                        {
                            FallTimer = 0.75f;
                        }
                    }
                }
            }
            if (transform.position.y < -40)
            {
                TakeDamage(Vector3.zero, 10000);
            }
        }
            Vector3 Temp = gameObject.transform.position;
            //Add Sliding!
            if (!Falling)
            {
                Temp += Force * Time.deltaTime;
                Force -= Force * Time.deltaTime * 1.4f;
                if (Force.sqrMagnitude < 2 && Force.sqrMagnitude > -2)
                {
                    Force = new Vector3();
                }
            }
            if (Falling)
            {
                Temp -= new Vector3(0, 25, 0) * Time.deltaTime;
            }
            transform.position = Temp;
        if (currentHealth <= 0)
        {
            RpcDestroy();
        }
    }
    [ClientRpc]
    void RpcDestroy()
    {
            Destroy(gameObject);
    }
}