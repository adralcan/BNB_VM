using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparador : MonoBehaviour
{
    public Ball bolaPrefab;
    public Material materialLineRenderer;

    public int fuerza = 5;
    public int contBolas = 5;
    private TextMesh bolasText;

    [HideInInspector] public Vector3 posicionAux; //Auxiliar para cuando la primera bola cambie la posicion del disparador

    Vector2 direccion; //Del disparo
    LineRenderer lineRenderer;

    public float grosorLinea = 0.2f;

    Color c1 = Color.grey;
    Color c2 = Color.cyan;

    private void Awake()
    {
        if (gameObject.GetComponentInChildren<TextMesh>() != null)
            bolasText = gameObject.GetComponentInChildren<TextMesh>();
        else Debug.Log("No se encontro texto del disparador");
    }

    // Use this for initialization
    void Start()
    {
        //Muestra el numero de bolas en el disparador
        SetContBolas(contBolas);

        //Propiedades de la linea-trayectoria
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = materialLineRenderer;
        lineRenderer.widthMultiplier = grosorLinea;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c2, 0.0f), new GradientColorKey(c1, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;        
    }
    
    void FixedUpdate()
    {
        if (!GameManager.instance.anuncioActivo)
        {
            //Button down
            if (Input.GetMouseButton(0) && LevelManager.instance.listaBolas.Count <= 0)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

                //Si no hay tiro en curso y el disparo es por encima de la Y
                if (!LevelManager.instance.disparoIniciado && mousePosition.y > transform.position.y + 0.7f)
                {
                    //Dibujar linea
                    lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 10));
                    lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
                }
            }

            //Button up
            if (!LevelManager.instance.disparoIniciado && Input.GetMouseButtonUp(0) && LevelManager.instance.listaBolas.Count <= 0) //Ultimo and para no disparar mientras vuelven bolas
            {
                //Borramos linea
                lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 10));
                lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, 10));

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

                direccion = mousePosition - transform.position;
                direccion = direccion.normalized;

                //Comprobamos que el disparo sea por encima del spawner
                if (mousePosition.y > transform.position.y + 0.7f)
                    StartCoroutine(Disparar());
                else if(mousePosition.y > transform.position.y - 0.5f) //Dispara en la pos mas alta
                {
                    mousePosition = new Vector2(mousePosition.x, transform.position.y + 0.7f);
                    direccion = mousePosition - transform.position;
                    direccion = direccion.normalized;
                    StartCoroutine(Disparar());
                }
            }
        }
    }

    public void InterrumpirDisparo()
    {
        contBolas = 0;
        StopAllCoroutines();        
    }

    IEnumerator Disparar()
    {
        yield return new WaitForSeconds(0.1f);
        if (contBolas > 0)
        {
            if (contBolas > 0)
                contBolas--;           

            //Si es la primera bola en ser disparada
            if (contBolas + 1 == LevelManager.instance.numBolasAux)
                LevelManager.instance.iniciarDisparo(); //Este metodo almacena el numero de bolas antes de tirar

            Ball aux = Instantiate(bolaPrefab);
            //Nos aseguramos z = 0
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0);            
            aux.Shoot(pos, direccion * fuerza * Time.deltaTime);

            yield return StartCoroutine(Disparar());
        }
        else        
            StopCoroutine(Disparar());            
        
        //SetContBolas(contBolas);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        LevelManager.instance.spriteDisparador.transform.position = gameObject.transform.position;
    }

    public void SetContBolas(int n)
    {
        contBolas = n;
        //Vector3 aux = posicionAux;
        //aux.y += 0.5f;
        //bolasText.transform.position = aux;
        bolasText.text = n.ToString();        
    }

    //Darle valor a la futura posicion cuando acabe el turno
    public void SetPosAux(Vector3 aux)
    {
        posicionAux = new Vector3(aux.x, transform.position.y, 0);
        LevelManager.instance.spriteDisparador.transform.position = posicionAux;
    }

    public Vector3 getPosAux()
    {
        return posicionAux;
    }
    
    public int getNumBolas()
    {
        return contBolas;
    }  

}
