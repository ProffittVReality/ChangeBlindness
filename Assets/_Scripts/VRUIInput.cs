using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(SteamVR_LaserPointer))]
public class VRUIInput : MonoBehaviour
{
	// gui
	public GameObject menu; // Assign canvas to this in inspector, make sure script is on EventHandler
	public KeyCode hideShow; // Set to the key you want to press to open and close the GUI
	public string FileName; // Title the file you want to export data to!  Will be saved in resources.
	public InputField raName, partic, exp, age, height, other;
	public Toggle left, right, click, turn;
	public Dropdown sex;
	public Text sexLabel;

	// controller/raycast
	private SteamVR_LaserPointer laserPointer;
	private SteamVR_TrackedController trackedController;
	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;

	public GameObject cam;

	//Set to True after Export has been clicked
	public bool programStarted = false;

	//Called only once to make sure items only set and clear once
	public bool itemsClearedAndSet = false;

	public GameObject startButton; // assign in the Inspector
	public GameObject correctButton;
    public GameObject incorrectButton;

    public float timer;

	public float timeOfClick;

	//Global list of lists to access which list inputted for study
	public List<List<int>> orderList = new List<List<int>> ();

	// first 4 objects always the same (training) - denny's change
	/*
	private List<int> order1 = new List<int> { 8, 9, 5, 3, 1, 2, 13, 4, 12, 6, 11, 7, 10 };
	private List<int> order2 = new List<int> { 8, 9, 5, 3, 2, 4, 1, 6, 13, 7, 12, 10, 11 };
	private List<int> order3 = new List<int> { 8, 9, 5, 3, 4, 6, 2, 7, 1, 10, 13, 11, 12 };
	private List<int> order4 = new List<int> { 8, 9, 5, 3, 6, 7, 4, 10, 2, 11, 1, 12, 13 };
	private List<int> order5 = new List<int> { 8, 9, 5, 3, 7, 10, 6, 11, 4, 12, 2, 13, 1 };
	private List<int> order6 = new List<int> { 8, 9, 5, 3, 10, 11, 7, 12, 6, 13, 4, 1, 2 };
	private List<int> order7 = new List<int> { 8, 9, 5, 3, 11, 12, 10, 13, 7, 1, 6, 2, 4 };
	private List<int> order8 = new List<int> { 8, 9, 5, 3, 12, 13, 11, 1, 10, 2, 7, 4, 6 };
	private List<int> order9 = new List<int> { 8, 9, 5, 3, 13, 1, 12, 2, 11, 4, 10, 6, 7 };
	*/

	private List<int> order1 = new List<int> { 8, 9, 5, 3, 1, 2, 13, 4, 12, 6, 11, 7, 10, 14 };
	private List<int> order2 = new List<int> { 8, 9, 5, 3, 2, 4, 1, 6, 13, 7, 12, 10, 11, 14 };
	private List<int> order3 = new List<int> { 8, 9, 5, 3, 4, 6, 2, 7, 1, 10, 13, 11, 12, 14 };
	private List<int> order4 = new List<int> { 8, 9, 5, 3, 6, 7, 4, 10, 2, 11, 1, 12, 13, 14 };
	private List<int> order5 = new List<int> { 8, 9, 5, 3, 7, 10, 6, 11, 4, 12, 2, 13, 1, 14 };
	private List<int> order6 = new List<int> { 8, 9, 5, 3, 10, 11, 7, 12, 6, 13, 4, 1, 2, 14 };
	private List<int> order7 = new List<int> { 8, 9, 5, 3, 11, 12, 10, 13, 7, 1, 6, 2, 4, 14 };
	private List<int> order8 = new List<int> { 8, 9, 5, 3, 12, 13, 11, 1, 10, 2, 7, 4, 6, 14 };
	private List<int> order9 = new List<int> { 8, 9, 5, 3, 13, 1, 12, 2, 11, 4, 10, 6, 7, 14 };

    public Dictionary<int, GameObject> tagScriptMap = new Dictionary<int, GameObject>();

    public Dictionary<int, int> checkItemPassed = new Dictionary<int, int>();

    //Index of current item changing
    public int globalIndex = 0;

	//Index of which list is being used
	public int globalListIndex = 0;

    public string currentItemTag;
    public int currentItemTagInt;

    private bool switchItems = false;
    private bool activateItem = false;

	public RaycastHit hitInfo;

	public int listInput;

