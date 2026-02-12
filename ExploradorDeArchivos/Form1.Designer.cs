namespace ExploradorDeArchivos
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonBack;
        private System.Windows.Forms.ToolStripButton toolStripButtonForward;
        private System.Windows.Forms.ToolStripButton toolStripButtonUp;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewFolder;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemViewLargeIcons;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemViewSmallIcons;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemViewList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemViewDetails;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemViewTile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxAddress;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TreeView treeViewDirectories;
        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ColumnHeader columnHeaderModified;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            toolStrip = new ToolStrip();
            toolStripButtonBack = new ToolStripButton();
            toolStripButtonForward = new ToolStripButton();
            toolStripButtonUp = new ToolStripButton();
            toolStripButtonNewFolder = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            toolStripButtonRefresh = new ToolStripButton();
            toolStripComboBoxAddress = new ToolStripComboBox();
            splitContainerMain = new SplitContainer();
            treeViewDirectories = new TreeView();
            listViewFiles = new ListView();
            columnHeaderName = new ColumnHeader();
            columnHeaderType = new ColumnHeader();
            columnHeaderSize = new ColumnHeader();
            columnHeaderModified = new ColumnHeader();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolStripDropDownButtonView = new ToolStripDropDownButton();
            toolStripMenuItemViewLargeIcons = new ToolStripMenuItem();
            toolStripMenuItemViewSmallIcons = new ToolStripMenuItem();
            toolStripMenuItemViewList = new ToolStripMenuItem();
            toolStripMenuItemViewDetails = new ToolStripMenuItem();
            toolStripMenuItemViewTile = new ToolStripMenuItem();
            toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.ImageScalingSize = new Size(20, 20);
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripButtonBack, toolStripButtonForward, toolStripButtonUp, toolStripButtonNewFolder, toolStripSeparator, toolStripButtonRefresh, toolStripComboBoxAddress, toolStripDropDownButtonView });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(7, 0, 1, 0);
            toolStrip.Size = new Size(1125, 28);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "toolStrip";
            // 
            // toolStripButtonBack
            // 
            toolStripButtonBack.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonBack.ImageTransparentColor = Color.Magenta;
            toolStripButtonBack.Name = "toolStripButtonBack";
            toolStripButtonBack.Size = new Size(47, 25);
            toolStripButtonBack.Text = "Atrás";
            toolStripButtonBack.Click += ToolStripButtonBack_Click;
            // 
            // toolStripButtonForward
            // 
            toolStripButtonForward.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonForward.ImageTransparentColor = Color.Magenta;
            toolStripButtonForward.Name = "toolStripButtonForward";
            toolStripButtonForward.Size = new Size(73, 25);
            toolStripButtonForward.Text = "Adelante";
            toolStripButtonForward.Click += ToolStripButtonForward_Click;
            // 
            // toolStripButtonUp
            // 
            toolStripButtonUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonUp.ImageTransparentColor = Color.Magenta;
            toolStripButtonUp.Name = "toolStripButtonUp";
            toolStripButtonUp.Size = new Size(29, 25);
            toolStripButtonUp.Text = "Subir";
            toolStripButtonUp.Click += ToolStripButtonUp_Click;
            // 
            // toolStripButtonNewFolder
            // 
            toolStripButtonNewFolder.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonNewFolder.Name = "toolStripButtonNewFolder";
            toolStripButtonNewFolder.Size = new Size(109, 25);
            toolStripButtonNewFolder.Text = "Nueva carpeta";
            toolStripButtonNewFolder.Click += ToolStripButtonNewFolder_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(6, 28);
            // 
            // toolStripButtonRefresh
            // 
            toolStripButtonRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonRefresh.ImageTransparentColor = Color.Magenta;
            toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            toolStripButtonRefresh.Size = new Size(29, 25);
            toolStripButtonRefresh.Text = "Actualizar";
            toolStripButtonRefresh.Click += ToolStripButtonRefresh_Click;
            // 
            // toolStripComboBoxAddress
            // 
            toolStripComboBoxAddress.AutoSize = false;
            toolStripComboBoxAddress.Name = "toolStripComboBoxAddress";
            toolStripComboBoxAddress.Size = new Size(457, 28);
            toolStripComboBoxAddress.SelectedIndexChanged += ToolStripComboBoxAddress_SelectedIndexChanged;
            toolStripComboBoxAddress.KeyDown += ToolStripComboBoxAddress_KeyDown;
            // 
            // toolStripDropDownButtonView
            // 
            toolStripDropDownButtonView.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButtonView.DropDownItems.AddRange(new ToolStripItem[] {
            toolStripMenuItemViewLargeIcons,
            toolStripMenuItemViewSmallIcons,
            toolStripMenuItemViewList,
            toolStripMenuItemViewDetails,
            toolStripMenuItemViewTile});
            toolStripDropDownButtonView.Name = "toolStripDropDownButtonView";
            toolStripDropDownButtonView.Size = new Size(58, 25);
            toolStripDropDownButtonView.Text = "Vista";
            // 
            // toolStripMenuItemViewLargeIcons
            // 
            toolStripMenuItemViewLargeIcons.Name = "toolStripMenuItemViewLargeIcons";
            toolStripMenuItemViewLargeIcons.Size = new Size(224, 26);
            toolStripMenuItemViewLargeIcons.Tag = "LargeIcon";
            toolStripMenuItemViewLargeIcons.Text = "Iconos grandes";
            toolStripMenuItemViewLargeIcons.Click += ToolStripMenuItemView_Click;
            // 
            // toolStripMenuItemViewSmallIcons
            // 
            toolStripMenuItemViewSmallIcons.Name = "toolStripMenuItemViewSmallIcons";
            toolStripMenuItemViewSmallIcons.Size = new Size(224, 26);
            toolStripMenuItemViewSmallIcons.Tag = "SmallIcon";
            toolStripMenuItemViewSmallIcons.Text = "Iconos pequeños";
            toolStripMenuItemViewSmallIcons.Click += ToolStripMenuItemView_Click;
            // 
            // toolStripMenuItemViewList
            // 
            toolStripMenuItemViewList.Name = "toolStripMenuItemViewList";
            toolStripMenuItemViewList.Size = new Size(224, 26);
            toolStripMenuItemViewList.Tag = "List";
            toolStripMenuItemViewList.Text = "Lista";
            toolStripMenuItemViewList.Click += ToolStripMenuItemView_Click;
            // 
            // toolStripMenuItemViewDetails
            // 
            toolStripMenuItemViewDetails.Name = "toolStripMenuItemViewDetails";
            toolStripMenuItemViewDetails.Size = new Size(224, 26);
            toolStripMenuItemViewDetails.Tag = "Details";
            toolStripMenuItemViewDetails.Text = "Detalles";
            toolStripMenuItemViewDetails.Click += ToolStripMenuItemView_Click;
            // 
            // toolStripMenuItemViewTile
            // 
            toolStripMenuItemViewTile.Name = "toolStripMenuItemViewTile";
            toolStripMenuItemViewTile.Size = new Size(224, 26);
            toolStripMenuItemViewTile.Tag = "Tile";
            toolStripMenuItemViewTile.Text = "Iconos en mosaico";
            toolStripMenuItemViewTile.Click += ToolStripMenuItemView_Click;
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Location = new Point(0, 28);
            splitContainerMain.Margin = new Padding(3, 4, 3, 4);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(treeViewDirectories);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(listViewFiles);
            splitContainerMain.Panel2.Padding = new Padding(6, 7, 6, 7);
            splitContainerMain.Size = new Size(1125, 683);
            splitContainerMain.SplitterDistance = 365;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 1;
            // 
            // treeViewDirectories
            // 
            treeViewDirectories.Dock = DockStyle.Fill;
            treeViewDirectories.HideSelection = false;
            treeViewDirectories.Location = new Point(0, 0);
            treeViewDirectories.Margin = new Padding(3, 4, 3, 4);
            treeViewDirectories.Name = "treeViewDirectories";
            treeViewDirectories.Size = new Size(365, 683);
            treeViewDirectories.TabIndex = 0;
            treeViewDirectories.BeforeExpand += TreeViewDirectories_BeforeExpand;
            treeViewDirectories.AfterSelect += treeViewDirectories_AfterSelect;
            treeViewDirectories.NodeMouseClick += TreeViewDirectories_NodeMouseClick;
            // 
            // listViewFiles
            // 
            listViewFiles.Columns.AddRange(new ColumnHeader[] { columnHeaderName, columnHeaderType, columnHeaderSize, columnHeaderModified });
            listViewFiles.Dock = DockStyle.Fill;
            listViewFiles.FullRowSelect = true;
            listViewFiles.GridLines = true;
            listViewFiles.Location = new Point(6, 7);
            listViewFiles.Margin = new Padding(3, 4, 3, 4);
            listViewFiles.MultiSelect = false;
            listViewFiles.Name = "listViewFiles";
            listViewFiles.Size = new Size(743, 669);
            listViewFiles.TabIndex = 0;
            listViewFiles.UseCompatibleStateImageBehavior = false;
            listViewFiles.View = View.Details;
            listViewFiles.DoubleClick += ListViewFiles_DoubleClick;
            listViewFiles.ShowItemToolTips = true;
            listViewFiles.ItemMouseHover += ListViewFiles_ItemMouseHover;
            // 
            // columnHeaderName
            // 
            columnHeaderName.Text = "Nombre";
            columnHeaderName.Width = 260;
            // 
            // columnHeaderType
            // 
            columnHeaderType.Text = "Tipo";
            columnHeaderType.Width = 120;
            // 
            // columnHeaderSize
            // 
            columnHeaderSize.Text = "Tamaño";
            columnHeaderSize.Width = 120;
            // 
            // columnHeaderModified
            // 
            columnHeaderModified.Text = "Modificado";
            columnHeaderModified.Width = 140;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 711);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 15, 0);
            statusStrip.Size = new Size(1125, 26);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(40, 20);
            toolStripStatusLabel.Text = "Listo";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1125, 737);
            Controls.Add(splitContainerMain);
            Controls.Add(toolStrip);
            Controls.Add(statusStrip);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Explorador de Archivos";
            Load += Form1_Load;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
