using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;

public class GUI_Handler : MonoBehaviour {

	public GameObject menu; // Assign canvas to this in inspector, make sure script is on EventHandler
	public KeyCode hideShow; // Set to the key you want to press to open and close the GUI
	public string FileName; // Title the file you want to export data to!  Will be saved in resources.

	private bool isShowing;
	public InputField raName, partic, exp, age, height, other; 
	public Toggle left, right, click, turn;
	public Dropdown sex;
	public Text sexLabel;
	public bool isVisible {
		get {
			return menu.activeSelf;
		}
	}

	//public RotationController translationScript;

	// Use this for initialization
	void Start () {
		if (FileName.Equals (""))
			FileName = "default";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (hideShow)) {
			isShowing = !isShowing;
			menu.SetActive (isShowing);
		}
	}

        public void exportData(List<string> data) {
            //string path = @"Assets\Data\data.txt";

            // changed to .csv datafile
            string path = @"Assets\Data\changeBlindness.csv";
            DateTime now = DateTime.Now;
            string theTime = DateTime.Now.ToString("hh:mm:ss");
            string theDate = DateTime.Now.ToString("d");
            if (!File.Exists(path))
            {
                string header = "Time,RAName,Participant#,Experiment#,Age,Height,Gender,Hand,Condition,Other";
                File.WriteAllText(path, header);
            }

            string hand = "";
            if (left.isOn)
                hand = hand + "L";
            if (right.isOn)
                hand = hand + "R";

            string condition = "";
            if (left.isOn)
                condition = condition + "Click";
            if (right.isOn)
                condition = condition + "Turn";

            //string appendText = "\r\n" + theTime + " " + theDate + "\t" + raName.text + "\t" + partic.text + "\t" + exp.text + "\t" + age.text +
            //"\t" + height.text + "\t" + sex.captionText.text + "\t" + hand + "\t" + condition + "\t" + other.text;

            // need this format for .csv
            string appendText = theTime + "," + theDate + "," + raName.text + "," + partic.text + "," + exp.text + "," + age.text +
                        "," + height.text + "," + sex.captionText.text + "," + hand + "," + condition + "," + other.text + "\n";
            File.AppendAllText(path, appendText);

    }

}
