using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // LoadScene�� ����ϴ� �� �ʿ��ϴ�.
using UnityEngine.UI;

public class ClearDirector : MonoBehaviour
{
    GameObject gScore = null;
    //GameObject gGameDirector = null;
    // Start is called before the first frame update
    void Start()
    {
        
        this.gScore = GameObject.Find("Score");
        //this.gGameDirector = GameObject.Find("GameDirector");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("GameScene");
            //GameDirector.iPoint = 0;
        }

        gScore.GetComponent<Text>().text = "����� ���� : " + GameDirector.iPoint;
    }
}
