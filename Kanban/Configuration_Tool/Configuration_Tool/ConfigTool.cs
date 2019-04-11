/*
 * FILE: Form1.cs
 * DATE: March 19, 2019
 * AUTHORS: Billy Parmenter
 *          Mike Ramoutsakis
 *          Austin Zalak
 * DESCRIPTION:
 *          This class handles all input from the user to create 
 *          a config table for the kanban table
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Configuration_Tool
{
    public partial class ConfigTool : Form
    {
        enum Experience { New = 1, Experienced, Super};

        public int initLaneCount = 0;   
        public bool customBins = false;

        Database db = new Database();

        Dictionary<string, int> configValues = new Dictionary<string, int>();

        Dictionary<int, int> lanes = new Dictionary<int, int>();
        List<string> display = new List<string>();





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        public ConfigTool()
        {
            InitializeComponent();

            Init();


        }





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        private void Init()
        {
            configValues = db.GetConfigValues();

            timeScale.Value = configValues["Time_Scale"];
            harnessInit.Value = configValues["Harness_Count"];
            reflectorInit.Value = configValues["Reflector_Count"];
            housingInit.Value = configValues["Housing_Count"];
            lensInit.Value = configValues["Lens_Count"];
            bulbInit.Value = configValues["Bulb_Count"];
            bezelInit.Value = configValues["Bezel_Count"];
            trayCap.Value = configValues["Test_Tray_Cap"];

            lanes = db.GetLanes();
            AddLanesToDisplay();

            //Set data source
            lanesListBox.DataSource = new BindingSource(display, null);
            lanesListBox.DisplayMember = "Value";

            initLaneCount = lanes.Count;

            //Set inital selected items
            laneNumberLabel.Text = "Lane #" + (lanes.Count() + 1);
            experienceComboBox.SelectedIndex = 0; 
        } 





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        private void SubmitLaneButton_Click(object sender, EventArgs e)
        {
            display.Add("Lane #" + (display.Count + 1) + " " + experienceComboBox.Text);
            lanes.Add(lanes.Count + 1, experienceComboBox.SelectedIndex + 1);

            laneNumberLabel.Text = "Lane #" + (lanes.Count() + 1);

            lanesListBox.DataSource = null;
            lanesListBox.DataSource = new BindingSource(display, null);
            lanesListBox.DisplayMember = "Value";
        }





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        private void FinishButton_Click(object sender, EventArgs e)
        {
            configValues["Num_Lanes"] = lanes.Count;
            configValues["Time_Scale"] = (int)timeScale.Value;
            configValues["Harness_Count"] = (int)harnessInit.Value;
            configValues["Reflector_Count"] = (int)reflectorInit.Value;
            configValues["Housing_Count"] = (int)housingInit.Value;
            configValues["Lens_Count"] = (int)lensInit.Value;
            configValues["Bulb_Count"] = (int)bulbInit.Value;
            configValues["Bezel_Count"] = (int)bezelInit.Value;
            configValues["Test_Tray_Cap"] = (int)trayCap.Value;

            db.UpdateDatabase(configValues, lanes, initLaneCount);

            this.Close();
        }





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        private void AddLanesToDisplay()
        {
            int i = 1;
            string expString;
            foreach (var experience in lanes)
            {
                if(lanes[i] == (int)Experience.New)
                {
                    expString = "New Worker";
                }
                else if (lanes[i] == (int)Experience.Experienced)
                {
                    expString = "Experienced Worker";
                }
                else
                {
                    expString = "Super Worker";
                }

                display.Add("Lane #" + (display.Count + 1) + " " + expString);

                i++;
            }
        }

        private void LaneCLicked(object sender, EventArgs e)
        {
            submitLaneButton.Visible = false;
            updateButton.Visible = true;
            cancelButton.Visible = true;

            laneNumberLabel.Text = "Lane #" + (lanesListBox.SelectedIndex + 1);

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            submitLaneButton.Visible = true;
            updateButton.Visible = false;
            cancelButton.Visible = false;

            laneNumberLabel.Text = "Lane #" + (lanes.Count() + 1);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            display[lanesListBox.SelectedIndex] = "Lane #" + (lanesListBox.SelectedIndex + 1) + " " + experienceComboBox.Text;
            lanes[lanesListBox.SelectedIndex + 1] = experienceComboBox.SelectedIndex + 1;

            submitLaneButton.Visible = true;
            updateButton.Visible = false;
            cancelButton.Visible = false;

            laneNumberLabel.Text = "Lane #" + (lanes.Count() + 1);

            lanesListBox.DataSource = null;
            lanesListBox.DataSource = new BindingSource(display, null);
            lanesListBox.DisplayMember = "Value";
        }
    }
}
