using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using Unity.Collections;
using UnityEngine;

public class SpecialAttacks : MonoBehaviour
{
    //describes all special things we can do with wrath
    public abstract class Special
    {   
        public abstract IEnumerator DoSpecial(); //function for activating the special technique
        public int NeededWrath; //the needed wrath to do technique
        public string Animation; //the technique animation name
        public float SpecialSpeed; // the speed of the player when healing
        [NonSerialized] public PlayerBehavior pb; //the PlayerBehavior
        [NonSerialized] public AnimationSwitcher animSwitch; //the animationSwitcher


        [NonSerialized] public bool isDoingSpecial = false;
    }

    [Serializable] 
    public class WrathHeal : Special
    {
        public int healAmount;
        public float HealTime;
        private float timecounter = 0;

        [SerializeField] private ParticleSystem particle;

        [NonSerialized] public bool Heal;
        public override IEnumerator DoSpecial()
        {
            
            isDoingSpecial = true;
            if(NeededWrath < pb.wrath) //if has the wrath needed
            {
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
                }
                //reset all
                particle.Stop();
                Heal = false;
                timecounter = 0;
                pb.MovementSpeed = defualtSpeed;
            }
            isDoingSpecial = false;
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
        public override IEnumerator DoSpecial()
        {
            isDoingSpecial = true;
            if(NeededWrath < pb.wrath)
            {
                //change and save speed
                float defualtSpeed = pb.MovementSpeed;
                pb.MovementSpeed = SpecialSpeed;
                //wait seconds
                yield return new WaitForSeconds(TimeToExplode);
                //get allenemies in vacinity
                Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, ExplodeRadius);
                List<GameObject> hits = new List<GameObject>();
                Debug.DrawLine(new Vector2(player.transform.position.x - ExplodeRadius, player.transform.position.y), new Vector2(player.transform.position.x + ExplodeRadius, player.transform.position.y), Color.green, 3f);
                //do for each collider
                //Debug.Log(colliders.Length);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.layer == LayerMask.NameToLayer(enemyLayer) && !hits.Contains(colliders[i].gameObject))
                    {
                        Debug.Log(colliders[i].gameObject.name);
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
                //particle effects
                particle.Play();
                //reset speed
                pb.MovementSpeed = defualtSpeed;
            }

            isDoingSpecial = false;
        }
    }
    public class Astroid : Special
    {
        //for functionality
        public PlayerMovement pm;
        public float downwardForce;
        //for waits
        public override IEnumerator DoSpecial()
        {
            throw new NotImplementedException();
        }
    }
    // all parameters needed for wrath heal
    public WrathHeal wrathHeal = new WrathHeal();

    // all parameters  needed for Explode
    public Explode explode = new Explode();
    //variables
    bool anySpecialActive = false;
    // Start is called before the first frame update
    void Start()
    {
        //init wrath heal
        wrathHeal.pb = GetComponent<PlayerBehavior>();
        wrathHeal.animSwitch = GetComponent<AnimationSwitcher>();
        //init Explode
        explode.animSwitch = GetComponent<AnimationSwitcher>();
        explode.pb = GetComponent<PlayerBehavior>();
        explode.player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //heal
        if(Input.GetKeyDown(KeyCode.Q) && !anySpecialActive) // if I press Q, and not already healing then WrathHeal
        {
            StartCoroutine(wrathHeal.DoSpecial());
        }
        else if(Input.GetKeyUp(KeyCode.Q)) // if lifted Q then stop healing process
        {
            wrathHeal.Heal = false;
        }
        //explode
        if (Input.GetKeyDown(KeyCode.E) && !anySpecialActive) // if I press Q, and not already healing then WrathHeal
        {
            Debug.Log("Explode");
            StartCoroutine(explode.DoSpecial());
        }

        //see if any special is active
        if (wrathHeal.isDoingSpecial || explode.isDoingSpecial)
        {
            anySpecialActive = true;
        }
        else if (anySpecialActive == true)
        {
            anySpecialActive = false;
        }
    }
}
