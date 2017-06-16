using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.015625f)]
public class Teleport : Spell {
    public override void Init(Vector3 aDirection)
    {
        SpellStats.Damage = 0;
        SpellStats.Cooldown = 6.0f;
        SpellStats.Duration = 0.25f;
        SpellStats.Speed = 85.0f;
        Direction = aDirection;
    }
    void Update() {
        SpellStats.Duration -= Time.deltaTime;
        if (SpellStats.Duration <= 0)
        {
            Warlock.GetComponent<Warlock>().Move(transform.position);
            Destroy(gameObject);
        }
    }
//    [Server]
    void OnCollisionEnter(Collision collision)
    {
        Warlock.GetComponent<Warlock>().Move(transform.position);
        Destroy(gameObject);
    }
}
