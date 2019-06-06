using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivarScroll : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<ScrollRect>().enabled = true;
        GetComponentInChildren<PopulateGrid>().enabled = true;
	}
	
	
}
