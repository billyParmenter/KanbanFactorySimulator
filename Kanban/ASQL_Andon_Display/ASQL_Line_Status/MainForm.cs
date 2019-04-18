using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASQL_Line_Status
{
    public partial class MainForm : Form
    {
        private Database database;

        private DataTable data = new DataTable();

        public MainForm()
        {
            InitializeComponent();

            try
            {
                database = new Database();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect to database: " + ex.Message);
                return;
            }

            database.GetLaneInformation(data);
            DataDisplay.DataSource = data;

            UpdateTimer.Start();
        }

        private void UpdateData(object sender, EventArgs args)
        {
            database.GetLaneInformation(data);
            DataDisplay.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
        }
    }
}
