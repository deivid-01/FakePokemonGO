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
    int numPlanes;
    List<int> planesTaken = new List<int>();
    List<ARPlane> arPlanes;
    void Start()
    {
        numPlanes = arPlaneManager.trackables.count;
        arPlanes = new List<ARPlane>();
        arPlaneManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        arPlaneManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        
       ShowMePlanes();
 
        
   
    }

    void ShowMePlanes()
    {
   
        if (arPlaneManager.trackables.count > numPlanes)
        {
            numPlanes = arPlaneManager.trackables.count;
            //print("Numplanes: "+numPlanes);
            int i = 0;
            foreach (var plane in arPlaneManager.trackables)
            {
                bool bigPlane = (plane.extents.x * plane.extents.y >= minimumPlaneSize.x * minimumPlaneSize.y);
                plane.gameObject.SetActive(bigPlane);
         
                if (bigPlane)
                {
                    if (!planesTaken.Contains(i))
                    {
                        if (ValidatePlane(plane))
                            {
                            arPlanes.Add(plane);
                            GameEvent.instance.SpawnPokemon(plane.center);
                            planesTaken.Add(i);
                        }
                        
                    }
                    print($"Plane {i }: {plane.extents}");
                }
                i += 1;
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
