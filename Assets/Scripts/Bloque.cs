using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Canvas y text

public class Bloque : MonoBehaviour
{
    public int contGolpes = 7;    
    int tipo = 0; //tipo de bloque
    Color colorOriginal;
    Renderer renderer;
    Vector3 posOriginal;

    private void Awake()
    {
        if (GetComponent<Renderer>() != null)
            renderer = GetComponent<Renderer>();
        else
            Debug.Log("Renderer no encontrado");        
    }

    // Use this for initialization
    void Start()
    {
        if(tipo == 21)
            AddText(" ");
        else
            AddText(contGolpes.ToString());
        if (GetComponent<Renderer>() != null)
            colorOriginal = GetComponent<Renderer>().material.color;
    }

    public void AddText(string cadena)
    {
        //El componente de texto está en el hijo
        TextMesh textAux = gameObject.GetComponentInChildren<TextMesh>();
        textAux.text = cadena;
    }

    public void setTipo(int n)
    {
        tipo = n;
    }    

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void destroyByPowerUp()
    {
        contGolpes = 0;
        if (contGolpes <= 0)
        {
            LevelManager.instance.listaBloques.Remove(this);
            Destroy(gameObject);
            Level.currentLevel.score += (10 + (10 * LevelManager.instance.combo));
            LevelManager.instance.combo++;
            Level.currentLevel.AddStar(LevelManager.instance.combo);
            if (LevelManager.instance.listaBloques.Count <= 0)
            {
                //Cambiar de nivel                
                //LevelManager.instance.SiguienteNivel();
                LevelManager.instance.nivelCompletado = true;
            }

        }
    }

    //Cuando los bloques descienden evaluan el estado de la partida en funcion de su posicion.y
    public void Descender()
    {
        StopCoroutine(meneoCorrutina());
        int time = 10;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 1, 0);

        //Alertar (posicion critica)
        if (pos.y == -5.5f)
            LevelManager.instance.Alertar(true);

        else if (pos.y < -5.5f)
        {
            //Has perdido
            StopAllCoroutines();
            LevelManager.instance.ReiniciarNivel();            
        }
        else //No es posicion critica
            LevelManager.instance.Alertar(false);

        StartCoroutine(DescenderCorrutina(time, pos));
    }

    IEnumerator DescenderCorrutina(float time, Vector3 pos)
    {
        bool termino = false;

        while (!termino)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, time * Time.deltaTime);

            if (transform.position == pos)
                termino = true;

            yield return new WaitForFixedUpdate();
        }

        if (termino)        
            StopCoroutine(DescenderCorrutina(time, pos));
        
        yield return new WaitForFixedUpdate();
    }


    public void Shake()
    {
        StopCoroutine(meneoCorrutina());
        StartCoroutine(meneoCorrutina());
    }

    IEnumerator meneoCorrutina()
    {
        posOriginal = transform.position;

        Vector3 pos = transform.position;
        pos.x += 0.7f;
        //movimiento a la derecha
        bool finalizado = false;

        while (!finalizado)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, 6 * Time.deltaTime);

            if (transform.position == pos)
                finalizado = true;

            yield return new WaitForFixedUpdate();
        }
        finalizado = false;
        //volver posicion original
        while (!finalizado)
        {
            transform.position = Vector3.MoveTowards(transform.position, posOriginal, 6 * Time.deltaTime);

            if (transform.position == posOriginal)
                finalizado = true;

            yield return new WaitForFixedUpdate();
        }

        if (finalizado)
            StopCoroutine(meneoCorrutina());

        yield break;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {
            contGolpes--;
            AddText(contGolpes.ToString());
            //Cambiar color por contacto
            StartCoroutine(toggleColor());

            comprobarGolpes();            
        }

    }

    //En funcion del numero de golpes se destruye o no
    public void comprobarGolpes()
    {
        if (contGolpes <= 0)
        {
            LevelManager.instance.listaBloques.Remove(this);
            LevelManager.instance.numBloques--;
            Destroy(gameObject);
            Level.currentLevel.score += (10 + (10 * LevelManager.instance.combo));
            Debug.Log("Puntos: " + Level.currentLevel.score);
            Debug.Log("Barra puntos: " + (Level.currentLevel.score / Level.currentLevel.maxScore));
            LevelManager.instance.pointsBar.fillAmount = Level.currentLevel.score / Level.currentLevel.maxScore;
            LevelManager.instance.combo++;
            Level.currentLevel.AddStar(LevelManager.instance.combo);
            if (LevelManager.instance.numBloques <= 0)
            {
                //Cambiar de nivel                
                //LevelManager.instance.SiguienteNivel();
                LevelManager.instance.nivelCompletado = true;
            }
        }
    }

    public void cambiarColor()
    {
        StopCoroutine(toggleColor());
        StartCoroutine(toggleColor());
    }

    IEnumerator toggleColor()
    {
        if (renderer != null)
            renderer.material.color = colorOriginal*1.07f;
        yield return new WaitForSeconds(0.05f);

        if (renderer != null)
            renderer.material.color = colorOriginal;
        yield break;
    }

    //Para el bloque de tipo 21
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {            
            contGolpes--; //

            if (tipo == 21)
            {
                //LevelManager.instance.Disparador.contBolas++;
                LevelManager.instance.contTemporal++;
                LevelManager.instance.numBolasAux++; //Variable de control de disparo tambien aumentada para correcto funcionamiento                
            }

            if (contGolpes <= 0)
            {
                LevelManager.instance.listaBloques.Remove(this);
                Destroy(gameObject);
                Level.currentLevel.score += (10 + (10 * LevelManager.instance.combo));
                LevelManager.instance.combo++;
                Level.currentLevel.AddStar(LevelManager.instance.combo);
                if (LevelManager.instance.numBloques <= 0)
                {
                    //Cambiar de nivel                   
                    LevelManager.instance.nivelCompletado = true;
                }

            }
        }
        
    }

}
