using UnityEngine;

public class MultiplayerLocal : MonoBehaviour
{
    public Camera camera1; 
    public Camera camera2; 

    void Start()
    { 

        camera1.rect = new Rect(0, 0, 0.5f, 1);   
        camera2.rect = new Rect(0.5f, 0, 0.5f, 1); 
    }
}