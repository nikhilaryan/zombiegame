using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk : MonoBehaviour {

    public Transform[] path;
    public float speed = 0.50f;
    public float reach = 1.0f;
    public int currentpoint = 0;

    private void Start()
    {

    }
    private void Update()
    {
        //Vector3 dir = path[currentpoint].position - transform.position;
        float dist = Vector3.Distance(path[currentpoint].position, transform.position);
        transform.position = Vector3.Lerp(transform.position, path[currentpoint].position, Time.deltaTime * speed); 
        if(dist<=reach)
        {
            currentpoint++;

        }
        if(currentpoint>=path.Length)
        {
            currentpoint = 0;
        }
    }
    
    }

