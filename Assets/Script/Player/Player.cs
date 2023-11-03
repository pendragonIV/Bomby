using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Vector3 mousePos;
    [SerializeField]
    private Vector3 defaultPos;
    [SerializeField]
    private float forceMultiplier = 250f;

    [SerializeField]
    private GameObject slingShoot;
    [SerializeField]
    private Sprite normalImg;
    [SerializeField]
    private Sprite pullImg;
    [SerializeField]
    private LineRenderer rubberBand;
    [SerializeField]
    private LineRenderer rubberBandBehind;
    [SerializeField]
    private GameObject supportPlate;
    private bool isShoot = false;
    private bool isGrounded = false;
    private bool isPull = false;

    Vector2 dir;
    Vector3 pos;

    private void Start()
    {
        rubberBand.enabled = false;
        rubberBandBehind.enabled = false;
        supportPlate.SetActive(false);
        animator.Play("Idle");
    }

    private void Update()
    {
        if (transform.position.x > slingShoot.transform.position.x)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }

        if (transform.position.y < -4f && !GameManager.instance.isGameLose() && !GameManager.instance.IsGameWin())
        {
            GameManager.instance.Lose();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            if (!isGrounded && this.rb.velocity.magnitude <= .5f && isShoot && !GameManager.instance.isGameLose() && !GameManager.instance.IsGameWin())
            {
                isGrounded = true;
                StartCoroutine(WaitToLose());
            }
        }
    }

    private IEnumerator WaitToLose()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.Lose();
    }

    private void OnMouseDown()
    {
        if (!isShoot)
        {
            rb.freezeRotation = true;

            slingShoot.GetComponent<SpriteRenderer>().sprite = pullImg;
            animator.Play("Aim");
            isPull = true;
        }
    }

    private void OnMouseDrag()
    {
        if (!isShoot)
        {
            rubberBand.enabled = true;
            rubberBandBehind.enabled = true;
            supportPlate.SetActive(true);
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            if (Vector2.Distance(mousePos, slingShoot.transform.position) > 6f)
            {
                var direction = mousePos - slingShoot.transform.position;
                direction = Vector2.ClampMagnitude(direction, 6f);
                pos = slingShoot.transform.position + direction;
            }
            else
            {
                pos = mousePos;
            }
            SetbandPos();
        }
    }

    private void FixedUpdate()
    {
        if (isPull)
        {
            rb.MovePosition(pos);

        }
    }

    private void SetbandPos()
    {
        if (transform.position.x > slingShoot.transform.position.x)
        {
            rubberBand.SetPosition(0, new Vector3(transform.position.x + .25f, transform.position.y - .25f));
            rubberBand.SetPosition(1, new Vector3(slingShoot.transform.position.x + .3f, slingShoot.transform.position.y + .35f));

            rubberBandBehind.SetPosition(0, new Vector3(transform.position.x + .25f, transform.position.y - .25f));
            rubberBandBehind.SetPosition(1, new Vector3(slingShoot.transform.position.x - .1f, slingShoot.transform.position.y + .35f));
        }
        else
        {
            rubberBand.SetPosition(0, new Vector3(transform.position.x - .25f, transform.position.y - .25f));
            rubberBand.SetPosition(1, new Vector3(slingShoot.transform.position.x + .3f, slingShoot.transform.position.y + .35f));

            rubberBandBehind.SetPosition(0, new Vector3(transform.position.x - .25f, transform.position.y - .25f));
            rubberBandBehind.SetPosition(1, new Vector3(slingShoot.transform.position.x - .1f, slingShoot.transform.position.y + .35f));
        }
    }

    private void OnMouseUp()
    {
        if (!isShoot)
        {
            rb.velocity = Vector2.zero;
            Vector2 dir = -(transform.position - slingShoot.transform.position);

            rb.AddForce(dir * forceMultiplier);
            rb.freezeRotation = false;

            slingShoot.GetComponent<SpriteRenderer>().sprite = normalImg;
            rubberBand.enabled = false;
            rubberBandBehind.enabled = false;
            supportPlate.SetActive(false);
            isShoot = true;
            animator.Play("Flying");
            isPull = false;
        }
    }
}