    private void OnEnable()
	{
		laserPointer = GetComponent<SteamVR_LaserPointer>();
		laserPointer.PointerIn -= HandlePointerIn;
		laserPointer.PointerIn += HandlePointerIn;
		laserPointer.PointerOut -= HandlePointerOut;
		laserPointer.PointerOut += HandlePointerOut;

		trackedObject = GetComponent<SteamVR_TrackedObject>();

		trackedController = GetComponent<SteamVR_TrackedController>();
		if (trackedController == null)
		{
			trackedController = GetComponentInParent<SteamVR_TrackedController>();
		}
		trackedController.TriggerClicked -= HandleTriggerClicked;
		trackedController.TriggerClicked += HandleTriggerClicked;

		trackedController.TriggerClicked += new ClickedEventHandler(SelectItem);
	}

    void Start()
    {

		Debug.Log ("hello start" );

        orderList.Add(order1);
        orderList.Add(order2);
        orderList.Add(order3);
        orderList.Add(order4);
        orderList.Add(order5);
        orderList.Add(order6);
        orderList.Add(order7);
        orderList.Add(order8);
		orderList.Add (order9);

		Debug.Log ("added" );


        correctButton.SetActive(false);
        incorrectButton.SetActive(false);
    }

    void Awake()
    {
		// changed from 13 to 14
        for (int i = 0; i < 14; i++)
        {
			string tag = (i+1).ToString();
            //Creates GameObject copy of current script to save in list for later access
            GameObject itemScript = GameObject.FindGameObjectWithTag(tag);
            tagScriptMap.Add(i, itemScript);
            //Disable the Script so item does not begin changing
			Debug.Log("i before: " + tag);
			try {
				itemScript.SetActive(false);
			} catch (NullReferenceException e) {
			}

        }
    }
		
	public void clearAndSaveItems()
	{   
		//Number of list grabbed through gui dropdown. If nothing picked, default is 1
		int listInput = GameObject.FindGameObjectWithTag("order").GetComponent<Dropdown>().value;

		Debug.Log ("inputList: " + listInput);

		//Sets input to variable keeping track
		globalListIndex = listInput; // dropdown starts at 0

		Debug.Log ("globalListIndex: " + globalListIndex);

		Debug.Log ("globalIndex: " + globalIndex);

        Debug.Log("check " + orderList);
		Debug.Log("check1 " + orderList[globalListIndex]);
		Debug.Log("check2 " + orderList[globalListIndex][globalIndex]);

		String tag = (orderList[globalListIndex][globalIndex]).ToString();
		Debug.Log ("tag " + tag);
		Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag(tag))
			.SetActive(true);

