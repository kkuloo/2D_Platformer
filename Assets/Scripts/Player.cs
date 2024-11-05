using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 7;
    public float jumpspeed = 15;
    public Collider2D bottomCollider;
    public CompositeCollider2D terrainCollider;
    public GameObject bulletPrefab;

    float vx = 0;
    float prevVX = 0;
    bool isGrounded;
    Vector2 originPosition;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position;
    }

    public void Restart()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<BoxCollider2D>().enabled = true;

        transform.eulerAngles = Vector3.zero;
        transform.position = originPosition;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        vx = Input.GetAxisRaw("Horizontal");

        if (vx<0)             // 이동방향으로 전환_else를 쓰지 않는 이유
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (vx>0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (bottomCollider.IsTouching(terrainCollider))
        {
            if (!isGrounded)   // 착지
            {
                if (vx == 0)
                {
                    GetComponent<Animator>().SetTrigger("Idle");
                }
                else
                {
                    GetComponent<Animator>().SetTrigger("Run");
                }
            }
            else                // 계속 걷는 중
            {
                if (prevVX != vx)
                {
                    if (vx == 0)
                    {
                        GetComponent<Animator>().SetTrigger("Idle");
                    }
                    else
                    {
                        GetComponent<Animator>().SetTrigger("Run");
                    }
                }

            }
            isGrounded = true;
        }
        else
        {
            if (isGrounded)       // 점프 시작
            {
                GetComponent<Animator>().SetTrigger("Jump");
            }
                isGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpspeed;        
        }
        prevVX = vx;

        
        if (Input.GetButtonDown("Fire1"))        // 총알 발사
        {
            Vector2 bulletV = Vector2.zero;

            if (GetComponent<SpriteRenderer>().flipX)
            {
                bulletV = new Vector2(-10, 0);
            }
            else
            {
                bulletV = new Vector2(10, 0);
            }

            GameObject bullet = ObjectPool.Instance.GetBullet();
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().velocity = bulletV;
        }

    }

    private void FixedUpdate() 
    {
        transform.Translate(Vector2.right * vx * speed * Time.fixedDeltaTime);    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().angularVelocity = 720;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10),ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;

        GameManager.Instance.Die();
    }
}
