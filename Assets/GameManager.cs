using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float m_timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > 10)
            // have it gradual instead of sudden change -- factor of time...
        {
            RenderSettings.fogEndDistance = 1500;
        }
        
    }
}
