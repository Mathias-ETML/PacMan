namespace PacManGame.GameView
{
    partial class GameForm
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
            this.panPanGame = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panPanGame
            // 
            this.panPanGame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panPanGame.Location = new System.Drawing.Point(100, 10);
            this.panPanGame.Name = "panPanGame";
            this.panPanGame.Size = new System.Drawing.Size(760, 760);
            this.panPanGame.TabIndex = 0;
            this.panPanGame.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawMapAndFood);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 786);
            this.Controls.Add(this.panPanGame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacMan";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyPressed);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panPanGame;
    }
}

