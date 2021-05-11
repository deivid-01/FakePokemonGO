using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;
using System.Linq;
public class ARFilteredPlanes : MonoBehaviour
{
    public Vector2 minimumPlaneSize;
    public float minimumDistance = 4;
    public ARPlaneManager arPlaneManager;

    List<ARPlane> arPlanes;
    void Start()
    {
        arPlanes = new List<ARPlane>();
        arPlaneManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        arPlaneManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added != null)
        {
            if (args.added.Count > 0)
            {
                foreach (ARPlane plane in args.added.Where(plane => (plane.extents.x * plane.extents.y >= (minimumPlaneSize.x * minimumPlaneSize.y)))) //At least 0.1 square meters
                {
                    if (ValidatePlane(plane))
                    { 
                        arPlanes.Add(plane);
                        print("...Spawning pokemon");
                        GameEvent.instance.SpawnPokemon(plane.center);
                    }
                  
                }

        
            }
            
        }
        
   
    }

    bool ValidatePlane(ARPlane newPlane)
    {
        if (arPlanes.Count == 0) return true;

        foreach (ARPlane plane in arPlanes)
        {
            if (Vector3.Distance(plane.center, newPlane.center) < minimumDistance)
            {
                return false;
            }
        }
        
        return true;
    }

 
}
