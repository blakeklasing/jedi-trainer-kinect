using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFuture : MonoBehaviour {

    public GameObject presentTerrain;
    public GameObject futureTerrain;
    public GameObject lava;
    public GameObject water;
    public GameObject metalBridges;
    public GameObject presentFighter;
    public GameObject futureFighter;
    public GameObject woodenBridges;
    public Material presentSkybox;
    public Material futureSkybox;
    public GameObject presentEnemyType1;
    public GameObject presentEnemyType2;
    public GameObject futureEnemyType1;
    public GameObject futureEnemyType2;
    public GameObject enemyManager;

    public Light gameLight;

    private bool isPresent = true;


    // Use this for initialization
    void Start()
    {
        // Set to present
        updateTerrain();
        updateLiquids();
        updateSkybox();
        updateLighting();
        updateBridges();
        updateFighter();
        updateEnemies();
        updateSpawnPoints();
    }

    public IEnumerator activate()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1f);
        isPresent = !isPresent;
        updateTerrain();
        updateLiquids();
        updateSkybox();
        updateLighting();
        updateBridges();
        updateFighter();
        updateEnemies();
        updateSpawnPoints();
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
    }

    void updateTerrain()
    {
        if (isPresent)
        {
            futureTerrain.SetActive(false);
            presentTerrain.SetActive(true);
        }
        else
        {
            futureTerrain.SetActive(true);
            presentTerrain.SetActive(false);
        }
    }

    void updateLiquids()
    {
        if (isPresent)
        {
            foreach(Light light in lava.GetComponentsInChildren<Light>()) {
                light.enabled = false;
            }
            lava.SetActive(false);
            water.SetActive(true);
        }
        else
        {
            foreach (Light light in lava.GetComponentsInChildren<Light>()) {
                light.enabled = true;
            }
            lava.SetActive(true);
            water.SetActive(false);
        }
    }

    void updateSkybox()
    {
        if (isPresent)
            RenderSettings.skybox = presentSkybox;
        else
            RenderSettings.skybox = futureSkybox;
    }
    
    void updateBridges()
    {
        if (isPresent)
        {
            metalBridges.SetActive(false);
            woodenBridges.SetActive(true);
        }
        else
        {
            metalBridges.SetActive(true);
            woodenBridges.SetActive(false);
        }
    }

    void updateLighting()
    {
        if (isPresent)
            gameLight.intensity = 0.50f;
        else
            gameLight.intensity = 0.20f;
    }

    void updateFighter()
    {
        if (isPresent)
        {
            futureFighter.SetActive(false);
            presentFighter.SetActive(true);
        }
        else
        {
            futureFighter.SetActive(true);
            presentFighter.SetActive(false);
        }
    }

    void updateEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Vector3 location = enemy.transform.position;
            Quaternion rotation = enemy.transform.rotation;

            if (enemy != null)
            {
                if (isPresent)
                {
                    if (enemy.name == futureEnemyType1.name + "(Clone)")
                    {
                        Destroy(enemy);
                        GameObject newEnemy = Instantiate(presentEnemyType1, location, rotation);
                        newEnemy.GetComponent<EnemyAttack>().CanAttack();
                        newEnemy.GetComponent<EnemyMovement>().updateMovementInstant();
                    }
                    else if (enemy.name == futureEnemyType2.name + "(Clone)")
                    {
                        Destroy(enemy);
                        GameObject newEnemy = Instantiate(presentEnemyType2, location, rotation);
                        newEnemy.GetComponent<EnemyAttack>().CanAttack();
                        newEnemy.GetComponent<EnemyMovement>().updateMovementInstant();
                    }
                }
                else
                {
                    if (enemy.name == presentEnemyType1.name + "(Clone)")
                    {
                        Destroy(enemy);
                        GameObject newEnemy = Instantiate(futureEnemyType1, location, rotation);
                        newEnemy.GetComponent<EnemyAttack>().CanAttack();
                        newEnemy.GetComponent<EnemyMovement>().updateMovementInstant();
                    }
                    else if (enemy.name == presentEnemyType2.name + "(Clone)")
                    {
                        Destroy(enemy);
                        GameObject newEnemy = Instantiate(futureEnemyType2, location, rotation);
                        newEnemy.GetComponent<EnemyAttack>().CanAttack();
                        newEnemy.GetComponent<EnemyMovement>().updateMovementInstant();
                    }
                }
            }
        }
    }

    void updateSpawnPoints()
    {
        EnemyManager[] enemyManagers = enemyManager.GetComponents<EnemyManager>();
        if (isPresent)
        {
            enemyManagers[0].enemy = presentEnemyType1;
            enemyManagers[1].enemy = presentEnemyType2;
        }
        else
        {
            enemyManagers[0].enemy = futureEnemyType1;
            enemyManagers[1].enemy = futureEnemyType2;
        }
    }
}
