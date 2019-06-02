using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    
    private Rigidbody2D rigidbody_;    

    private void Awake()
    {
        rigidbody_ = GetComponent<Rigidbody2D>();
        if (rigidbody_ == null)
            Debug.Log("La bola no tiene rigidbody");
    }

    //Posicion desde la que sale disparada, y el vector con la direccion y potencia ya normalizado
    public void Shoot(Vector3 posIni, Vector3 direVelocity)
    {
        transform.position = posIni;
        rigidbody_.velocity = direVelocity;
        LevelManager.instance.listaBolas.Add(this); //Añadimos la bola a la lista del GameManager
    }

    public Rigidbody2D GetRigidbody()
    {
        return rigidbody_;
    }

    //Retorno de la bola a la pos (disparador)
    public void MoveTo(Vector3 pos, float time, System.Action<Ball> callback = null)
    {
        rigidbody_.velocity = new Vector3(0, 0, 0);
        StartCoroutine(MoveToCoroutine(pos, time, callback));
    }

    IEnumerator MoveToCoroutine(Vector3 pos, float time, System.Action<Ball> callback = null)
    {
        bool termino = false;

        while (!termino)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, time * Time.deltaTime);

            if (transform.position == pos)            
                termino = true;

            yield return new WaitForFixedUpdate();
        }

        if (callback != null && termino)
        {            
            StopCoroutine(MoveToCoroutine(pos, time, callback));
            callback(this); //El GameManager se encarga de destruir este gameObject            
        }
        
        yield return new WaitForFixedUpdate();                      
    }
    
}
