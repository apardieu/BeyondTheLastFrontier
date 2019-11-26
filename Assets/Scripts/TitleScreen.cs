using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;


public class TitleScreen : MonoBehaviour
{
  private string savePath;

  void Start()
  {
    savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    if (!System.IO.File.Exists(savePath))
    {
      Button go = GameObject.Find("ButtonContinuerPartie").GetComponent<Button>();
      go.interactable = false;
    }
  }

  public void OnClickNouvellePartie()
  {
    GameManager.InitGameManager();
    SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
  }

  public void OnClickChargerPartie()
  {
    GameManager.saveMode = true;
    GameManager.InitGameManager();
    SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
  }

  public void OnClickQuitter()
  {
    Application.Quit();
  }
}
