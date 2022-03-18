using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactManager : MonoBehaviour
{
    [HideInInspector] public FireReact fireReact;
    [HideInInspector] public ArrowReact arrowReact;
    [HideInInspector] public ArrowTrapManager arrowManager;
    public void InitReacts(ReflexUI refUI)
    {
        //GetComponent will get attached given component on same gameObject
        fireReact = GetComponent<FireReact>(); 
        fireReact.Init(refUI);
        arrowManager = GetComponent<ArrowTrapManager>();
        arrowReact = GetComponent<ArrowReact>();
        arrowReact.Init(arrowManager,refUI);
    }
}
