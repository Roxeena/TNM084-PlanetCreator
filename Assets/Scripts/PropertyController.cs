using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyController : MonoBehaviour {

  private Renderer terrainRender, waterRender;

  //Values that can be changed in the shaders
  [Range(0.0f, 1.0f)]
  public float terrainAmount = 0.4f;

  [Range(0.0f, 5.0f)]
  public float terrainFreq = 1.2f;

  [Range(0.0f, 0.2f)]
  public float waterAmount = 0.015f;

  [Range(5.0f, 20.0f)]
  public float waterFreq = 10.0f;

  [Range(0.0f, 1.0f)]
  public float waterSpeed = 0.01f;

  // Use this for initialization
	void Start () {
    terrainRender = GetComponent<Renderer>();
    waterRender = GetComponent<Renderer>();

    waterRender.material.shader = Shader.Find("Custom/Water");
    terrainRender.material.shader = Shader.Find("Custom/Terrain");
  }
	
	// Update is called once per frame
	void Update () {
    waterRender.material.SetFloat("_Amount", waterAmount);
    waterRender.material.SetFloat("_Freq", waterFreq);
    waterRender.material.SetFloat("_WaterSpeed", waterSpeed);

    terrainRender.material.SetFloat("_Amount", terrainAmount);
    terrainRender.material.SetFloat("_Freq", terrainFreq);

  }
}
