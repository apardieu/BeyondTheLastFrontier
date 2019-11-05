using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[InitializeOnLoad]
public static class GameManager
{

  public static List<Ressource> items = new List<Ressource>();
  public static List<Ressource> tabBag = new List<Ressource>();
  public static Sprite[] tabSprite = new Sprite[8];

  public static float valeurVie;
  public static float valeurFaim;
  public static float valeurSoif;
  public static float valeurEnergie;
  public static float valeurTemperature;

  public static System.DateTime calendrier;
  public static System.DateTime startTime;
  public static string meteo;

  public static Vector3 posJacob;
  public static Quaternion rotJacob;


  public static int cptInventaire = -1;
  public static int cptJeu = -1;


  static GameManager()
  {
    EditorApplication.update += Update;
    InitRessources();
  }

  static void Update()
  {
    if (Input.GetKey(KeyCode.Escape))
    {
      Scene scene = SceneManager.GetActiveScene();
      if (scene.name == "MainScene")
      {
        SaveValeursJeu();
        SceneManager.LoadScene("Inventaire");
        cptInventaire++;
      }
      else if (scene.name == "Inventaire")
      {
        SceneManager.LoadScene("MainScene");
        cptJeu++;
      }
    }
  }

  static void SaveValeursJeu()
  {
    GameObject tmp = GameObject.Find("Canvas");
    GameObject jacob = GameObject.Find("Jacob");

    posJacob = jacob.transform.position;
    rotJacob = jacob.transform.rotation;

    valeurVie = tmp.GetComponent<HUD>().jaugeVie.GetComponent<Slider>().value;
    valeurFaim = tmp.GetComponent<HUD>().jaugeFaim.GetComponent<Slider>().value;
    valeurSoif = tmp.GetComponent<HUD>().jaugeSoif.GetComponent<Slider>().value;
    valeurEnergie = tmp.GetComponent<HUD>().jaugeEnergie.GetComponent<Slider>().value;
    valeurTemperature = tmp.GetComponent<HUD>().jaugeTemperature.GetComponent<Slider>().value;

    meteo = HUD.meteoActuelle;
    calendrier = HUD.calendrier;
    startTime = HUD.startTime;

  }



  static void InitRessources()
  {

    items.Add(new Ressource("Bois", 10, 0, null));
    items.Add(new Ressource("Viande", 10, 0, null));
    items.Add(new Ressource("Poisson", 10, 0, null));
    items.Add(new Ressource("Silex", 10, 0, null));
    items.Add(new Ressource("Caillou", 10, 0, null));
    items.Add(new Ressource("Eau", 10, 0, null));
    items.Add(new Ressource("Cuir", 10, 0, null));
    items.Add(new Ressource("Peau", 10, 0, null));
    items.Add(new Ressource("Ficelle", 10, 0, null));
    items.Add(new Ressource("Corde", 10, 0, null));
    items.Add(new Ressource("Sac", 10, 0, null));


    Dictionary<Ressource, int> dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 1);
    dic.Add(items[3], 4);
    items.Add(new Ressource("Scie", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 4);
    dic.Add(items[3], 2);
    dic.Add(items[9], 1);
    items.Add(new Ressource("Pelle", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 1);
    dic.Add(items[3], 4);
    dic.Add(items[9], 3);
    items.Add(new Ressource("Pioche", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 4);
    dic.Add(items[3], 1);
    dic.Add(items[9], 3);
    items.Add(new Ressource("Hache", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 2);
    dic.Add(items[3], 2);
    dic.Add(items[9], 2);
    items.Add(new Ressource("Couteau", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[3], 10);
    items.Add(new Ressource("Piege", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[6], 3);
    dic.Add(items[7], 2);
    dic.Add(items[8], 4);
    items.Add(new Ressource("Chapeau", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[6], 6);
    dic.Add(items[7], 4);
    dic.Add(items[8], 8);
    items.Add(new Ressource("Manteau", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[6], 4);
    dic.Add(items[7], 1);
    dic.Add(items[8], 4);
    items.Add(new Ressource("Chaussures", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[6], 3);
    dic.Add(items[7], 2);
    dic.Add(items[8], 4);
    items.Add(new Ressource("Gants", 0, 1, dic));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 3);
    dic.Add(items[3], 2);
    items.Add(new Ressource("Feu", 0, 1, dic));

  }

}
