using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // LoadScene을 사용하는 데 필요하다.

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
        // 카메라 밖으로 못나가게 하는 함수
        NoOutside();

        // 점프
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

        // 움직임관련
        Move();

        AnimationStop();
    }


    // 이동 관련, 애니메이션을 플레이어 속도에 맞추기 함수
    void Move()
    {

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
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.fWalkForce);
        }

        // 움직이는 방향에 따라 반전
        if (key != 0)
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

        

    }

    // 고양이가 충돌했을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 태그가 Finish(깃발)인 오브젝트에 닿았을때
        if (collision.CompareTag("Finish"))
        {
            Debug.Log("골");
           
            SceneManager.LoadScene("ClrearScene");
        }

        // 태그가 Score(아이템)인 것과 오브젝트의 레이어이름이 Ignore Raycast가 아닌것에 충돌했을 때
        if (collision.CompareTag("Score") && collision.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            // 스프라이트에 충돌한 오브젝트 가져오기
            Sprite sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;

            // 그 스프라이트의 이름 가져오기(충돌한 오브젝트)
            string spriteName = sprite.name;
            //Debug.Log("스프라이트 이름: " + spriteName);

            // Enum에서 오브젝트의 이름과 같은것의 점수 가져오기
            ScoreType scoreType = (ScoreType)System.Enum.Parse(typeof(ScoreType), spriteName);

            // 점수 증가시키는 함수
            gGameDirector.GetComponent<GameDirector>().PointIncrease(scoreType);
           
            // 충돌한 오브젝트의 레이어 마스크를 Ignore Raycast로 바꾸기
            collision.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            //Destroy(collision.gameObject);

            // 충돌한 오브젝트 비활성화
            collision.gameObject.SetActive(false);



        }

        // 구름이라는 태그를 가진것들이랑 충돌했을 때,
        if (collision.CompareTag("Cloud"))
        {
            // 상체, 하체 부분 Collider를 isTrigger모드로 바꾼다.
            GetComponent<CircleCollider2D>().isTrigger = true;
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

    }


    // 충돌에서 빠져나왔을 때
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 구름이라는 태그를 가진것들이랑 충돌에서 빠져나왔을 때
        if (collision.CompareTag("Cloud"))
        {
            // 상체, 하체 부분 Collider의 isTrigger모드를 끈다.
            GetComponent<CircleCollider2D>().isTrigger = false;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }

    }


    

    // 점프하는 함수
    void Jump()
    {
        this.animator.SetTrigger("JumpTrigger");
        this.rigid2D.AddForce(transform.up * this.fJumpForce);
    }

    // 카메라 밖으로 못나가게 하는 함수와 화면 밖으로 나가면 다시 시작
    void NoOutside()
    {
        // 오브젝트의 월드 좌표를 카메라 뷰포트 좌표계로 변환
        pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0.1f)
        {
            pos.x = 0.1f;
        }

        if (pos.x > 0.9f)
        {
            pos.x = 0.9f;
        }
        // 뷰포트 좌표계를 다시 월드좌표로 변환시키고 오브젝트 위치 변환
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        // 플레이어가 화면 밖으로 나갔다면 처음부터
        if (transform.position.y < -10)
        {
            GameDirector.iPoint = 0;
            SceneManager.LoadScene("GameScene");
        }
    }

    // 반만 걸었을 때 애니메이션이 도중에 멈추는 현상 없애는 함수
    void AnimationStop()
    {
        //***************************************
        // 캐릭이 멈췄을 때 정지모션
        if (this.rigid2D.velocity.x == 0 && this.rigid2D.velocity.y == 0)
        {
            // Play("이름", 재생타입, 레이어)
            animator.Play("Walk", -1, 0);
        }
        //***************************************

    }
}
