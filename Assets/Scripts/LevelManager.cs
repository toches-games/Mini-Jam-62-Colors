using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Estados del nivel
public enum LevelState
{
    Happy,
    Sad
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager sharedInstance;

    [Header("Level Settings")]
    //Tiempo que se demorará en cambiar de estado el nivel
    [Range(1, 10)]
    public int nextStateTime = 5;

    [Header("Tile Gameobjects")]
    //Guarda los tiles del nivel en estado feliz
    public GameObject happyTiles;
    Tilemap happyTileMap;
    TilemapCollider2D happyTilesCollider;

    //Guarda los tiles del nivel en estado triste
    public GameObject sadTiles;
    Tilemap sadTileMap;
    TilemapCollider2D sadTilesCollider;

    //Estado actual del nivel
    [HideInInspector]
    public LevelState currentState = LevelState.Happy;

    //Solo es para mostrar el tiempo en pantalla
    [HideInInspector]
    public float tempTime;

    private void Awake()
    {
        //Singlenton
        if (!sharedInstance)
        {
            sharedInstance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        happyTileMap = happyTiles.GetComponent<Tilemap>();
        happyTilesCollider = happyTiles.GetComponent<TilemapCollider2D>();

        sadTileMap = sadTiles.GetComponent<Tilemap>();
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
        tempTime -= Time.deltaTime;

        if(tempTime <= 0)
        {
            UpdateNextStateTiles();
            tempTime = nextStateTime;
        }
    }

    //Desactiva los tiles
    void DisableTiles(Tilemap tileRender, TilemapCollider2D tileCollider)
    {
        Color tempActiveColor = tileRender.color;
        tileRender.color = new Color(tempActiveColor.r, tempActiveColor.g, tempActiveColor.b, 0.15f);
        tileCollider.enabled = false;
    }

    //Activa los tiles
    void ActiveTiles(Tilemap tileRender, TilemapCollider2D tileCollider)
    {
        Color tempActiveColor = tileRender.color;
        tileRender.color = new Color(tempActiveColor.r, tempActiveColor.g, tempActiveColor.b, 1);
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
        happyTiles.SetActive(true);
        sadTiles.SetActive(true);

        //Asigna al estado actual un estado random, se usa al inicio para que sea aleatorio cada vez que inicia el juego
        currentState = (LevelState)Random.Range(0, System.Enum.GetValues(typeof(LevelState)).Length);
        
        //Cambia los tiles del nivel
        ChangeTiles();
    }

    //Desactiva y activa los gameobjects correspondientes para cambiar el estado del nivel
    void ChangeTiles()
    {
        if (currentState == LevelState.Happy)
        {
            ActiveTiles(happyTileMap, happyTilesCollider);
            DisableTiles(sadTileMap, sadTilesCollider);
        }

        else
        {
            ActiveTiles(sadTileMap, sadTilesCollider);
            DisableTiles(happyTileMap, happyTilesCollider);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), tempTime.ToString());
    }
}
