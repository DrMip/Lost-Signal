using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    Dictionary<string,GameObject> enemyTypes = new Dictionary<string, GameObject>();
    //prefabs
    [SerializeField] GameObject skeletonPrefab;
    [SerializeField] GameObject batPrefab;


    public class Enemy
    {
        public string enemyType;
        public GameObject enemyObject;
        public Vector3 spawnPosition;
    }
    //Array for Enemies
    Enemy[] enemies;
    // Start is called before the first frame update
    void Awake()
    {
        enemies = CreateEnemyList();
        enemyTypes.Add("Skeleton", skeletonPrefab);
        enemyTypes.Add("Bat", batPrefab);
    }


    GameObject[] GetAllInLayer(int layer)
    {
        GameObject[] allGObj = FindObjectsOfType<GameObject>();
        List<GameObject> gObjLayer = new List<GameObject>();
        foreach (GameObject gO in allGObj)
        {
            if (gO.layer == layer)
            {
                gObjLayer.Add(gO);
            }
        }
        if(gObjLayer.Count == 0)
        {
            Debug.LogWarning("No Enemies Found");
            return null;
        }
        return gObjLayer.ToArray();    
    }
    Enemy[] CreateEnemyList()
    {
        GameObject[] enemiesGO = GetAllInLayer(LayerMask.NameToLayer("Enemies"));
        if (enemiesGO.Length == 0)
            return null;
        List<Enemy> enemyList = new List<Enemy>();
        foreach(GameObject gO in enemiesGO)
        {
            Enemy enemyTemp = new Enemy();
            enemyTemp.enemyType = gO.tag;
            enemyTemp.enemyObject = gO;
            enemyTemp.spawnPosition = gO.transform.position;
            enemyList.Add(enemyTemp);
        }
        return enemyList.ToArray();
    }

    public void SpawnAllEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            if(enemy.enemyObject == null)
            {
                continue;
            }
            Destroy(enemy.enemyObject);
        }
        foreach(Enemy enemy in enemies)
        {

            enemy.enemyObject = Instantiate(enemyTypes[enemy.enemyType], enemy.spawnPosition, Quaternion.identity);
        }
    }
}
