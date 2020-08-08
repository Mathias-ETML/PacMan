namespace PacMan
{
    partial class frm_FormPrincipal
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.pan_PanGame = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pan_PanGame
            // 
            this.pan_PanGame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pan_PanGame.Location = new System.Drawing.Point(100, 12);
            this.pan_PanGame.Name = "pan_PanGame";
            this.pan_PanGame.Size = new System.Drawing.Size(760, 760);
            this.pan_PanGame.TabIndex = 0;
            this.pan_PanGame.Paint += new System.Windows.Forms.PaintEventHandler(this.pan_PanGame_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(919, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // frm_FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 786);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pan_PanGame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frm_FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacMan";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyPressed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel pan_PanGame;
        public System.Windows.Forms.Label label1;
    }
}

