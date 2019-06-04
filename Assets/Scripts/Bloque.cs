using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Canvas y text

public class Bloque : MonoBehaviour
{
    public int contGolpes = 7;    
    int tipo = 0; //tipo de bloque
    Color colorOriginal;

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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {
            contGolpes--;
            AddText(contGolpes.ToString());
            //Cambiar color por contacto
            StartCoroutine(toggleColor());

            if (contGolpes <= 0)
            {
                LevelManager.instance.listaBloques.Remove(this);
                Destroy(gameObject);
                Level.currentLevel.score += (10 + (10*LevelManager.instance.combo));
                Debug.Log("Puntos: " + Level.currentLevel.score);
                Debug.Log("Barra puntos: " + (Level.currentLevel.score / Level.currentLevel.maxScore));
                LevelManager.instance.pointsBar.fillAmount = Level.currentLevel.score / Level.currentLevel.maxScore;
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

    }

    IEnumerator toggleColor()
    {
        if (GetComponent<Renderer>() != null)
            GetComponent<Renderer>().material.color = colorOriginal*1.07f;
        yield return new WaitForSeconds(0.05f);

        if (GetComponent<Renderer>() != null)
            GetComponent<Renderer>().material.color = colorOriginal;
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
                if (LevelManager.instance.listaBloques.Count <= 0)
                {
                    //Cambiar de nivel                
                    //LevelManager.instance.SiguienteNivel();
                    LevelManager.instance.nivelCompletado = true;
                }

            }
        }
        
    }

}
