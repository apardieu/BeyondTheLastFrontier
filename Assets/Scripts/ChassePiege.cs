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
            limiteDeCapture = Time.time * GameManager.coefWeather/100;
            if (randomAnimal < limiteDeCapture)
            {
                Debug.Log("capture!!!!!!");
                this.GetComponent<SpriteRenderer>().sprite = close;
                active = false;
                float typeAnimal= Random.Range(0, 100);
                if (typeAnimal >= 0 && typeAnimal <= 40)
                {
                    Debug.Log("C'est une moufette");
                }else if (typeAnimal <= 50)
                {
                    Debug.Log("C'est un carcajou");
                }else if (typeAnimal <= 65)
                {
                    Debug.Log("C'est un grizzli");
                }else if (typeAnimal <= 85)
                {
                    Debug.Log("C'est un lynx");
                }else
                {
                    Debug.Log("C'est un rat musqué");
                }
                lienScript.collectable = true;
            }
        }
    }

}
