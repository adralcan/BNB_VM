using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(RompeBloques(0.5f));
    }
    
    IEnumerator RompeBloques(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        LevelManager.instance.objetosADestruir.Add(this.gameObject);
        this.gameObject.SetActive(false);

        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bloque>())
        {            
            collision.gameObject.GetComponent<Bloque>().destroyByPowerUp();
        }
    }
}
