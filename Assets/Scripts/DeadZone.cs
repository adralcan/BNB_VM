using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {          
        if (other.gameObject.GetComponent<Ball>())
        {
            LevelManager.instance.llegadaPelota(other.gameObject.GetComponent<Ball>()); //GameManager llama al MoveTo de la bola    
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
