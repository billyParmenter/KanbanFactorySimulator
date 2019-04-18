using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASQL_Andon_Display
{
    public partial class MainForm : Form
    {
        private Database db;

        public MainForm()
        {
            try
            {
                db = new Database();
            }catch(Exception e)
            {
                MessageBox.Show("Can not connect to database: " + e.Message);
                return;
            }

            InitializeComponent();

            workstationsBindingSource.DataSource = db.Lanes;

            BinChart.DataSource = db.Bins;
            BinChart.DataBind();
            
            UpdateTimer.Start();

            db.UpdateLanes();
            LaneSelector.SelectedIndex = 0;
        }

        /// <summary>
        /// Updates the lanes when the user attempts to change the lane.
        /// </summary>
        private void UpdateLanes(object sender, EventArgs args)
        {
            db.UpdateLanes();
        }

        /// <summary>
        /// Updates the database's currently selected lane.
        /// </summary>
        private void UpdateSelectedLane(object sender, EventArgs args)
        {
            if (LaneSelector.SelectedValue != null)
            {
                db.CurrentLane = (int)LaneSelector.SelectedValue;
                Text = "Andon Display for Lane " + db.CurrentLane;
            }
        }

        /// <summary>
        /// Updates all information about the current lane
        /// </summary>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            db.UpdateBins();
            BinChart.DataBind();
            
            AssemblyProgress.Value = (int)(db.GetAssemblyProgress() * AssemblyProgress.Maximum);
            RunnerProgress.Value = (int)(db.GetRunnerProgress() * RunnerProgress.Maximum);

            RunnerSignal.Checked = db.IsRunnerRequired();
        }
    }
}
