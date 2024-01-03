using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class EnemyAIFeature : MonoBehaviour
{
    [Serializable] 
    public class Sight// sight class
    {
        public bool moveOnSight;

        public bool drawGizmos;

        public float sightLength;
        [Range(0.01f,180)]public float sightFOV;
        [Range(-90,90)] public float sightAngle;
        [Range(2,20)]public int numOfSightRays;
        [NonSerialized] public Vector3[] sightRays;
        [NonSerialized] public float GizmoDuration;
        private Vector2[,] visionLines;

        [NonSerialized] public MonoBehaviour mono;
        private bool enableAI;

        

        public void _init_(MonoBehaviour mn, float DisplayTime)
        {
            mono = mn;
            GizmoDuration = DisplayTime;

            //init sight vectors
            sightRays =  new Vector3[2*numOfSightRays];
            //init temp array for one side of vision
            Vector3[] tempRays = new Vector3[numOfSightRays];
            //init Vision lines
            visionLines = new Vector2[2*numOfSightRays, 2];

            float angle = sightFOV/2 + sightAngle;
            for(int i = 0; i < numOfSightRays; i++)//gets all rays
            {
                Vector3 temp = new Vector3(sightLength ,0 ,0);
                //temp = Quaternion.AngleAxis(angle*Mathf.Deg2Rad, Vector3.forward) * temp;
                tempRays[i] = Quaternion.AngleAxis(angle, Vector3.forward) * temp;
                
                if(angle > sightAngle)
                {
                    angle = sightAngle - (angle - sightAngle);
                }
                else if(angle < sightAngle)
                {
                    angle = sightAngle - (angle - sightAngle);
                    angle -= sightFOV/(numOfSightRays-1);
                }
            }
            //add temp rays to sight rays
            for(int i = 0; i < tempRays.Length; i++)
                sightRays[i] = tempRays[i];
            //mirror temp on y axis
            for(int i = 0; i < tempRays.Length; i++)
            {
                tempRays[i] = new Vector3(-tempRays[i].x, tempRays[i].y , tempRays[i].z);
            }
            //puts all vectors in sightRays
            for(int i = 0; i < tempRays.Length; i++)
                sightRays[numOfSightRays + i] = tempRays[i];


        }
        public void DrawSight()
        {
            if(moveOnSight)
                for(int i = 0; i < 2*numOfSightRays; i++)
                {
                    Debug.DrawLine(visionLines[i,0], visionLines[i,1], Color.red, GizmoDuration);
                }
        }
        
        //check for players and dont see through walls
        public bool See()
        {
            enableAI = false;
            if(moveOnSight)
            {
                for(int i = 0; i < sightRays.Length; i++)
                {
                    RaycastHit2D[] rayHits = Physics2D.RaycastAll(mono.transform.position, sightRays[i], sightLength);
                    if(rayHits.Length == 0)
                    {
                        break;
                    }
                    bool foundHit = false;
                    for(int j = 0; j < rayHits.Length; j++)
                    {
                        if(foundHit)
                        {
                            break;
                        }

                        //Debug.Log(i + " " + rayHits.Length);
                        visionLines[i, 0] = mono.transform.position;
                        if(rayHits[j].collider.gameObject.layer == LayerMask.NameToLayer("Middleground"))//hits background
                        {
                            visionLines[i , 1] = rayHits[j].point;
                            foundHit = true;
                        }
                        else if(rayHits[j].collider.CompareTag("Player"))//hits player
                        {
                            enableAI = true;
                            visionLines[i , 1] = rayHits[j].point;
                            foundHit = true;
                        }
                        else//hits nothing
                        {
                            //get if ray is on left or on right

                            visionLines[i , 1] = visionLines[i , 0] + new Vector2(sightRays[i].x, sightRays[i].y);
                        }
                    }
                }
            }

            //drawlines
            if(drawGizmos)
                DrawSight();
            return enableAI;
        }
    }
    [Serializable]
    public class RoomActivation
    {
        public bool DoRoomActivation = true;
        GameObject room;
        RoomTrigger trigger;
        static ContactFilter2D filter;
        
        bool entered = false;
        bool exited = false;

        List<RaycastHit2D> rays = new List<RaycastHit2D>();

        bool enableAI;

        [NonSerialized] public bool didReset;

            //return to poistion
        [NonSerialized] public Vector3 startPos;

        [NonSerialized] public MonoBehaviour mono;

        public void _init_(MonoBehaviour mn)
        {
            mono = mn;
            filter.NoFilter();
            Physics2D.Raycast(new Vector2(mono.transform.position.x,mono.transform.position.y), Vector2.down, filter, rays);//see where you are
            foreach(RaycastHit2D ray in rays)//find the room
            {
                if(ray.transform.gameObject.layer == LayerMask.NameToLayer("Room"))
                {
                    room = ray.transform.gameObject;
                }
            }
            // now room is the room
            
            trigger = room.GetComponent<RoomTrigger>();
            //set start pos
            startPos = mono.transform.position;
        }
        public bool Check()
        {
            if(DoRoomActivation)
            {
                if(trigger.PlayerInRoom == true && !entered)
                {
                    entered = true;
                    exited = false;
                    enableAI = true;
                }
                if(trigger.PlayerInRoom == false)
                {

                    enableAI = false;
                    if(!exited)
                    {
                        entered = false;
                        exited = true;
                        //return to your positions
                        mono.StartCoroutine(ResetPosition());
                    }

                }
            }
            return enableAI;
        }

        IEnumerator ResetPosition()
        {
            yield return new WaitForSeconds(1f);
            didReset = true;
            if(!trigger.PlayerInRoom) // if player hasn't returned to the room
                mono.transform.position = startPos;
        }
    }
}
