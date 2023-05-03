using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // LoadScene�� ����ϴ� �� �ʿ��ϴ�.

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rigid2D;
    Animator animator;
    float fJumpForce = 680.0f;
    float fWalkForce = 30.0f;
    float maxWalkSpeed = 2.0f;
    //float threshold = 0.1f;

    GameObject gGameDirector;

    Vector3 pos = Vector3.zero;
  

    // Start is called before the first frame update
    void Start()
    {
        
        gGameDirector = GameObject.Find("GameDirector");

        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // ī�޶� ������ �������� �ϴ� �Լ�
        NoOutside();

        // ����
        if (Input.GetKeyDown(KeyCode.Space) && this.rigid2D.velocity.y == 0)
        {
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.Space))
            {
                GetComponent<CircleCollider2D>().isTrigger = true;
                GetComponent<BoxCollider2D>().isTrigger = true;
            }
            else
            {
                Jump();
            }
        }

        // �����Ӱ���
        Move();

        AnimationStop();
    }


    // �̵� ����, �ִϸ��̼��� �÷��̾� �ӵ��� ���߱� �Լ�
    void Move()
    {

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
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.fWalkForce);
        }

        // �����̴� ���⿡ ���� ����
        if (key != 0)
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

        

    }

    // ����̰� �浹���� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �±װ� Finish(���)�� ������Ʈ�� �������
        if (collision.CompareTag("Finish"))
        {
            Debug.Log("��");
           
            SceneManager.LoadScene("ClrearScene");
        }

        // �±װ� Score(������)�� �Ͱ� ������Ʈ�� ���̾��̸��� Ignore Raycast�� �ƴѰͿ� �浹���� ��
        if (collision.CompareTag("Score") && collision.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            // ��������Ʈ�� �浹�� ������Ʈ ��������
            Sprite sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;

            // �� ��������Ʈ�� �̸� ��������(�浹�� ������Ʈ)
            string spriteName = sprite.name;
            //Debug.Log("��������Ʈ �̸�: " + spriteName);

            // Enum���� ������Ʈ�� �̸��� �������� ���� ��������
            ScoreType scoreType = (ScoreType)System.Enum.Parse(typeof(ScoreType), spriteName);

            // ���� ������Ű�� �Լ�
            gGameDirector.GetComponent<GameDirector>().PointIncrease(scoreType);
           
            // �浹�� ������Ʈ�� ���̾� ����ũ�� Ignore Raycast�� �ٲٱ�
            collision.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            //Destroy(collision.gameObject);

            // �浹�� ������Ʈ ��Ȱ��ȭ
            collision.gameObject.SetActive(false);



        }

        // �����̶�� �±׸� �����͵��̶� �浹���� ��,
        if (collision.CompareTag("Cloud"))
        {
            // ��ü, ��ü �κ� Collider�� isTrigger���� �ٲ۴�.
            GetComponent<CircleCollider2D>().isTrigger = true;
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

    }


    // �浹���� ���������� ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        // �����̶�� �±׸� �����͵��̶� �浹���� ���������� ��
        if (collision.CompareTag("Cloud"))
        {
            // ��ü, ��ü �κ� Collider�� isTrigger��带 ����.
            GetComponent<CircleCollider2D>().isTrigger = false;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }

    }


    

    // �����ϴ� �Լ�
    void Jump()
    {
        this.animator.SetTrigger("JumpTrigger");
        this.rigid2D.AddForce(transform.up * this.fJumpForce);
    }

    // ī�޶� ������ �������� �ϴ� �Լ��� ȭ�� ������ ������ �ٽ� ����
    void NoOutside()
    {
        // ������Ʈ�� ���� ��ǥ�� ī�޶� ����Ʈ ��ǥ��� ��ȯ
        pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0.1f)
        {
            pos.x = 0.1f;
        }

        if (pos.x > 0.9f)
        {
            pos.x = 0.9f;
        }
        // ����Ʈ ��ǥ�踦 �ٽ� ������ǥ�� ��ȯ��Ű�� ������Ʈ ��ġ ��ȯ
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        // �÷��̾ ȭ�� ������ �����ٸ� ó������
        if (transform.position.y < -10)
        {
            GameDirector.iPoint = 0;
            SceneManager.LoadScene("GameScene");
        }
    }

    // �ݸ� �ɾ��� �� �ִϸ��̼��� ���߿� ���ߴ� ���� ���ִ� �Լ�
    void AnimationStop()
    {
        //***************************************
        // ĳ���� ������ �� �������
        if (this.rigid2D.velocity.x == 0 && this.rigid2D.velocity.y == 0)
        {
            // Play("�̸�", ���Ÿ��, ���̾�)
            animator.Play("Walk", -1, 0);
        }
        //***************************************

    }
}
