using UnityEngine.UI;
using UnityEngine;

public class ControllTerrain : MonoBehaviour {

  //Planet to controll parameters of
  public GameObject planet;

  //Renderer of the sun
  private Renderer render;

  //Values that can be changed in the shader
  /*************************************/
  [Range(0.0f, 1.0f)]
  public float terrainAmount = 0.4f;

  [Range(0.0f, 5.0f)]
  public float terrainFreq = 1.2f;

  public Color land1 = Color.green;
  public Color land2 = Color.grey;

  [Range(0.0f, 1.0f)]
  public float color1Ratio = 0.5f;

  public Color mountain = Color.white;

  [Range(-0.05f, 0.15f)]
  public float mountainAmount = 0.0f;

  public Color beach = Color.yellow;

  [Range(-1.0f, 1.0f)]
  public float beachAmount = 0.0f;

  [Range(0.0f, 20.0f)]
  public float landColorFreq = 5.0f;
  /**************************************/

  // ---- UI ---
  /**************************************/
  public Slider sizeSlider;
  public Slider amountSlider;
  public Slider freqSlider;
  public Slider colorRatioSlider;
  public Slider landFreqSlider;
  public Slider mountainSlider;
  public Slider beachSlider;

  //Colors
  public Slider land1ColorR;
  public Slider land1ColorG;
  public Slider land1ColorB;

  public Slider land2ColorR;
  public Slider land2ColorG;
  public Slider land2ColorB;

  public Slider mountainColorR;
  public Slider mountainColorG;
  public Slider mountainColorB;

  public Slider beachColorR;
  public Slider beachColorG;
  public Slider beachColorB;
  /**************************************/

  //Get the size of the water to determine the beach size
  public ControllWater waterController;
  private float waterSize;
  private float newSize;

  // Used for initialization
  void Start () {

    if (!planet) return;

    render = planet.GetComponent<Renderer>();
    render.material.shader = Shader.Find("Custom/Terrain");

    //Get current values in shader
    terrainAmount = render.material.GetFloat("_Amount");
    terrainFreq = render.material.GetFloat("_Freq");
    land1 = render.material.GetColor("_LandColor1");
    land2 = render.material.GetColor("_LandColor2");
    color1Ratio = render.material.GetFloat("_Color1Ratio");
    mountain = render.material.GetColor("_MountColor");
    beach = render.material.GetColor("_BeachColor");
    landColorFreq = render.material.GetFloat("_ColorFreq");

    //Set sliders to theses values
    sizeSlider.value = planet.GetComponent<Transform>().localScale.x;
    amountSlider.value = terrainAmount;
    freqSlider.value = terrainFreq;
    colorRatioSlider.value = color1Ratio;
    landFreqSlider.value = landColorFreq;
    mountainAmount = mountainSlider.value;
    beachAmount = beachSlider.value;

    //Color sliders
    land1ColorR.value = land1.r;
    land1ColorG.value = land1.g;
    land1ColorB.value = land1.b;

    land2ColorR.value = land2.r;
    land2ColorG.value = land2.g;
    land2ColorB.value = land2.b;

    mountainColorR.value = mountain.r;
    mountainColorG.value = mountain.g;
    mountainColorB.value = mountain.b;

    beachColorR.value = beach.r;
    beachColorG.value = beach.g;
    beachColorB.value = beach.b;
  }
	
	// Update is called once per frame
	void Update () {

    if (!planet) return;

    //Get new value in slider
    newSize = sizeSlider.value;
    waterSize = waterController.getWaterSize();
    terrainAmount = amountSlider.value;
    terrainFreq = freqSlider.value;
    color1Ratio = colorRatioSlider.value;
    landColorFreq = landFreqSlider.value;
    mountainAmount = mountainSlider.value;
    beachAmount = beachSlider.value;

    //Color sliders
    land1.r = land1ColorR.value;
    land1.g = land1ColorG.value;
    land1.b = land1ColorB.value;

    land2.r = land2ColorR.value;
    land2.g = land2ColorG.value;
    land2.b = land2ColorB.value;

    mountain.r = mountainColorR.value;
    mountain.g = mountainColorG.value;
    mountain.b = mountainColorB.value;

    beach.r = beachColorR.value;
    beach.g = beachColorG.value;
    beach.b = beachColorB.value; 

    //Apply the slider values to the parameters in the shader
    planet.GetComponent<Transform>().localScale = new Vector3(newSize, newSize, newSize);
    render.material.SetFloat("_Amount", terrainAmount);
    render.material.SetFloat("_Freq", terrainFreq);
    render.material.SetColor("_LandColor1", land1);
    render.material.SetColor("_LandColor2", land2);
    render.material.SetFloat("_Color1Ratio", color1Ratio);
    render.material.SetColor("_MountColor", mountain);
    render.material.SetFloat("_MountHeight", newSize - mountainAmount);
    render.material.SetColor("_BeachColor", beach);
    render.material.SetFloat("_BecahHeigth", waterSize + beachAmount);
    render.material.SetFloat("_ColorFreq", landColorFreq);
  }
}
