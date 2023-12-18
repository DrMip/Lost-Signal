using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class SpecialAttacks : MonoBehaviour
{
    //describes all special things we can do with wrath

    public abstract class Special
    {   

        public abstract IEnumerator DoSpecialInterior(); //function for activating the special technique
        public void DoSpecial()
        {
            
            if (!anySpecialActive)
            {

                if (NeededWrath < pb.wrath) //if has the wrath needed
                {
                    mono.StartCoroutine(DoSpecialInterior());
                }
                else if (pb.wrath > 0)//if has les than needed but not 0
                {
                    mono.StartCoroutine(NoEnoughWrathBlinker());
                }
                else // if has zero
                {
                    mono.StartCoroutine(ZeroWrathBlinker());
                }
                
            }
            

        }

        public int NeededWrath; //the needed wrath to do technique
        public string Animation; //the technique animation name
        public float SpecialSpeed; // the speed of the player when healing
        [NonSerialized] public PlayerBehavior pb; //the PlayerBehavior
        [NonSerialized] public AnimationSwitcher animSwitch; //the animationSwitcher

        [NonSerialized] public bool isDoingSpecial = false;

        public static MonoBehaviour mono;
        public static bool anySpecialActive = false;

        //for blinker
        public static Color defaultColor;
        public static Color defaultColorBG;
        public static Image image;
        public static float BlinkerTime;

        public static Image bgImage;
        public static Slider bgslider;
        protected IEnumerator NoEnoughWrathBlinker()
        {
            image.color = new Color(168 / 255f, 120 / 255f, 74 / 255f); ;
            yield return new WaitForSeconds(BlinkerTime);
            image.color = defaultColor;

        }
        protected IEnumerator ZeroWrathBlinker()
        {
            bgslider.value = bgslider.maxValue;

            bgImage.color = new Color(69/255f, 214/255f, 192 / 255f, 102 / 255f);
            yield return new WaitForSeconds(BlinkerTime);
            bgImage.color = defaultColorBG;
            bgslider.value = bgslider.minValue;
        }


    }

    [Serializable] 
    public class WrathHeal : Special
    {
        public int healAmount;
        public float HealTime;
        private float timecounter = 0;

        [SerializeField] private ParticleSystem particle;

        [NonSerialized] public bool Heal;
        [NonSerialized] public PlayerMovement pm;
        public override IEnumerator DoSpecialInterior()
        {
            if(pm.isGrounded)
            {
                isDoingSpecial = true;

                //change and save speed
                float defualtSpeed = pb.MovementSpeed;
                pb.MovementSpeed = SpecialSpeed;
                Heal = true;
                particle.Play();
                while (Heal && timecounter < HealTime) // if is told to wait and to heal, wait
                {
                    timecounter += Time.deltaTime;
                    yield return null;
                }
                if (timecounter >= HealTime && pb.health < pb.MaxHealth) // if while ended because the timer ended do healing and take wrath (dont heal if you dint need)
                {
                    pb.health += healAmount;
                    if (pb.health > pb.MaxHealth) pb.health = pb.MaxHealth;

                    pb.wrath -= NeededWrath;
                    //play sound
                    AudioManager mana = FindObjectOfType<AudioManager>();
                    mana.Play("Healing", mana.sounds);
                }
                //reset all
                particle.Stop();
                Heal = false;
                timecounter = 0;
                pb.MovementSpeed = defualtSpeed;

                isDoingSpecial = false;
            }


        }
    }
    [Serializable]
    public class Explode : Special
    {
        //for functionality
        public float ExplodeRadius;
        public int Damage;
        [NonSerialized] public GameObject player;
        [SerializeField] private ParticleSystem particle;
        public string enemyLayer;
        //for waits
        public float TimeToExplode;
        public override IEnumerator DoSpecialInterior()
        {
            isDoingSpecial = true;

            //change and save speed
            float defualtSpeed = pb.MovementSpeed;
            pb.MovementSpeed = SpecialSpeed;
            //wait seconds
            yield return new WaitForSeconds(TimeToExplode);
            //get all enemies in vacinity
            Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, ExplodeRadius);
            List<GameObject> hits = new List<GameObject>();
            Debug.DrawLine(new Vector2(player.transform.position.x - ExplodeRadius, player.transform.position.y), new Vector2(player.transform.position.x + ExplodeRadius, player.transform.position.y), Color.green, 3f);
            //do for each collider
            //gets all enemies found
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.layer == LayerMask.NameToLayer(enemyLayer) && !hits.Contains(colliders[i].gameObject))
                {
                    hits.Add(colliders[i].gameObject);
                }

            }
            //deal damage to them all
            foreach (GameObject go in hits)
            {
                go.GetComponent<EnemyBehavior>().health -= Damage;
                go.GetComponent<EnemyHealth>().hitBySpecial = true;
            }
            pb.wrath -= NeededWrath;
            //play Sound
            AudioManager mana = FindObjectOfType<AudioManager>();
            mana.Play("Explode", mana.sounds);
            //particle effects
            particle.Play();
            //reset speed
            pb.MovementSpeed = defualtSpeed;

            isDoingSpecial = false;
        }
    }
    public class Astroid : Special
    {
        //for functionality
        public PlayerMovement pm;
        public float downwardForce;
        //for waits
        public override IEnumerator DoSpecialInterior()
        {
            throw new NotImplementedException();
        }
    }




    // all parameters needed for wrath heal
    public WrathHeal wrathHeal = new WrathHeal();

    // all parameters  needed for Explode
    public Explode explode = new Explode();
    





    // Start is called before the first frame update
    private void Awake()
    {
        //initiate all variables for blinking when have no wrath
        //to change colors dive into the functions themselves
        Special.mono = this;
        Special.image = GameObject.Find("WrathFill").GetComponent<Image>();

        Special.BlinkerTime = 0.5f;

        Special.bgImage = GameObject.Find("BackgroundWrathFill").GetComponent<Image>();
        Special.bgslider = GameObject.Find("BackgroundWrathFillController").GetComponent<Slider>();
        Special.defaultColor = Special.image.color;
        Special.defaultColorBG = Special.bgImage.color;
    }


    void Start()
    {
        //init wrath heal
        wrathHeal.pb = GetComponent<PlayerBehavior>();
        wrathHeal.pm = GetComponent<PlayerMovement>();
        wrathHeal.animSwitch = GetComponent<AnimationSwitcher>();
        //init Explode
        explode.animSwitch = GetComponent<AnimationSwitcher>();
        explode.pb = GetComponent<PlayerBehavior>();
        explode.player = gameObject;


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(wrathHeal.isDoingSpecial + " " + explode.isDoingSpecial);
        //heal
        if(Input.GetKeyDown(KeyCode.Q)) // if I press Q, and not already healing then WrathHeal
        {
            wrathHeal.DoSpecial();
        }
        else if(Input.GetKeyUp(KeyCode.Q)) // if lifted Q then stop healing process
        {
            wrathHeal.Heal = false;
        }
        //explode
        if (Input.GetKeyDown(KeyCode.E)) // if I press Q, and not already healing then WrathHeal
        {
            //Debug.Log("Explode");
            explode.DoSpecial();
        }
        //see if any special is active
        if (wrathHeal.isDoingSpecial || explode.isDoingSpecial)
        {
            Special.anySpecialActive = true;
        }
        else if (Special.anySpecialActive == true)
        {
            Special.anySpecialActive = false;
        }
    }

}
