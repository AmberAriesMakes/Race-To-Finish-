using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour
{
    //Public Variables
    public Rigidbody2D RigidPlayer;
    public int Force;
    public bool jump;
    public float jumpdirection;
    public float maxspeed;
    public float jumpforce;
    public int keycount;
    public AudioClip NoContest;
    public AudioClip start;
    public AudioClip Game;
    public AudioClip Failures;
    public AudioClip winner;
    bool forwardfacing = true;
    SpriteRenderer flipper;
    AudioSource audiosource;
    Animator anim;
    
    //Success & Failure PNGS. Not sure if Failure shows with current method of defeat.
    public  GameObject Success;
    public GameObject Failure;
    public GameObject QuitButton;
    public GameObject RestartButton;
    public GameObject countdown;
    public GameObject keyss;
  

    //Timer and Win & Death State RIP announcer sound files added but couldnt' be used in time.. would've added nice flair I wanted in it badly.
    float currentTime;
    float startingTime = 120;
    
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
        
        audiosource = GetComponent<AudioSource>();
        flipper = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //Announce Both png states to be false at start.
        Success.SetActive(false);
        Failure.SetActive(false);
        audiosource.PlayOneShot(start, 0.7f);
        QuitButton.SetActive(false);
        RestartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

       
        float x = Input.GetAxisRaw("Horizontal");
       
        //Jumping Done by Vector 2's upward force ontop of the public jump force variable. 
        if (Input.GetButtonDown("Jump") && jump == true)
        {
            RigidPlayer.velocity = Vector2.up * jumpforce;
            anim.SetBool("JumpF", true);

        }

        //This method works with movement to allow diagonal jump movement.
        if(Input.GetKey(KeyCode.RightArrow))
        {
            RigidPlayer.velocity = new Vector2(Force, RigidPlayer.velocity.y);
            anim.SetBool("IsMoving", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            RigidPlayer.velocity = new Vector2(-Force, RigidPlayer.velocity.y);
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        if((Input.GetKey(KeyCode.RightArrow))&& !forwardfacing == true)
        {
            flip();
            
            flipper.flipX = false;
        }
        if ((Input.GetKey(KeyCode.LeftArrow)) && forwardfacing == true)
        {
            flip();

            flipper.flipX = true;
        }

        if (RigidPlayer.velocity.magnitude > maxspeed)
        {
            RigidPlayer.velocity = RigidPlayer.velocity.normalized * maxspeed;
        }
        currentTime -= 1 * Time.deltaTime;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        Countdowntext.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        Keys.text = keycount.ToString();


        //If you run out of time, you fail! 
        if (currentTime <= 0)
        {
            currentTime = 0;
            audiosource.PlayOneShot(NoContest, 0.7f);
            Failure.SetActive(true);
            QuitButton.SetActive(true);
            RestartButton.SetActive(true);

        }
        if (win == true)
        {
            currentTime +=  Time.deltaTime;
        }
        if (death== true)
        {
            Time.timeScale = 0;
            
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    //COLLISION CHECKS FOR EVERYTHING THAT NEEDS IT.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jump = true;        //jUMP CHECK TO ENSURE NO DOUBLE JUMPING
            anim.SetBool("IsGrounded", true);
            anim.SetBool("JumpF", false);
        }
        if (collision.gameObject.CompareTag("Spike"))
        {
            audiosource.PlayOneShot(Failures, 0.7f);
            Failure.SetActive(true);
            QuitButton.SetActive(true);
            RestartButton.SetActive(true);
            keyss.SetActive(false);
            countdown.SetActive(false);
            death = true;

            
            
        }
        if (collision.gameObject.CompareTag("Key"))
        {
            keycount++;
            Destroy(collision.gameObject);
        }

    }
    private void flip()
    {
        forwardfacing = !forwardfacing;
        //transform.Rotate(0f, 180f, 0f);
       
        
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
        if (collision.gameObject.CompareTag("Slow"))    //Trigger platforms change the movement speed of the player to be slow...
        {
            Force = 3;
        }
        if (collision.gameObject.CompareTag("Speed")) //Or fast..
        {
            Force++;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        //This restores the force to its original speed when out of the benefit/debuff
        if (collision.gameObject.CompareTag("Slow"))
        {
            Force = 8;
        }
            if (collision.gameObject.CompareTag("Speed"))
            {
                Force = 8;
            }
        }
    //Finally the trigger for victory.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Winner"))
        {
            
            Success.SetActive(true); 
            QuitButton.SetActive(true);
            RestartButton.SetActive(true);
            win = true;
            keyss.SetActive(false);
            countdown.SetActive(false);
            audiosource.PlayOneShot(winner, 0.7f);
        }
    }

  public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        death = false;
        SceneManager.LoadScene(0);
    }
   
}
