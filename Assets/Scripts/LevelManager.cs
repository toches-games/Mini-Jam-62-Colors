using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

//Estados del nivel
public enum LevelState
{
    Happy,
    Sad
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager sharedInstance;

    [Header("Dissolve Effect")]
    //Tiempo en el que iniciará el efecto
    public float dissolveInitTime = 0.5f;
    //Tiempo de duración del efecto
    public float dissolveSmoothTime = 0.25f;

    [Header("Level Settings")]
    //Tiempo que se demorará en cambiar de estado el nivel
    [Range(1, 10)]
    public int nextStateTime = 5;

    [Header("Tile Gameobjects")]
    //Guarda los tiles del nivel en estado feliz
    public GameObject happyTiles;
    //Con este cambiamos el alpha a transparente de los tiles desactivados
    Tilemap happyTileMap;
    //Con este se hace la animación del material con el shader Dissolve
    TilemapRenderer happyTileMapRenderer;
    //Guarda los collider de los tiles happy
    TilemapCollider2D happyTilesCollider;

    //Guarda los tiles del nivel en estado triste
    public GameObject sadTiles;
    //Con este cambiamos el alpha a transparente de los tiles desactivados
    Tilemap sadTileMap;
    //Con este se hace la animación del material con el shader Dissolve
    TilemapRenderer sadTileMapRenderer;
    //Guarda los collider de los tiles sad
    TilemapCollider2D sadTilesCollider;

    //Estado actual del nivel
    [HideInInspector]
    public LevelState currentState = LevelState.Happy;

    //Solo es para mostrar el tiempo en pantalla
    [HideInInspector]
    public float tempTime;

    //Guarda el valor que se usará para hacer el effecto de dissolve
    float smoothValue;

    //Velocidad del metodo SmoothDamp
    float velocity;

    public PlayerController player;
    public Slider slider;
    public GameObject freezingTime;

    private void Awake()
    {
        //Singlenton
        if (!sharedInstance)
        {
            sharedInstance = this;
        }

        else
        {
            Destroy(this);
            return;
        }

        happyTileMap = happyTiles.GetComponent<Tilemap>();
        happyTileMapRenderer = happyTiles.GetComponent<TilemapRenderer>();
        happyTilesCollider = happyTiles.GetComponent<TilemapCollider2D>();

        sadTileMap = sadTiles.GetComponent<Tilemap>();
        sadTileMapRenderer = sadTiles.GetComponent<TilemapRenderer>();
        sadTilesCollider = sadTiles.GetComponent<TilemapCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        tempTime = nextStateTime;

        //Crea el nivel inicial con un estado aleatorio
        UpdateRandomTilesLevel();
    }

    void Update()
    {
        if (player.GetPauseGame())
        {
            return;
        }

        tempTime -= Time.deltaTime;

        if(tempTime <= 0)
        {
            tempTime = nextStateTime;
            smoothValue = 0;
            UpdateNextStateTiles();
        }

        else if(tempTime <= dissolveInitTime)
        {
            smoothValue = Mathf.SmoothDamp(smoothValue, 1f, ref velocity, dissolveSmoothTime);
            DissolveEffect();
        }

    }

    void DissolveEffect()
    {
        if (currentState == LevelState.Happy)
        {
            happyTileMapRenderer.material.SetFloat("Fade", 1 - smoothValue + 0.15f);
            happyTileMap.color = new Color(happyTileMap.color.r, happyTileMap.color.g, happyTileMap.color.b, 1 - smoothValue + 0.15f);

            sadTileMapRenderer.material.SetFloat("Fade", smoothValue);
            sadTileMap.color = new Color(sadTileMap.color.r, sadTileMap.color.g, sadTileMap.color.b, smoothValue);
        }

        else
        {
            sadTileMapRenderer.material.SetFloat("Fade", 1 - smoothValue + 0.15f);
            sadTileMap.color = new Color(sadTileMap.color.r, sadTileMap.color.g, sadTileMap.color.b, 1 - smoothValue + 0.15f);

            happyTileMapRenderer.material.SetFloat("Fade", smoothValue);
            happyTileMap.color = new Color(happyTileMap.color.r, happyTileMap.color.g, happyTileMap.color.b, smoothValue);
        }
    }

    //Desactiva los tiles
    void DisableTiles(Tilemap tileMap, TilemapCollider2D tileCollider, TilemapRenderer tileRenderer)
    {
        tileMap.color = new Color(tileMap.color.r, tileMap.color.g, tileMap.color.b, 0.15f);

        tileRenderer.material.SetFloat("Fade", 1);
        tileCollider.enabled = false;
    }

