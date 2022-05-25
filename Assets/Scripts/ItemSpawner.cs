using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject leftLimit;
    [SerializeField] private GameObject rightLimit;

    [SerializeField] private GameObject heartfruit;


    public void SpawnItem()
    {
        gameObject.transform.position = new Vector3(Random.Range(leftLimit.transform.position.x, rightLimit.transform.position.x), transform.position.y, transform.position.z);
        Instantiate(heartfruit, gameObject.transform.position, gameObject.transform.rotation);
    }

}
