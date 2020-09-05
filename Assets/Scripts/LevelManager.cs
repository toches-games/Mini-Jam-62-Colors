using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    TilemapRenderer happyTilesRenderer;
    TilemapCollider2D happyTilesCollider;

    //Guarda los tiles del nivel en estado triste
    public GameObject sadTiles;
    TilemapRenderer sadTilesRenderer;
    TilemapCollider2D sadTilesCollider;

    //Estado actual del nivel
    [HideInInspector]
    public LevelState currentState = LevelState.Happy;

    //Solo es para mostrar el tiempo en pantalla
    int tempTime;

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

        happyTilesRenderer = happyTiles.GetComponent<TilemapRenderer>();
        happyTilesCollider = happyTiles.GetComponent<TilemapCollider2D>();

        sadTilesRenderer = sadTiles.GetComponent<TilemapRenderer>();
        sadTilesCollider = sadTiles.GetComponent<TilemapCollider2D>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        tempTime = nextStateTime;

        //Crea el nivel inicial con un estado aleatorio
        UpdateRandomTilesLevel();

        while (true)
        {
            //Cada tiempo se actualiza al siguiente estado
            yield return new WaitForSeconds(1);
            tempTime--;

            if(tempTime <= 0)
            {
                tempTime = nextStateTime;
                UpdateNextStateTiles();
            }
        }
    }

    void DisableTiles(TilemapRenderer tileRender, TilemapCollider2D tileCollider)
    {
        Color tempActiveColor = tileRender.material.color;
        tileRender.material.color = new Color(tempActiveColor.r, tempActiveColor.g, tempActiveColor.b, 0.15f);
        tileCollider.enabled = false;
    }

    void ActiveTiles(TilemapRenderer tileRender, TilemapCollider2D tileCollider)
    {
        Color tempActiveColor = tileRender.material.color;
        tileRender.material.color = new Color(tempActiveColor.r, tempActiveColor.g, tempActiveColor.b, 1);
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
            ActiveTiles(happyTilesRenderer, happyTilesCollider);
            DisableTiles(sadTilesRenderer, sadTilesCollider);
        }

        else
        {
            ActiveTiles(sadTilesRenderer, sadTilesCollider);
            DisableTiles(happyTilesRenderer, happyTilesCollider);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), tempTime.ToString());
    }
}
