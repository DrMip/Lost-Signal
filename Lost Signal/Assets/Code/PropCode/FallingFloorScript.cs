using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FallingFloorScript : MonoBehaviour
{

    //animation for being Idle when in cams
    Animator anim;
    //Halting
    HaltMovement halt;
    GameObject floor;
    [SerializeField] float ShakeTime = 2;
    [SerializeField] float shakeDelay = 0.5f;
    [SerializeField] float shakeLen;
    float shakeTimeCounter = 0;
    Vector3 initialPos;
    bool cr_running;

    //Theme changer
    [SerializeField] AudioManager.ThemeSetter ThemeSetter = new AudioManager.ThemeSetter();

    // Start is called before the first frame update
    void Start()
    {
        halt = GameObject.Find("Player").GetComponent<HaltMovement>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
        floor = transform.GetChild(0).gameObject;
        floor.SetActive(true);
        initialPos = floor.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.name == "Player" && !cr_running)
        {
            halt.HaltAll();
            ThemeSetter.Send();//kill the music
            Invoke("CollapseShell", shakeDelay);
        }
    }
    void CollapseShell()
    {
        StartCoroutine(Collapse());
    }
    IEnumerator Collapse()
    {
        cr_running = true;
        
        while(shakeTimeCounter < ShakeTime)
        {
            anim.Play("Player_Idle");
            floor.transform.position = new Vector3(initialPos.x + Random.Range(0, shakeLen), initialPos.y + Random.Range(0, shakeLen), initialPos.z + Random.Range(0, shakeLen));
            //Debug.Log(shakeTimeCounter);
            shakeTimeCounter += Time.deltaTime;
            yield return null;
        }
        //Debug.Log(shakeTimeCounter >= ShakeTime);
        //Debug.Log(shakeTimeCounter + " " + ShakeTime);
        if(shakeTimeCounter >= ShakeTime)
        {
            floor.SetActive(false);

            anim.Play("Player_Falling_Down");
        }

        cr_running = false;
    }
}
