using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    private float polling_time = 1f;
    private float time = 0;
    private int framecount;
    [SerializeField] private bool show = false;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        framecount++;

        if(time >= polling_time)
        {
            if(show) Debug.Log(Mathf.RoundToInt(framecount / time));
            time = 0;
            framecount = 0;
            
        }
    }
}
