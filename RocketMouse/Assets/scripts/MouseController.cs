using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    private uint coins = 0;
    public Transform groundCheckTransform;
    public ParticleSystem jetpack;
    private bool grounded;
    private bool dead = false;
    public LayerMask groundCheckLayerMask;
    public Texture2D coinIconTexture;
    Animator animator;
    public GUIStyle restartButtonStyle;
    public AudioClip coinCollectSound;
    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;
   public ParallaxCamera paralax;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnGUI()
    {
        DisplayCoinsCount();
        DisplayRestartButton();
    }
   
    void DisplayRestartButton()
    {
        if (dead && grounded)
        {
            

            Rect buttonRect = new Rect(Screen.width * 0.35f, Screen.height * 0.45f, Screen.width * 0.30f, Screen.height * 0.1f);
            if (GUI.Button(buttonRect, "Tap to restart!"))
            {
                Application.LoadLevel(Application.loadedLevelName);

              
            };

            Vector3 v = new Vector3(0, 0, -2);
            GetComponent<Rigidbody2D>().velocity = v;


        }

    }
    void DisplayCoinsCount()
    {
        Rect coinIconRect = new Rect(10, 10, 32, 32);
        GUI.DrawTexture(coinIconRect, coinIconTexture);

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        Rect labelRect = new Rect(coinIconRect.xMax, coinIconRect.y, 60, 32);
        GUI.Label(labelRect, coins.ToString(), style);
    }
    // Use this for initialization
    void UpdateGroundedStatus()
    {
        //1
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        Debug.Log(grounded);
        //2
        animator.SetBool("grounded", grounded);
    }
    void AdjustJetpack(bool jetpackActive)
    {
        jetpack.enableEmission = !grounded;
        jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f;
    }
    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");

        if (jetpackActive)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jetpackForce));
        }
        if (!dead)
        {
            Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
            newVelocity.x = forwardMovementSpeed;
            GetComponent<Rigidbody2D>().velocity = newVelocity;
        }
        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);
        AdjustFootstepsAndJetpackSound(jetpackActive);
        paralax.offset = transform.position.x;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coins"))
            CollectCoin(collider);
       
        else
            HitByLaser(collider);
    }

    void HitByLaser(Collider2D laserCollider)
    {
        if (!dead) {
            laserCollider.GetComponent<AudioSource>().Play();
        }
    
        dead = true;
    animator.SetBool("dead", true);
        forwardMovementSpeed = 0;
    }
    void CollectCoin(Collider2D coinCollider)
    {
        coins++;

        Destroy(coinCollider.gameObject);
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
    }
    void AdjustFootstepsAndJetpackSound(bool jetpackActive)
    {
        footstepsAudio.enabled = !dead && grounded;

        jetpackAudio.enabled = !dead && !grounded;
        jetpackAudio.volume = jetpackActive ? 1.0f : 0.5f;
    }
}
