namespace ExploradorDeArchivos
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // ── ToolStrip ─────────────────────────────────────────────────────────
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
        private System.Windows.Forms.ToolStripLabel toolStripLabelIndexSearch;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxIndex;
        private System.Windows.Forms.ToolStripButton toolStripButtonIndexSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxAddress;

        // ── Paneles ───────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelContent;

        // ── ListView ──────────────────────────────────────────────────────────
        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ColumnHeader columnHeaderModified;

        // ── StatusStrip ───────────────────────────────────────────────────────
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;

        // ── Menú contextual ───────────────────────────────────────────────────
        private System.Windows.Forms.ContextMenuStrip contextMenuArchivo;
        private System.Windows.Forms.ToolStripMenuItem menuItemEnviarCorreo;
        private System.Windows.Forms.ToolStripMenuItem menuItemCortar;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopiar;
        private System.Windows.Forms.ToolStripMenuItem menuItemPegar;
        private System.Windows.Forms.ToolStripMenuItem menuItemCrearAcceso;
        private System.Windows.Forms.ToolStripMenuItem menuItemEliminar;
        private System.Windows.Forms.ToolStripMenuItem menuItemCambiarNombre;
        private System.Windows.Forms.ToolStripMenuItem menuItemPropiedades;

        // ── Stubs (no añadidos a Controls, usados solo para compilar) ─────────
        private System.Windows.Forms.TreeView treeViewDirectories;
        private System.Windows.Forms.RichTextBox richTextBoxIndexResults;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // ── Instanciación ─────────────────────────────────────────────────
            toolStrip                       = new System.Windows.Forms.ToolStrip();
            toolStripButtonBack             = new System.Windows.Forms.ToolStripButton();
            toolStripButtonForward          = new System.Windows.Forms.ToolStripButton();
            toolStripButtonUp               = new System.Windows.Forms.ToolStripButton();
            toolStripButtonNewFolder        = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator              = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonRefresh          = new System.Windows.Forms.ToolStripButton();
            toolStripComboBoxAddress        = new System.Windows.Forms.ToolStripComboBox();
            toolStripLabelIndexSearch       = new System.Windows.Forms.ToolStripLabel();
            toolStripTextBoxIndex           = new System.Windows.Forms.ToolStripTextBox();
            toolStripButtonIndexSearch      = new System.Windows.Forms.ToolStripButton();
            toolStripDropDownButtonView     = new System.Windows.Forms.ToolStripDropDownButton();
            toolStripMenuItemViewLargeIcons = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemViewSmallIcons = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemViewList       = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemViewDetails    = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemViewTile       = new System.Windows.Forms.ToolStripMenuItem();
            panelSidebar                    = new System.Windows.Forms.Panel();
            panelContent                    = new System.Windows.Forms.Panel();
            listViewFiles                   = new System.Windows.Forms.ListView();
            columnHeaderName                = new System.Windows.Forms.ColumnHeader();
            columnHeaderType                = new System.Windows.Forms.ColumnHeader();
            columnHeaderSize                = new System.Windows.Forms.ColumnHeader();
            columnHeaderModified            = new System.Windows.Forms.ColumnHeader();
            statusStrip                     = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel            = new System.Windows.Forms.ToolStripStatusLabel();
            contextMenuArchivo              = new System.Windows.Forms.ContextMenuStrip();
            menuItemEnviarCorreo            = new System.Windows.Forms.ToolStripMenuItem();
            menuItemCortar                  = new System.Windows.Forms.ToolStripMenuItem();
            menuItemCopiar                  = new System.Windows.Forms.ToolStripMenuItem();
            menuItemPegar                   = new System.Windows.Forms.ToolStripMenuItem();
            menuItemCrearAcceso             = new System.Windows.Forms.ToolStripMenuItem();
            menuItemEliminar                = new System.Windows.Forms.ToolStripMenuItem();
            menuItemCambiarNombre           = new System.Windows.Forms.ToolStripMenuItem();
            menuItemPropiedades             = new System.Windows.Forms.ToolStripMenuItem();
            // Stubs
            treeViewDirectories    = new System.Windows.Forms.TreeView();
            richTextBoxIndexResults = new System.Windows.Forms.RichTextBox();

            toolStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();

            // ── ToolStrip ─────────────────────────────────────────────────────
            toolStrip.BackColor        = AppColors.ToolbarBg;
            toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                toolStripButtonBack, toolStripButtonForward, toolStripButtonUp,
                toolStripButtonNewFolder, toolStripSeparator, toolStripButtonRefresh,
                toolStripComboBoxAddress, toolStripLabelIndexSearch,
                toolStripTextBoxIndex, toolStripButtonIndexSearch, toolStripDropDownButtonView
            });
            toolStrip.Location = new System.Drawing.Point(0, 0);
            toolStrip.Name     = "toolStrip";
            toolStrip.Padding  = new System.Windows.Forms.Padding(7, 0, 1, 0);
            toolStrip.Size     = new System.Drawing.Size(1125, 28);
            toolStrip.TabIndex = 0;

            toolStripButtonBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButtonBack.Name         = "toolStripButtonBack";
            toolStripButtonBack.Size         = new System.Drawing.Size(55, 25);
            toolStripButtonBack.Text         = "◄ Atrás";
            toolStripButtonBack.Click       += ToolStripButtonBack_Click;

            toolStripButtonForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButtonForward.Name         = "toolStripButtonForward";
            toolStripButtonForward.Size         = new System.Drawing.Size(88, 25);
            toolStripButtonForward.Text         = "Adelante ►";
            toolStripButtonForward.Click       += ToolStripButtonForward_Click;

            toolStripButtonUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButtonUp.Name         = "toolStripButtonUp";
            toolStripButtonUp.Size         = new System.Drawing.Size(29, 25);
            toolStripButtonUp.Text         = "▲";
            toolStripButtonUp.ToolTipText  = "Subir un nivel";
            toolStripButtonUp.Click       += ToolStripButtonUp_Click;

            toolStripButtonNewFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButtonNewFolder.Name         = "toolStripButtonNewFolder";
            toolStripButtonNewFolder.Size         = new System.Drawing.Size(120, 25);
            toolStripButtonNewFolder.Text         = "📁 Nueva carpeta";
            toolStripButtonNewFolder.Click       += ToolStripButtonNewFolder_Click;

            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new System.Drawing.Size(6, 28);

            toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButtonRefresh.Name         = "toolStripButtonRefresh";
            toolStripButtonRefresh.Size         = new System.Drawing.Size(29, 25);
            toolStripButtonRefresh.Text         = "↻";
            toolStripButtonRefresh.ToolTipText  = "Actualizar";
            toolStripButtonRefresh.Click       += ToolStripButtonRefresh_Click;

            toolStripComboBoxAddress.AutoSize = false;
            toolStripComboBoxAddress.Name     = "toolStripComboBoxAddress";
            toolStripComboBoxAddress.Size     = new System.Drawing.Size(460, 28);
            toolStripComboBoxAddress.SelectedIndexChanged += ToolStripComboBoxAddress_SelectedIndexChanged;
            toolStripComboBoxAddress.KeyDown              += ToolStripComboBoxAddress_KeyDown;

            toolStripLabelIndexSearch.Name = "toolStripLabelIndexSearch";
            toolStripLabelIndexSearch.Size = new System.Drawing.Size(55, 25);
            toolStripLabelIndexSearch.Text = "🔍 Índice";

            toolStripTextBoxIndex.Name    = "toolStripTextBoxIndex";
            toolStripTextBoxIndex.Size    = new System.Drawing.Size(130, 28);
            toolStripTextBoxIndex.KeyDown += ToolStripTextBoxIndex_KeyDown;

            toolStripButtonIndexSearch.Name   = "toolStripButtonIndexSearch";
            toolStripButtonIndexSearch.Size   = new System.Drawing.Size(60, 25);
            toolStripButtonIndexSearch.Text   = "Buscar";
            toolStripButtonIndexSearch.Click += ToolStripButtonIndexSearch_Click;

            toolStripDropDownButtonView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButtonView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                toolStripMenuItemViewLargeIcons, toolStripMenuItemViewSmallIcons,
                toolStripMenuItemViewList, toolStripMenuItemViewDetails, toolStripMenuItemViewTile
            });
            toolStripDropDownButtonView.Name = "toolStripDropDownButtonView";
            toolStripDropDownButtonView.Size = new System.Drawing.Size(55, 25);
            toolStripDropDownButtonView.Text = "Vista";

            toolStripMenuItemViewLargeIcons.Name   = "toolStripMenuItemViewLargeIcons";
            toolStripMenuItemViewLargeIcons.Tag    = "LargeIcon";
            toolStripMenuItemViewLargeIcons.Text   = "Iconos grandes";
            toolStripMenuItemViewLargeIcons.Click += ToolStripMenuItemView_Click;

            toolStripMenuItemViewSmallIcons.Name   = "toolStripMenuItemViewSmallIcons";
            toolStripMenuItemViewSmallIcons.Tag    = "SmallIcon";
            toolStripMenuItemViewSmallIcons.Text   = "Iconos pequeños";
            toolStripMenuItemViewSmallIcons.Click += ToolStripMenuItemView_Click;

            toolStripMenuItemViewList.Name   = "toolStripMenuItemViewList";
            toolStripMenuItemViewList.Tag    = "List";
            toolStripMenuItemViewList.Text   = "Lista";
            toolStripMenuItemViewList.Click += ToolStripMenuItemView_Click;

            toolStripMenuItemViewDetails.Name   = "toolStripMenuItemViewDetails";
            toolStripMenuItemViewDetails.Tag    = "Details";
            toolStripMenuItemViewDetails.Text   = "Detalles";
            toolStripMenuItemViewDetails.Click += ToolStripMenuItemView_Click;

            toolStripMenuItemViewTile.Name   = "toolStripMenuItemViewTile";
            toolStripMenuItemViewTile.Tag    = "Tile";
            toolStripMenuItemViewTile.Text   = "Iconos en mosaico";
            toolStripMenuItemViewTile.Click += ToolStripMenuItemView_Click;

            // ── Panel Sidebar ─────────────────────────────────────────────────
            panelSidebar.Dock        = System.Windows.Forms.DockStyle.Left;
            panelSidebar.Width       = 190;
            panelSidebar.BackColor   = AppColors.SidebarBg;
            panelSidebar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            panelSidebar.AutoScroll  = false;
            panelSidebar.Name        = "panelSidebar";

            // ── Panel Content ─────────────────────────────────────────────────
            panelContent.Dock      = System.Windows.Forms.DockStyle.Fill;
            panelContent.BackColor = System.Drawing.Color.White;
            panelContent.Name      = "panelContent";
            panelContent.Controls.Add(listViewFiles);

            // ── ListView ──────────────────────────────────────────────────────
            listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
                { columnHeaderName, columnHeaderType, columnHeaderSize, columnHeaderModified });
            listViewFiles.Dock                            = System.Windows.Forms.DockStyle.Fill;
            listViewFiles.FullRowSelect                   = true;
            listViewFiles.GridLines                       = false;
            listViewFiles.MultiSelect                     = false;
            listViewFiles.Name                            = "listViewFiles";
            listViewFiles.ShowItemToolTips                = true;
            listViewFiles.UseCompatibleStateImageBehavior = false;
            listViewFiles.View                            = System.Windows.Forms.View.Details;
            listViewFiles.BackColor                       = System.Drawing.Color.White;
            listViewFiles.Font                            = new System.Drawing.Font("Segoe UI", 9.5F);
            listViewFiles.HeaderStyle                     = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            listViewFiles.LabelEdit                       = true;
            listViewFiles.AllowDrop                       = true;
            listViewFiles.ItemMouseHover        += ListViewFiles_ItemMouseHover;
            listViewFiles.SelectedIndexChanged  += listViewFiles_SelectedIndexChanged;
            listViewFiles.DoubleClick           += ListViewFiles_DoubleClick;
            listViewFiles.MouseClick            += ListViewFiles_MouseClick;
            listViewFiles.AfterLabelEdit        += ListViewFiles_AfterLabelEdit;
            listViewFiles.MouseDown             += ListViewFiles_MouseDown;
            listViewFiles.MouseMove             += ListViewFiles_MouseMove;
            listViewFiles.MouseUp               += ListViewFiles_MouseUp;
            listViewFiles.DragEnter             += ListViewFiles_DragEnter;
            listViewFiles.DragOver              += ListViewFiles_DragOver;
            listViewFiles.DragDrop              += ListViewFiles_DragDrop;

            columnHeaderName.Text     = "Nombre";     columnHeaderName.Width     = 320;
            columnHeaderType.Text     = "Tipo";       columnHeaderType.Width     = 140;
            columnHeaderSize.Text     = "Tamaño";     columnHeaderSize.Width     = 110;
            columnHeaderModified.Text = "Modificado"; columnHeaderModified.Width = 160;

            // ── ContextMenu ───────────────────────────────────────────────────
            menuItemCortar.Text        = "Cortar";
            menuItemCortar.Font        = new System.Drawing.Font("Segoe UI", 9.5F);
            menuItemCortar.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X;
            menuItemCortar.Click      += MenuItemCortar_Click;

            menuItemCopiar.Text        = "Copiar";
            menuItemCopiar.Font        = new System.Drawing.Font("Segoe UI", 9.5F);
            menuItemCopiar.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            menuItemCopiar.Click      += MenuItemCopiar_Click;

            menuItemPegar.Text        = "Pegar";
            menuItemPegar.Font        = new System.Drawing.Font("Segoe UI", 9.5F);
            menuItemPegar.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            menuItemPegar.Click      += MenuItemPegar_Click;

            menuItemCrearAcceso.Text  = "Crear acceso directo";
            menuItemCrearAcceso.Font  = new System.Drawing.Font("Segoe UI", 9.5F);
            menuItemCrearAcceso.Click += MenuItemCrearAcceso_Click;

            menuItemEliminar.Text        = "Eliminar";
            menuItemEliminar.Font        = new System.Drawing.Font("Segoe UI", 9.5F);
            menuItemEliminar.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            menuItemEliminar.Click      += MenuItemEliminar_Click;

            menuItemCambiarNombre.Text        = "Cambiar nombre";
            menuItemCambiarNombre.Font        = new System.Drawing.Font("Segoe UI", 9.5F);
            menuItemCambiarNombre.ShortcutKeys = System.Windows.Forms.Keys.F2;
            menuItemCambiarNombre.Click      += MenuItemCambiarNombre_Click;

            menuItemPropiedades.Text  = "Propiedades";
            menuItemPropiedades.Font  = new System.Drawing.Font("Segoe UI", 9.5F);
            menuItemPropiedades.Click += MenuItemPropiedades_Click;

            menuItemEnviarCorreo.Text  = "📧  Enviar por correo electrónico...";
            menuItemEnviarCorreo.Font  = new System.Drawing.Font("Segoe UI", 9.5F);
            menuItemEnviarCorreo.Click += MenuItemEnviarCorreo_Click;

            contextMenuArchivo.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                menuItemCortar,
                menuItemCopiar,
                menuItemPegar,
                new System.Windows.Forms.ToolStripSeparator(),
                menuItemEnviarCorreo,
                new System.Windows.Forms.ToolStripSeparator(),
                menuItemCrearAcceso,
                menuItemEliminar,
                menuItemCambiarNombre,
                new System.Windows.Forms.ToolStripSeparator(),
                menuItemPropiedades
            });
            contextMenuArchivo.Font     = new System.Drawing.Font("Segoe UI", 9.5F);
            contextMenuArchivo.Opening += ContextMenuArchivo_Opening;
            listViewFiles.ContextMenuStrip = contextMenuArchivo;

            // ── StatusStrip ───────────────────────────────────────────────────
            statusStrip.BackColor        = AppColors.StatusBg;
            statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new System.Drawing.Point(0, 711);
            statusStrip.Name     = "statusStrip";
            statusStrip.Padding  = new System.Windows.Forms.Padding(1, 0, 15, 0);
            statusStrip.Size     = new System.Drawing.Size(1125, 26);
            statusStrip.TabIndex = 2;

            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new System.Drawing.Size(40, 20);
            toolStripStatusLabel.Text = "Listo";

            // ── Form ──────────────────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize          = new System.Drawing.Size(1125, 737);
            Controls.Add(panelContent);
            Controls.Add(panelSidebar);
            Controls.Add(toolStrip);
            Controls.Add(statusStrip);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name   = "Form1";
            Text   = "Explorador de Archivos";
            Font   = new System.Drawing.Font("Segoe UI", 9.5F);
            Load  += Form1_Load;

            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
