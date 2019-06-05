using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    //Objetos escena
    [Header("Scene elements")]
    public Disparador Disparador;
    public DeadZone deadZone;
    public GameObject gameField;
    public GameObject backGround;
    public GameObject spriteDisparador;
    public GameObject pauseMenu;
    public Button returnBolasButton;
    public Text contPuntosText;
    public Image pointsBar;
    public Image star;
    public Image accelerationIcon;
    public float flashSpeed = 5;
    private int contStars = 3;

    public Sprite sprite;
    [HideInInspector] public GameObject spriteField;
    GameObject muroIzquierdo;
    GameObject muroDerecho;
    GameObject muroArriba;

    [Header("Camera settings")]
    public float TARGET_WIDTH = 720.0f;
    public float TARGET_HEIGHT = 1019.0f; //919 + 100 pixels extra
    public int PIXELS_TO_UNITS = 30; // 1:1 ratio of pixels to units

    //Prefabs
    public GameObject muro;
    public List<Bloque> prefabsBloques;

    //Listas
    [HideInInspector] public List<Bloque> listaBloques;
    [HideInInspector] public List<Ball> listaBolas;
    [HideInInspector] public List<GameObject> objetosADestruir;

    //Variables para controlar el disparo
    [HideInInspector] public bool disparoIniciado = false;
    [HideInInspector] public int numBloques = 0;
    [HideInInspector] public int numBolasAux = 0;
    [HideInInspector] public int contTemporal = 0;
    [HideInInspector] public int bolasIniciales = 0;
    [HideInInspector] public int numDisparos = 0; //Controlar el num de disparos por partida   

    public Text powerUpText;

    [HideInInspector] public int combo = 0;

    [HideInInspector] public bool nivelCompletado = false;

    // Use this for initialization
    void Start()
    {
        listaBloques = new List<Bloque>();
        objetosADestruir = new List<GameObject>();
        instance = this;
        iniciarObjetos();
        colocarObjetos();
        if (accelerationIcon != null)
        {
            accelerationIcon.color = Color.clear;
            accelerationIcon.gameObject.SetActive(true);
        }
        if (pointsBar != null)
        {
            pointsBar.fillAmount = 0;
            pauseMenu.SetActive(false);
        }

        if (powerUpText != null)
            powerUpText.text = Game.currentGame.powerUp.ToString();

        nivelCompletado = false;

        if (SceneManager.GetActiveScene().name == "Juego")
        {
            GameManager.instance.level = Level.currentLevel.levelID;
            GameManager.instance.ReadLevel("mapdata" + GameManager.instance.level); //Esto le llama el GameManager, que carga el archivo de guardado
            Level.currentLevel.maxScore = (numBloques * numBloques) * 2 * Level.currentLevel.levelID;
            Debug.Log("Numero Nivel: " + Level.currentLevel.levelID);
            numBolasAux = Disparador.getNumBolas(); //Cogemos el numero de bolas antes de disparar
            bolasIniciales = numBolasAux;
            Disparador.SetPosAux(Disparador.transform.position);
        }
        Alertar(false);
        ResizeCamera();
    }

    public void powerUpTerremoto()
    {
        if (Game.currentGame.powerUp > 0)
        {
            Game.currentGame.powerUp--;
            powerUpText.text = Game.currentGame.powerUp.ToString();

            for (int i = 0; i < listaBloques.Count; i++)
            {
                if (listaBloques[i] != null)
                {
                    listaBloques[i].contGolpes -= 4;
                    listaBloques[i].AddText(listaBloques[i].contGolpes.ToString());
                    listaBloques[i].cambiarColor();
                    listaBloques[i].Shake();
                    listaBloques[i].comprobarGolpes();
                }
            }
        }
    }

    //Aparece la pantalla en rojo
    public void Alertar(bool b)
    {
        spriteField.GetComponent<SpriteRenderer>().enabled = b;
    }

    void destruirObjetos()
    {
        if (objetosADestruir.Count != 0)
        {
            foreach (GameObject gameO in objetosADestruir)
                Destroy(gameO);

            objetosADestruir.Clear();
        }
    }

    private void FixedUpdate()
    {
        ResizeCamera();
        if (contPuntosText != null)
            contPuntosText.text = Level.currentLevel.score.ToString();

        if (!GameManager.instance.anuncioActivo)
        {
            if (Input.GetButtonUp("Jump"))
            {
                BotonVolverSpawner();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ReiniciarNivel();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SiguienteNivel();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                TogglePause();
        }
        destruirObjetos();
    }

    void iniciarObjetos()
    {
        muroIzquierdo = Instantiate(muro);
        muroDerecho = Instantiate(muro);
        muroArriba = Instantiate(muro);

        spriteField = new GameObject("SpriteField");
        spriteField.AddComponent<SpriteRenderer>().sprite = sprite;

        //Sprite (colocado en el centro del viewport)
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
        spriteField.transform.position = new Vector3(pos.x, pos.y, 0);

        //Gamefield        
        Renderer rend = spriteField.GetComponent<Renderer>(); //Cogemos bounds del sprite
        gameField.transform.position = new Vector3(rend.bounds.min.x, rend.bounds.max.y, 0); //Esquina sup izq

        //Disparador
        if (Disparador != null)
            Disparador.SetPosition(new Vector3(rend.bounds.center.x, rend.bounds.min.y, 0));
    }

    void colocarObjetos()
    {
        //Sprite
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
        spriteField.transform.position = new Vector3(pos.x, pos.y, 0);
        backGround.transform.position = spriteField.transform.position;

        //Gamefield        
        Renderer rend = spriteField.GetComponent<Renderer>(); //Cogemos bounds del sprite
        gameField.transform.position = new Vector3(rend.bounds.min.x, rend.bounds.max.y, 0); //Esquina sup izq

        //Deadzone        
        deadZone.SetPosition(new Vector3(rend.bounds.center.x, rend.bounds.min.y, 0));
        deadZone.SetScale(new Vector3(50, 0.2f, 0.2f));

        //M-Izq        
        muroIzquierdo.transform.position = new Vector3(rend.bounds.min.x, rend.bounds.center.y, 0);
        muroIzquierdo.transform.localScale = new Vector3(0.1f, 50, 10);

        //M-dch        
        muroDerecho.transform.position = new Vector3(rend.bounds.max.x, rend.bounds.center.y, 0);
        muroDerecho.transform.localScale = new Vector3(0.1f, 50, 10);

        //M-top        
        muroArriba.transform.position = new Vector3(rend.bounds.center.x, rend.bounds.max.y, 0);
        muroArriba.transform.localScale = new Vector3(50, 0.1f, 10);

        //Disparador
        if (Disparador != null)
            Disparador.SetPosition(new Vector3(Disparador.transform.position.x, rend.bounds.min.y + 0.5f, 0));
    }

    //Aceleramos las pelotas cada 6 segundos
    IEnumerator iniciarContador(int iteracion)
    {
        yield return new WaitForSeconds(5);

        accelerationIcon.color = new Color(1, 1, 1, 0.7f);
        StopCoroutine(imagenAceleracion());
        StartCoroutine(imagenAceleracion());

        //Si aun quedan bolas desperdigadas de un disparo concreto, recogemos
        if (listaBolas.Count > 0 && numDisparos == iteracion)
        {
            //Si alguna pelota está fuera del gameField, la hacemos volver
            for (int i = 0; i < listaBolas.Count; i++)
            {
                if (!dentroPantalla(listaBolas[i].transform.position))
                    listaBolas[i].MoveTo(Disparador.transform.position, 20, destruyePelota);
                else
                {
                    if (listaBolas[i].GetComponent<Rigidbody2D>() != null)
                        listaBolas[i].GetRigidbody().velocity *= 1.5f;
                }
            }

            yield return iniciarContador(iteracion);
        }
        else
            yield break;
    }

    IEnumerator imagenAceleracion()
    {
        bool termino = false;

        while (!termino)
        {
            accelerationIcon.color = Color.Lerp(accelerationIcon.color, Color.clear, flashSpeed * Time.deltaTime);

            if (accelerationIcon.color == Color.clear || !disparoIniciado)
            {
                accelerationIcon.color = Color.clear;
                termino = true;
            }

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    public void llegadaPelota(Ball p)
    {
        contTemporal++;

        if (contTemporal == 1)
        { //Si es la primera pelota, guardas futura posicion del disparador               
            Disparador.SetPosAux(new Vector3(p.transform.position.x, Disparador.transform.position.y, 0));
        }
        Disparador.SetTexto(contTemporal);

        p.MoveTo(Disparador.posicionAux, 10, destruyePelota); //Cuando finalice llamamos a destruyepelota

        if (Disparador.getNumBolas() <= 0 && contTemporal >= numBolasAux) //Si tiene las mismas o mas bolas que antes de iniciar el disparo, podrá volver a disparar
        {
            Disparador.SetContBolas(contTemporal);
            Disparador.SetPosition(Disparador.getPosAux());
            contTemporal = 0;
            disparoIniciado = false;
            returnBolasButton.gameObject.SetActive(false);

            //Desplazamos los bloques
            for (int i = 0; i < listaBloques.Count; i++)
            {
                listaBloques[i].Descender(); //Descienden una posicion y comprueban si hay peligro (activar pantalla roja)
            }
            combo = 0;

            //Pasamos de nivel cuando todas las bolas han llegado al origen de nuevo
            if (nivelCompletado)
                SiguienteNivel();
        }
    }

    public void iniciarDisparo()
    {
        returnBolasButton.gameObject.SetActive(true);
        disparoIniciado = true;
        numDisparos++;
        StopCoroutine(iniciarContador(numDisparos));
        StartCoroutine(iniciarContador(numDisparos));

        numBolasAux = Disparador.getNumBolas() + 1; //Porque ya ha disparado una        
    }

    public void RestartStatsSpawner()
    {
        if (Disparador != null)
        {
            Disparador.InterrumpirDisparo();
            disparoIniciado = false;
            Disparador.SetContBolas(numBolasAux); //Esto haria que se acumulasen las bolas extra de un nivel para otro        
        }

        contTemporal = 0;
        combo = 0;
    }

    //Llama al MoveTo de todas las bolas en juego
    public void BotonVolverSpawner()
    {
        if (listaBolas.Count > 0)
        {
            RestartStatsSpawner();

            for (int i = 0; i < listaBolas.Count; i++)
            {
                if (listaBolas[i].GetRigidbody() != null)
                    Destroy(listaBolas[i].GetRigidbody()); //Destruimos rigidBody para que no colisione camino de vuelta

                listaBolas[i].MoveTo(Disparador.posicionAux, 10, destruyePelota);
            }

            //Hacemos pasar el turno descendiendo los bloques
            for (int i = 0; i < listaBloques.Count; i++)
                listaBloques[i].Descender();

            Disparador.SetPosition(Disparador.getPosAux());
            returnBolasButton.gameObject.SetActive(false);

            if (nivelCompletado)
                SiguienteNivel();
        }
    }

    public void CreaEstrella()
    {
        if (Level.currentLevel.stars > 0 && contStars > 0)
        {
            
            if (Level.currentLevel.stars == 1 && contStars == 3)
            {
                Image newobj = Instantiate(star);
                newobj.transform.SetParent(pointsBar.transform);
                newobj.rectTransform.localScale = new Vector3(1.2f, 1.2f, 0);
                Debug.Log("Tamaño: " + pointsBar.rectTransform.sizeDelta.x + ", posicion: " + pointsBar.rectTransform.sizeDelta.x * 0.2f);
                newobj.rectTransform.localPosition = new Vector3(pointsBar.rectTransform.sizeDelta.x * 0.2f, 0, 0);
                contStars--;
            }

            else if (Level.currentLevel.stars == 2 && contStars == 2)
            {
                Image newobj = Instantiate(star);
                newobj.transform.SetParent(pointsBar.transform);
                newobj.rectTransform.localScale = new Vector3(1.2f, 1.2f, 0);
                newobj.rectTransform.localPosition = new Vector3(pointsBar.rectTransform.sizeDelta.x * 0.7f, 0, 0);
                contStars--;
            }

            else if (Level.currentLevel.stars == 3 && contStars == 1)
            {
                Image newobj = Instantiate(star);
                newobj.transform.SetParent(pointsBar.transform);
                newobj.rectTransform.localScale = new Vector3(1.2f, 1.2f, 0);
                newobj.rectTransform.localPosition = new Vector3(pointsBar.rectTransform.sizeDelta.x, 0, 0);
                contStars--;
            }
        }
    }

    public void destruyePelota(Ball p)
    {
        listaBolas.Remove(p);
        Destroy(p.gameObject);

        if (Disparador.getNumBolas() <= 0 && contTemporal >= numBolasAux)
        {
            //Pasamos de nivel cuando todas las bolas han llegado al origen de nuevo
            if (nivelCompletado)
                SiguienteNivel();
        }
    }

    //Dado un vector, nos dice si esta dentro del gameField o no
    public bool dentroPantalla(Vector3 pos)
    {
        if (pos.x < muroDerecho.transform.position.x && pos.x > muroIzquierdo.transform.position.x && pos.y < muroArriba.transform.position.y)
            return true;

        return false;
    }

    void ResizeCamera()
    {
        float desiredRatio = TARGET_WIDTH / TARGET_HEIGHT;
        float currentRatio = (float)Screen.width / (float)Screen.height;

        if (currentRatio >= desiredRatio)
        {
            Camera.main.orthographicSize = TARGET_HEIGHT / 4 / PIXELS_TO_UNITS;
        }
        else
        {
            float differenceInSize = desiredRatio / currentRatio;
            Camera.main.orthographicSize = TARGET_HEIGHT / 4 / PIXELS_TO_UNITS * differenceInSize;
        }
    }

    public void SiguienteNivel()
    {
        RestartStatsSpawner();
        Disparador.SetContBolas(bolasIniciales);

        // Serializacion
        if (SaveLoad.savedGame.playedLevels.ContainsKey(GameManager.instance.level))
        {
            if (SaveLoad.savedGame.playedLevels[GameManager.instance.level][0] < Level.currentLevel.score)
            {
                SaveLoad.savedGame.score = (int)Level.currentLevel.score;
                SaveLoad.savedGame.stars = Level.currentLevel.stars;
                SaveLoad.savedGame.playedLevels[GameManager.instance.level] = new int[] { SaveLoad.savedGame.score, SaveLoad.savedGame.stars };
                SaveLoad.Save();
            }
        }

        GameManager.instance.level++;

        NuevoNivelSerializable(GameManager.instance.level);
        Game.currentGame.monedas += 50;
        SaveLoad.savedGame = Game.currentGame;
        SaveLoad.Save();

        Alertar(false);
        SceneManager.LoadScene("Juego", LoadSceneMode.Single);
    }

    public void ReiniciarNivel() //Aun no funciona bien
    {
        RestartStatsSpawner();
        Disparador.SetContBolas(bolasIniciales);
        Level.currentLevel.levelID = GameManager.instance.level;
        Level.currentLevel.score = 0;

        Alertar(false);
        SceneManager.LoadScene("Juego", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    public void NuevoNivelSerializable(int nivel)
    {
        GameManager.instance.level = nivel;
        // Serializacion
        if (SaveLoad.Load())
        {
            Level.currentLevel = new Level(GameManager.instance.level, 0);
            Game.currentGame.score = (int)Level.currentLevel.score;
            Game.currentGame.stars = Level.currentLevel.stars;

            if (!SaveLoad.savedGame.playedLevels.ContainsKey(GameManager.instance.level))
            {
                Game.currentGame.playedLevels.Add(Level.currentLevel.levelID, new int[] { Game.currentGame.score, Game.currentGame.stars });
            }
        }
        else
        {
            Level.currentLevel = new Level(GameManager.instance.level, 0);
            Game.currentGame.score = (int)Level.currentLevel.score;
            Game.currentGame.stars = Level.currentLevel.stars;

            Game.currentGame.powerUp = 2; //2 PowerUps cuando empiezas una partida nueva
            Game.currentGame.playedLevels.Add(Level.currentLevel.levelID, new int[] { Game.currentGame.score, Game.currentGame.stars });
        }
        SaveLoad.savedGame = Game.currentGame;
        SaveLoad.Save();
    }

    public void CargarNivel(int nivel)
    {
        //No recuerdo si hay que reiniciar stats o no
        //RestartStatsSpawner();
        //Disparador.SetContBolas(bolasIniciales);
        Debug.Log("EL nivel que cargo: " + nivel); //Esto esta bien
        NuevoNivelSerializable(nivel);
        Alertar(false);
        SceneManager.LoadScene("Juego", LoadSceneMode.Single);
    }

    public void TogglePause()
    {
        bool aux = pauseMenu.activeSelf;
        pauseMenu.SetActive(!aux);
        if (aux)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;

    }

    public void IrMenuSeleccion()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Menu_Seleccion_Niveles");
        Time.timeScale = 1;
    }

    public void SalirApplicacion()
    {
        Application.Quit();
    }
}


/*
 *  Antigua version
 *  /*for (int i = 0; i < listaBolas.Count; i++)
        {
            if (listaBolas[i] != null)
                Destroy(listaBolas[i].gameObject);

        }
        listaBolas.Clear();

        for (int i = 0; i < listaBloques.Count; i++)
        {
            if (listaBloques[i] != null)
                Destroy(listaBloques[i].gameObject);

        }
        listaBloques.Clear();
 */
