using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class clickCollecte : MonoBehaviour
{

  private float timeButtonHeld = 0;
  private int pointsDeVie = 10;
  private bool clicking = false;

  public GameObject health;
  private GameObject healthUsed;

  private GameObject jacob;


  // Start is called before the first frame update
  void Start()
  {
    jacob = GameObject.FindWithTag("Player");
  }

  void OnGUI()
  {
    if (healthUsed != null)
    {
      healthUsed.transform.GetChild(0).transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }
  }

  // Update is called once per frame
  void Update()
  {

    if (clicking)
    {

      float distance = Mathf.Sqrt(Mathf.Pow((jacob.transform.position.x - transform.position.x), 2) + Mathf.Pow((jacob.transform.position.y - transform.position.y), 2));
      if (distance < 2.5)
      {
        if (healthUsed == null)
        {
          healthUsed = Instantiate(health, new Vector3(0, 0, 0), Quaternion.identity);
          healthUsed.transform.GetChild(0).transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
        timeButtonHeld += Time.deltaTime;
        if (timeButtonHeld > 0.3)
        {
          pointsDeVie -= 1;
          timeButtonHeld = 0;
          healthUsed.transform.GetChild(0).GetComponent<HealthBar>().TakeDamage(1);
          Debug.Log(pointsDeVie);
          if (pointsDeVie <= 0)
          {
            Debug.Log("bois récolté");
            foreach (Ressource r in GameManager.items)
            {
              if (r.name == "Bois")
              {
                r.quantite++;
                if (r.quantite < 0)
                  r.quantite = 0;
              }

            }
            for(int i = 0; i < GameManager.tabBag.Count; i++)
            {
              if(GameManager.tabBag[i].name == "Bois")
              {
                GameObject tmp = GameObject.Find("ButtonHUDMiddle" + (i + 1).ToString());
                tmp.GetComponentInChildren<Text>().text = GameManager.tabBag[i].quantite.ToString();
              }
            }
            Destroy(gameObject);
            return;
          }
        }
      }
      else
      {
        Debug.Log("Trop loin");
      }
    }
  }

  void OnMouseDown()
  {
    clicking = true;
  }

  void OnMouseUp()
  {
    clicking = false;
    timeButtonHeld = 0;
  }
}
