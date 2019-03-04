using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LightIntensityChanger : MonoBehaviour          
{
	public GameObject IntensityprogressBar, SelectedLight, IntensityChanger, sndCam;
	Quaternion fromRotation, toRotation;
    private string hitObj;
	private float xDeg, lerpSpeed;
    public float fillSpeed;

    private int speed;

	// Use this for initialization
	void Start ()
	{
        hitObj = "";
        speed = 5; lerpSpeed = 8f; fillSpeed = 0.005f;
		IntensityprogressBar.GetComponent<Image>().fillAmount = 0.6f;

        EventTrigger trigger = IntensityChanger.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }


    public void OnDragDelegate(PointerEventData data)
    {
        if (data.pointerEnter.name == "LightIntensityController")
            hitObj = "LightController";        
     }


	// Update is called once per frame
	void Update () 
	{
        if (Input.GetMouseButton(0))
        {
            if (hitObj == "LightController")
            {
                xDeg -= Input.GetAxis("Mouse Y") * speed;
                fromRotation = IntensityChanger.transform.rotation;
                toRotation = Quaternion.Euler(0f, 0f, xDeg);


                IntensityChanger.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);

                Vector3 axis = new Vector3(0.0f, 1.0f, 0.0f);
                Vector3 vectorA = fromRotation * axis;
                Vector3 vectorB = toRotation * axis;

                float angleA = Mathf.Atan2(vectorA.y, vectorA.x) * Mathf.Rad2Deg;
                float angleB = Mathf.Atan2(vectorB.y, vectorB.x) * Mathf.Rad2Deg;


                // get the signed difference in these angles
                var angleDiff = Mathf.DeltaAngle(angleA, angleB);

                if (angleDiff > 0)
                {
                    IntensityprogressBar.GetComponent<Image>().fillAmount -= fillSpeed;
                    Debug.Log("Clock Wise.......");
                    // here you can rotate 3d objects 
                    // 3D object rotation code... 


                }
                else if (angleDiff < 0)
                {
                    IntensityprogressBar.GetComponent<Image>().fillAmount += fillSpeed;
                    Debug.Log("Counter Clock Wise......");
                    // here you can rotate 3d objects 
                    // 3D object rotation code... 

                }

                // fillamount is 0 to 1, put it to intensity 
                SelectedLight.GetComponent<Light>().intensity = IntensityprogressBar.GetComponent<Image>().fillAmount;
                SelectedLight.GetComponent<Light>().shadowStrength = IntensityprogressBar.GetComponent<Image>().fillAmount;
            }
        }
        else if (Input.GetMouseButtonUp(0))        
            hitObj = "";
        
    }
}
