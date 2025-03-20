using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject LOScreen;
    // Start is called before the first frame update
    void Start()
    {
        LOScreen.SetActive(false);
        StartScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
