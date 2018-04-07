namespace Roughness {
	
    partial class FormSettings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_console_line_p1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_p1 = new System.Windows.Forms.ComboBox();
            this.button_put_p1 = new System.Windows.Forms.Button();
            this.button_down_p1 = new System.Windows.Forms.Button();
            this.button_right_p1 = new System.Windows.Forms.Button();
            this.button_up_p1 = new System.Windows.Forms.Button();
            this.button_left_p1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button_up_p2 = new System.Windows.Forms.Button();
            this.button_left_p2 = new System.Windows.Forms.Button();
            this.button_right_p2 = new System.Windows.Forms.Button();
            this.button_down_p2 = new System.Windows.Forms.Button();
            this.button_put_p2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_p2 = new System.Windows.Forms.ComboBox();
            this.label_console_line_p2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(885, 495);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(877, 469);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Controllers";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_console_line_p2);
            this.groupBox2.Controls.Add(this.comboBox_p2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.button_put_p2);
            this.groupBox2.Controls.Add(this.button_down_p2);
            this.groupBox2.Controls.Add(this.button_right_p2);
            this.groupBox2.Controls.Add(this.button_left_p2);
            this.groupBox2.Controls.Add(this.button_up_p2);
            this.groupBox2.Location = new System.Drawing.Point(4, 243);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(867, 230);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Player 2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_console_line_p1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox_p1);
            this.groupBox1.Controls.Add(this.button_put_p1);
            this.groupBox1.Controls.Add(this.button_down_p1);
            this.groupBox1.Controls.Add(this.button_right_p1);
            this.groupBox1.Controls.Add(this.button_up_p1);
            this.groupBox1.Controls.Add(this.button_left_p1);
            this.groupBox1.Location = new System.Drawing.Point(4, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(867, 230);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Player 1";
            // 
            // label_console_line_p1
            // 
            this.label_console_line_p1.AutoSize = true;
            this.label_console_line_p1.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_console_line_p1.Location = new System.Drawing.Point(273, 196);
            this.label_console_line_p1.Name = "label_console_line_p1";
            this.label_console_line_p1.Size = new System.Drawing.Size(19, 15);
            this.label_console_line_p1.TabIndex = 7;
            this.label_console_line_p1.Text = "---";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Control";
            // 
            // comboBox_p1
            // 
            this.comboBox_p1.FormattingEnabled = true;
            this.comboBox_p1.Items.AddRange(new object[] {
            "Keyboard"});
            this.comboBox_p1.Location = new System.Drawing.Point(10, 36);
            this.comboBox_p1.Name = "comboBox_p1";
            this.comboBox_p1.Size = new System.Drawing.Size(262, 21);
            this.comboBox_p1.TabIndex = 5;
            this.comboBox_p1.Text = "Keyboard";
            // 
            // button_put_p1
            // 
            this.button_put_p1.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_put_p1.Location = new System.Drawing.Point(218, 120);
            this.button_put_p1.Name = "button_put_p1";
            this.button_put_p1.Size = new System.Drawing.Size(50, 50);
            this.button_put_p1.TabIndex = 4;
            this.button_put_p1.Text = "Put bomb";
            this.button_put_p1.UseVisualStyleBackColor = true;
            this.button_put_p1.Click += new System.EventHandler(this.button_put_p1_Click);
            // 
            // button_down_p1
            // 
            this.button_down_p1.Font = new System.Drawing.Font("Rockwell Condensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_down_p1.Location = new System.Drawing.Point(60, 175);
            this.button_down_p1.Name = "button_down_p1";
            this.button_down_p1.Size = new System.Drawing.Size(40, 40);
            this.button_down_p1.TabIndex = 3;
            this.button_down_p1.Text = "Down";
            this.button_down_p1.UseVisualStyleBackColor = true;
            this.button_down_p1.Click += new System.EventHandler(this.button_down_p1_Click);
            // 
            // button_right_p1
            // 
            this.button_right_p1.Font = new System.Drawing.Font("Rockwell Condensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_right_p1.Location = new System.Drawing.Point(110, 125);
            this.button_right_p1.Name = "button_right_p1";
            this.button_right_p1.Size = new System.Drawing.Size(40, 40);
            this.button_right_p1.TabIndex = 2;
            this.button_right_p1.Text = "Right";
            this.button_right_p1.UseVisualStyleBackColor = true;
            this.button_right_p1.Click += new System.EventHandler(this.button_right_p1_Click);
            // 
            // button_up_p1
            // 
            this.button_up_p1.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_up_p1.Location = new System.Drawing.Point(60, 75);
            this.button_up_p1.Name = "button_up_p1";
            this.button_up_p1.Size = new System.Drawing.Size(40, 40);
            this.button_up_p1.TabIndex = 1;
            this.button_up_p1.Text = "Up";
            this.button_up_p1.UseVisualStyleBackColor = true;
            this.button_up_p1.Click += new System.EventHandler(this.button_up_p1_Click);
            // 
            // button_left_p1
            // 
            this.button_left_p1.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_left_p1.Location = new System.Drawing.Point(10, 125);
            this.button_left_p1.Name = "button_left_p1";
            this.button_left_p1.Size = new System.Drawing.Size(40, 40);
            this.button_left_p1.TabIndex = 0;
            this.button_left_p1.Text = "Left";
            this.button_left_p1.UseVisualStyleBackColor = true;
            this.button_left_p1.Click += new System.EventHandler(this.button_left_p1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(624, 469);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Sound";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Rockwell", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(12, 513);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 31);
            this.button1.TabIndex = 1;
            this.button1.Text = "<<<  Back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_up_p2
            // 
            this.button_up_p2.Font = new System.Drawing.Font("Rockwell", 9.75F);
            this.button_up_p2.Location = new System.Drawing.Point(60, 75);
            this.button_up_p2.Name = "button_up_p2";
            this.button_up_p2.Size = new System.Drawing.Size(40, 40);
            this.button_up_p2.TabIndex = 0;
            this.button_up_p2.Text = "Up";
            this.button_up_p2.UseVisualStyleBackColor = true;
            this.button_up_p2.Click += new System.EventHandler(this.button_up_p2_Click);
            // 
            // button_left_p2
            // 
            this.button_left_p2.Font = new System.Drawing.Font("Rockwell", 9.75F);
            this.button_left_p2.Location = new System.Drawing.Point(10, 125);
            this.button_left_p2.Name = "button_left_p2";
            this.button_left_p2.Size = new System.Drawing.Size(40, 40);
            this.button_left_p2.TabIndex = 1;
            this.button_left_p2.Text = "Left";
            this.button_left_p2.UseVisualStyleBackColor = true;
            this.button_left_p2.Click += new System.EventHandler(this.button_left_p2_Click);
            // 
            // button_right_p2
            // 
            this.button_right_p2.Font = new System.Drawing.Font("Rockwell Condensed", 9.75F);
            this.button_right_p2.Location = new System.Drawing.Point(110, 125);
            this.button_right_p2.Name = "button_right_p2";
            this.button_right_p2.Size = new System.Drawing.Size(40, 40);
            this.button_right_p2.TabIndex = 2;
            this.button_right_p2.Text = "Right";
            this.button_right_p2.UseVisualStyleBackColor = true;
            this.button_right_p2.Click += new System.EventHandler(this.button_right_p2_Click);
            // 
            // button_down_p2
            // 
            this.button_down_p2.Font = new System.Drawing.Font("Rockwell Condensed", 9.75F);
            this.button_down_p2.Location = new System.Drawing.Point(60, 175);
            this.button_down_p2.Name = "button_down_p2";
            this.button_down_p2.Size = new System.Drawing.Size(40, 40);
            this.button_down_p2.TabIndex = 3;
            this.button_down_p2.Text = "Down";
            this.button_down_p2.UseVisualStyleBackColor = true;
            this.button_down_p2.Click += new System.EventHandler(this.button_down_p2_Click);
            // 
            // button_put_p2
            // 
            this.button_put_p2.Font = new System.Drawing.Font("Rockwell", 9.75F);
            this.button_put_p2.Location = new System.Drawing.Point(218, 120);
            this.button_put_p2.Name = "button_put_p2";
            this.button_put_p2.Size = new System.Drawing.Size(50, 50);
            this.button_put_p2.TabIndex = 4;
            this.button_put_p2.Text = "Put bomb";
            this.button_put_p2.UseVisualStyleBackColor = true;
            this.button_put_p2.Click += new System.EventHandler(this.button_put_p2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Control";
            // 
            // comboBox_p2
            // 
            this.comboBox_p2.FormattingEnabled = true;
            this.comboBox_p2.Items.AddRange(new object[] {
            "Keyboard"});
            this.comboBox_p2.Location = new System.Drawing.Point(10, 36);
            this.comboBox_p2.Name = "comboBox_p2";
            this.comboBox_p2.Size = new System.Drawing.Size(262, 21);
            this.comboBox_p2.TabIndex = 6;
            this.comboBox_p2.Text = "Keyboard";
            // 
            // label_console_line_p2
            // 
            this.label_console_line_p2.AutoSize = true;
            this.label_console_line_p2.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Bold);
            this.label_console_line_p2.Location = new System.Drawing.Point(273, 196);
            this.label_console_line_p2.Name = "label_console_line_p2";
            this.label_console_line_p2.Size = new System.Drawing.Size(19, 15);
            this.label_console_line_p2.TabIndex = 7;
            this.label_console_line_p2.Text = "---";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 555);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormSettings";
            this.Text = "Settings";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_put_p1;
        private System.Windows.Forms.Button button_down_p1;
        private System.Windows.Forms.Button button_right_p1;
        private System.Windows.Forms.Button button_up_p1;
        private System.Windows.Forms.Button button_left_p1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_p1;
        private System.Windows.Forms.Label label_console_line_p1;
        private System.Windows.Forms.ComboBox comboBox_p2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_put_p2;
        private System.Windows.Forms.Button button_down_p2;
        private System.Windows.Forms.Button button_right_p2;
        private System.Windows.Forms.Button button_left_p2;
        private System.Windows.Forms.Button button_up_p2;
        private System.Windows.Forms.Label label_console_line_p2;
    }
}