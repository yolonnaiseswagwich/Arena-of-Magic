using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
    public GameObject Pillar;
    public int numofPillars;
    private PillarPos[] spawnPoints;
    public override void OnStartServer() {
        spawnPoints = FindObjectsOfType<PillarPos>();
        int[] k = new int[numofPillars];
        for (int i = 0; i < numofPillars; i++) {
            k[i] = -1;
        }
        //setup the level with random elements
            for (int i = 0; i < numofPillars; i++) {
            bool check = false;
            while (!check)
            {
                int Temp = (int)Random.Range(0, spawnPoints.Length);
                if (!k.Contains(Temp)) {
                    k[i] = Temp;
                    check = true;
                }
            }
            GameObject apillar = (GameObject)Instantiate(Pillar, spawnPoints[k[i]].transform.position + new Vector3(0,11,0), spawnPoints[k[i]].transform.rotation);
            NetworkServer.Spawn(apillar);
        }
    }

    public void Update() {
    }
}
