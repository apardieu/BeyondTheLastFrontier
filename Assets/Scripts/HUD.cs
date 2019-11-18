using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEditor;
using UB.Simple2dWeatherEffects.Standard;

public class HUD : MonoBehaviour
{
  public GameObject soleil;
  public GameObject jaugeVie;
  public GameObject jaugeFaim;
  public GameObject jaugeSoif;
  public GameObject jaugeEnergie;
  public GameObject jaugeTemperature;
  public Text textMeteo;
  public Text textDate;
  public Text textHeure;
  public static string meteoActuelle;

  public GameObject boutonManger;
  public GameObject boutonBoire;
  public GameObject boutonUtiliser;
  public GameObject boutonPoser;

  public GameObject prefabFeu;



  public static System.DateTime calendrier;
  private float tmpSecondes = -1;
  private float tmpHeures = -1;
  private bool debut = true;
  public static System.DateTime startTime;
  private List<string> meteo = new List<string>();
  private List<Color> palette = new List<Color>();

  private GameObject[] tabBagPos = new GameObject[8];

  private GameObject gameObjectActuel;

  private GameObject tilemap;
    
  private float coefEnergie;

  private List<GameObject> currentWeather = new List<GameObject>();
  public GameObject snowWeather;
  public GameObject tempeteWeather1;
  public GameObject tempeteWeather2;


  void Start()
  {

    boutonManger.transform.position += new Vector3(0, 0, -50);
    boutonBoire.transform.position += new Vector3(0, 0, -50);
    boutonUtiliser.transform.position += new Vector3(0, 0, -50);
    boutonPoser.transform.position += new Vector3(0, 0, -50);

    startTime = System.DateTime.UtcNow;
    calendrier = new System.DateTime(2019, 9, 1, 11, 0, 0);
    meteo.Add("Ensoleillé");
    meteo.Add("Couvert");
    meteo.Add("Neige");
    meteo.Add("Brouillard");
    meteo.Add("Tempete");


    palette.Add(jaugeVie.GetComponentInChildren<Image>().color);
    palette.Add(jaugeFaim.GetComponentInChildren<Image>().color);
    palette.Add(jaugeSoif.GetComponentInChildren<Image>().color);
    palette.Add(jaugeEnergie.GetComponentInChildren<Image>().color);
    palette.Add(jaugeTemperature.GetComponentInChildren<Image>().color);

    InitBag();
    while (GameManager.trigger == false) ;


    if (GameManager.saveMode)
      LoadJeu();
    else
    {
      if (GameManager.cptJeu >= 0)
        LoadJeu();
    }
  }


  void InitBag()
  {
    for (int i = 0; i < 8; i++)
    {
      GameObject tmp = GameObject.Find("ButtonHUDMiddle" + (i + 1).ToString());
      tabBagPos[i] = tmp;
      tabBagPos[i].GetComponentInChildren<Text>().text = "";
      tabBagPos[i].GetComponentInChildren<Button>().interactable = false;
      tabBagPos[i].GetComponentInChildren<Button>().enabled = false;
    }

  }

  void CraftFeu()
  {
    GameObject jacob = GameObject.Find("Jacob");
    Instantiate(prefabFeu, jacob.transform.position, Quaternion.identity);
  }


