using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cabane : MonoBehaviour
{
  public GameObject canvas;
  public GameObject panelCabaneGris;
  public GameObject panelCabaneMenu;
  public GameObject panelCabaneAction;

  private GameObject jacob;
  private float distance;

  //private int etat; //-1 pour rien, 0 pour rechauffer, 1 pour dormir

  // Start is called before the first frame update
  void Start()
  {
    jacob = GameObject.FindWithTag("Player");
    canvas.transform.localScale = new Vector3(0, 0, 0);
    panelCabaneGris.transform.localScale = new Vector3(0, 0, 0);
    panelCabaneMenu.transform.localScale = new Vector3(0, 0, 0);
    panelCabaneAction.transform.localScale = new Vector3(0, 0, 0);
  }

  public void OnClickEnterCabane()
  {
    GameManager.inCabane = true;
    canvas.transform.localScale = new Vector3(1, 1, 1);
    panelCabaneGris.transform.localScale = new Vector3(1, 1, 1);
    panelCabaneMenu.transform.localScale = new Vector3(1, 1, 1);
    GameManager.stateCabane = -1;
  }

  public void OnClickQuitterCabane()
  {
    canvas.transform.localScale = new Vector3(0, 0, 0);
    panelCabaneMenu.transform.localScale = new Vector3(0, 0, 0);
    panelCabaneAction.transform.localScale = new Vector3(0, 0, 0);
    panelCabaneGris.transform.localScale = new Vector3(0, 0, 0);
    GameManager.stateCabane = -1;
    GameManager.inCabane = false;
  }

  public void OnClickRechauffer()
  {
    //GameManager.inCabane = true;
    panelCabaneMenu.transform.localScale = new Vector3(0, 0, 0);
    panelCabaneAction.transform.localScale = new Vector3(1, 1, 1);
    Text tmp = GameObject.Find("TextCabaneAction").GetComponent<Text>();

    tmp.text = "Jacob se réchauffe. Si vous voulez quitter la cabane, appuyez sur la touche espace.";
    GameManager.stateCabane = 0;
  }

  public void OnClickDormir()
  {
    //GameManager.inCabane = true;
    panelCabaneMenu.transform.localScale = new Vector3(0, 0, 0);
    panelCabaneAction.transform.localScale = new Vector3(1, 1, 1);
    Text tmp = GameObject.Find("TextCabaneAction").GetComponent<Text>();

    tmp.text = "Jacob dort. Si vous voulez quitter la cabane, appuyez sur la touche espace.";
    GameManager.stateCabane = 1;
  }

  // Update is called once per frame
  void Update()
  {
    if ((Input.GetKey(KeyCode.Space) && (GameManager.stateCabane != -1) && (GameManager.inCabane == true)))
    {
      canvas.transform.localScale = new Vector3(0, 0, 0);
      panelCabaneGris.transform.localScale = new Vector3(0, 0, 0);
      panelCabaneMenu.transform.localScale = new Vector3(0, 0, 0);
      panelCabaneAction.transform.localScale = new Vector3(0, 0, 0);
      GameManager.stateCabane = -1;
      GameManager.inCabane = false;
    }


    distance = Mathf.Sqrt(Mathf.Pow((jacob.transform.position.x - this.transform.position.x), 2) + Mathf.Pow((jacob.transform.position.y - this.transform.position.y), 2));
  }


  void OnMouseOver()
  {
    if(GameManager.inCabane == false)
    {
      if (distance < 4)
      {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
      }
      else
      {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
      }


      if (distance < 4 && Input.GetMouseButton(0))
      {
        OnClickEnterCabane();
      }
    }
    
  }

  void OnMouseExit()
  {
    if(!GameManager.inCabane)
      gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
  }

}