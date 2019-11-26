using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Boutique : MonoBehaviour
{
  public GameObject panelMenu;
  public GameObject panelVendre;
  public GameObject panelRecuperer;

  private float cagnotte = 0;
  private int etat = -1; //0 pour acheter, 1 pour vendre

  // Start is called before the first frame update
  void Start()
  {
    this.transform.localScale = new Vector3(0, 0, 0);
    panelMenu.transform.localScale = new Vector3(0, 0, 0);
    panelVendre.transform.localScale = new Vector3(0, 0, 0);
    panelRecuperer.transform.localScale = new Vector3(0, 0, 0);

    
  }

  public void OnClickLoadBoutique()
  {

    GameObject jacob = GameObject.Find("jacob_head_marker");
    jacob.transform.localScale = new Vector3(0, 0, 0);
    cagnotte = 0;
    etat = -1;
    this.transform.localScale = new Vector3(1, 1, 1);
    panelVendre.transform.localScale = new Vector3(0, 0, 0);
    panelRecuperer.transform.localScale = new Vector3(0, 0, 0);
    panelMenu.transform.localScale = new Vector3(1, 1, 1);

    InitQuantityButtons();
    Text tmp = GameObject.Find("TextCagnote").GetComponent<Text>();
    tmp.text = "Argent dans le coffre fort: $" + GameManager.argent;

    Text tmp4 = GameObject.Find("TextDispo").GetComponent<Text>();
    tmp4.text = "Disponible le " + GameManager.dateLivraison.Date.ToShortDateString() + " à " + GameManager.dateLivraison.TimeOfDay;
    Button tmp2 = GameObject.Find("ButtonRecuperer").GetComponent<Button>();
    Button tmp3 = GameObject.Find("ButtonVendre").GetComponent<Button>();
    Button tmp5 = GameObject.Find("ButtonAcheter").GetComponent<Button>();
    
    if (GameManager.dateLivraison.Year == 1)
    {
      tmp4.text = "";
      tmp2.interactable = false;
      tmp3.interactable = false;
      tmp5.interactable = false;

    }

    if (GameManager.calendrier < GameManager.dateLivraison)
    {
      tmp2.interactable = false;
      tmp3.interactable = false;
      tmp5.interactable = false;
      tmp4.color = new Color(138.0f/255.0f, 29.0f/255.0f, 23.0f/255.0f);
    }
    else
    {
      tmp2.interactable = true;
      tmp3.interactable = true;
      tmp5.interactable = true;
      tmp4.color = new Color(39.0f / 255.0f, 114.0f / 255.0f, 49.0f / 255.0f);
    }


    if ((GameManager.commandeAchat.Count != 0) || (GameManager.argentEnAttente > 0))
    {
      tmp3.interactable = false;
      tmp5.interactable = false;
    }

  }

  public void OnClickQuitterBoutique()
  {
    etat = -1;
    this.transform.localScale = new Vector3(0, 0, 0);
    panelMenu.transform.localScale = new Vector3(0, 0, 0);
    foreach (Ressource r in GameManager.items)
    {
      r.UpdateQuantite();
    }
    GameObject jacob = GameObject.Find("jacob_head_marker");
    jacob.transform.localScale = new Vector3(47.7f, 47.7f, 47.7f);
  }


  public void OnClickBoutonRecuperer()
  {
    List<Ressource> items = new List<Ressource>();
    foreach (Ressource r in GameManager.commandeAchat)
    {
      if (!items.Contains(r))
      {
        items.Add(new Ressource(r.name, r.quantite, 0, null, 0));
        for (int i = 0; i < items.Count; i++)
        {
          if (items[i].name == r.name)
          {
            items[i].quantite = 1;
          }
        }
      }
      else if (items.Contains(r))
      {
        for (int i = 0; i < items.Count; i++)
        {
          if (items[i].name == r.name)
          {
            items[i].quantite++;
          }
        }
      }
    }

    foreach (Ressource r in GameManager.items)
    {
      for(int i = 0; i < items.Count; i++)
      {
        if(r.name == items[i].name)
        {
          r.quantite += items[i].quantite;
          if (r.type == 1)
            r.jamaisCrafte = false;
        }
      }
    }
    panelRecuperer.transform.localScale = new Vector3(0, 0, 0);
    GameManager.argent += GameManager.argentEnAttente;
    GameManager.argentEnAttente = 0;
    GameManager.commandeAchat.Clear();
    GameManager.commandeVente.Clear();
    foreach (Ressource r in GameManager.items)
    {
      r.UpdateQuantite();
    }
    Inventaire go = GameObject.Find("Main Camera").GetComponent<Inventaire>();
    go.CheckIfObjectAddableToBag();
    go.CheckIfObjectIsCraftable();
    go.RearrangeBag();

    OnClickLoadBoutique();
    Text tmp4 = GameObject.Find("TextDispo").GetComponent<Text>();
    tmp4.text = "";
  }

  public void OnClickPanelRecuperer()
  {
    panelMenu.transform.localScale = new Vector3(0, 0, 0);
    panelRecuperer.transform.localScale = new Vector3(1, 1, 1);
    Text tmp = GameObject.Find("TextRecupRessources1").GetComponent<Text>();
    tmp.text = "";
    if(GameManager.argentEnAttente > 0)
    {
      tmp.text = "-   $ " + GameManager.argentEnAttente + "\n";
    }
    List<Ressource> items = new List<Ressource>();
    bool cpt = false;
    foreach (Ressource r in GameManager.commandeAchat)
    {
      cpt = false;
      for (int i = 0; i < items.Count; i++)
      {
        if (items[i].name == r.name)
        {
          items[i].quantite++;
          cpt = true;
        }
      }
      if(cpt == false)
      {
        items.Add(new Ressource(r.name, r.quantite, r.type, r.requirements, r.prix));
        for (int i = 0; i < items.Count; i++)
        {
          if (items[i].name == r.name)
          {
            items[i].quantite = 1;
          }
        }
      }
    }

    foreach(Ressource r in items)
    {
      tmp.text += "-   " + r.quantite + " " + r.name + "\n";
    }

    Button tmp2 = GameObject.Find("ButtonRecupRecuperer").GetComponent<Button>();
    if (tmp.text == "")
    {
      tmp2.interactable = false;
    }
    else
      tmp2.interactable = true;
  }

  public void OnClickPanelVendre()
  {
    cagnotte = 0;
    etat = 1;
    panelMenu.transform.localScale = new Vector3(0, 0, 0);
    panelVendre.transform.localScale = new Vector3(1, 1, 1);
    Text tmp2 = GameObject.Find("TextVendreCagnote").GetComponent<Text>();
    tmp2.text = "Argent dans le coffre fort: $" + GameManager.argent;


    for (int i = 0; i < GameManager.items.Count - 1; i++)
    {
      if (GameManager.items[i].name != "Sac")
      {
        Text tmp3 = GameObject.Find("TextPrix" + GameManager.items[i].name).GetComponent<Text>();
        tmp3.text = "$ " + (GameManager.items[i].prix - ((25f / 100f) * GameManager.items[i].prix)).ToString();

        Button tmp4 = GameObject.Find("ButtonVendre" + GameManager.items[i].name).GetComponent<Button>();
        tmp4.GetComponentInChildren<Text>().text = GameManager.items[i].quantite.ToString();
        if (tmp4.GetComponentInChildren<Text>().text == "0")
        {
          tmp4.interactable = false;
        }
        else
          tmp4.interactable = true;
      }
    }

    DisableQuantityButtons();
    Button tmp5 = GameObject.Find("ButtonVendrePour").GetComponent<Button>();
    tmp5.GetComponentInChildren<Text>().text = "Vendre pour $ ";
  }

  public void OnClickPanelAcheter()
  {
    cagnotte = 0;
    etat = 0;
    panelMenu.transform.localScale = new Vector3(0, 0, 0);
    panelVendre.transform.localScale = new Vector3(1, 1, 1);
    Text tmp2 = GameObject.Find("TextVendreCagnote").GetComponent<Text>();

    tmp2.text = "Argent dans le coffre fort: $" + GameManager.argent;

    for (int i = 0; i < GameManager.items.Count - 1; i++)
    {
      if (GameManager.items[i].name != "Sac")
      {
        Text tmp3 = GameObject.Find("TextPrix" + GameManager.items[i].name).GetComponent<Text>();
        tmp3.text = "$ " + GameManager.items[i].prix.ToString();

        Button tmp4 = GameObject.Find("ButtonVendre" + GameManager.items[i].name).GetComponent<Button>();
        tmp4.GetComponentInChildren<Text>().text = GameManager.items[i].quantite.ToString();
        tmp4.interactable = true;
      }
      
    }
    DisableQuantityButtons();
    Button tmp5 = GameObject.Find("ButtonVendrePour").GetComponent<Button>();
    tmp5.GetComponentInChildren<Text>().text = "Acheter pour $ ";
  }

  public void InitQuantityButtons()
  {
    for (int i = 0; i < GameManager.items.Count - 1; i++)
    {
      if (GameManager.items[i].name != "Sac")
      {
        Text tmp3 = GameObject.Find("TextQuantite" + GameManager.items[i].name).GetComponent<Text>();
        Button tmp = GameObject.Find("ButtonMoins" + GameManager.items[i].name).GetComponent<Button>();
        Button tmp2 = GameObject.Find("ButtonPlus" + GameManager.items[i].name).GetComponent<Button>();
        tmp3.text = "0";
        tmp.interactable = true;
        tmp2.interactable = true;
      }
    }
  }


  public void DisableQuantityButtons()
  {
    for (int i = 0; i < GameManager.items.Count - 1; i++)
    {
      if (GameManager.items[i].name != "Sac")
      {
        Button tmp = GameObject.Find("ButtonMoins" + GameManager.items[i].name).GetComponent<Button>();
        Text tmp3 = GameObject.Find("TextQuantite" + GameManager.items[i].name).GetComponent<Text>();
        if (tmp3.text == "0")
        {
          tmp.interactable = false;
        }
        else
        {
          tmp.interactable = true;
        }
        if(etat == 1)
        {
          Button tmp2 = GameObject.Find("ButtonPlus" + GameManager.items[i].name).GetComponent<Button>();
          if (Int32.Parse(tmp3.GetComponentInChildren<Text>().text) >= GameManager.items[i].quantite)
          {
            tmp2.interactable = false;
          }
          else
            tmp2.interactable = true;
        }
      }

    }

    Button tmp4 = GameObject.Find("ButtonVendrePour").GetComponent<Button>();
    if(etat == 0)
    {
      if ((cagnotte > GameManager.argent) || (cagnotte == 0))
      {
        tmp4.interactable = false;
      }
      else
        tmp4.interactable = true;
    }
    else
    {
      if(cagnotte == 0)
        tmp4.interactable = false;
      else
        tmp4.interactable = true;
    }
  }

  public void OnClickReturnToBoutique()
  {
    cagnotte = 0;
    GameManager.commandeAchat.Clear();
    GameManager.commandeVente.Clear();
    OnClickLoadBoutique();
  }

  public void OnClickReturnToBoutiqueRecup()
  {
    OnClickLoadBoutique();
  }

  public void OnClickItemMoins(GameObject item)
  {
    foreach(Ressource r in GameManager.items)
    {
      if (item.name.Contains(r.name))
      {
        Text tmp = GameObject.Find("TextQuantite" + r.name).GetComponent<Text>();
        if(tmp.text != "0")
        {
          int tmp2 = Int32.Parse(tmp.text);
          tmp.text = (tmp2 - 1).ToString();
          Text tmp4 = GameObject.Find("TextPrix" + r.name).GetComponent<Text>();
          cagnotte -= float.Parse(tmp4.text.Substring(2));
          if (etat == 1)
            GameManager.commandeVente.Remove(r);
          else
            GameManager.commandeAchat.Remove(r);
          

        }
        DisableQuantityButtons();
      }
    }
    Button tmp3 = GameObject.Find("ButtonVendrePour").GetComponent<Button>();
    if(etat == 1)
      tmp3.GetComponentInChildren<Text>().text = "Vendre pour $ " + cagnotte;
    else
      tmp3.GetComponentInChildren<Text>().text = "Acheter pour $ " + cagnotte;

  }

  public void OnClickItemPlus(GameObject item)
  {
    foreach (Ressource r in GameManager.items)
    {
      if (item.name.Contains(r.name))
      {
        Text tmp = GameObject.Find("TextQuantite" + r.name).GetComponent<Text>();
        int tmp2 = Int32.Parse(tmp.text);
        if(etat == 1)
        {
          if (tmp2 < r.quantite)
          {
            tmp.text = (tmp2 + 1).ToString();
            Text tmp4 = GameObject.Find("TextPrix" + r.name).GetComponent<Text>();
            cagnotte += float.Parse(tmp4.text.Substring(2));
            GameManager.commandeVente.Add(r);
          }
        }
        else
        {
          tmp.text = (tmp2 + 1).ToString();
          Text tmp4 = GameObject.Find("TextPrix" + r.name).GetComponent<Text>();
          cagnotte += float.Parse(tmp4.text.Substring(2));
          GameManager.commandeAchat.Add(r);
        }
        
        DisableQuantityButtons();
      }
    }
    Button tmp3 = GameObject.Find("ButtonVendrePour").GetComponent<Button>();
    tmp3.GetComponentInChildren<Text>().text = "Vendre pour $ " + cagnotte;
    if (etat == 1)
      tmp3.GetComponentInChildren<Text>().text = "Vendre pour $ " + cagnotte;
    else
      tmp3.GetComponentInChildren<Text>().text = "Acheter pour $ " + cagnotte;
  }


  public void OnClickVendrePour()
  {
    if (etat == 1)
    {
      GameManager.argentEnAttente = cagnotte;
      List<Ressource> items2 = new List<Ressource>();
      
      foreach (Ressource r in GameManager.commandeVente)
      {
        if (!items2.Contains(r))
        {
          items2.Add(new Ressource(r.name, r.quantite, 0, null, 0));
          for (int i = 0; i < items2.Count; i++)
          {
            if (items2[i].name == r.name)
            {
              items2[i].quantite = 1;
            }
          }
        }
        else if (items2.Contains(r))
        {
          for (int i = 0; i < items2.Count; i++)
          {
            if (items2[i].name == r.name)
            {
              items2[i].quantite++;
            }
          }
        }
      }

      foreach (Ressource r in GameManager.items)
      {
        for (int i = 0; i < items2.Count; i++)
        {
          if (r.name == items2[i].name)
          {
            r.quantite -= items2[i].quantite;
          }
        }
      }
    }
    else if (etat == 0)
    {
      GameManager.argent -= cagnotte;

    }

    Button tmp = GameObject.Find("ButtonAcheter").GetComponent<Button>();
    tmp.interactable = false;
    Button tmp2 = GameObject.Find("ButtonVendre").GetComponent<Button>();
    tmp2.interactable = false;
    Button tmp3 = GameObject.Find("ButtonRecuperer").GetComponent<Button>();
    //tmp3.interactable = false;

    Text tmp4 = GameObject.Find("TextDispo").GetComponent<Text>();
    System.DateTime calendrier;
    calendrier = GameManager.calendrier.AddDays(3);
    GameManager.dateLivraison = calendrier;
    calendrier.ToString("ddd dd MMM");
    tmp4.text = "Disponible le " + calendrier.Date.ToShortDateString() + " à " + calendrier.TimeOfDay;

    foreach (Ressource r in GameManager.items)
    {
      r.UpdateQuantite();
    }
    Inventaire go = GameObject.Find("Main Camera").GetComponent<Inventaire>();
    go.CheckIfObjectAddableToBag();
    go.CheckIfObjectIsCraftable();
    go.RearrangeBag();

    OnClickLoadBoutique();
  }
}
