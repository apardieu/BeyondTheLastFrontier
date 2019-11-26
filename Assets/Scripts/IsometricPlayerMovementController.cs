using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{

  public float movementSpeed = 1f;
  IsometricCharacterRenderer isoRenderer;
  Vector2 targetPosition; //Stocke la position dans le monde du clic du joueur
  bool moveWithClick;

  Rigidbody2D rbody;

  private void Awake()
  {
    rbody = GetComponent<Rigidbody2D>();
    isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
  }


  private void Start()
  {
    if (GameManager.cptJeu >= 0)
    {
      transform.position = GameManager.posJacob;
      //transform.rotation = GameManager.rotJacob;
    }
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (!GameManager.inCabane)
    {
      if (Input.GetKey(KeyCode.Space))
      {
        movementSpeed = 2f;
        gameObject.GetComponent<audioPlayer>().setPitch(2.0f);
      }
      else
      {
        movementSpeed = 1f;
        gameObject.GetComponent<audioPlayer>().setPitch(1.0f);
      }
      if (Input.GetMouseButton(0))
      {

        // Pour le déplacement au clic, en cours de réal
        //               moveWithClick = true;
        //               targetPosition =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //              Debug.Log("Clic position to reach : " + targetPosition);
      }
      Vector2 currentPos = rbody.position;
      Vector2 movement = new Vector2();


      if (Input.anyKey && !Input.GetMouseButton(0))
        moveWithClick = false;

      if (moveWithClick == false)
      {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        movement = inputVector * movementSpeed;
        if (inputVector.Equals(new Vector2(0, 0)))
        {
          GameManager.stateDeplacement = 0.033f;
        }
        else
        {
          GameManager.stateDeplacement = movementSpeed + 0.015f;
        }

      }
      else
      {
        Vector2 directionVector = new Vector2(targetPosition.x - currentPos.x, targetPosition.y - currentPos.y);
        directionVector = Vector2.ClampMagnitude(directionVector, 1);
        movement = directionVector * movementSpeed;
      }

      Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
      isoRenderer.SetDirection(movement);
      rbody.MovePosition(newPos);

      if (newPos == targetPosition)
        moveWithClick = false;
    }
  }
    
}
