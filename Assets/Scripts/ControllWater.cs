using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ControllWater : MonoBehaviour {

  //Planet to controll parameters of
  public GameObject planet;

  //Renderer of the sun
  private Renderer render;

  //Values that can be changed in the shader
  /*************************************/
  [Range(0.0f, 0.2f)]
  public float waterAmount = 0.01f;

  [Range(5.0f, 20.0f)]
  public float waterFreq = 10.0f;

  public Color water = Color.blue;
  public Color wave = Color.white;

  [Range(0.0f, 1.0f)]
  public float waveAmount = 0.5f;

  [Range(0.0f, 0.2f)]
  public float waterSpeed = 0.2f;
  /**************************************/

  // ---- UI ---
  /**************************************/
  public Slider sizeSlider;
  public Slider amountSlider;
  public Slider freqSlider;
  public Slider waveAmountSlider;
  public Slider speedSlider;
  /**************************************/

  // Use this for initialization
  void Start () {

    if (!planet) return;

    render = planet.GetComponent<Renderer>();
    render.material.shader = Shader.Find("Custom/Water");

    waterAmount = render.material.GetFloat("_Amount");
    waterFreq = render.material.GetFloat("_Freq");
    water = render.material.GetColor("_WaterColor");
    wave = render.material.GetColor("_WaveColor");
    waveAmount = render.material.GetFloat("_WaveAmount");
    waterSpeed = render.material.GetFloat("_WaterSpeed");

    sizeSlider.value = planet.GetComponent<Transform>().localScale.x;
    amountSlider.value = waterAmount;
    freqSlider.value = waterFreq;
    waveAmountSlider.value = waveAmount;
    speedSlider.value = waterSpeed;
  }
	
	// Update is called once per frame
	void Update () {

    if (!planet) return;

    float newSize = sizeSlider.value;
    waterAmount = amountSlider.value;
    waterFreq = freqSlider.value;
    waveAmount = waveAmountSlider.value;
    waterSpeed = speedSlider.value;

    planet.GetComponent<Transform>().localScale = new Vector3(newSize, newSize, newSize);
    render.material.SetFloat("_Amount", waterAmount);
    render.material.SetFloat("_Freq", waterFreq);
    render.material.SetColor("_WaterColor", water);
    render.material.SetColor("_WaveColor", wave);
    render.material.SetFloat("_WaveAmount", waveAmount);
    render.material.SetFloat("_WaterSpeed", waterSpeed);
  }
}
