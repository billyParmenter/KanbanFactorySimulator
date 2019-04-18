namespace ASQL_Andon_Display
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.LaneSelector = new System.Windows.Forms.ComboBox();
            this.workstationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.binBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AssemblyProgress = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.RunnerSignal = new System.Windows.Forms.CheckBox();
            this.RunnerProgress = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.BinChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.workstationsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinChart)).BeginInit();
            this.SuspendLayout();
            // 
            // LaneSelector
            // 
            this.LaneSelector.DataSource = this.workstationsBindingSource;
            this.LaneSelector.FormattingEnabled = true;
            this.LaneSelector.Location = new System.Drawing.Point(12, 25);
            this.LaneSelector.Name = "LaneSelector";
            this.LaneSelector.Size = new System.Drawing.Size(139, 21);
            this.LaneSelector.TabIndex = 0;
            this.LaneSelector.DropDown += new System.EventHandler(this.UpdateLanes);
            this.LaneSelector.SelectedIndexChanged += new System.EventHandler(this.UpdateSelectedLane);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Workstation ID";
            // 
            // AssemblyProgress
            // 
            this.AssemblyProgress.Location = new System.Drawing.Point(12, 65);
            this.AssemblyProgress.Maximum = 500;
            this.AssemblyProgress.Name = "AssemblyProgress";
            this.AssemblyProgress.Size = new System.Drawing.Size(139, 23);
            this.AssemblyProgress.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Assembly Progress";
            // 
            // RunnerSignal
            // 
            this.RunnerSignal.AutoCheck = false;
            this.RunnerSignal.AutoSize = true;
            this.RunnerSignal.Location = new System.Drawing.Point(12, 136);
            this.RunnerSignal.Name = "RunnerSignal";
            this.RunnerSignal.Size = new System.Drawing.Size(107, 17);
            this.RunnerSignal.TabIndex = 7;
            this.RunnerSignal.Text = "Runner Required";
            this.RunnerSignal.UseVisualStyleBackColor = true;
            // 
            // RunnerProgress
            // 
            this.RunnerProgress.Location = new System.Drawing.Point(12, 107);
            this.RunnerProgress.Maximum = 500;
            this.RunnerProgress.Name = "RunnerProgress";
            this.RunnerProgress.Size = new System.Drawing.Size(139, 23);
            this.RunnerProgress.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Runner Progress";
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // BinChart
            // 
            this.BinChart.BorderlineWidth = 0;
            chartArea3.Name = "ChartArea1";
            this.BinChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.BinChart.Legends.Add(legend3);
            this.BinChart.Location = new System.Drawing.Point(170, 9);
            this.BinChart.Name = "BinChart";
            series3.ChartArea = "ChartArea1";
            series3.IsValueShownAsLabel = true;
            series3.Legend = "Legend1";
            series3.Name = "Bins";
            series3.XValueMember = "Bin Name";
            series3.YValueMembers = "Part Count";
            this.BinChart.Series.Add(series3);
            this.BinChart.Size = new System.Drawing.Size(402, 240);
            this.BinChart.TabIndex = 11;
            this.BinChart.Text = "chart1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.BinChart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RunnerProgress);
            this.Controls.Add(this.RunnerSignal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AssemblyProgress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LaneSelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.workstationsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox LaneSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar AssemblyProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox RunnerSignal;
        private System.Windows.Forms.ProgressBar RunnerProgress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.BindingSource workstationsBindingSource;
        private System.Windows.Forms.BindingSource binBindingSource;
        private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.DataVisualization.Charting.Chart BinChart;
    }
}

