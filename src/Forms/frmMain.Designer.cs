namespace OpenInsider
{
    partial class frmMain
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
			this.Watch = new System.Windows.Forms.DataGridView();
			this.Variable = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Period = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.button4 = new System.Windows.Forms.Button();
			this.WatchMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miWatchNew = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.miWatchDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.miWatchEdit = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.Watch)).BeginInit();
			this.WatchMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// Watch
			// 
			this.Watch.AllowUserToAddRows = false;
			this.Watch.AllowUserToDeleteRows = false;
			this.Watch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.Watch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.Watch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Variable,
            this.Value,
            this.Address,
            this.Period});
			this.Watch.ContextMenuStrip = this.WatchMenuStrip;
			this.Watch.Location = new System.Drawing.Point(21, 269);
			this.Watch.MultiSelect = false;
			this.Watch.Name = "Watch";
			this.Watch.RowHeadersVisible = false;
			this.Watch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.Watch.Size = new System.Drawing.Size(561, 169);
			this.Watch.TabIndex = 0;
			this.Watch.VirtualMode = true;
			this.Watch.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Watch_CellValueNeeded);
			// 
			// Variable
			// 
			this.Variable.HeaderText = "Variable";
			this.Variable.Name = "Variable";
			// 
			// Value
			// 
			this.Value.HeaderText = "Value";
			this.Value.Name = "Value";
			// 
			// Address
			// 
			this.Address.HeaderText = "Address";
			this.Address.Name = "Address";
			this.Address.ReadOnly = true;
			// 
			// Period
			// 
			this.Period.HeaderText = "Period";
			this.Period.Name = "Period";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(121, 125);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 42);
			this.button1.TabIndex = 1;
			this.button1.Text = "Edit Connection";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(245, 125);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 42);
			this.button2.TabIndex = 2;
			this.button2.Text = "Open";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(378, 125);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 3;
			this.button3.Text = "button3";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = false;
			this.propertyGrid1.Location = new System.Drawing.Point(617, 32);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(257, 324);
			this.propertyGrid1.TabIndex = 4;
			this.propertyGrid1.ToolbarVisible = false;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(339, 41);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 5;
			this.button4.Text = "Open ELF...";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// WatchMenuStrip
			// 
			this.WatchMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWatchNew,
            this.toolStripSeparator1,
            this.miWatchDelete,
            this.miWatchEdit});
			this.WatchMenuStrip.Name = "WatchMenuStrip";
			this.WatchMenuStrip.Size = new System.Drawing.Size(158, 76);
			// 
			// miWatchNew
			// 
			this.miWatchNew.Name = "miWatchNew";
			this.miWatchNew.Size = new System.Drawing.Size(157, 22);
			this.miWatchNew.Text = "New watch ...";
			this.miWatchNew.Click += new System.EventHandler(this.miWatchNew_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(154, 6);
			// 
			// miWatchDelete
			// 
			this.miWatchDelete.Name = "miWatchDelete";
			this.miWatchDelete.Size = new System.Drawing.Size(157, 22);
			this.miWatchDelete.Text = "Delete this watch";
			this.miWatchDelete.Click += new System.EventHandler(this.miWatchDelete_Click);
			// 
			// miWatchEdit
			// 
			this.miWatchEdit.Name = "miWatchEdit";
			this.miWatchEdit.Size = new System.Drawing.Size(157, 22);
			this.miWatchEdit.Text = "Edit this watch ";
			this.miWatchEdit.Click += new System.EventHandler(this.miWatchEdit_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(896, 525);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.Watch);
			this.Name = "frmMain";
			this.Text = "OpenInsider";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.Watch)).EndInit();
			this.WatchMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Watch;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Period;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ContextMenuStrip WatchMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem miWatchNew;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem miWatchDelete;
		private System.Windows.Forms.ToolStripMenuItem miWatchEdit;
    }
}

