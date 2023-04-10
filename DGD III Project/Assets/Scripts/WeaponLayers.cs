using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponLayers : MonoBehaviour
{
    public RigBuilder weaponRig;
    // Start is called before the first frame update
    void Start()
    {
        weaponRig.layers[0].active = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
