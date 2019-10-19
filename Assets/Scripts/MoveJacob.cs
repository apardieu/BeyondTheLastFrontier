using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJacob : MonoBehaviour
{

    public int movementspeed = 100;
    public int rotationspeed = 100;
    public float ratio = 0.8f;
    public float ratio2 = 0.8f;
    private int prevDirection = 4;
  // Start is called before the first frame update
  void Start()
    {
    }

  void Update()
  {
    if (Input.GetKey(KeyCode.D)) //right
    {
      
      //transform.position = transform.position + new Vector3(movementspeed * Time.deltaTime, - ratio2 * movementspeed * Time.deltaTime, 0);
      transform.position = transform.position + new Vector3(ratio * movementspeed * Time.deltaTime, - movementspeed * Time.deltaTime, 0);

      //Vector3 movement = new Vector3(transform.rotation.eulerAngles.x, -45, transform.rotation.eulerAngles.z);
      //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,  0, transform.localEulerAngles.z); 
      if (prevDirection != 1)
      {
        

        if(prevDirection == 2)
          transform.localRotation *= Quaternion.AngleAxis(180, Vector3.up);
        else if(prevDirection == 3)
          transform.localRotation *= Quaternion.AngleAxis(90, Vector3.up);
        else if(prevDirection == 4)
          transform.localRotation *= Quaternion.AngleAxis(270, Vector3.up);
        prevDirection = 1;
      }
    

  }
    if (Input.GetKey(KeyCode.Q)) //left
    {
     // transform.position = transform.position + new Vector3(- (movementspeed * Time.deltaTime), ratio2 * movementspeed * Time.deltaTime, 0);
      transform.position = transform.position + new Vector3(- ratio * (movementspeed * Time.deltaTime), movementspeed * Time.deltaTime, 0);

      if (prevDirection != 2)
      {
        if (prevDirection == 1)
          transform.localRotation *= Quaternion.AngleAxis(180, Vector3.up);
        else if (prevDirection == 3)
          transform.localRotation *= Quaternion.AngleAxis(270, Vector3.up);
        else if (prevDirection == 4)
          transform.localRotation *= Quaternion.AngleAxis(90, Vector3.up);
        prevDirection = 2;
      }
    }
    if (Input.GetKey(KeyCode.Z)) //up
    {
      transform.position = transform.position + new Vector3(ratio * movementspeed * Time.deltaTime, movementspeed * Time.deltaTime, 0);
      if (prevDirection != 3)
      {
        if (prevDirection == 1)
          transform.localRotation *= Quaternion.AngleAxis(270, Vector3.up);
        else if (prevDirection == 2)
          transform.localRotation *= Quaternion.AngleAxis(90, Vector3.up);
        else if (prevDirection == 4)
          transform.localRotation *= Quaternion.AngleAxis(180, Vector3.up);
        prevDirection = 3;
      }
    }
    if (Input.GetKey(KeyCode.S)) //down
    {
      transform.position = transform.position + new Vector3(- ratio * movementspeed * Time.deltaTime, -(movementspeed * Time.deltaTime), 0);
      if (prevDirection != 4)
      {
        if (prevDirection == 1)
          transform.localRotation *= Quaternion.AngleAxis(90, Vector3.up);
        else if (prevDirection == 2)
          transform.localRotation *= Quaternion.AngleAxis(270, Vector3.up);
        else if (prevDirection == 3)
          transform.localRotation *= Quaternion.AngleAxis(180, Vector3.up);
        prevDirection = 4;
      }
    }

  }
}
