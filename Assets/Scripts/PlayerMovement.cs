using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public  Button nextButton;
    public GameObject movCanv;
    public GameObject[] camPositions;
    public GameObject currentScene;
    public GameObject playerCharacter;
    
    public bool able;
    // Start is called before the first frame update
    void Start()
    {
        able = false;
        movCanv = GameObject.FindGameObjectWithTag("Movement");
        currentScene = GameObject.FindGameObjectWithTag("FirstScene");
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        movCanv.SetActive(false);
        nextButton.onClick.AddListener(MovePlayer);

      
    }

   

    // Update is called once per frame
    void Update()
    {
        if (able)
        {
            movCanv.SetActive(true);
        }
        else
        {
            movCanv.SetActive(false);
        }

        

        
    }


    void MovePlayer()
    {
        for (int i = 0; i < camPositions.Length-1; i++)
        {
            if (camPositions[i].transform.position == currentScene.transform.position)
            {
                playerCharacter.transform.position = new Vector3(camPositions[i + 1].transform.position.x,
                     camPositions[i + 1].transform.position.y-50, 0);
                currentScene = camPositions[i + 1];
                break;

            }
        }
    }

    public void ContinueArrow()
    {
        able = !able;
    }
}
