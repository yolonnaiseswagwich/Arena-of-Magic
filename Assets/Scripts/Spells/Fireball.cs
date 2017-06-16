using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.015625f)]
public class Fireball : Spell {

    public override void Init(Vector3 aDirection) {
        SpellStats.Damage = 10;
        SpellStats.Cooldown = 3.0f;
        SpellStats.Duration = 1.5f;
        SpellStats.Speed = 55.0f;
        Direction = aDirection;
    }
//    [Server]
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
