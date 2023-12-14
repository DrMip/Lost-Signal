using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthAndWrath : MonoBehaviour
{
    [SerializeField] BarFuncs bar;
    PlayerBehavior pb;
    HaltMovement halt;
    [SerializeField] GameObject GameOver;
    GameObject gameoverEnter;
    Color Entercolor;
    [SerializeField] float opaqueSpeed = 0.05f;
    bool cr_GameOver_running = false;
    Vector3 startpos;
    int lastHealth;
    int lastWrath;
    // Start is called before the first frame update
    void Start()
    {
        //set start position
        startpos = transform.position;

        //set GameOver screen as not active and relevent children as active 
        for(int i=0; i < GameOver.transform.childCount; i++)
        {
            //Debug.Log(GameOver.transform.childCount + "||" + i + "||" + GameOver.transform.GetChild(i).name);
            GameOver.transform.GetChild(i).gameObject.SetActive(true);

            if(GameOver.transform.GetChild(i).name == "Enter")
            {
                gameoverEnter = GameOver.transform.GetChild(i).gameObject;
                Entercolor = gameoverEnter.GetComponent<Text>().color;
                //make text transparent
                gameoverEnter.GetComponent<Text>().color = new Color(Entercolor.r,Entercolor.g,Entercolor.b, 0);
            }
        }
        GameOver.SetActive(false);

        pb = GetComponent<PlayerBehavior>();
        halt = GetComponent<HaltMovement>();
        bar.SetMaxHealth(pb.MaxHealth);
        bar.SetMaxWrath(pb.MaxWrath);
        pb.health = pb.MaxHealth;
        pb.wrath = 0;
        bar.SetWrath(pb.wrath);
        lastWrath = 0;

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.T))
        {
            pb.health -= 5;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            pb.wrath -= 20;
        }
        //if feels lava
        if(Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y), 0.1f, Vector2.zero, 0, LayerMask.GetMask("Lava")))
        {
            pb.health = 0;
        }

        //death if 0 (needs to stay at the end of cicle in order to catch all negative healths)
        if(pb.health <=0 && cr_GameOver_running == false)
        {
            pb.health = 0;
            StartCoroutine(GameOverScreen());
        }
        //handles what the bars say
        HealthHandler();
        WrathHandler();
    }
    IEnumerator GameOverScreen()
    {
        cr_GameOver_running = true;
        yield return new WaitForSeconds(0.2f);
        //stop moving
        halt.HaltAll();
        yield return new WaitForSeconds(0.8f);
        //bring screen
        GameOver.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //bring text gradually and wait for enter
        float i = 0f;
        while(i < 1f && !Input.GetKeyDown(KeyCode.Return))
        {
            gameoverEnter.GetComponent<Text>().color = new Color(Entercolor.r,Entercolor.g,Entercolor.b, i);
            i+=opaqueSpeed * Time.deltaTime;
            yield return null;
            
        }
        //continue to wait
        while(!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        //set health
        pb.health = pb.MaxHealth;
        pb.wrath = 0;
        bar.SetHealth(pb.health);
        bar.SetMaxWrath(pb.wrath);
        //reset
        transform.position = startpos;
        gameoverEnter.GetComponent<Text>().color = new Color(Entercolor.r,Entercolor.g,Entercolor.b, 0);
        GameOver.SetActive(false);
        halt.ResumeAll();
        cr_GameOver_running = false;

    }
    //handles what the bars show
    void HealthHandler()
    {
        //health
        //check if needs to be changed
        if(pb.health == lastHealth) return;
        //play animation
        bar.SetHealth(pb.health);
        //remember state
        lastHealth = pb.health;
    }
    void WrathHandler()
    {
        //wrath
        //check if needs to be changed
        if(pb.wrath == lastWrath) return;
        //play animation
        bar.SetWrath(pb.wrath);
        //remember state
        lastWrath = pb.wrath;
    }
}
