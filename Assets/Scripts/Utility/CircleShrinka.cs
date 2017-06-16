using UnityEngine;
using UnityEngine.Networking;

public class CircleShrinka : NetworkBehaviour {
    [SyncVar]
    public float timer;
    [SyncVar]
    public Vector3 StartScale;
    [SyncVar]
    public Vector3 EndScale;
    [SyncVar]
    public float Starttime;
    [SyncVar]
    public float Length;
    [SyncVar]
    public Vector3 Identity =new Vector3(10,10,10);
    [SyncVar]
    public Transform A;
    // Update is called once per frame
    public override void OnStartServer() {
        A = gameObject.transform;
        timer = (NetworkServer.connections.Count)*10;
        StartScale = transform.localScale;
        EndScale = transform.localScale;
    }
    void Update() {
        if (!isClient)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0 && transform.localScale != Identity)
        {
            timer = (NetworkServer.connections.Count) * 10;
            Starttime = Time.time;
            EndScale = StartScale - new Vector3(5, 5, 5);
            Length = Vector3.Distance(StartScale, EndScale);
        }
        if (transform.localScale != EndScale && StartScale != EndScale) {
            float distCovered = (Time.time - Starttime)*5.0f;
            float fracJourney = distCovered/Length;
            transform.localScale = Vector3.Lerp(StartScale, EndScale, fracJourney);
        } else {
            StartScale = EndScale;
        }
    }
}
