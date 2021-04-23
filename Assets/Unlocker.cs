using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocker : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody2D rigiddoor;
    public MovementScript movescript;
    bool islocked = true;

    private void Awake()
    {
        rigiddoor.freezeRotation = true;
        movescript = Player.GetComponent<MovementScript>();
    }
    private void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.CompareTag("Player") && movescript.keycount > 0 && islocked == true)
        {
           rigiddoor.freezeRotation = false;
            movescript.keycount--;
            islocked = false;
        }
      
    }
}
