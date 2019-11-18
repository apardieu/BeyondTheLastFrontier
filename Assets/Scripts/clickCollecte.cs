using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class clickCollecte : MonoBehaviour
{

  private float timeButtonHeld = 0;
  private int pointsDeVie;
  public int pointsDeVieMax;
  private bool clicking = false;
  public string ressource;
    public bool collectable;

    public GameObject health;
  private GameObject healthUsed;

  private GameObject jacob;
  private Vector3 myPosition;
  private float distance = 0;
  private bool lake = false;

  private audioPlayer audioPlayer;


  // Start is called before the first frame update
  void Start()
  {
    jacob = GameObject.FindWithTag("Player");

    if (gameObject.GetComponent<audioPlayer>() != null)
    {
      audioPlayer = gameObject.GetComponent<audioPlayer>();
      if (audioPlayer != null)
        audioPlayer.setPitch(1.0f);
    }


    pointsDeVie = pointsDeVieMax;
    myPosition = new Vector3(0, 0, 0);
    if (this.tag == "Lakes")
    {
      lake = true;
    }
  }

  void OnGUI()
  {
    if (healthUsed != null)
    {
      healthUsed.transform.position = (myPosition);
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (lake)
    {
      myPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    else
    {
      myPosition = transform.position;
    }
    myPosition.z = 0.5f;
    distance = Mathf.Sqrt(Mathf.Pow((jacob.transform.position.x - myPosition.x), 2) + Mathf.Pow((jacob.transform.position.y - myPosition.y), 2));

    if (clicking && collectable)
    {


      if (healthUsed == null)
      {
        healthUsed = Instantiate(health, new Vector3(0, 0, 0), Quaternion.identity);
        healthUsed.GetComponent<HealthBar>().setHpMax(pointsDeVieMax);
        healthUsed.transform.parent = GameObject.Find("CanvasHealthBar").transform;
        healthUsed.transform.position = (myPosition);
      }
      timeButtonHeld += Time.deltaTime;
      if (timeButtonHeld > 0.3)
      {
        pointsDeVie -= 1;
        timeButtonHeld = 0;
        healthUsed.GetComponent<HealthBar>().TakeDamage(1);


        if (this.tag != "Lakes")
        {
          audioPlayer.setPitch(2.0f);
          audioPlayer.playSound(0, false, 0.3f);
        }


        if (pointsDeVie <= 0)
        {

          clicking = false;
          GameManager.setIsCollecting(false);
          foreach (Ressource r in GameManager.items)
          {
            if (r.name == ressource)
            {

              r.quantite++;
              if (r.quantite < 0)
                r.quantite = 0;
            }

          }
          for (int i = 0; i < GameManager.tabBag.Count; i++)
          {
            if (GameManager.tabBag[i].name == ressource)
            {
              GameObject tmp = GameObject.Find("ButtonHUDMiddle" + (i + 1).ToString());
              tmp.GetComponentInChildren<Text>().text = GameManager.tabBag[i].quantite.ToString();
            }
          }
          if (this.tag != "Lakes")
          {

            Destroy(healthUsed);
            Destroy(gameObject);
            return;
          }
          else
          {
            pointsDeVie = pointsDeVieMax;
            healthUsed.GetComponent<HealthBar>().HealMax();
            audioPlayer.playSound(0, false, 0.3f);

          }
        }





      }

    }
  }

  void OnMouseUp()
  {
        if (collectable)
        {
            clicking = false;
            GameManager.setIsCollecting(false);
            timeButtonHeld = 0;
            if (lake)
            {
                Destroy(healthUsed);
                pointsDeVie = pointsDeVieMax;
            }
        }
    }

  void OnMouseOver()
  {
        if (collectable)
        {
            if (!GameManager.inCabane)
            {
                if (distance < 2.5 && Input.GetMouseButton(0))
                {
                    clicking = true;
                    GameManager.setIsCollecting(true);
                }
                else
                {
                    clicking = false;
                    GameManager.setIsCollecting(false);
                    timeButtonHeld = 0;
                }
                if (!lake)
                {
                    if (distance < 2.5)
                    {
                        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    else
                    {
                        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                    }
                }
                else if (distance > 2.5)
                {
                    Destroy(healthUsed);
                    pointsDeVie = pointsDeVieMax;
                }
            }
        }
    

  }

  // ...and the mesh finally turns white when the mouse moves away.
  void OnMouseExit()
  {
    if (!GameManager.inCabane && collectable)
    {
      clicking = false;
      GameManager.setIsCollecting(false);
      timeButtonHeld = 0;
      if (!lake)
      {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
      }
      else
      {
        Destroy(healthUsed);
        pointsDeVie = pointsDeVieMax;
      }
    }
    
  }

  void OnDestroy()
  {

    if (GameObject.Find(gameObject.name) != null)
      AudioSource.PlayClipAtPoint(audioPlayer.getClip(1), jacob.transform.position, 1f);

  }
}
