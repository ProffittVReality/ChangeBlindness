using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOrder : MonoBehaviour
{
	/*
    // latin square - OLD
    public List<int> order1 = new List<int> { 1, 2, 14, 3, 13, 4, 12, 5, 11, 6, 10, 7, 9, 8 };
    public List<int> order2 = new List<int> { 2, 3, 1, 14, 4, 5, 13, 6, 12, 7, 11, 8, 10, 9 };
    public List<int> order3 = new List<int> { 3, 4, 2, 5, 1, 6, 14, 7, 13, 8, 12, 9, 11, 10 };
    public List<int> order4 = new List<int> { 4, 5, 3, 6, 2, 7, 1, 8, 14, 9, 13, 10, 12, 11 };
    public List<int> order5 = new List<int> { 5, 6, 4, 7, 3, 8, 2, 9, 1, 10, 14, 11, 13, 12 };
    public List<int> order6 = new List<int> { 6, 7, 5, 8, 4, 9, 3, 10, 2, 11, 1, 12, 14, 13 };
    public List<int> order7 = new List<int> { 7, 8, 6, 9, 5, 10, 4, 11, 3, 12, 2, 13, 1, 14 };
    public List<int> order8 = new List<int> { 8, 9, 7, 10, 6, 11, 5, 12, 4, 13, 3, 14, 2, 1 };
    public List<int> order9 = new List<int> { 9, 10, 8, 11, 7, 12, 6, 13, 5, 14, 4, 1, 3, 2 };
    public List<int> order10 = new List<int> { 10, 11, 9, 12, 8, 13, 7, 14, 6, 1, 5, 2, 4, 3 };
    public List<int> order11 = new List<int> { 11, 12, 10, 13, 9, 14, 8, 1, 7, 2, 6, 3, 5, 4 };
    public List<int> order12 = new List<int> { 12, 13, 11, 14, 10, 1, 9, 2, 8, 3, 7, 4, 6, 5 };
    public List<int> order13 = new List<int> { 13, 14, 12, 1, 11, 2, 10, 3, 9, 4, 8, 5, 7, 6 };
    public List<int> order14 = new List<int> { 14, 1, 13, 2, 12, 3, 11, 4, 10, 5, 9, 6, 8, 7 };

	*/ 

    public Dictionary<int, string> dict1 = new Dictionary<int, string>(); // all original objects
    public Dictionary<int, string> dict2 = new Dictionary<int, string>(); // all changed objects


	// new latin square; first 4 objects always the same (training)
	private List<int> order1 = new List<int> { 8, 9, 5, 3, 1, 2, 13, 4, 12, 6, 11, 7, 10 };
	private List<int> order2 = new List<int> { 8, 9, 5, 3, 2, 4, 1, 6, 13, 7, 12, 10, 11 };
	private List<int> order3 = new List<int> { 8, 9, 5, 3, 4, 6, 2, 7, 1, 10, 13, 12, 11 };
	private List<int> order4 = new List<int> { 8, 9, 5, 3, 6, 7, 4, 10, 2, 11, 1, 12, 13 };
	private List<int> order5 = new List<int> { 8, 9, 5, 3, 7, 10, 6, 11, 4, 12, 2, 13, 1 };
	private List<int> order6 = new List<int> { 8, 9, 5, 3, 10, 11, 7, 12, 6, 13, 4, 1, 2 };
	private List<int> order7 = new List<int> { 8, 9, 5, 3, 11, 12, 10, 13, 7, 1, 6, 2, 4};
	private List<int> order8 = new List<int> { 8, 9, 5, 3, 12, 13, 11, 1, 10, 2, 7, 4, 6};
	private List<int> order9 = new List<int> { 8, 9, 5, 3, 13, 1, 12, 2, 11, 4, 10, 6, 7 };


    // get info from gui about which order to use
    public InputField orderInput;
    public int orderNumber;
    //public List<int> orderList = new List<int> { 2, 3, 1, 4, 5, 13, 6, 12, 7, 11, 8, 10, 9 };
    public List<int> orderList;

    // Use this for initialization
    void Start()
    {
        // add objects to dict1
        dict1.Add(1, "Toy_Blocks");
        dict1.Add(2, "Picture_Middle");
        dict1.Add(3, "Pineapples");
        dict1.Add(4, "Mug");
        dict1.Add(5, "Couch_Pillow");
        dict1.Add(6, "Book_Stack");
        dict1.Add(7, "Plates_bowls");
        dict1.Add(8, "Oval_Rug");
        dict1.Add(9, "Lamp_ON_Left");
        dict1.Add(10, "Blender_plant_master");
        dict1.Add(11, "Table_Clock");
        dict1.Add(12, "Fruit_Bowl");
        dict1.Add(13, "Mantle_Clock");
       	dict1.Add(14, "NC_Button_Frame");

        // add objects to dict2
        dict2.Add(1, "Toy_Blocks");
        dict2.Add(2, "Picture_Middle_new");
        dict2.Add(3, "Pineapples");
        dict2.Add(4, "Mug_backward");
        dict2.Add(5, "Couch");
        dict2.Add(6, "Book_Stack");
        dict2.Add(7, "Plates_bowls");
        dict2.Add(8, "Oval_Rug");
        dict2.Add(9, "Lamp_OFF_Left");
        dict2.Add(10, "Blender_plant_master");
        dict2.Add(11, "Table_Clock");
        dict2.Add(12, "Fruit_Bowl");
        dict2.Add(13, "Mantle_Clock_new");
        dict2.Add(14, "NC_Button_Frame");
    }
	/*
    // Update is called once per frame
    void Update()
    {
        orderNumber = int.Parse(orderInput.text);

        if (orderNumber == 1)
        {
            orderList = order1;
        }
        else if (orderNumber == 2)
        {
            orderList = order2;
        }
        else if (orderNumber == 3)
        {
            orderList = order3;
        }
        else if (orderNumber == 4)
        {
            orderList = order4;
        }
        else if (orderNumber == 5)
        {
            orderList = order5;
        }
        else if (orderNumber == 6)
        {
            orderList = order6;
        }
        else if (orderNumber == 7)
        {
            orderList = order7;
        }
        else if (orderNumber == 8)
        {
            orderList = order8;
        }
        else if (orderNumber == 9)
        {
            orderList = order9;
        }
        else if (orderNumber == 10)
        {
            orderList = order10;
        }
        else if (orderNumber == 11)
        {
            orderList = order11;
        }
        else if (orderNumber == 12)
        {
            orderList = order12;
        }
        else if (orderNumber == 13)
        {
            orderList = order13;
        }
        else if (orderNumber == 14)
        {
            orderList = order14;
        }

    }
	*/



}