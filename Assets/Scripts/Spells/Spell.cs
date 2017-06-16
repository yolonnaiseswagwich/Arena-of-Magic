using System;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.015625f)]
public class Spell : NetworkBehaviour {
    [Serializable]
    public struct Stats
    {
        public float Duration;
        public float Damage;
        public float Speed;
        public float Cooldown;
    };
    [SyncVar]
    public GameObject Warlock;
    public Vector3 Direction;
    public virtual void Init(Vector3 aDirection) {

    }
    [SyncVar]
    public Stats SpellStats;
  //  [Server]
    void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        Health HP = hit.GetComponent<Health>();
        if (HP != null) {
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
