using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // LoadScene�� ����ϴ� �� �ʿ��ϴ�.

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 680.0f;
    float walkForce = 20.0f;
    float maxWalkSpeed = 2.0f;
    float threshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // ����
        if (Input.GetKeyDown(KeyCode.Space) && this.rigid2D.velocity.y == 0)
        {
            Jump();   
        }

        // �¿�
        int key = 0;
        if (Input.GetKey(KeyCode.LeftArrow))//Input.acceleration.x > this.threshold)
        {
            key = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))//Input.acceleration.x < this.threshold)
        {
            key = 1;
        }

        // �÷��̾��� �ӵ�
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        // ���ǵ� ����
        if(speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }

        // �����̴� ���⿡ ���� ����
        if(key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        // �÷��̾� �ӵ��� ���� �ִϸ��̼� �ӵ��� �ٲ۴�.
        if (this.rigid2D.velocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;
        }

        //***************************************
        // ĳ���� ������ �� �������
        if (this.rigid2D.velocity.x == 0 && this.rigid2D.velocity.y == 0)
        {
            // walk �ִϸ��̼��� �ݺ��� �߰��� ���� 0�����Ӻ��� �ٽ� �����Ѵ�.
            animator.Play("Walk", -1, 0);
        }
        //***************************************


        // �÷��̾ ȭ�� ������ �����ٸ� ó������
        if (transform.position.y < -10)
        {
            SceneManager.LoadScene("GameScene");
        }

    }

    // ��߿� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("��");
        SceneManager.LoadScene("ClrearScene");
    }

    private void ResetAnimation()
    {
        animator.enabled = false;
   
        animator.enabled = true;
    }

    void Jump()
    {
        this.animator.SetTrigger("JumpTrigger");
        this.rigid2D.AddForce(transform.up * this.jumpForce);
    }
}
