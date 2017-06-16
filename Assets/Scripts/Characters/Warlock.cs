using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.015625f)]
public class Warlock : NetworkBehaviour
{
    public GameObject FireballPrefab;
    public GameObject FrostPrefab;
    public GameObject TelePrefab;
    public GameObject HomeingPrefab;
    public float Speed = 10.0f;
    protected Quaternion LookTarget;




    [SyncVar]
    public bool Tele = false;
    [SyncVar]
    public Vector3 TelePos;
    [SyncVar]
    public float Delay;
    [SyncVar]
    public float Spell1CD;
    [SyncVar]
    public float Spell2CD;
    [SyncVar]
    public float Spell3CD;
    [SyncVar]
    public float Spell4CD;
    private Vector3 Constrations;
    public override void OnStartLocalPlayer()
    {
        Constrations = new Vector3(transform.rotation.eulerAngles.x, transform.position.y, transform.rotation.eulerAngles.z);
        LookTarget = gameObject.transform.rotation;
        Camera.main.GetComponent<CameraFollow>().Player = gameObject;
}
    void Update()
    {
        if (isServer) {
            Spell1CD -= Time.deltaTime;
            Spell2CD -= Time.deltaTime;
            Spell3CD -= Time.deltaTime;
            Spell4CD -= Time.deltaTime;
            Delay -= Time.deltaTime;
        }
        if (!isLocalPlayer)
        {
            return;
        }
        if (Delay < 0 && !GetComponent<Health>().Falling) {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Spell1CD <= 0.0f) {
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, /*Layer 8*/ (1 << 8) + (1 << 13));
                hit.point = new Vector3(hit.point.x, 0, hit.point.z);
                if (hit.point.magnitude == 0) // handles load time rotation error until mouse enters screen for first time. 
                {
                    hit.point = new Vector3(0, 0, 0.1f);
                }
                LookTarget = Quaternion.LookRotation(hit.point - new Vector3(transform.position.x, 0, transform.position.z));
                Vector3 Pos = new Vector3(transform.position.x - hit.point.x, 0, transform.position.z - hit.point.z);
                Pos.Normalize();
                CmdFire(Pos);
                transform.rotation = Quaternion.Lerp(transform.rotation, LookTarget, Time.deltaTime*24);
                Delay = 0.35f;
                return;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1) && Spell2CD <= 0.0f) {
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, /*Layer 8*/ (1 << 8) + (1 << 13));
                hit.point = new Vector3(hit.point.x, 0, hit.point.z);
                if (hit.point.magnitude == 0) // handles load time rotation error until mouse enters screen for first time. 
                {
                    hit.point = new Vector3(0, 0, 0.1f);
                }
                LookTarget = Quaternion.LookRotation(hit.point - new Vector3(transform.position.x, 0, transform.position.z));
                Vector3 Pos = new Vector3(transform.position.x - hit.point.x, 0, transform.position.z - hit.point.z);
                Pos.Normalize();
                CmdTele(Pos);
                transform.rotation = Quaternion.Lerp(transform.rotation, LookTarget, Time.deltaTime*24);
                Delay = 0.35f;
                return;
            }
            if (Input.GetKeyDown(KeyCode.Q) && Spell3CD <= 0.0f) {
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, /*Layer 8*/ (1 << 8) + (1 << 13));
                hit.point = new Vector3(hit.point.x, 0, hit.point.z);
                if (hit.point.magnitude == 0) // handles load time rotation error until mouse enters screen for first time. 
                {
                    hit.point = new Vector3(0, 0, 0.1f);
                }
                LookTarget = Quaternion.LookRotation(hit.point - new Vector3(transform.position.x, 0, transform.position.z));
                Vector3 Pos = new Vector3(transform.position.x - hit.point.x, 0, transform.position.z - hit.point.z);
                Pos.Normalize();
                CmdFrost(Pos);
                transform.rotation = Quaternion.Lerp(transform.rotation, LookTarget, Time.deltaTime*24);
                Delay = 0.35f;
                return;
            }
            if (Input.GetKeyDown(KeyCode.E) && Spell4CD <= 0.0f) {
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, /*Layer 8*/ (1 << 8) + (1 << 13));
                hit.point = new Vector3(hit.point.x, 0, hit.point.z);
                if (hit.point.magnitude == 0) // handles load time rotation error until mouse enters screen for first time. 
                {
                    hit.point = new Vector3(0, 0, 0.1f);
                }
                LookTarget = Quaternion.LookRotation(hit.point - new Vector3(transform.position.x, 0, transform.position.z));
                Vector3 Pos = new Vector3(transform.position.x - hit.point.x, 0, transform.position.z - hit.point.z);
                Pos.Normalize();
                CmdHome(Pos);
                transform.rotation = Quaternion.Lerp(transform.rotation, LookTarget, Time.deltaTime*24);
                Delay = 0.35f;
                return;
            }
            //var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
            //var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

            //transform.Rotate(0, 0, 0);
            //transform.Translate(x, 0, z);
        if (Tele)
            {
                Tele = false;
                transform.position = TelePos;
            }
            Vector3 Temp = gameObject.transform.position;
            Vector3 Look = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W)) {
                //remove running to override the click to move key
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                    Temp.z += Speed*Time.deltaTime*0.7071067811865475f;
                } else {
                    Temp.z += Speed*Time.deltaTime;
                }
                Look += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.S)) {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                    Temp.z -= Speed*Time.deltaTime*0.7071067811865475f;
                } else {
                    Temp.z -= Speed*Time.deltaTime;
                }
                Look -= new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.A)) {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) {
                    Temp.x -= Speed*Time.deltaTime*0.7071067811865475f;
                } else {
                    Temp.x -= Speed*Time.deltaTime;
                }
                Look -= new Vector3(1, 0, 0);
            }
            if (Input.GetKey(KeyCode.D)) {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) {
                    Temp.x += Speed*Time.deltaTime*0.7071067811865475f;
                } else {
                    Temp.x += Speed*Time.deltaTime;
                }
                Look += new Vector3(1, 0, 0);
            }
            transform.position = Temp;
            if (Look != new Vector3(0, 0, 0)) {
                LookTarget = Quaternion.LookRotation(Look);
            }
            Camera.main.GetComponent<CameraFollow>().Update();
        }
        if (Delay > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, LookTarget, Time.deltaTime * 24);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, LookTarget, Time.deltaTime * 6);
        }
    }
    void LateUpdate() {
        Vector3 Temp = transform.eulerAngles;
        Temp.x = Constrations.x;
        Temp.z = Constrations.z;
        transform.eulerAngles = Temp;

        if (!GetComponent<Health>().Falling)
        {
            Temp = transform.position;
            Temp.y = Constrations.y;
            transform.position = Temp;
        }
    }
    [Command]
    void CmdFire(Vector3 Direction)
    {
        // Create the Bullet from the Bullet Prefab
        GameObject SpellToFire = (GameObject)Instantiate(FireballPrefab, (transform.position + new Vector3(0, 2, 0)) - Direction * 3, Quaternion.identity);

        Spell aSpell = SpellToFire.GetComponent<Spell>();
        // Add velocity to the bullet
        aSpell.Init(-Direction);
            SpellToFire.GetComponent<Rigidbody>().velocity = -Direction * aSpell.SpellStats.Speed;
        // Spawn the bullet on the Clients
        NetworkServer.Spawn(SpellToFire);
            Destroy(SpellToFire, aSpell.SpellStats.Duration);
            Spell1CD = aSpell.SpellStats.Cooldown;
    }
    [Command]
    void CmdTele(Vector3 Direction)
    {
        // Create the Bullet from the Bullet Prefab
        GameObject SpellToFire = (GameObject)Instantiate(TelePrefab, (transform.position + new Vector3(0, 2, 0)) - Direction * 3, Quaternion.identity);

        Spell aSpell = SpellToFire.GetComponent<Spell>();
        // Add velocity to the bullet
        aSpell.Init(-Direction);
        aSpell.Warlock = gameObject;
            SpellToFire.GetComponent<Rigidbody>().velocity = -Direction * aSpell.SpellStats.Speed;
        // Spawn the bullet on the Clients
        NetworkServer.Spawn(SpellToFire);
            Spell2CD = aSpell.SpellStats.Cooldown;
    }
    [Command]
    void CmdFrost(Vector3 Direction)
    {
        // Create the Bullet from the Bullet Prefab
        GameObject SpellToFire = (GameObject)Instantiate(FrostPrefab, (transform.position + new Vector3(0, 2, 0)) - Direction * 3, Quaternion.identity);

        Spell aSpell = SpellToFire.GetComponent<Spell>();
        // Add velocity to the bullet
        aSpell.Init(-Direction);
            SpellToFire.GetComponent<Rigidbody>().velocity = -Direction * aSpell.SpellStats.Speed;
        // Spawn the bullet on the Clients
        NetworkServer.Spawn(SpellToFire);
            Destroy(SpellToFire, aSpell.SpellStats.Duration);
            Spell3CD = aSpell.SpellStats.Cooldown;
    }
    [Command]
    void CmdHome(Vector3 Direction)
    {
        // Create the Bullet from the Bullet Prefab
        GameObject SpellToFire = (GameObject)Instantiate(HomeingPrefab, (transform.position + new Vector3(0, 2, 0)) - Direction * 3, Quaternion.identity);

        Spell aSpell = SpellToFire.GetComponent<Spell>();
        // Add velocity to the bullet
        aSpell.Init(-Direction);
        aSpell.Warlock = gameObject;
        // Spawn the bullet on the Clients
        NetworkServer.Spawn(SpellToFire);
            Destroy(SpellToFire, aSpell.SpellStats.Duration);
            Spell4CD = aSpell.SpellStats.Cooldown;
    }
    public void Move(Vector3 Pos) {
        Tele = true;
        TelePos = Pos;
    }

}