  void LoadJeu()
  {
    jaugeVie.GetComponent<Slider>().value = GameManager.valeurVie;
    jaugeFaim.GetComponent<Slider>().value = GameManager.valeurFaim;
    jaugeSoif.GetComponent<Slider>().value = GameManager.valeurSoif;
    jaugeEnergie.GetComponent<Slider>().value = GameManager.valeurEnergie;
    jaugeTemperature.GetComponent<Slider>().value = GameManager.valeurTemperature;

    calendrier = GameManager.calendrier;
    Debug.Log(calendrier);
    startTime = GameManager.startTime;
    textMeteo.text = GameManager.meteo;
    meteoActuelle = GameManager.meteo;
    setWeatherAspect(GameManager.meteo);
    textHeure.text = calendrier.Hour.ToString() + ":00";
    textDate.text = calendrier.ToString("ddd dd MMM");
    soleil.transform.eulerAngles += (new Vector3(0, 0, 0.5f) * ((calendrier.Hour - 12) * 30));


    tilemap = GameObject.Find("Snow - Harvestable");
    List<Vector3> tmp = new List<Vector3>();
    for (int i = 0; i < tilemap.transform.childCount; i++)
    {
      tmp.Add(tilemap.transform.GetChild(i).transform.position);
    }
    for (int i = 0; i < GameManager.posTree.Count; i++)
    {
      if (!(GameManager.posTree.Contains(tmp[i])))
      {
        for (int j = 0; j < tilemap.transform.childCount; j++)
        {
          if (tilemap.transform.GetChild(j).transform.position == tmp[i])
          {
            Destroy(tilemap.transform.GetChild(j).gameObject);
          }
        }
      }
    }


    for (int i = 0; i < 8; i++)
    {
      tabBagPos[i].GetComponent<Image>().sprite = GameManager.tabSprite[i];
      if (tabBagPos[i].GetComponent<Image>().sprite == null)
      {
        tabBagPos[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        tabBagPos[i].GetComponentInChildren<Text>().text = "";
        tabBagPos[i].GetComponentInChildren<Button>().interactable = false;
        tabBagPos[i].GetComponentInChildren<Button>().enabled = false;
      }
      else
      {
        tabBagPos[i].GetComponentInChildren<Text>().text = GameManager.tabBag[i].quantite.ToString();
        tabBagPos[i].GetComponent<Image>().color += new Color(0, 0, 0, 1);
        tabBagPos[i].GetComponentInChildren<Button>().interactable = true;
        tabBagPos[i].GetComponentInChildren<Button>().enabled = true;
      }
    }

    if (GameManager.craftfeu)
    {
      GameManager.craftfeu = false;
      GameManager.loop = false;
      CraftFeu();
    }
  }

  void Update()
  {
    System.TimeSpan ts = System.DateTime.UtcNow - startTime;

    //Jacob perd 4.166 sur jauge soif par heure (jauge vide au bout de 24h)
    //Jacob perd 2.083 sur jauge faim par heure (jauge vide au bout de 2 jours)
    //1 jour ingame est 12 min IRL



    if ((ts.Seconds % 30 == 0) && (ts.Seconds != tmpHeures)) //every 30 seconds IRL, every hour in game
    {
      calendrier = calendrier.AddHours(1.0);

      textHeure.text = calendrier.Hour.ToString() + ":00";
      textDate.text = calendrier.ToString("ddd dd MMM");
      if (calendrier.Hour % 3 == 0)
      {
        System.Random rnd = new System.Random();
        textMeteo.text = meteo[rnd.Next(0, meteo.Count)];
        meteoActuelle = textMeteo.text;
        setWeatherAspect(meteoActuelle);


      }
      ChangementLumiere();
      tmpHeures = ts.Seconds;
    }
    if (ts.Seconds != tmpSecondes) //every second IRL, every 2 min in game
    {
      soleil.transform.eulerAngles += new Vector3(0, 0, 0.5f);

      jaugeFaim.GetComponent<Slider>().value -= 0.069f;
      jaugeSoif.GetComponent<Slider>().value -= 0.138f;
      coefEnergie = GameManager.stateDeplacement;
      if (GameManager.isCollecting)
      {
        coefEnergie += 0.5f;
      }

      if(GameManager.inCabane && GameManager.stateCabane == 1)
      {
        jaugeEnergie.GetComponent<Slider>().value += 0.4f;
      }
      else
        jaugeEnergie.GetComponent<Slider>().value -= coefEnergie; //a modifier plus tard



      if (GameManager.inCabane && GameManager.stateCabane == 1)
      {
        jaugeTemperature.GetComponent<Slider>().value += 1;
      }
      else
        jaugeTemperature.GetComponent<Slider>().value -= GameManager.coefWeather; //a modifier plus tard


      jaugeVie.GetComponent<Slider>().value = CalculerJaugeVie();

      ClignotementJauge(ts.Seconds);

      tmpSecondes = ts.Seconds;
    }


  }

  void ChangementLumiere()
  {
    GameObject go = GameObject.Find("Directional Light");
    if ((calendrier.Hour >= 15 && calendrier.Hour < 16) || (calendrier.Hour >= 8 && calendrier.Hour < 9))
      go.GetComponent<Light>().color = new Color(166.0f / 255.0f, 169.0f / 255.0f, 183.0f / 255.0f);
    else if ((calendrier.Hour >= 16 && calendrier.Hour < 17) || (calendrier.Hour >= 7 && calendrier.Hour < 8))
      go.GetComponent<Light>().color = new Color(115.0f / 255.0f, 119.0f / 255.0f, 138.0f / 255.0f);
    else if ((calendrier.Hour >= 17) || (calendrier.Hour < 7))
      go.GetComponent<Light>().color = new Color(66.0f / 255.0f, 69.0f / 255.0f, 82.0f / 255.0f);
    else
    {
      go.GetComponent<Light>().color = new Color(1, 1, 1);
    }
  }

  void ClignotementJauge(float seconds)
  {
    if (jaugeVie.GetComponent<Slider>().value <= 20)
    {
      if (seconds % 2 == 0)
        jaugeVie.GetComponentInChildren<Image>().color = new Color(255, 255, 255, 0f);
      else
        jaugeVie.GetComponentInChildren<Image>().color = palette[0];
    }
    if (jaugeFaim.GetComponent<Slider>().value <= 20)
    {
      if (seconds % 2 == 0)
        jaugeFaim.GetComponentInChildren<Image>().color = new Color(255, 255, 255, 0f);
      else
        jaugeFaim.GetComponentInChildren<Image>().color = palette[1];
    }
    if (jaugeSoif.GetComponent<Slider>().value <= 20)
    {
      if (seconds % 2 == 0)
        jaugeSoif.GetComponentInChildren<Image>().color = new Color(255, 255, 255, 0f);
      else
        jaugeSoif.GetComponentInChildren<Image>().color = palette[2];
    }
    if (jaugeEnergie.GetComponent<Slider>().value <= 20)
    {
      if (seconds % 2 == 0)
        jaugeEnergie.GetComponentInChildren<Image>().color = new Color(255, 255, 255, 0f);
      else
        jaugeEnergie.GetComponentInChildren<Image>().color = palette[3];
    }
    if (jaugeTemperature.GetComponent<Slider>().value <= 20)
    {
      if (seconds % 2 == 0)
        jaugeTemperature.GetComponentInChildren<Image>().color = new Color(255, 255, 255, 0f);
      else
        jaugeTemperature.GetComponentInChildren<Image>().color = palette[4];
    }


  }


  float CalculerJaugeVie()
  {
    float result;
    if ((jaugeSoif.GetComponent<Slider>().value == 0) || (jaugeTemperature.GetComponent<Slider>().value == 0))
      result = 0;
    else
      result = (jaugeFaim.GetComponent<Slider>().value * 2 + jaugeSoif.GetComponent<Slider>().value * 3 + jaugeEnergie.GetComponent<Slider>().value * 1 + jaugeTemperature.GetComponent<Slider>().value * 3) / 9;

    return result;
  }

  public void UpdateQuantiteBag()
  {
    for (int i = 0; i < 8; i++)
    {
      if (tabBagPos[i].GetComponent<Image>().sprite != null)
      {
        if (tabBagPos[i].GetComponentInChildren<Text>().text == "0")
        {
          tabBagPos[i].GetComponentInChildren<Button>().interactable = false;
        }
      }
      else
      {
        tabBagPos[i].GetComponentInChildren<Button>().interactable = true;
      }
    }
  }


  public void OnClickBag(GameObject bouton)
  {
    if ((bouton.GetComponent<Image>().sprite.name.Contains("Viande")) || (bouton.GetComponent<Image>().sprite.name.Contains("Poisson")))
    {
      boutonManger.transform.position = bouton.transform.position + new Vector3(0.7f, 0, 0);
    }
    else if (bouton.GetComponent<Image>().sprite.name.Contains("Eau"))
    {
      boutonBoire.transform.position = bouton.transform.position + new Vector3(0.7f, 0, 0);
    }
    else if (bouton.GetComponent<Image>().sprite.name.Contains("Piege"))
    {
      boutonPoser.transform.position = bouton.transform.position + new Vector3(0.7f, 0, 0);
    }
    else
    {
      boutonUtiliser.transform.position = bouton.transform.position + new Vector3(0.7f, 0, 0);
    }

    gameObjectActuel = bouton;
  }


  public void OnClickPoser()
  {

    boutonPoser.transform.position += new Vector3(0, 0, -50);
    foreach (Ressource r in GameManager.items)
    {
      if (r.name == "Piege")
      {
        r.quantite--;
        if (r.quantite < 0)
          r.quantite = 0;

        gameObjectActuel.GetComponentInChildren<Text>().text = r.quantite.ToString();
      }

    }
    UpdateQuantiteBag();

  }

  public void OnClickUtiliser()
  {

    boutonUtiliser.transform.position += new Vector3(0, 0, -50);

  }

  public void OnClickManger()
  {

    boutonManger.transform.position += new Vector3(0, 0, -50);

    if (gameObjectActuel.GetComponent<Image>().sprite.name.Contains("Viande"))
    {
      foreach (Ressource r in GameManager.items)
      {
        if (r.name == "Viande")
        {
          r.quantite--;
          if (r.quantite < 0)
            r.quantite = 0;

          gameObjectActuel.GetComponentInChildren<Text>().text = r.quantite.ToString();
        }
      }

      jaugeFaim.GetComponent<Slider>().value += 5;
      UpdateQuantiteBag();
    }

    else if (gameObjectActuel.GetComponent<Image>().sprite.name.Contains("Poisson"))
    {
      foreach (Ressource r in GameManager.items)
      {
        if (r.name == "Poisson")
        {
          r.quantite--;
          if (r.quantite < 0)
            r.quantite = 0;

          gameObjectActuel.GetComponentInChildren<Text>().text = r.quantite.ToString();
        }
      }

      jaugeFaim.GetComponent<Slider>().value += 2;
      UpdateQuantiteBag();
    }

  }


  public void OnClickBoire()
  {
    boutonBoire.transform.position += new Vector3(0, 0, -50);

    foreach (Ressource r in GameManager.items)
    {
      if (r.name == "Eau")
      {
        r.quantite--;
        if (r.quantite < 0)
          r.quantite = 0;

        gameObjectActuel.GetComponentInChildren<Text>().text = r.quantite.ToString();
      }

    }

    jaugeSoif.GetComponent<Slider>().value += 10;
    UpdateQuantiteBag();
  }


  public void setWeatherAspect(string weather)
  {

    //A chaque changement de météo, on vide la liste des systèmes de particules utilisés pour simuler le temps et on les désactive
    if (currentWeather != null && currentWeather.Count != 0)
    {
      foreach (GameObject w in currentWeather)
      {
        w.SetActive(false);
      }
      Camera.main.GetComponent<D2FogsPE>().enabled = false;

    }

    currentWeather.Clear();


    //On active le système de particule en fonction de la météo 
    switch (weather)
    {
      case "Ensoleillé":
        GameManager.coefWeather = 0.033f;
        break;
      case "Couvert":
                GameManager.coefWeather = 0.036f;
        break;
      case "Neige":
        snowWeather.SetActive(true);
        currentWeather.Add(snowWeather);
                GameManager.coefWeather = 0.066f;
        break;
      case "Tempete":
        tempeteWeather1.SetActive(true);
        tempeteWeather2.SetActive(true);
        Camera.main.GetComponent<D2FogsPE>().enabled = true;
        currentWeather.Add(tempeteWeather1);
        currentWeather.Add(tempeteWeather2);
                GameManager.coefWeather = 0.099f;
        break;
      case "Brouillard":
        Camera.main.GetComponent<D2FogsPE>().enabled = true;
                GameManager.coefWeather = 0.040f;
        break;
    }


  }

}
