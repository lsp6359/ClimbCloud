using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreType // ���� ������ ����Ʈ �ο� enum
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

    // ����
    public static int iPoint = 0;

    // enum ����
    public ScoreType itemType;

    
    // Start is called before the first frame update
    void Start()
    {
        GameDirector.iPoint = 0;

        // ���� �̹��� �ε�
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
        // Score �±׸� ���� ������Ʈ���� ã�´�.
        scoreObjects = GameObject.FindGameObjectsWithTag("Score");

        // �迭�� ������ ���� �ݺ��ؼ� ��������Ʈ�� �ٲ��ִ� ����
        foreach (GameObject scoreObject in scoreObjects)
        {
            spriteRenderer = scoreObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // ���� �̹��� �� �������� �ϳ� ����                  // 0, ScoreType.Length�� ���� �ʴ� ������ ScoreType ������ ��ü���� Length��� �Ӽ��� �������� �ʴ´�.
                ScoreType randomScoreType = (ScoreType)Random.Range(0, ScoreType.GetValues(typeof(ScoreType)).Length);
                                            
                // ���õ� ���� �̹��� ��������
                Sprite sprite = fruitSprites[(int)randomScoreType];

                // SpriteRenderer�� Sprite �Ӽ��� �����մϴ�.
                spriteRenderer.sprite = sprite;
            }
        }
    }

    public void PointIncrease(ScoreType itemType)
    {
        int itemScore = (int)itemType;
        
        //Debug.Log("������ : " + itemType);
        // iPoint�� ScoreType�� �ش��ϴ� ���� ������
        iPoint += itemScore;
        if (itemType.ToString() == "fish")
        {
            Debug.Log("���� ���� : " + iPoint);
        }
        else
        {
            Debug.Log("���� ���� : " + iPoint);
        }
    }
}