    //Activa los tiles
    void ActiveTiles(Tilemap tileMap, TilemapCollider2D tileCollider, TilemapRenderer tileRenderer)
    {
        tileMap.color = new Color(tileMap.color.r, tileMap.color.g, tileMap.color.b, 1f);

        tileRenderer.material.SetFloat("Fade", 1);
        tileCollider.enabled = true;
    }

    //Asigna al estado actual el siguiente estado para cambiarlo
    void UpdateNextStateTiles()
    {
        //Asigna al estado actual el siguiente estado para cambiarlo
        if (currentState == LevelState.Happy)
        {
            currentState = LevelState.Sad;
        }

        else
        {
            currentState = LevelState.Happy;
        }

        //Cambia los tiles del nivel
        ChangeTiles();
    }

    //Asigna al estado actual un estado random, se usa al inicio para que sea aleatorio cada vez que inicia el juego
    void UpdateRandomTilesLevel()
    {
        //Como solo se llama al inicio, activamos los dos objetos que guardan todos los tiles aqui
        //happyTiles.SetActive(true);
        //sadTiles.SetActive(true);

        //Asigna al estado actual un estado random, se usa al inicio para que sea aleatorio cada vez que inicia el juego
        //currentState = (LevelState)Random.Range(0, System.Enum.GetValues(typeof(LevelState)).Length);
        currentState = LevelState.Happy;
        //Cambia los tiles del nivel
        ChangeTiles();
    }

    //Desactiva y activa los gameobjects correspondientes para cambiar el estado del nivel
    void ChangeTiles()
    {
        if (currentState == LevelState.Happy)
        {
            ActiveTiles(happyTileMap, happyTilesCollider, happyTileMapRenderer);
            DisableTiles(sadTileMap, sadTilesCollider, sadTileMapRenderer);
        }

        else
        {
            ActiveTiles(sadTileMap, sadTilesCollider, sadTileMapRenderer);
            DisableTiles(happyTileMap, happyTilesCollider, happyTileMapRenderer);
        }
    }

    /*private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), tempTime.ToString());
    }*/

    public void ResetGame()
    {
        SceneManager.LoadScene(0); 
    }

    //Metodo que será llamado cuando querramos reanudar el juego luego de 
    //una pausa (presionando el 'Resume Button')
    public void CountDownResume(bool ready = false)
    {
        //Si entra por primera vez
        if (!ready)
        {
            PauseGame();
            //player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100);
            //Activamos el texto del temporizador
            freezingTime.SetActive(true);

            //Iniciamos la corrutina para el temporizador
            StartCoroutine("CountDown");
        }
        //Si el temporizador acabó
        if (ready)
        {
            //Detenemos la corrutina del temporizador
            StopCoroutine("CountDown");

            //Desactivamos el texto del temporizador
            freezingTime.SetActive(false);
            //Iniciamos el juego 
            ResumeGame();
            //Reactivamos el boton de pausa
            //userInterfaceManager.pauseButton.gameObject.SetActive(true);
        }
    }

    //Corrutina que llevará a cabo el temporizador para reanudar el juego
    public IEnumerator CountDown()
    {
        //Ciclo que decrementa cada numero a mostrar
        for (int i = 5; i >= 1; i--)
        {
            //Asignamos el valor del ciclo al texto del temporizador
            freezingTime.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            //Retornamos la corrutina durante 1 segundo
            yield return new WaitForSeconds(1f);
        }
        //Volvemos a llamar al metodo de reanudar partida con el parametro
        //que permite llevar a cabo esto
        CountDownResume(true);
    }

    public void PauseGame()
    {
        player.PauseGame();
        //StopCoroutine("Start");
        tempTime = nextStateTime;
    }

    public void ResumeGame()
    {
        player.ResumeGame();
        tempTime = nextStateTime;
        //StartCoroutine("Start");
    }

    public IEnumerator UpdateBipolarityBar(float target)
    {
        for (float i = slider.value; i < target; i+=0.1f)
        {
            slider.value = i;
            yield return new WaitForSeconds(0.025f);
        }
    }
    public float CalculateVariability(int changeTime)
    {
        float target;
        switch (changeTime)
        {
            case 1:
                target = 10;
                break;
            case 2:
                target = 8;
                break;
            case 5:
                target = 1;
                break;
            default:
                target = 1;
                break;
        }
        return target;
    }

    public void FinishGame()
    {
        PauseGame();
        GameObject.Find("FinishTimeLine").GetComponent<PlayableDirector>().Play();
    }
}
