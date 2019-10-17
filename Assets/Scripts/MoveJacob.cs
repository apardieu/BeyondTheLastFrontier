using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJacob : MonoBehaviour
{

    public int movementspeed = 100;
  // Start is called before the first frame update
    void Start()
    {
        
    }

  void Update()
  {
    if (Input.GetKey(KeyCode.D))
    {
      transform.Translate(Vector3.left * movementspeed * Time.deltaTime);
    }
    else if (Input.GetKey(KeyCode.Q))
    {
      transform.Translate(Vector3.right * movementspeed * Time.deltaTime);
    }
    else if (Input.GetKey(KeyCode.Z))
    {
      transform.Translate(Vector3.up * movementspeed * Time.deltaTime);
    }
    else if (Input.GetKey(KeyCode.S))
    {
      transform.Translate(Vector3.down * movementspeed * Time.deltaTime);
    }

  }
}
