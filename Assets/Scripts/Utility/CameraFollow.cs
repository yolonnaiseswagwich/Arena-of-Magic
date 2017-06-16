using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public float Speed = 50.0f;
    private Vector3 Pos;
    void Start()
    {
        Pos = transform.position;
    }
    public void Update()
    {
        if (Player != null)
        {
            if (Player.GetComponent<Warlock>().enabled == true && !Player.GetComponent<Health>().Falling) {
                transform.position = Player.transform.position + Pos;
            }
            else if (Player.GetComponent<Health>().Falling && Player.GetComponent<Warlock>().enabled == true) {
                transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z) + Pos;
            }
            else {
                Vector3 Temp = gameObject.transform.position;
                if (Input.GetKey(KeyCode.W))
                {
                    //remove running to override the click to move key
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    {
                        Temp.z += Speed * Time.deltaTime * 0.7071067811865475f;
                    }
                    else
                    {
                        Temp.z += Speed * Time.deltaTime;
                    }
                }
                if (Input.GetKey(KeyCode.S))
                {
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    {
                        Temp.z -= Speed * Time.deltaTime * 0.7071067811865475f;
                    }
                    else
                    {
                        Temp.z -= Speed * Time.deltaTime;
                    }
                }
                if (Input.GetKey(KeyCode.A))
                {
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                    {
                        Temp.x -= Speed * Time.deltaTime * 0.7071067811865475f;
                    }
                    else
                    {
                        Temp.x -= Speed * Time.deltaTime;
                    }
                }
                if (Input.GetKey(KeyCode.D))
                {
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                    {
                        Temp.x += Speed * Time.deltaTime * 0.7071067811865475f;
                    }
                    else
                    {
                        Temp.x += Speed * Time.deltaTime;
                    }
                }
                transform.position = Temp;
            }
        }
    }
}
