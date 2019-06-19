using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public Button turnChanger;
    // Start is called before the first frame update
    void Start()
    { 
        turnChanger.onClick.AddListener(ChangeTurn);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTurn()
    {
        GameObject.FindObjectOfType<EnemyHolder>().Attack(GameObject.FindObjectOfType<PlayerHolder>());
        Debug.Log("Funciona y no estoy loco");
        //Energia.Regresa();
    }
}
