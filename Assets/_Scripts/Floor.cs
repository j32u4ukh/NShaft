using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private float moving_speed = 2f;
    [SerializeField] private int m_layer = 0;

    public int layer
    {
        set
        {
            m_layer = Math.Max(value, 0);
        }
        get
        {
            return m_layer;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, moving_speed * Time.deltaTime, 0f);

        if(transform.position.y > 6f)
        {
            Destroy(gameObject);
            transform.parent.GetComponent<FloorManager>().spanFloor();
        }
    }

    
}
