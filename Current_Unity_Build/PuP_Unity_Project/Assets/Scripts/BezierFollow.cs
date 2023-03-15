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
        if (coroutineAllowed)
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
        float relativePos;

        posX = PM.transform.position.x;
        
        if (posX > routes[routeNumber].position.x)
        {
            relativePos = (Mathf.Abs(posX - routes[routeNumber].position.x));
            relativePos = (relativePos + (p3.x - routes[routeNumber].position.x)) / (p3.x - p0.x);
        }
        else if (posX > p0.x)
        {
            relativePos = (Mathf.Abs(posX - p0.x));
            relativePos = (relativePos) / (p3.x - p0.x);
        }
        else
        {
            relativePos = Mathf.Abs(posX - routes[routeNumber].position.x);
            relativePos = (relativePos - (p3.x - routes[routeNumber].position.x)) / (p3.x - p0.x);
        }

        tParam = relativePos;

        //Debug.Log(tParam);

        playerPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;


        while ((tParam < 1 && posX > p0.x && posX < p3.x && PM.transform.position.y - 0.5f <= playerPosition.y))
        {
            Debug.Log(PM.speed_y);
            if (PM.speed_y > 0 && (PM.transform.position.y > playerPosition.y))
            {
                Debug.Log("BREAK");
                break;
            }
            PM.onCurve = true;
            posX = PM.transform.position.x;


            tParam += Time.deltaTime * speedMod * PM.current_speed.x;
            
            
            
            //Debug.Log(tParam);
           
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
