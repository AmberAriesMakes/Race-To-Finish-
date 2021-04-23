using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{

    public Rigidbody2D RigidPlayer;
    public int Force;
    public bool jump;
    public float jumpdirection;
    public float maxspeed;
    public float jumpforce;
    public int keycount;
    
    public  GameObject Success;
    public GameObject Failure;

    float currentTime;
    float startingTime = 200;
    bool win;
    bool death;



    [SerializeField] Text Countdowntext;
    
    [SerializeField] Text Keys;
    // Start is called before the first frame update
    void Start()
    {
        RigidPlayer = GetComponent<Rigidbody2D>();
        keycount = 0;
        currentTime = startingTime;
        

        Success.SetActive(false);
        Failure.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
       

        //if (RigidPlayer.velocity.x > 0)
        //{
        //    jumpdirection = 1;
        //    print(jumpdirection);
        //}
        //if (RigidPlayer.velocity.x < 0)
        //{
        //    jumpdirection = -1;
        //    print(jumpdirection);
        //}
        if (Input.GetButtonDown("Jump") && jump == true)
        {
            RigidPlayer.velocity = Vector2.up * jumpforce;
          
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            RigidPlayer.velocity = new Vector2(Force, RigidPlayer.velocity.y);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RigidPlayer.velocity = new Vector2(-Force, RigidPlayer.velocity.y);
        }

        if (RigidPlayer.velocity.magnitude > maxspeed)
        {
            RigidPlayer.velocity = RigidPlayer.velocity.normalized * maxspeed;
        }
        currentTime -= 1 * Time.deltaTime;
        Countdowntext.text = currentTime.ToString("0:00");
        Keys.text = keycount.ToString();

        if (currentTime <= 0)
        {
            currentTime = 0;
            Failure.SetActive(false);
        }
        if (win == true)
        {
            currentTime +=  Time.deltaTime;
        }
        if (death== true)
        {
            Time.timeScale = 0;
        }

    }

        

            //if (RigidPlayer.velocity.y > 0){
            //    RigidPlayer.AddForce(jumpdirection,ForceMode2D impulse)
            // }





        


    private void FixedUpdate()
    {
      

    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jump = true;
            
        }
        if (collision.gameObject.CompareTag("Spike"))
        {
            Failure.SetActive(true);
            death = true;
            
            
        }
        if (collision.gameObject.CompareTag("Key"))
        {
            keycount++;
            Destroy(collision.gameObject);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jump = false;
        }
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slow"))
        {
            Force = 3;
        }
        if (collision.gameObject.CompareTag("Speed"))
        {
            Force = 8;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slow"))
        {
            Force = 5;
        }
            if (collision.gameObject.CompareTag("Speed"))
            {
                Force = 5;
            }
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Winner"))
        {
            
            Success.SetActive(true);
            win = true;
        }
    }
}
