using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform[] routes;

    [HideInInspector]
    public int routeToGo;

    private float tParam;

    [HideInInspector]
    public Vector3 playerPosition;

    [HideInInspector]
    public bool isActive = true;

    public PrinceMovement PM;

    public float speedMod;

    private bool coroutineAllowed;

    private void Start()
    {
        routeToGo = 0;
        tParam = 0;
        //speedMod = 0.5f;
        coroutineAllowed = true;
        isActive = true;

    }

    private void Update()
    {
        if (coroutineAllowed && isActive)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
            PM.onCurve = false;
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;
        Vector3 p0 = routes[routeNumber].GetChild(0).position;
        Vector3 p1 = routes[routeNumber].GetChild(1).position;
        Vector3 p2 = routes[routeNumber].GetChild(2).position;
        Vector3 p3 = routes[routeNumber].GetChild(3).position;
        float posX = PM.transform.position.x;

        posX = PM.transform.position.x;
        if (posX > p3.x - 0.5f)
        {
            tParam = 0.99f;
        }
        else if (posX < p1.x + 0.5f)
        {
            tParam = 0f;
        }
        else
        {
            tParam = 0f;
        }
        while (tParam < 1 && posX > p0.x && posX < p3.x)
        {
            PM.onCurve = true;
            posX = PM.transform.position.x;


            tParam += Time.deltaTime * speedMod * PM.current_speed.x;
            
            
            
            Debug.Log(tParam);
           
            playerPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = playerPosition;
            yield return new WaitForEndOfFrame();
        }

        if (posX < p0.x || posX > p3.x)
        {
            PM.onCurve = false;
        }

        
        
        routeToGo += 1;

        if(routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;
    }

}
