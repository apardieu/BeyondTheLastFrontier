using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

  private System.DateTime calendrier;
  private float tmpSecondes = -1;
  private float tmpHeures = -1;
  private System.DateTime startTime;
  private List<string> meteo = new List<string>();
  private List<Color> palette = new List<Color>();

  void Start()
  {
    startTime = System.DateTime.UtcNow;
    calendrier = new System.DateTime(2019, 9, 1, 11, 0, 0);
    meteo.Add("Ensoleillé");
    meteo.Add("Couvert");
    meteo.Add("Neige");
    meteo.Add("Brouillard");
    meteo.Add("Vent fort");
    meteo.Add("Tempete");
    meteo.Add("Grèle");

    palette.Add(jaugeVie.GetComponentInChildren<Image>().color);
    palette.Add(jaugeFaim.GetComponentInChildren<Image>().color);
    palette.Add(jaugeSoif.GetComponentInChildren<Image>().color);
    palette.Add(jaugeEnergie.GetComponentInChildren<Image>().color);
    palette.Add(jaugeTemperature.GetComponentInChildren<Image>().color);
  }

  void Update()
  {
    System.TimeSpan ts = System.DateTime.UtcNow - startTime;

    //Jacob perd 4.166 sur jauge soif par heure (jauge vide au bout de 24h)
    //Jacob perd 2.083 sur jauge faim par heure (jauge vide au bout de 2 jours)
    //1 jour ingame est 12 min IRL

    if((ts.Seconds%30 == 0) && (ts.Seconds != tmpHeures)) //every 30 seconds IRL, every hour in game
    {
      calendrier = calendrier.AddHours(1.0);
      textHeure.text = calendrier.Hour.ToString() + ":00";
      textDate.text = calendrier.ToString("ddd dd MMM");
      if(calendrier.Hour %3 == 0)
      {
        System.Random rnd = new System.Random();
        textMeteo.text = meteo[rnd.Next(0, meteo.Count)];
      }
      tmpHeures = ts.Seconds;
    }
    if(ts.Seconds != tmpSecondes) //every second IRL, every 2 min in game
    {
      soleil.transform.eulerAngles += new Vector3(0,0,0.5f);

      jaugeFaim.GetComponent<Slider>().value -= 0.069f;
      jaugeSoif.GetComponent<Slider>().value -= 0.138f;
      jaugeEnergie.GetComponent<Slider>().value -= 0.033f; //a modifier plus tard
      jaugeTemperature.GetComponent<Slider>().value -= 0.033f; //a modifier plus tard
      jaugeVie.GetComponent<Slider>().value = CalculerJaugeVie();

      ClignotementJauge(ts.Seconds);

      tmpSecondes = ts.Seconds;
    }

    
  }



  void ClignotementJauge(float seconds)
  {
    if(jaugeVie.GetComponent<Slider>().value <= 20)
    {
      if(seconds%2 == 0)
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
      result = (jaugeFaim.GetComponent<Slider>().value*2 + jaugeSoif.GetComponent<Slider>().value*3 + jaugeEnergie.GetComponent<Slider>().value*1 + jaugeTemperature.GetComponent<Slider>().value*3) / 9;

    return result;
  }
}
