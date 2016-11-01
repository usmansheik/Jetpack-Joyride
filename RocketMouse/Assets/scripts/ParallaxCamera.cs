using UnityEngine;
using System.Collections;

public class ParallaxCamera : MonoBehaviour {
    public Renderer background;
    public Renderer foreground;
    public float offset = 0;
    public float backgroundSpeed = 0.02f;
    public float foregroundSpeed = 0.06f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float backgroundOffset = offset * backgroundSpeed;
        float foregroundOffset = offset * foregroundSpeed;

        background.material.mainTextureOffset = new Vector2(backgroundOffset, 0);
        foreground.material.mainTextureOffset = new Vector2(foregroundOffset, 0);
    }
}
