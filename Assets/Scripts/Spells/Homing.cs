using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.015625f)]
public class Homing : Spell
{
    private Vector3 Velocity;
    private GameObject Target;
    private Warlock[] Warlocks;
    public override void Init(Vector3 aDirection)
    {
        SpellStats.Damage = 10;
        SpellStats.Cooldown = 14.0f;
        SpellStats.Duration = 10.0f;
        SpellStats.Speed = 30.0f;
        Direction = -aDirection;
        Velocity = Direction * SpellStats.Speed * 0.2f;
        Warlocks = FindObjectsOfType<Warlock>();
    }
    void Update()
    {
        Vector3 Temp = gameObject.transform.position;
        if (Target == null)
        {
            int WarID = -1;
            float Num = 1000000;
            for (int i = 0; i < Warlocks.Length; ++i)
            {
                if (Warlocks[i] != Warlock.GetComponent<Warlock>())
                {
                    if ((Temp - Warlocks[i].transform.position).sqrMagnitude < 900) //20 radius
                    {
                        if ((Temp - Warlocks[i].transform.position).sqrMagnitude < Num)
                        {
                            WarID = i;
                            Num = (Temp - Warlocks[i].transform.position).sqrMagnitude;
                        }
                    }
                }
            }
            if (WarID != -1)
            {
                Target = Warlocks[WarID].gameObject;
            }
        }
        else
        {
            // gameObject.GetComponent<Renderer>().material.color = Color.red;
            Vector3 Location = Target.transform.position;
            Direction = new Vector3(Temp.x - Location.x, 0, Temp.z - Location.z);
            Direction.Normalize();
        }
        Velocity += Direction * SpellStats.Speed * 0.2f * Time.deltaTime;
        Velocity -= Velocity * 0.5f * Time.deltaTime;
        Temp -= Velocity * Time.deltaTime;
        gameObject.transform.position = Temp;
    }
 //   [Server]
    void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        Health HP = hit.GetComponent<Health>();
        if (HP != null)
        {
            Vector3 Temp = new Vector3(collision.transform.position.x - transform.position.x, 0, collision.transform.position.z - transform.position.z).normalized;
            HP.TakeDamage(Temp, SpellStats.Damage);
        }
        Dynamic Pillar = hit.GetComponent<Dynamic>();
        if (Pillar != null)
        {
            Vector3 Temp = new Vector3(collision.transform.position.x - transform.position.x, 0, collision.transform.position.z - transform.position.z).normalized;
            Pillar.TakeDamage(Temp, SpellStats.Damage);
        }
        Destroy(gameObject);
    }
}