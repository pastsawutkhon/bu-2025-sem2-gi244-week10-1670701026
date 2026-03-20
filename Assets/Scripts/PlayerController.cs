using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;
    public AudioClip crashSfx;
    public float playerHealth = 3;
    public bool isDash = false;

    private Rigidbody rb;
    private InputAction jumpAction;
    private bool isOnGround = true;
    private bool isDbJump = true;
    

    private Animator playerAnim;
    private AudioSource playerAudio;

    public bool gameOver = false;
    public GameObject backGround;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        backGround.GetComponent<MoveLeft>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");

        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.triggered && isOnGround && !gameOver)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);
            
        }
        else if (jumpAction.triggered && !isOnGround && isDbJump && !gameOver)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isDbJump = false;
            playerAudio.PlayOneShot(jumpSfx);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isDash = true;

        }
        else
        {
            isDash = false;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            isDbJump = true;
            dirtParticle.Play();
        }
        
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerHealth--;
            if (playerHealth <= 0)
            {
                Debug.Log("Game Over!");
                gameOver = true;
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);
                explosionParticle.Play();
                dirtParticle.Stop();
                playerAudio.PlayOneShot(crashSfx);
            }
            else
            {
                
                explosionParticle.Play();
                playerAudio.PlayOneShot(crashSfx);
                Destroy(collision.gameObject);
            }
                
            
        }
    }
    

}