using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.015625f)]
public class Freeze : Spell {
    public override void Init(Vector3 aDirection)
    {
        SpellStats.Damage = 0;
        SpellStats.Cooldown = 8.0f;
        SpellStats.Duration = 3.0f;
        SpellStats.Speed = 20.0f;
        Direction = aDirection;
    }
   // [Server]
    void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        Warlock HP = hit.GetComponent<Warlock>();
        if (HP != null) {
            HP.Delay = 2.0f;
        }
        Destroy(gameObject);
    }

}
