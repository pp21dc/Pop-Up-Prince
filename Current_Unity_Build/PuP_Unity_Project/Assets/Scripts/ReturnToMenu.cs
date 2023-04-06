using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{

    public bool startTimer = false;
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer == true)
        {
            timer += Time.deltaTime;
            print("time +1");
            if(timer > 5)
            {
                LoadNextScene();
            }
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            startTimer = true;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(7);
    }
}
