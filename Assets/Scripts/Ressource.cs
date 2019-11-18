using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ressource
{
  // Start is called before the first frame update

  public string name;
  public int quantite;
  public Dictionary<Ressource, int> requirements = new Dictionary<Ressource, int>();
  public int type = -1; //0 pour non-craftable, 1 pour craftable
  public bool etat = false; //false pour pas assez de ressources pour crafter, vrai pour l'inverse
  public bool jamaisCrafte = true;

  public Button buttonCase;
  public Button buttonCraft;


  public Ressource(string name, int quantite, int type, Dictionary<Ressource, int> req)
  {
    this.name = name;
    this.quantite = quantite;
    this.requirements = req;
    this.type = type;

  }

  public void FindButton()
  {
    
    if (this.name != "Feu")
      buttonCase = GameObject.Find(name).GetComponent<Button>();

    if (type == 1)
      buttonCraft = GameObject.Find(name + "Craft").GetComponent<Button>();


    UpdateQuantite();
  }

  public void DisableButtonCraft()
  {
    if (buttonCraft != null)
    {

      buttonCraft.interactable = false;

    }
  }

  public void EnableButtonCase()
  {
    if (this.name != "Feu")
    {
      buttonCase.interactable = true;

    }
  }

  public void DisableButtonCase()
  {
    if (this.name != "Feu")
    {
      buttonCase.interactable = false;
     
    }
  }


  public void UpdateQuantite()
  {
    if (this.name != "Feu")
    {
      buttonCase.GetComponentInChildren<Text>().text = quantite.ToString();
      if ((type == 1) && (jamaisCrafte == true))
      {
        DisableButtonCase();
        buttonCase.GetComponentInChildren<Text>().text = "";
      }
      if (quantite < 0)
      {
        quantite = 0;
        UpdateQuantite();
      }
    }
    

  }

}
