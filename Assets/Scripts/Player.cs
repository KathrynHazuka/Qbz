using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //Components
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private AudioSource _audioSource;

    //GameManager values
    private GameManager manager;
    public bool usesManager = false;

    //Lazer related
    public int inLazerCount = 0;
    private float timeToDeath;
    private float colorSmooth = 5f;

    private Color playerColor;
    private Color heatColor = new Color(238f / 255, 109f / 255, 109f / 255, 0);

    //Movement variables
    public float maxSpeed;
    public float movementSpeed;
    public float friction;
    public bool canMove = true;
    public bool grabbing = false;
    public Transform grabbingTransform;

    //Collision testing raycast hits
    private RaycastHit hitUp;
    private RaycastHit hitDown;
    private RaycastHit hitRight;
    private RaycastHit hitLeft;

    //Particle prefabs
    [SerializeField]
    private GameObject deathParti;
    [SerializeField]
    private GameObject coinParti;

    //Set initial spawn position 
    private Transform spawn;

    //Audio
    [SerializeField]
    private AudioClip[] audioClip;

    void Awake()
    {
        //Set components
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        playerColor = _renderer.material.color;

        //If scene uses GameManager, set manager to GameManager component
        GameManager fetchedGameManager = FindObjectOfType<GameManager>();
        if (fetchedGameManager != null)
            manager = fetchedGameManager;

        spawn = GameObject.FindGameObjectWithTag("Spawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Lazer();
        FallingOffCheck();
    }

    //FixedUpdate called on fixed framerate
    void FixedUpdate()
    {
        if (canMove)
        {
            PlayerMovement();
            MovementFriction();
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    //Runs when intersecting a lazer
    void Lazer()
    {
        if (inLazerCount > 0)
        {
            //Imitate player heating up
            _renderer.material.color = Color.Lerp(_renderer.material.color, heatColor, Time.deltaTime * colorSmooth);
            //Increment death timer
            timeToDeath += Time.deltaTime;
            LazerDeathCounter();
        }
        else
        {
            //Imitate player cooling down if player is heated up
            if (_renderer.material.color != playerColor)
            {
                _renderer.material.color = Color.Lerp(_renderer.material.color, playerColor, Time.deltaTime * colorSmooth);
            }

            //If timer is above 0, decrease timer by seconds
            if (timeToDeath > 0)
            {
                timeToDeath -= Time.deltaTime;
            }
        }
    }

    //Kills player and resets lazer values / player color
    void LazerDeathCounter()
    {
        if (timeToDeath >= 0.5f)
        {
            Die();
            _renderer.material.color = playerColor;
            timeToDeath = 0;
        }
    }

    void FallingOffCheck()
    {
        //Check if Player is falling off the map
        if (transform.position.y < 0.2)
            Die();
    }

    void PlayerMovement()
    {
        if (_rigidbody.velocity.magnitude < maxSpeed)
        {
            Vector3 inputKeys = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            _rigidbody.AddForce(inputKeys * movementSpeed);
        }
    }

    //Adds friction to the character (created to avoid using rigidbody drag)
    void MovementFriction()
    {
        if (_rigidbody.velocity.magnitude > 0)
            _rigidbody.AddForce(-(_rigidbody.velocity.x * friction), -(_rigidbody.velocity.y * friction), -(_rigidbody.velocity.z * friction));
    }

    void OnCollisionEnter(Collision other)
    {
        //If hitting an enemy, kill player
        if (other.transform.tag == "Enemy")
        {
            Die();
        }
    }

    //set clip to play and play it
    void PlaySound(int clip)
    {
        _audioSource.clip = audioClip[clip];
        _audioSource.Play();
    }

    //Kill player, spawn death particle, and call respawn player method
    public void Die()
    {
        if (grabbing)
        {
            Destroy(grabbingTransform.GetComponent<FixedJoint>());
        }
        PlaySound(2);
        Instantiate(deathParti, transform.position, deathParti.transform.rotation);
        StartCoroutine(RespawnPlayer(1f));
    }

    //Respawn player after 
    IEnumerator RespawnPlayer(float respawnTimer)
    {
        //Move player out of visible scene
        transform.position = new Vector3(0, 100, 0);
        //Freeze player
        canMove = false;
        //Wait for respawn time
        yield return new WaitForSeconds(respawnTimer);
        //Respawn player in spawn position
        transform.position = spawn.position;
        //Unfreze player
        canMove = true;
    }

    void OnTriggerEnter(Collider other)
    {
        //If player reaches the end: finish level, stop time, and call game manager
        if (other.transform.tag == "End")
        {
            if (usesManager)
            {
                PlaySound(1);
                manager.CompleteLevel();
            }
        }

        //If player touches an enemy trigger, kill player
        if (other.transform.tag == "Enemy")
        {
            Die();
        }

        //If player picks up a coin, play sound, add to coin count, and destroy the object.
        if (other.transform.tag == "Coin")
        {
            if (usesManager)
            {
                PlaySound(0);
                manager.coinCount += 1;
                Instantiate(coinParti, other.transform.position, coinParti.transform.rotation);
                Destroy(other.gameObject);
            }
        }
    }
}
