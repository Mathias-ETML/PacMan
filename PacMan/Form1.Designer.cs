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
            this.SuspendLayout();
            // 
            // pan_PanGame
            // 
            this.pan_PanGame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pan_PanGame.Location = new System.Drawing.Point(124, 241);
            this.pan_PanGame.Name = "pan_PanGame";
            this.pan_PanGame.Size = new System.Drawing.Size(334, 137);
            this.pan_PanGame.TabIndex = 0;
            this.pan_PanGame.Click += new System.EventHandler(this.pan_PanGame_Click);
            // 
            // frm_FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 454);
            this.Controls.Add(this.pan_PanGame);
            this.Name = "frm_FormPrincipal";
            this.Text = "PacMan";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan_PanGame;
    }
}

