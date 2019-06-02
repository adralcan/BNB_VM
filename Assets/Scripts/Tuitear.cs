using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Tuitear : MonoBehaviour {

    //Twitter Share Link
    string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";

    //Language
    string TWEET_LANGUAGE = "es";
    
    string textToDisplay = "He conseguido una puntuacion de: ";
    
    public void Twittear ()
    {        
        Application.OpenURL(TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(textToDisplay) + 
            Level.currentLevel.score + WWW.EscapeURL(" puntos!") + "&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
    }	

}
