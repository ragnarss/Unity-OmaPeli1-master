using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    private Animator myAnimator;

    [SerializeField]
    private Transform TuliPalloPos;

    [SerializeField]
    private float movementSpeed;

    
    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private GameObject tuliPalloPrefab;

    public Rigidbody2D MyRigidbody { get; set; }


public bool Attack { get; set; }

    public bool Slide { get; set; }

    public bool Jump { get; set; }

    public bool OnGround { get; set; }


    // Use this for initialization
    void Start () {
        facingRight = true;
        MyRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

    }

    void Update()
    {
        HandleInput();
    }

	
	// Update is called once per frame
	void FixedUpdate () {

        float horizontal = Input.GetAxis("Horizontal");

        OnGround = IsGrounded();

        HandleMovement(horizontal);

        Flip(horizontal);

        HandleLayers();

       
    

	}

    //Liikkuminen
private void HandleMovement(float horizontal)
    {

        if (MyRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }
        if(!Attack && !Slide && (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }

        if(Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    
    //Näppäin bindit
    private void HandleInput()
    {


        if(Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetTrigger("jump");
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            myAnimator.SetTrigger("attack");
        }
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            myAnimator.SetTrigger("slide");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            myAnimator.SetTrigger("throw");
            
        }
    }
    //Kääntää hahmon
    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }
    
  


private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y<=0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);


                for(int i=0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject!=gameObject)
                    {
                       
                        return true;
                    }
                }

            }
            
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
   
    public void ThrowBall(int value)
    {

        if(!OnGround && value ==1 || OnGround && value == 0)
        {

        }

        if (facingRight)
        {
           GameObject tmp=(GameObject) Instantiate(tuliPalloPrefab, TuliPalloPos.position, Quaternion.Euler(new Vector3(0,0,0)));
            tmp.GetComponent<TuliPallo>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(tuliPalloPrefab, TuliPalloPos.position, Quaternion.Euler(new Vector3(0, 0, -180)));
            tmp.GetComponent<TuliPallo>().Initialize(Vector2.left);
        }
    }
}
