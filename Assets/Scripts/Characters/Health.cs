using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[NetworkSettings(sendInterval = 0.015625f)]
public class Health : NetworkBehaviour
{
    [SyncVar]
    public Vector3 Force;
    [SyncVar]
    public float SlideMultiplier = 0;
    [SyncVar]
    public float RespawnTimer = 0;
    [SyncVar]
    public float FallTimer = 0.75f;
    [SyncVar]
    public bool Dead = false;
    [SyncVar]
    public bool Falling = false;
    public const float maxHealth = 100;
    private NetworkStartPosition[] spawnPoints;
    [SyncVar(hook = "OnChangeHealth")]
    public float currentHealth = maxHealth;

    public RectTransform healthBar;
    private RigidbodyConstraints RbConst;
    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            RbConst = GetComponent<Rigidbody>().constraints;
        }
    }
    public void TakeDamage(Vector3 Postion, float Damage)
    {
        if (!isServer)
        {
            return;
        }
        Force += Postion * Damage * (1.0f + SlideMultiplier);
        SlideMultiplier += Damage * 0.0075f;
        currentHealth -= Damage;
        //do knockback
        if (currentHealth <= 0 && !Dead) {
            Dead = true;
            RespawnTimer = NetworkServer.connections.Count*5.0f;
            RpcKill();
        }
    }
    void Update()
    {
        if (isServer) {
            if (Dead && RespawnTimer > 0.0f) {
                RespawnTimer -= Time.deltaTime;
            }
            if (Dead && RespawnTimer < 0) {
                currentHealth = maxHealth;
                Dead = false;
                RpcRespawn();
            }
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
                Vector3 Temp2 = transform.position;
                Temp2.y = 0;
                transform.position = Temp2;
                TakeDamage(Vector3.zero, 10000);
            }
        }
        if (isLocalPlayer) {
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
            if (Falling && !Dead)
            {
                Temp -= new Vector3(0, 25, 0) * Time.deltaTime;
            }
            transform.position = Temp;
        }
    }
    void OnChangeHealth(float health) { healthBar.offsetMin = new Vector2(100 - health, healthBar.offsetMin.y); }
    [ClientRpc]
    void RpcKill()
    {
            if (GetComponent<Warlock>() != null) {
                GetComponent<Warlock>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                var Meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (SkinnedMeshRenderer i in Meshes) {
                    i.enabled = false;
                }
                GetComponentInChildren<Canvas>().enabled = false;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
    }
    [ClientRpc]
    void RpcRespawn()
    {
        if (GetComponent<Warlock>() != null)
        {
            GetComponent<Warlock>().enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
            var Meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer i in Meshes)
            {
                i.enabled = true;
            }
            GetComponentInChildren<Canvas>().enabled = true;
            GetComponent<Rigidbody>().constraints = RbConst;
            Force = Vector3.zero;
            SlideMultiplier = 0;
            Falling = false;
        }

        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;
        }
    }

}