        for (int i = 0; i < orderList[globalListIndex].Count; i++)
        {
            checkItemPassed.Add(orderList[globalListIndex][i], 0);
        }
    }
		
	void Update()
	{
		//device = SteamVR_Controller.Input((int)trackedObject.index);

		if (programStarted)
		{
			//dropdown.Select ();
			//dropdown.RefreshShownValue ();

            if (!itemsClearedAndSet)
            {				
                itemsClearedAndSet = true;
                clearAndSaveItems();
            }		
            currentItemTagInt = orderList[globalListIndex][globalIndex];
            currentItemTag = orderList[globalListIndex][globalIndex].ToString();

			timer += Time.deltaTime;
        }  
	}

    IEnumerator activateItems(bool active)
    {
        yield return null;

        Debug.Log("entered activateitems");

        print("active in hierarchy: " + tagScriptMap[currentItemTagInt].activeInHierarchy);
        print("active self: " + tagScriptMap[currentItemTagInt].activeSelf);
        print("active bool: " + active);

        tagScriptMap[currentItemTagInt].SetActive(active);
        switchItems = false;
    }

    public void ClearTimer()
	{
		timer = 0;
	}

	// called on click for export button in gui
	public void exportStart() 
	{
		programStarted = true;
	}

 
	public void SelectItem(object sender, ClickedEventArgs e)
	{
		//RaycastHit hitInfo;

		if (Physics.Raycast(transform.position, transform.forward * 10, out hitInfo))
		{
			if (trackedController.triggerPressed)
			{
				// for exporting the data from each trial
				List<string> guessData = new List<string> {};
				timeOfClick = timer;

				Debug.Log("Hit: " + hitInfo.transform.name);                
                
				//Grabs the tag of current Script
                Debug.Log("Hit1: " + hitInfo.transform.name);

                //Grabs the first number of tag of parent holding the Script
                string hitItemTag = hitInfo.transform.tag.Substring(0, currentItemTag.Length);
                Debug.Log("Hit2cit: " + currentItemTag);

				// feedback for tutorial room

				GameObject tutorialDresser = GameObject.FindGameObjectWithTag("tutorial");
				if(hitInfo.transform.name.Equals(tutorialDresser.transform.name))
				{
					showFeedback (true);
				}


                //Checks to see if they are the same item (i.e. Tag 3 compared with tag 3(_1))
				Debug.Log("chck tag " + currentItemTag);

                if (currentItemTag.Equals(hitItemTag))
				{
                    Debug.Log("currentItemTag: " + currentItemTagInt);
                    Debug.Log("Hit4: " + hitInfo.transform.name);

                    showFeedback (true);
                                				
                    Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag(currentItemTag))
                        .SetActive(false);

                    switchItems = true;
                    activateItem = false;

                    //Move on the next item in list
                    if (checkItemPassed[currentItemTagInt] == 0)
                    {
						Debug.Log("ented checkItem passed: " + currentItemTag);

                        globalIndex++;
						checkItemPassed[currentItemTagInt] = 1;
						currentItemTag = orderList[globalListIndex][globalIndex].ToString();

                    }
						
                    Debug.Log("checktag changed: " + currentItemTag);
					                                     	
					guessData.Add(timeOfClick.ToString());
					guessData.Add("Correct");
					guessData.Add(hitInfo.transform.name);

					Debug.Log("currentItemTag2: " + currentItemTag);

					Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag(currentItemTag))
						.SetActive(true);
				}
				else
				{
					showFeedback (false);

					guessData.Add(timeOfClick.ToString());
					guessData.Add("Incorrect");
					guessData.Add(hitInfo.transform.name);
				}
				// exports after every click
				ExportData(guessData);
			}
		}
	}

	public void showFeedback(bool correct)
	{
        if (correct) 
		{
            correctButton.SetActive(true);
		}
		if(hitInfo.transform.name.Equals("Teleport_Button") || hitInfo.transform.name.Equals("Grass") || hitInfo.transform.name.Equals("tree_1_trunk"))
		{
			incorrectButton.SetActive(false);
			correctButton.SetActive(false);
		}
		else 
		{
			incorrectButton.SetActive(true); 
		}
        StartCoroutine(HideFeedback());
    }

	                                                                                     
    IEnumerator HideFeedback()
    {
        yield return new WaitForSeconds(2.0f);
        correctButton.SetActive(false);
        incorrectButton.SetActive(false);
    }
    

	// DEFAULTS
	private void HandleTriggerClicked(object sender, ClickedEventArgs e)
	{
		if (EventSystem.current.currentSelectedGameObject != null)
		{
			ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}
	}

	private void HandlePointerIn(object sender, PointerEventArgs e)
	{
		var button = e.target.GetComponent<Button>();
		if (button != null)
		{
			button.Select();
			Debug.Log("HandlePointerIn", e.target.gameObject);
		}
	}

	private void HandlePointerOut(object sender, PointerEventArgs e)
	{
		var button = e.target.GetComponent<Button>();
		if (button != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
			Debug.Log("HandlePointerOut", e.target.gameObject);
		}
	}



	public void ExportData(List<string> guessData)
	{
    // changed to .csv datafile

        string path = @"Assets\Assets\Data\changeBlindness_real.csv";
		DateTime now = DateTime.Now;
		string theTime = DateTime.Now.ToString("hh:mm:ss");
		string theDate = DateTime.Now.ToString("d");
		// create file manually

		if (!File.Exists(path))
		{
			string header = "Time,Date,RAName,Participant#,Experiment#,Age,Height,Gender,Hand,Order,Condition,Other,TimeofClick,Correct/Incorrect,ObjectSelected";
			File.WriteAllText(path, header);
		}


		string hand = "";
		if (left.isOn)
			hand = hand + "L";
		if (right.isOn)
			hand = hand + "R";

		string condition = "";
		if (click.isOn)
			condition = condition + "Click";
		if (turn.isOn)
			condition = condition + "Turn";

		int order = globalListIndex + 1;


		// kathryn's edits; from height study gui script
		string guessDataText = "";
		foreach (string item in guessData)
		{
			guessDataText += "," + item;
		}

		string fixedData = theTime + "," + theDate + "," + raName.text + "," + partic.text + "," + exp.text + "," + age.text +
			"," + height.text + "," + sex.captionText.text + "," + hand + "," + order + "," + condition + "," + other.text;
		string appendData = fixedData + guessDataText + "\n";
		File.AppendAllText(path, appendData);
	}

}