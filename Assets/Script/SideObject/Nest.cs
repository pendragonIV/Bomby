using System.Collections;
using UnityEngine;

public class Nest : MonoBehaviour
{
    [SerializeField]
    private GameObject circleBorder;
    private void Update()
    {
        circleBorder.transform.Rotate(0, 0, 10 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<Rigidbody2D>().velocity.magnitude < 10f && !GameManager.instance.isGameLose() && !GameManager.instance.IsGameWin())
            {
                collision.transform.parent = transform;
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.transform.position = new Vector3(transform.position.x, transform.position.y + .5f);
                collision.transform.rotation = new Quaternion(0, 0, 0, 0);
                collision.GetComponent<Rigidbody2D>().freezeRotation = true;
                collision.GetComponent<Animator>().Play("Idle");
                StartCoroutine(WaitToWin());
            }

        }
    }
    private IEnumerator WaitToWin()
    {
        yield return new WaitForSecondsRealtime(.5f);
        GameManager.instance.Win();
    }
}
