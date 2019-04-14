using System;

namespace Configuration_Tool
{
    partial class ConfigTool
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
            this.lanesListBox = new System.Windows.Forms.ListBox();
            this.submitLaneButton = new System.Windows.Forms.Button();
            this.experienceComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.laneNumberLabel = new System.Windows.Forms.Label();
            this.timeScale = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.trayCap = new System.Windows.Forms.NumericUpDown();
            this.bezelInit = new System.Windows.Forms.NumericUpDown();
            this.bulbInit = new System.Windows.Forms.NumericUpDown();
            this.lensInit = new System.Windows.Forms.NumericUpDown();
            this.housingInit = new System.Windows.Forms.NumericUpDown();
            this.reflectorInit = new System.Windows.Forms.NumericUpDown();
            this.harnessInit = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.updateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.finishButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.runnerDelay = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.timeScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trayCap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bezelInit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bulbInit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lensInit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.housingInit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reflectorInit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.harnessInit)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runnerDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // lanesListBox
            // 
            this.lanesListBox.FormattingEnabled = true;
            this.lanesListBox.Location = new System.Drawing.Point(21, 82);
            this.lanesListBox.Name = "lanesListBox";
            this.lanesListBox.Size = new System.Drawing.Size(219, 95);
            this.lanesListBox.TabIndex = 0;
            this.lanesListBox.Click += new System.EventHandler(this.LaneCLicked);
            // 
            // submitLaneButton
            // 
            this.submitLaneButton.Location = new System.Drawing.Point(9, 53);
            this.submitLaneButton.Name = "submitLaneButton";
            this.submitLaneButton.Size = new System.Drawing.Size(75, 23);
            this.submitLaneButton.TabIndex = 3;
            this.submitLaneButton.Text = "Add Lane";
            this.submitLaneButton.UseVisualStyleBackColor = true;
            this.submitLaneButton.Click += new System.EventHandler(this.SubmitLaneButton_Click);
            // 
            // experienceComboBox
            // 
            this.experienceComboBox.FormattingEnabled = true;
            this.experienceComboBox.Items.AddRange(new object[] {
            "New Worker",
            "Experienced Worker",
            "Super Worker"});
            this.experienceComboBox.Location = new System.Drawing.Point(108, 23);
            this.experienceComboBox.Name = "experienceComboBox";
            this.experienceComboBox.Size = new System.Drawing.Size(100, 21);
            this.experienceComboBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Worker experience";
            // 
            // laneNumberLabel
            // 
            this.laneNumberLabel.AutoSize = true;
            this.laneNumberLabel.Location = new System.Drawing.Point(6, 3);
            this.laneNumberLabel.Name = "laneNumberLabel";
            this.laneNumberLabel.Size = new System.Drawing.Size(47, 13);
            this.laneNumberLabel.TabIndex = 0;
            this.laneNumberLabel.Text = "Lane #1";
            // 
            // timeScale
            // 
            this.timeScale.Location = new System.Drawing.Point(204, 132);
            this.timeScale.Name = "timeScale";
            this.timeScale.Size = new System.Drawing.Size(47, 20);
            this.timeScale.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 116);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Other settings:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(138, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Time Scale";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Parts Per Bin:";
            // 
            // trayCap
            // 
            this.trayCap.Location = new System.Drawing.Point(75, 132);
            this.trayCap.Name = "trayCap";
            this.trayCap.Size = new System.Drawing.Size(47, 20);
            this.trayCap.TabIndex = 18;
            // 
            // bezelInit
            // 
            this.bezelInit.Location = new System.Drawing.Point(204, 72);
            this.bezelInit.Name = "bezelInit";
            this.bezelInit.Size = new System.Drawing.Size(47, 20);
            this.bezelInit.TabIndex = 17;
            // 
            // bulbInit
            // 
            this.bulbInit.Location = new System.Drawing.Point(204, 46);
            this.bulbInit.Name = "bulbInit";
            this.bulbInit.Size = new System.Drawing.Size(47, 20);
            this.bulbInit.TabIndex = 16;
            // 
            // lensInit
            // 
            this.lensInit.Location = new System.Drawing.Point(204, 20);
            this.lensInit.Name = "lensInit";
            this.lensInit.Size = new System.Drawing.Size(47, 20);
            this.lensInit.TabIndex = 15;
            // 
            // housingInit
            // 
            this.housingInit.Location = new System.Drawing.Point(75, 72);
            this.housingInit.Name = "housingInit";
            this.housingInit.Size = new System.Drawing.Size(47, 20);
            this.housingInit.TabIndex = 14;
            // 
            // reflectorInit
            // 
            this.reflectorInit.Location = new System.Drawing.Point(75, 46);
            this.reflectorInit.Name = "reflectorInit";
            this.reflectorInit.Size = new System.Drawing.Size(47, 20);
            this.reflectorInit.TabIndex = 13;
            // 
            // harnessInit
            // 
            this.harnessInit.Location = new System.Drawing.Point(75, 19);
            this.harnessInit.Name = "harnessInit";
            this.harnessInit.Size = new System.Drawing.Size(47, 20);
            this.harnessInit.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(138, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Bezel Bin";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(138, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Bulb Bin";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 134);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Tray cap";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(138, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Lens Bin";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Housing Bin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Reflector Bin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Harness Bin";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(272, 213);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.updateButton);
            this.tabPage1.Controls.Add(this.cancelButton);
            this.tabPage1.Controls.Add(this.submitLaneButton);
            this.tabPage1.Controls.Add(this.laneNumberLabel);
            this.tabPage1.Controls.Add(this.experienceComboBox);
            this.tabPage1.Controls.Add(this.lanesListBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(264, 187);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Lanes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(9, 53);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 5;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Visible = false;
            this.updateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(183, 53);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.runnerDelay);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.timeScale);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.trayCap);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.bezelInit);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.bulbInit);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.lensInit);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.housingInit);
            this.tabPage2.Controls.Add(this.harnessInit);
            this.tabPage2.Controls.Add(this.reflectorInit);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(264, 187);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // finishButton
            // 
            this.finishButton.Location = new System.Drawing.Point(206, 231);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(75, 23);
            this.finishButton.TabIndex = 5;
            this.finishButton.Text = "Finish";
            this.finishButton.UseVisualStyleBackColor = true;
            this.finishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 158);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Runner Time";
            // 
            // runnerDelay
            // 
            this.runnerDelay.Location = new System.Drawing.Point(75, 156);
            this.runnerDelay.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.runnerDelay.Name = "runnerDelay";
            this.runnerDelay.Size = new System.Drawing.Size(47, 20);
            this.runnerDelay.TabIndex = 25;
            // 
            // ConfigTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 261);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.tabControl1);
            this.Name = "ConfigTool";
            this.Text = "Kanban Configuration Tool";
            ((System.ComponentModel.ISupportInitialize)(this.timeScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trayCap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bezelInit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bulbInit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lensInit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.housingInit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reflectorInit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.harnessInit)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runnerDelay)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.ListBox lanesListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label laneNumberLabel;
        private System.Windows.Forms.Button submitLaneButton;
        private System.Windows.Forms.ComboBox experienceComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown trayCap;
        private System.Windows.Forms.NumericUpDown bezelInit;
        private System.Windows.Forms.NumericUpDown bulbInit;
        private System.Windows.Forms.NumericUpDown lensInit;
        private System.Windows.Forms.NumericUpDown housingInit;
        private System.Windows.Forms.NumericUpDown reflectorInit;
        private System.Windows.Forms.NumericUpDown harnessInit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown timeScale;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button finishButton;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown runnerDelay;
    }
}

