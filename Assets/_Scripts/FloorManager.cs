using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] GameObject[] floor_prefabs;
    [SerializeField] private int layer = 0;

    public void spanFloor()
    {
        layer++;

        int rand = Random.Range(0, floor_prefabs.Length);
        GameObject obj = Instantiate(floor_prefabs[rand], transform);
        obj.transform.position = new Vector3(Random.Range(-3.8f, 3.8f), -6f, 0f);

        Floor floor = obj.GetComponent<Floor>();
        floor.layer = layer;
    }
}
