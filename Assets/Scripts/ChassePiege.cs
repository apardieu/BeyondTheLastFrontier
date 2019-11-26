using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChassePiege : MonoBehaviour
{
    private int randomAnimal;
    private double limiteDeCapture;
    private bool active;
    public Sprite close;
    private clickCollecte lienScript;

    void Start()
    {
        active = true;
        lienScript = GetComponent<clickCollecte>();
    }

    void Update()
    {
        if (active)
        {
            randomAnimal = Random.Range(0, 100);
            limiteDeCapture = (Time.time/10000) * GameManager.coefWeather;
            if (randomAnimal < limiteDeCapture)
            {
                Debug.Log("capture!!!!!!");
                this.GetComponent<SpriteRenderer>().sprite = close;
                active = false;
                float typeAnimal= Random.Range(0, 100);
                if (typeAnimal >= 0 && typeAnimal <= 35)
                {
                    Debug.Log("C'est une moufette");
                    lienScript.nbRessources[0] = 2;
                    lienScript.nbRessources[1] = 1;
                }
                else if (typeAnimal <= 50)
                {
                    Debug.Log("C'est un carcajou");
                    lienScript.nbRessources[0] = 2;
                    lienScript.nbRessources[1] = 4;
                }
                else if (typeAnimal <= 58)
                {
                    Debug.Log("C'est un grizzli");
                    lienScript.nbRessources[0] = 10;
                    lienScript.nbRessources[1] = 8;
                }
                else if (typeAnimal <= 66)
                {
                    Debug.Log("C'est un lynx");
                    lienScript.nbRessources[0] = 5;
                    lienScript.nbRessources[1] = 6;
                }
                else
                {
                    Debug.Log("C'est un rat musqué");
                    lienScript.nbRessources[0] = 1;
                    lienScript.nbRessources[1] = 1;
                }
                lienScript.collectable = true;
            }
        }
    }

}
