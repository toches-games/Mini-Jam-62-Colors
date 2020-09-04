using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Playables;
//using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";

    public float runningSpeed = 5f;
    public float jumpForce = 3f;
    public float smoothMovement = 0.5f;
    //public float gravity = 10f;
    public LayerMask groundMask;
    
    Rigidbody2D playerRb;
    Vector3 startPosition;
    Animator animator;

    bool isWalking;

    public float jumpRaycastDistance = 0.6f;
    //public float positionSidesRaycast = 0.3f;

    Vector2 positionLeft;
    Vector2 positionRight;
    Vector2 positionCenter;

    float lastMovement = 1f;

    BoxCollider2D boxCollider;

    Vector2 velocity;

    /*
    //public AudioSource fallDown;
    const string IS_ALIVE = "isAlive";
    const string IS_ON_THE_GROUND = "isOnTheGround";
    const string VERTICAL_FORCE = "verticalForce";
    const string HORIZONTAL_FORCE = "horizontalForce";
    const string DAMAGE_ENEMY = "damageEnemy";
    const string LAST_HORIZONTAL = "LastHorizontal";
    const string WALKING_STATE = "Walking";
    const string IS_TOUCHING_GROUND = "isTouchingGround";
    */

    /*
    int healthPoints, manaPoints;

    public const int INITIAL_HEALTH = 3, INITIAL_MANA = 15,
        MIN_HEALTH = 1, MIN_MANA = 0, MAX_HEALTH = 3, MAX_MANA = 30;

    public const int SUPERJUMP_COST = 10;
    public const float SUPERJUMP_FORCE = 2f;

    public List<Image>lives;
    */

    //Variables para el conseguir el swipe up y saltar
    //Vector2 startTouchPosition, endTouchPosition;

    //variable publica donde meteremos la referencia a la capa o layer 
    //con la que vamos a referenciar los rayos o raycast

    /*
    //GameState currentState;
    SpriteRenderer sprite;
    //SFXManager sfx;
    */

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        //sprite = GetComponent<SpriteRenderer>();
        //sfx = FindObjectOfType<SFXManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        /*
        runningSpeed = GameManager.sharedInstance.gameSpeed;
        //GameObject.Find("CM vcam3").GetComponent<CinemachineVirtualCamera>().Priority++;
        //playerRb.MoveRotation(Quaternion.Euler(0, 90, 0));
        //transform.rotation = Quaternion.Euler(0, 90, 0);
        //transform.Rotate(0, 90, 0);
        //playerRb.rotation = Quaternion.Euler(0, 90, 0);
        healthPoints = INITIAL_HEALTH;
        */

    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        //Hacemos que la velocidad vaya avanzando de manera suavizada para que se vea un movimiento fluido
        Vector2 targetVelocity = new Vector2(lastMovement, playerRb.velocity.y);
        playerRb.velocity = Vector2.SmoothDamp(playerRb.velocity, targetVelocity, ref velocity, smoothMovement);

        //Se hallan las posiciones de los raycast para el suelo, en los extremos y el centro del box collider
        positionLeft = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y);
        positionRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y);
        positionCenter = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
        
        //Muestra los raycast en la scena
        Debug.DrawRay(positionLeft, -transform.up * jumpRaycastDistance, Color.blue);
        Debug.DrawRay(positionRight, -transform.up * jumpRaycastDistance, Color.blue);
        Debug.DrawRay(positionCenter, -transform.up * jumpRaycastDistance, Color.blue);

        /*
        currentState = GameManager.sharedInstance.currentGameState;
        if (currentState == GameState.menu || currentState == GameState.gameOver)
        {
            return;
        }

        switch (currentState)
        {
            case GameState.lvl1:
                playerRb.velocity = new Vector3(lastMovement,
                                                playerRb.velocity.y, 0);
                positionLeft = new Vector3(transform.position.x - positionSidesRaycast,
                                    transform.position.y, transform.position.z);
                positionRight = new Vector3(transform.position.x + positionSidesRaycast,
                                            transform.position.y, transform.position.z);
                jumpForce = 10;

                break;

            case GameState.lvl2:
                playerRb.velocity = new Vector3(0, playerRb.velocity.y,
                                                -lastMovement);
                positionLeft = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z + positionSidesRaycast);
                positionRight = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z - positionSidesRaycast);

                break;

            case GameState.lvl3:
                playerRb.velocity = new Vector3(-lastMovement,
                                                playerRb.velocity.y, 0);
                positionLeft = new Vector3(transform.position.x + positionSidesRaycast,
                                            transform.position.y, transform.position.z);
                positionRight = new Vector3(transform.position.x - positionSidesRaycast,
                                            transform.position.y, transform.position.z);

                break;

            case GameState.lvl4:
                playerRb.velocity = new Vector3(0, playerRb.velocity.y,
                                                lastMovement);
                positionLeft = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z + positionSidesRaycast);
                positionRight = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z - positionSidesRaycast);
                
                break;

            case GameState.lvl5:
                playerRb.velocity = new Vector3(0, playerRb.velocity.y,
                                                lastMovement);
                positionLeft = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z + positionSidesRaycast);
                positionRight = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z - positionSidesRaycast);
                jumpForce = 7;

                break;
        }

        
        
        //Debug.DrawRay(this.transform.position, Vector3.down * jumpRaycastDistance, Color.red);
        Debug.DrawRay(positionLeft, -transform.up * jumpRaycastDistance, Color.red);
        Debug.DrawRay(positionRight, -transform.up * jumpRaycastDistance, Color.red);
        //Debug.Log(positionRight);
        //Debug.Log(positionLeft);
        //Debug.DrawRay(this.transform.position, Vector3.down * jumpRaycastDistance, Color.red);
        


        */
        isWalking = false;
        
        if (Input.GetAxisRaw(HORIZONTAL) >= 0 || Input.GetAxisRaw(HORIZONTAL) <= 0)
        {
            isWalking = true;
            lastMovement = Input.GetAxis(HORIZONTAL) * runningSpeed;
        }

        if (!isWalking)
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            //sfx.paso.Stop();
        }

        //Si está en el aire
        if (!IsTouchingTheGround()){
            //Se le aplica una fuerza hacia abajo para que caiga
            //playerRb.AddForce(Vector2.up * -gravity);
        }

        else{
            //Si está en el suelo se pone la velocidad en y a 0 para que
            //no se ralentice por el peso de la gravedad que traia al caer
            //playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);

            if (isWalking)
            {
                /*if (!sfx.paso.isPlaying)
                {
                    sfx.paso.Play();
                }*/
            }
        }

        /*
        animator.SetFloat(HORIZONTAL, Input.GetAxis(HORIZONTAL));
        animator.SetBool(WALKING_STATE, walking);
        animator.SetFloat(LAST_HORIZONTAL, lastMovement);
        animator.SetFloat(VERTICAL, playerRb.velocity.y);
        animator.SetBool(IS_TOUCHING_GROUND, IsTouchingTheGround());
        */

    }

    public void Jump()
    {
        if (IsTouchingTheGround())
        {
            //ForceMode2D me dos opcionesm force que seria como una 
            //fuerza constante e impulse que seria como aplicar una
            //fuerza en instante nada mas.
            
            playerRb.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //GetComponent<AudioSource>().Play();
            //sfx.salto.Play();
        }
    }

    //Swipe up
    void SwipeCheck()
    {
        /*
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            startTouchPosition = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;
            if (endTouchPosition.y - 200f > startTouchPosition.y)
            {
                //jumpSuper = true;
            }
            else
            {
                //jumpNormal = true;
            }
        
        }
        //Debug.Log(startTouchPosition.y);
        //Debug.Log(endTouchPosition.y);
        */
    }

    bool IsTouchingTheGround()
    {
        if (Physics2D.Raycast(positionRight,
            -transform.up, jumpRaycastDistance, groundMask))
        {

            return true;
        }else
        if (Physics2D.Raycast(positionLeft,
            -transform.up, jumpRaycastDistance, groundMask))
        {

            return true;
        }else if(Physics2D.Raycast(positionCenter, -transform.up, jumpRaycastDistance, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }

        /**
        if (Physics.Raycast(positionLeft,
            Vector3.down, jumpRaycastDistance, groundMask))
        {
            return true;
        }
        else
        if (Physics.Raycast(positionRight,
            Vector3.down, jumpRaycastDistance, groundMask))
        {
            return true;
        }

        if (Physics.Raycast(this.transform.position, 
            Vector3.down, jumpRaycastDistance, groundMask))
        {
            Debug.Log("Ground");

            return true;
        
        }
        **/
    }

    public void Die()
    {
        //float travelledDistance = GetTraveledDistance();
        //PlayerPrefs funciona como una variable que puede guardar y permite
        //acceder a valores entre sesiones de juego (investigar mas)
        //float previousMaxDistance = PlayerPrefs.GetFloat("maxScore", 0f);
        //if (travelledDistance > previousMaxDistance)
        //{
        //    PlayerPrefs.SetFloat("maxScore", travelledDistance);
        //}
        //sfx.muerte.Play();
        //animator.SetBool(IS_ALIVE, false);
        //GameManager.sharedInstance.GameOver();
        //sfx.paso.Stop();
        //sfx.salto.Stop();
    }

    void RestartPosition()
    {
        if(this.transform.position.x >= 0f)
        {
            this.transform.position = startPosition;
        }

        this.playerRb.velocity = Vector2.zero;

        //Camera.main.GetComponent<CameraFollow>().ResetCameraPosition();

        //GameObject mainCamera = GameObject.Find("Main Camera");
        //mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    public void CollectHealth(int points)
    {
        /*
        this.healthPoints += points;
        lives[healthPoints].gameObject.SetActive(false);

        if (this.healthPoints > MAX_HEALTH)
        {
            healthPoints = MAX_HEALTH;
        }
        if (this.healthPoints <= 0)
        {
            Die();
        }
        else
        {
            sfx.herida.Play();
        }
        */
    }

    public void CollectMana(int points)
    {
        /*
        this.manaPoints += points;
        if (this.manaPoints > MAX_MANA)
        {
            manaPoints = MAX_MANA;
        }
        */
    }

    /*public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }*/

    public float GetTraveledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }

    public void StopAllSFX()
    {
        //sfx.salto.Stop();
        //sfx.paso.Stop();

    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        
        if (other.CompareTag("initAnimation"))
        {
            //  Director.Play();
            GameManager.sharedInstance.NextLevel(((int)GameManager.sharedInstance.currentGameState) + 1);
            Debug.Log(((int)GameManager.sharedInstance.currentGameState) + 1);
            playerRb.freezeRotation = false;
            other.GetComponent<BoxCollider>().enabled = false;
            sfx.alerta.Play();
        }
        if (other.CompareTag("finishAnimation"))
        {
            playerRb.freezeRotation = true;
        }
        */
    }
    private void OnTriggerExit(Collider other)
    {
        /*
        if (other.CompareTag("finishAnimation"))
        {
            //playableDirector.Stop();
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            //Destroy(GameObject.Find("Platforms" + ((int)GameManager.sharedInstance.currentGameState - 1)));
        }
        if (other.CompareTag("KillZone"))
        {
            Die();
        }

        */
    }

}