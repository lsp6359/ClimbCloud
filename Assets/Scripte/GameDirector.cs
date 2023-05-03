using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreType // 과일 각각에 포인트 부여 enum
{
    bobs = 10,
    cocount = 20,
    orange = 30,
    fish = -10
}

public class GameDirector : MonoBehaviour
{
    GameObject[] scoreObjects;
    SpriteRenderer spriteRenderer;
    Sprite[] fruitSprites;

    // 점수
    public static int iPoint = 0;

    // enum 선언
    public ScoreType itemType;

    
    // Start is called before the first frame update
    void Start()
    {
        GameDirector.iPoint = 0;

        // 과일 이미지 로드
        fruitSprites = new Sprite[4];
        fruitSprites[0] = Resources.Load<Sprite>("bobs");
        fruitSprites[1] = Resources.Load<Sprite>("cocount");
        fruitSprites[2] = Resources.Load<Sprite>("orange");
        fruitSprites[3] = Resources.Load<Sprite>("fish");

        SpawnScore();

    }

    // Update is called once per frame
    void Update()
    {

       

    }

    public void SpawnScore()
    {
        // Score 태그를 가진 오브젝트들을 찾는다.
        scoreObjects = GameObject.FindGameObjectsWithTag("Score");

        // 배열이 끝날때 까지 반복해서 스프라이트를 바꿔주는 형식
        foreach (GameObject scoreObject in scoreObjects)
        {
            spriteRenderer = scoreObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // 과일 이미지 중 랜덤으로 하나 선택                  // 0, ScoreType.Length가 되지 않는 이유는 ScoreType 열거형 자체에는 Length라는 속성이 존재하지 않는다.
                ScoreType randomScoreType = (ScoreType)Random.Range(0, ScoreType.GetValues(typeof(ScoreType)).Length);
                                            
                // 선택된 과일 이미지 가져오기
                Sprite sprite = fruitSprites[(int)randomScoreType];

                // SpriteRenderer의 Sprite 속성을 변경합니다.
                spriteRenderer.sprite = sprite;
            }
        }
    }

    public void PointIncrease(ScoreType itemType)
    {
        int itemScore = (int)itemType;
        
        //Debug.Log("아이템 : " + itemType);
        // iPoint에 ScoreType에 해당하는 값을 더해줌
        iPoint += itemScore;
        if (itemType.ToString() == "fish")
        {
            Debug.Log("점수 감소 : " + iPoint);
        }
        else
        {
            Debug.Log("점수 증가 : " + iPoint);
        }
    }
}