﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Inventaire : MonoBehaviour
{
  public GameObject boutonAddBag;
  public GameObject boutonDeleteBag;
  public GameObject boutonCraft;
  public GameObject jacob;

  /*public List<Ressource> GameManager.items = new List<Ressource>();

  private List<Ressource> tabBag = new List<Ressource>();*/
  private GameObject[] tabBagPos = new GameObject[8];
  private GameObject gameObjectActuel;
  private int state; //0 pour craft, 1 pour addBag, 2 pour deleteBag


  void Start()
  {
    state = -1;
    boutonAddBag.transform.position += new Vector3(0,0,-10);
    boutonDeleteBag.transform.position += new Vector3(0,0,-10);
    boutonCraft.transform.position += new Vector3(0,0,-10);

    foreach(Ressource r in GameManager.items)
    {
      r.FindButton();
    }
    InitBag();
    if (GameManager.cptInventaire != 0)
      ReprintBag();
    CheckIfObjectIsCraftable();
    CheckIfObjectAddableToBag();
    Map();
  }

  private void Update()
  {
    if (Input.GetKey(KeyCode.Escape))
    {
      SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
      GameManager.cptJeu++;
    }

    if (Input.GetKey(KeyCode.Tab)) //cheat code, faudra enlever
    {
      GameManager.calendrier = GameManager.calendrier.AddDays(1);
    }

    if (GameManager.craftfeu == true && GameManager.loop == false)
    {
      GameManager.loop = true;
      SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
      GameManager.cptJeu++;
    }

  }

  void InitBag()
  {
    for (int i = 0; i < 8; i++)
    {
      GameObject tmp = GameObject.Find("HUDEmplacement" + (i + 1).ToString());
      tabBagPos[i] = tmp;       
      
    }
    
  }


  void Map()
  {
    jacob.transform.localPosition = GameManager.posJacobMap;
  }

  public void CheckIfObjectIsCraftable()
  {
    foreach(Ressource r in GameManager.items)
    {
      if (r.requirements != null)
      {
        foreach (KeyValuePair<Ressource, int> dic in r.requirements)
        {
          foreach (Ressource p in GameManager.items)
          {
            if (p.name == dic.Key.name)
            {
              if (p.quantite < dic.Value)
                r.DisableButtonCraft();
              else
                r.EnableButtonCraft();

            }
          }
        }
      }
    }
  }

  public void CheckIfObjectAddableToBag()
  {
      
    if (GameManager.tabBag.Count >= 8)
    {
      foreach (Ressource r in GameManager.items)
      {
        r.DisableButtonCase();
      }
    }
    else
    {
      foreach (Ressource r in GameManager.items)
      {
        if (GameManager.tabBag.Contains(r))
        {
          r.DisableButtonCase();
        }
        
        else
        {
          if (r.quantite != 0)
          {
            r.EnableButtonCase();
          }
          else
            r.DisableButtonCase();

        }
          
      }
    }
  }

  public void DisableClickAddBag()
  {
    boutonAddBag.GetComponentInChildren<Button>().interactable = false;
    boutonAddBag.GetComponentInChildren<Button>().enabled = false;
  }

  public void EnableClickAddBag()
  {
    
    boutonAddBag.GetComponentInChildren<Button>().interactable = true;
    boutonAddBag.GetComponentInChildren<Button>().enabled = true;
  }

  public void ClickAddBag(GameObject bouton)
  {
    boutonAddBag.transform.position = bouton.transform.position + new Vector3(0.7f,0,0);
    boutonDeleteBag.transform.position += new Vector3(0, 0, -10);
    boutonCraft.transform.position += new Vector3(0, 0, -10);
    gameObjectActuel = bouton;
  }

  public void ClickDeleteBag(GameObject bouton)
  {
    if(bouton.GetComponent<Image>().sprite != null)
    {
      boutonDeleteBag.transform.position = bouton.transform.position + new Vector3(0.7f, 0, 0);
      boutonAddBag.transform.position += new Vector3(0, 0, -10);
      boutonCraft.transform.position += new Vector3(0, 0, -10);
      gameObjectActuel = bouton;
    }
    
  }

  public void ClickCraft(GameObject bouton)
  {
    boutonCraft.transform.position = bouton.transform.position + new Vector3(0.7f, 0, 0);
    boutonAddBag.transform.position += new Vector3(0, 0, -10);
    boutonDeleteBag.transform.position += new Vector3(0, 0, -10);
    gameObjectActuel = bouton;

    foreach(Ressource r in GameManager.items)
    {
      if (r.name == gameObjectActuel.name.Replace("Craft", ""))
      {
        
        Text text = GameObject.Find("TextCraft").GetComponent<Text>();
        text.text = "";
        bool tmp = true;
        foreach (KeyValuePair<Ressource, int> dic3 in r.requirements)
        {
          
          if (!tmp)
          {
            text.text += "\n";
            text.text += dic3.Key.name.ToString() + ": " + dic3.Value.ToString();
          }
          else
          {
            text.text += dic3.Key.name.ToString() + ": " + dic3.Value.ToString();
            tmp = false;
          }
        }
      }
    }
    
  }



  public void Craft()
  {
    state = 0;
    bool feu = false;
    boutonCraft.transform.position += new Vector3(0, 0, -10);

    foreach (Ressource r in GameManager.items)
    {
      if (r.name == gameObjectActuel.name.Replace("Craft", ""))
      {
        foreach (KeyValuePair<Ressource, int> dic2 in r.requirements)
        {
          foreach (Ressource p in GameManager.items)
          {
            if (p.name == dic2.Key.name)
            {
              p.quantite -= dic2.Value;
              p.UpdateQuantite();
              
            }
          }
        }
        if (r.jamaisCrafte == true)
        {
          r.EnableButtonCase();
          r.jamaisCrafte = false;
        }
        r.quantite = r.quantite + 1;
        r.UpdateQuantite();
        RearrangeBag();
        if (r.name == "Feu")
          feu = true;
      }
    }

    CheckIfObjectIsCraftable();
    if (feu == true)
      GameManager.craftfeu = true;
  }


  public void AddToBag()
  {
    state = 1;

    boutonAddBag.transform.position += new Vector3(0, 0, -10);

    
    foreach (Ressource r in GameManager.items)
    {
      if (r.name == gameObjectActuel.name)
      {
        GameManager.tabBag.Add(r);
        tabBagPos[GameManager.tabBag.Count - 1].GetComponent<Image>().sprite = gameObjectActuel.GetComponent<Image>().sprite;
        tabBagPos[GameManager.tabBag.Count - 1].GetComponent<Image>().color += new Color(0, 0, 0, 1);
        tabBagPos[GameManager.tabBag.Count - 1].GetComponentInChildren<Text>().text = r.quantite.ToString();

      }
    }

    CheckIfObjectAddableToBag();
    RearrangeBag();
  }


  public void ReprintBag()
  {
    GameObject[] tmp = new GameObject[8];
    for (int i = 0; i < GameManager.tabBag.Count; i++)
    {
      tmp[i] = GameObject.Find(GameManager.tabBag[i].name.ToString());
    }

    for (int i = 0; i < 8; i++)
    {
      if (tmp[i] == null)
      {
        tabBagPos[i].GetComponent<Image>().sprite = null;
        tabBagPos[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        tabBagPos[i].GetComponentInChildren<Text>().text = "";
      }
      else if (tmp[i].GetComponent<Image>().sprite != null)
      {
        tabBagPos[i].GetComponent<Image>().sprite = tmp[i].GetComponent<Image>().sprite;
        tabBagPos[i].GetComponent<Image>().color += new Color(0, 0, 0, 1);
        tabBagPos[i].GetComponentInChildren<Text>().text = tmp[i].GetComponentInChildren<Text>().text;
      }
    }

  }


  public void RearrangeBag()
  {

    GameObject[] tmp = new GameObject[8];

    for(int i = 0; i < GameManager.tabBag.Count; i++)
    {
      for(int j = 0; j < tabBagPos.Length; j++)
      {
        if (tabBagPos[j].GetComponent<Image>().sprite != null)
        {
          if ((GameManager.tabBag[i].name == tabBagPos[j].name) || (GameManager.tabBag[i].name == tabBagPos[j].GetComponent<Image>().sprite.name))
          {
            tmp[i] = tabBagPos[j];
            tmp[i].GetComponent<Image>().sprite = tabBagPos[j].GetComponent<Image>().sprite;
            tmp[i].GetComponentInChildren<Text>().text = GameManager.tabBag[i].quantite.ToString();

          }
        }
      }
    }

    for(int i = 0; i < 8; i++)
    {
      if(tmp[i] == null)
      {
        tabBagPos[i].GetComponent<Image>().sprite = null;
        tabBagPos[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        tabBagPos[i].GetComponentInChildren<Text>().text = "";
        GameManager.tabSprite[i] = null;
      }
      else if (tmp[i].GetComponent<Image>().sprite != null){
        tabBagPos[i].GetComponent<Image>().sprite = tmp[i].GetComponent<Image>().sprite;
        tabBagPos[i].GetComponent<Image>().color += new Color(0, 0, 0, 1);
        tabBagPos[i].GetComponentInChildren<Text>().text = tmp[i].GetComponentInChildren<Text>().text;
        Sprite sprite = Instantiate(tmp[i].GetComponent<Image>().sprite);
        GameManager.tabSprite[i] = sprite;
      }
    }
  }


  public void DeleteFromBag()
  {
    state = 2;

    boutonDeleteBag.transform.position += new Vector3(0, 0, -10);

    foreach (Ressource r in GameManager.items)
    {
      if ((r.name == gameObjectActuel.name) || (r.name == gameObjectActuel.GetComponent<Image>().sprite.name))
      {
        int i = -1;
        i = Array.FindIndex<GameObject>(tabBagPos, 0, t => t == gameObjectActuel);
        tabBagPos[i].GetComponent<Image>().sprite = null;
        tabBagPos[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        tabBagPos[i].GetComponentInChildren<Text>().text = "";
        GameManager.tabBag.Remove(r);
        break;
      }
    }
    CheckIfObjectAddableToBag();
    RearrangeBag();

  }


  public void OnClickSauvegarder()
  {
    GameManager.Save();
  }


  public void OnClickQuitter()
  {
    Application.Quit();
  }

}
