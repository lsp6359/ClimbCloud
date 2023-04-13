using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // LoadScene을 사용하는 데 필요하다.

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
        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && this.rigid2D.velocity.y == 0)
        {
            Jump();   
        }

        // 좌우
        int key = 0;
        if (Input.GetKey(KeyCode.LeftArrow))//Input.acceleration.x > this.threshold)
        {
            key = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))//Input.acceleration.x < this.threshold)
        {
            key = 1;
        }

        // 플레이어의 속도
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        // 스피드 제한
        if(speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }

        // 움직이는 방향에 따라 반전
        if(key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        // 플레이어 속도에 맞춰 애니메이션 속도를 바꾼다.
        if (this.rigid2D.velocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;
        }

        //***************************************
        // 캐릭이 멈췄을 때 정지모션
        if (this.rigid2D.velocity.x == 0 && this.rigid2D.velocity.y == 0)
        {
            // walk 애니메이션을 반복시 중간에 끊고 0프레임부터 다시 시작한다.
            animator.Play("Walk", -1, 0);
        }
        //***************************************


        // 플레이어가 화면 밖으로 나갔다면 처음부터
        if (transform.position.y < -10)
        {
            SceneManager.LoadScene("GameScene");
        }

    }

    // 깃발에 도착
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("골");
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
