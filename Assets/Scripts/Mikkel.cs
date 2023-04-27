using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mikkel : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource runSound;



    private bool isGround = false;

    public bool isDead = false;
    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;


    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    public static Mikkel Instance {get; set;}


    private States State
    {
        get {return (States)anim.GetInteger("state");}
        set {anim.SetInteger("state", (int)value);}
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Instance = this;
        isRecharged = true;


    }
    
   

    private void Update() 
    {
        if (isDead) {
            // Chill
        } else {
            if(isGround && !isAttacking) State = States.idle;

            if (!isAttacking && Input.GetButton("Horizontal"))
                Run();

            if (!isAttacking && isGround && Input.GetButton("Jump"))
                Jump();

            if (Input.GetButtonDown("Fire1"))
                Attack();
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    public void Attack()
    {
        if (isGround && isRecharged)
        {
            State = States.attack;
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    public void OnAttack()
    {   
        Collider2D[] collider = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
        Debug.Log("colider length: " + collider.Length);

        for (int i = 0; i < collider.Length; i++)
        {
            collider[i].GetComponent<MonstersBehavior>().GetDamage();
        }
        attackSound.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private void Run()
{

   if (isGround) State = States.run;

    Vector3 dir = transform.right * Input.GetAxis("Horizontal");
    transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

    // Flip game object based on input direction
    if (dir.x < 0.0f) {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    
    else if (dir.x > 0.0f) {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }


}

 private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGround = collider.Length > 1;

        if (!isGround && !isDead) State = States.jump;
    }

    public void GetDamage()
    {
        lives -= 1;
       // Debug.Log(lives);
    }

  private void Jump()
{
    if (isGround && Input.GetButtonDown("Jump")) // Only apply force when jump button is initially pressed down
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    if (!isGround && Input.GetButtonUp("Jump")) // Stop applying force when jump button is released
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    jumpSound.Play();
}

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.55f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("spike"))
        {   
            Debug.Log("Collision made");
            Die();
        }
    }

    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.55f);
        isAttacking = false;
        StartCoroutine(AttackCoolDown());
    }

    private void Die()
    {   
        Debug.Log(" player is dead");
        State = (States)(-1);

        isDead = true;
        anim.SetTrigger("death");
        deathSound.Play();
        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        Debug.Log("Restarting the scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Destroy(this);
    }
}

public enum States
{
    idle,
    run,
    jump,
    attack
}
