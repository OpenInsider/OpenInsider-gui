namespace OpenInsider
{
	partial class frmWatch
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.GroupBox groupBox2;
			System.Windows.Forms.GroupBox groupBox3;
			System.Windows.Forms.Label label4;
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.Address = new System.Windows.Forms.TextBox();
			this.DataSize = new System.Windows.Forms.TextBox();
			this.DataType = new System.Windows.Forms.ComboBox();
			this.Period = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			groupBox1 = new System.Windows.Forms.GroupBox();
			groupBox2 = new System.Windows.Forms.GroupBox();
			groupBox3 = new System.Windows.Forms.GroupBox();
			label4 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(43, 365);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(138, 365);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			label1.Location = new System.Drawing.Point(6, 16);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(86, 20);
			label1.TabIndex = 2;
			label1.Text = "Address";
			label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Address
			// 
			this.Address.Location = new System.Drawing.Point(98, 16);
			this.Address.Name = "Address";
			this.Address.Size = new System.Drawing.Size(120, 20);
			this.Address.TabIndex = 3;
			// 
			// DataSize
			// 
			this.DataSize.Location = new System.Drawing.Point(98, 42);
			this.DataSize.Name = "DataSize";
			this.DataSize.Size = new System.Drawing.Size(120, 20);
			this.DataSize.TabIndex = 4;
			// 
			// label2
			// 
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			label2.Location = new System.Drawing.Point(6, 42);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(86, 20);
			label2.TabIndex = 6;
			label2.Text = "Data size";
			label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// DataType
			// 
			this.DataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DataType.FormattingEnabled = true;
			this.DataType.Location = new System.Drawing.Point(98, 68);
			this.DataType.Name = "DataType";
			this.DataType.Size = new System.Drawing.Size(120, 21);
			this.DataType.TabIndex = 7;
			// 
			// label3
			// 
			label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			label3.Location = new System.Drawing.Point(6, 68);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(86, 21);
			label3.TabIndex = 8;
			label3.Text = "Data type";
			label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(this.Period);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(label1);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(this.Address);
			groupBox1.Controls.Add(this.DataType);
			groupBox1.Controls.Add(this.DataSize);
			groupBox1.Controls.Add(label2);
			groupBox1.Location = new System.Drawing.Point(12, 115);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(240, 138);
			groupBox1.TabIndex = 9;
			groupBox1.TabStop = false;
			groupBox1.Text = "Memory";
			// 
			// groupBox2
			// 
			groupBox2.Location = new System.Drawing.Point(12, 259);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(240, 100);
			groupBox2.TabIndex = 10;
			groupBox2.TabStop = false;
			groupBox2.Text = "Options";
			// 
			// groupBox3
			// 
			groupBox3.Location = new System.Drawing.Point(12, 9);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(240, 100);
			groupBox3.TabIndex = 11;
			groupBox3.TabStop = false;
			groupBox3.Text = "Variable";
			// 
			// Period
			// 
			this.Period.Location = new System.Drawing.Point(98, 95);
			this.Period.Name = "Period";
			this.Period.Size = new System.Drawing.Size(120, 20);
			this.Period.TabIndex = 9;
			// 
			// label4
			// 
			label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			label4.Location = new System.Drawing.Point(6, 95);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(86, 20);
			label4.TabIndex = 10;
			label4.Text = "Period";
			label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// frmWatch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(262, 393);
			this.Controls.Add(groupBox3);
			this.Controls.Add(groupBox2);
			this.Controls.Add(groupBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "frmWatch";
			this.Text = "frmWatch";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox Address;
		private System.Windows.Forms.TextBox DataSize;
		private System.Windows.Forms.ComboBox DataType;
		private System.Windows.Forms.TextBox Period;
	}
}