using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "TilemapSave", order = 1)]
public class TilemapSave : ScriptableObject
{
  // Start is called before the first frame update
  public GameObject tm;

  private void Awake()
  {
    Debug.Log("coucou");
    GameObject tm2 = GameObject.Find("Snow - Harvestable");
    tm = Instantiate(tm2);

    tm.hideFlags = HideFlags.DontSave;
  }

  void Start()
    {
    Debug.Log("coucou");
  }

    // Update is called once per frame
    void Update()
    {
        
    }
}
