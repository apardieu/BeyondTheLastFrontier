using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

//[InitializeOnLoad]
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
  public static Vector3 posJacobMap;
  public static Quaternion rotJacob;
  
  public static int cptInventaire = -1;
  public static int cptJeu = -1;
  public static float stateDeplacement = 0;
  public static bool isCollecting;
    public static float coefWeather;

    public static GameObject objs;
  private static string savePath;

  public static List<Vector3> posTree = new List<Vector3>();

  public static bool trigger = false;
  public static bool saveMode = false;

  public static float argent = 100;
  public static float argentEnAttente = 0;

  public static List<Ressource> commandeAchat = new List<Ressource>();
  public static List<Ressource> commandeVente = new List<Ressource>();

  public static DateTime dateLivraison;
  public static bool craftfeu = false;
  public static bool loop = false;
  public static bool inCabane = false;
  public static int stateCabane = -1;


  static GameManager()
  {
    EditorApplication.update += Update;
    InitRessources();

    objs = GameObject.FindGameObjectWithTag("Grid");
    savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    if(saveMode)
      Load();
    trigger = true;
  }

  static void Update()
  {
    if (Input.GetKey(KeyCode.Escape))
    {
      Scene scene = SceneManager.GetActiveScene();
      if (scene.name == "MainScene")
      {
        SaveValeursJeu();
        Save();
        SceneManager.LoadScene("Inventaire", LoadSceneMode.Single);
        cptInventaire++;
      }
      else if (scene.name == "Inventaire")
      {
        Save();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        cptJeu++;
      }
    }

    if (Input.GetKey(KeyCode.Tab)) //cheat code, faudra enlever
    {
      calendrier = calendrier.AddDays(1);
      Debug.Log(calendrier);
    }

    if(craftfeu == true && loop == false)
    {
      loop = true;
      Save();
      SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
      cptJeu++;
    }
  }

  static void Load()
  {
    using (
       var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
     )
    {
      posJacob.x = reader.ReadSingle();
      posJacob.y = reader.ReadSingle();
      posJacob.z = reader.ReadSingle();
      valeurVie = reader.ReadSingle();
      valeurFaim = reader.ReadSingle();
      valeurSoif = reader.ReadSingle();
      valeurEnergie = reader.ReadSingle();
      valeurTemperature = reader.ReadSingle();
      meteo = reader.ReadString();
      int tmpSec = reader.ReadInt32();
      int tmpMin = reader.ReadInt32();
      int tmpHour = reader.ReadInt32();
      int tmpDay = reader.ReadInt32();
      int tmpMonth = reader.ReadInt32();
      int tmpYear = reader.ReadInt32();
      calendrier = new System.DateTime(tmpYear, tmpMonth, tmpDay, tmpHour, tmpMin, tmpSec);
      tmpSec = reader.ReadInt32();
      tmpMin = reader.ReadInt32();
      tmpHour = reader.ReadInt32();
      tmpDay = reader.ReadInt32();
      tmpMonth = reader.ReadInt32();
      tmpYear = reader.ReadInt32();
      startTime = new System.DateTime(tmpYear, tmpMonth, tmpDay, tmpHour, tmpMin, tmpSec);
      for (int i = 0; i < items.Count; i++)
      {
        items[i].quantite = reader.ReadInt32();
      }
      int count = reader.ReadInt32();
      for (int i = 0; i < count; i++)
      {
        Vector3 vec;
        vec.x = reader.ReadSingle();
        vec.y = reader.ReadSingle();
        vec.z = reader.ReadSingle();
        posTree.Add(vec);
      }
      argent = reader.ReadSingle();
      
    }
  }

    public static void setIsCollecting(bool set)
    {
        isCollecting = set;
    }

  static void Save()
  {
    using (
       var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
     )
    {
      writer.Write(posJacob.x);
      writer.Write(posJacob.y);
      writer.Write(posJacob.z);
      writer.Write(valeurVie);
      writer.Write(valeurFaim);
      writer.Write(valeurSoif);
      writer.Write(valeurEnergie);
      writer.Write(valeurTemperature);
      writer.Write(meteo);
      writer.Write(calendrier.Second);
      writer.Write(calendrier.Minute);
      writer.Write(calendrier.Hour);
      writer.Write(calendrier.Day);
      writer.Write(calendrier.Month);
      writer.Write(calendrier.Year);
      writer.Write(startTime.Second);
      writer.Write(startTime.Minute);
      writer.Write(startTime.Hour);
      writer.Write(startTime.Day);
      writer.Write(startTime.Month);
      writer.Write(startTime.Year);
      for (int i = 0; i < items.Count; i++)
      {
        writer.Write(items[i].quantite);
      }
      writer.Write(posTree.Count);
      foreach (Vector3 vec in posTree)
      {
        writer.Write(vec.x);
        writer.Write(vec.y);
        writer.Write(vec.z);
      }
      writer.Write(argent);
    }

  }

  static void SaveValeursJeu()
  {
    GameObject tmp = GameObject.Find("Canvas");
    GameObject jacob = GameObject.Find("Jacob");
    GameObject tm2 = GameObject.Find("Snow - Harvestable");

    posTree.Clear();
    
    for (int i = 0; i < tm2.transform.childCount; i++)
    {
      posTree.Add(tm2.transform.GetChild(i).transform.position);
    }

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

    GameObject tmp2 = GameObject.Find("Snow - Ground");
    Tilemap tm = tmp2.GetComponent<Tilemap>();
    posJacobMap = tm.WorldToCell(posJacob);
    Debug.Log(posJacobMap);

  }



  static void InitRessources()
  {

    items.Add(new Ressource("Bois", 10, 0, null, 5));
    items.Add(new Ressource("Viande", 10, 0, null, 5));
    items.Add(new Ressource("Poisson", 10, 0, null, 3));
    items.Add(new Ressource("Silex", 10, 0, null, 5));
    items.Add(new Ressource("Caillou", 10, 0, null, 1));
    items.Add(new Ressource("Eau", 10, 0, null, 1));
    items.Add(new Ressource("Cuir", 10, 0, null, 200));
    items.Add(new Ressource("Peau", 10, 0, null, 500));
    items.Add(new Ressource("Ficelle", 10, 0, null, 2));
    items.Add(new Ressource("Corde", 10, 0, null, 10));
    items.Add(new Ressource("Sac", 1, 0, null, 50));


    Dictionary<Ressource, int> dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 1);
    dic.Add(items[3], 4);
    items.Add(new Ressource("Scie", 0, 1, dic, 30));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 4);
    dic.Add(items[3], 2);
    dic.Add(items[9], 1);
    items.Add(new Ressource("Pelle", 0, 1, dic, 30));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 1);
    dic.Add(items[3], 4);
    dic.Add(items[9], 3);
    items.Add(new Ressource("Pioche", 0, 1, dic, 30));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 4);
    dic.Add(items[3], 1);
    dic.Add(items[9], 3);
    items.Add(new Ressource("Hache", 0, 1, dic, 30));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 2);
    dic.Add(items[3], 2);
    dic.Add(items[9], 2);
    items.Add(new Ressource("Couteau", 0, 1, dic, 15));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[3], 10);
    items.Add(new Ressource("Piege", 0, 1, dic, 50));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[6], 3);
    dic.Add(items[7], 2);
    dic.Add(items[8], 4);
    items.Add(new Ressource("Chapeau", 0, 1, dic, 100));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[6], 6);
    dic.Add(items[7], 4);
    dic.Add(items[8], 8);
    items.Add(new Ressource("Manteau", 0, 1, dic, 200));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[6], 4);
    dic.Add(items[7], 1);
    dic.Add(items[8], 4);
    items.Add(new Ressource("Chaussures", 0, 1, dic, 200));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[6], 3);
    dic.Add(items[7], 2);
    dic.Add(items[8], 4);
    items.Add(new Ressource("Gants", 0, 1, dic, 150));

    dic = new Dictionary<Ressource, int>();
    dic.Add(items[0], 3);
    dic.Add(items[3], 2);
    items.Add(new Ressource("Feu", 0, 1, dic, 0));

  }

}
