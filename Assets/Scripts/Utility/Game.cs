using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Game : NetworkBehaviour
{
    [SyncVar]
    public bool Gamemode = false; //false = Deathmatch // True = Rounds

}
