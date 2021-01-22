using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //The camera will read the Player class's data on 
        Player player = GameObject.FindObjectOfType<Player>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
