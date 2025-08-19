
namespace Template_Matcher
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
            this.imbSearchImage = new Emgu.CV.UI.ImageBox();
            this.gbSearchImage = new System.Windows.Forms.GroupBox();
            this.btnViewPyramid = new System.Windows.Forms.Button();
            this.btnSelectSearchImage = new System.Windows.Forms.Button();
            this.gbTemplateImage = new System.Windows.Forms.GroupBox();
            this.btnSelectTemplateImage = new System.Windows.Forms.Button();
            this.imbTemplateImage = new Emgu.CV.UI.ImageBox();
            this.imbProcessedImage = new Emgu.CV.UI.ImageBox();
            this.gbResult = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnProcessImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imbSearchImage)).BeginInit();
            this.gbSearchImage.SuspendLayout();
            this.gbTemplateImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imbTemplateImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imbProcessedImage)).BeginInit();
            this.gbResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // imbSearchImage
            // 
            this.imbSearchImage.Location = new System.Drawing.Point(4, 16);
            this.imbSearchImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.imbSearchImage.Name = "imbSearchImage";
            this.imbSearchImage.Size = new System.Drawing.Size(427, 312);
            this.imbSearchImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imbSearchImage.TabIndex = 2;
            this.imbSearchImage.TabStop = false;
            // 
            // gbSearchImage
            // 
            this.gbSearchImage.Controls.Add(this.btnViewPyramid);
            this.gbSearchImage.Controls.Add(this.btnSelectSearchImage);
            this.gbSearchImage.Controls.Add(this.imbSearchImage);
            this.gbSearchImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSearchImage.Location = new System.Drawing.Point(8, 8);
            this.gbSearchImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbSearchImage.Name = "gbSearchImage";
            this.gbSearchImage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbSearchImage.Size = new System.Drawing.Size(539, 331);
            this.gbSearchImage.TabIndex = 4;
            this.gbSearchImage.TabStop = false;
            this.gbSearchImage.Text = "Search Image";
            // 
            // btnViewPyramid
            // 
            this.btnViewPyramid.Location = new System.Drawing.Point(433, 58);
            this.btnViewPyramid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnViewPyramid.Name = "btnViewPyramid";
            this.btnViewPyramid.Size = new System.Drawing.Size(102, 38);
            this.btnViewPyramid.TabIndex = 4;
            this.btnViewPyramid.Text = "View Pyramid";
            this.btnViewPyramid.UseVisualStyleBackColor = true;
            this.btnViewPyramid.Click += new System.EventHandler(this.btnViewPyramid_Click);
            // 
            // btnSelectSearchImage
            // 
            this.btnSelectSearchImage.Location = new System.Drawing.Point(435, 16);
            this.btnSelectSearchImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSelectSearchImage.Name = "btnSelectSearchImage";
            this.btnSelectSearchImage.Size = new System.Drawing.Size(102, 38);
            this.btnSelectSearchImage.TabIndex = 3;
            this.btnSelectSearchImage.Text = "Select Image";
            this.btnSelectSearchImage.UseVisualStyleBackColor = true;
            this.btnSelectSearchImage.Click += new System.EventHandler(this.btnSelectSearchImage_Click);
            // 
            // gbTemplateImage
            // 
            this.gbTemplateImage.Controls.Add(this.btnSelectTemplateImage);
            this.gbTemplateImage.Controls.Add(this.imbTemplateImage);
            this.gbTemplateImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTemplateImage.Location = new System.Drawing.Point(8, 343);
            this.gbTemplateImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbTemplateImage.Name = "gbTemplateImage";
            this.gbTemplateImage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbTemplateImage.Size = new System.Drawing.Size(539, 331);
            this.gbTemplateImage.TabIndex = 5;
            this.gbTemplateImage.TabStop = false;
            this.gbTemplateImage.Text = "Template Image";
            // 
            // btnSelectTemplateImage
            // 
            this.btnSelectTemplateImage.Location = new System.Drawing.Point(435, 16);
            this.btnSelectTemplateImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSelectTemplateImage.Name = "btnSelectTemplateImage";
            this.btnSelectTemplateImage.Size = new System.Drawing.Size(102, 38);
            this.btnSelectTemplateImage.TabIndex = 3;
            this.btnSelectTemplateImage.Text = "Select Image";
            this.btnSelectTemplateImage.UseVisualStyleBackColor = true;
            this.btnSelectTemplateImage.Click += new System.EventHandler(this.btnSelectTemplateImage_Click);
            // 
            // imbTemplateImage
            // 
            this.imbTemplateImage.Location = new System.Drawing.Point(4, 16);
            this.imbTemplateImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.imbTemplateImage.Name = "imbTemplateImage";
            this.imbTemplateImage.Size = new System.Drawing.Size(427, 312);
            this.imbTemplateImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imbTemplateImage.TabIndex = 2;
            this.imbTemplateImage.TabStop = false;
            // 
            // imbProcessedImage
            // 
            this.imbProcessedImage.Location = new System.Drawing.Point(4, 23);
            this.imbProcessedImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.imbProcessedImage.Name = "imbProcessedImage";
            this.imbProcessedImage.Size = new System.Drawing.Size(1067, 585);
            this.imbProcessedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imbProcessedImage.TabIndex = 2;
            this.imbProcessedImage.TabStop = false;
            // 
            // gbResult
            // 
            this.gbResult.Controls.Add(this.button3);
            this.gbResult.Controls.Add(this.button2);
            this.gbResult.Controls.Add(this.btnProcessImage);
            this.gbResult.Controls.Add(this.imbProcessedImage);
            this.gbResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbResult.Location = new System.Drawing.Point(551, 8);
            this.gbResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbResult.Name = "gbResult";
            this.gbResult.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbResult.Size = new System.Drawing.Size(1075, 666);
            this.gbResult.TabIndex = 6;
            this.gbResult.TabStop = false;
            this.gbResult.Text = "Result";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(959, 612);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 49);
            this.button3.TabIndex = 6;
            this.button3.Text = "Save Image";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(120, 612);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 49);
            this.button2.TabIndex = 5;
            this.button2.Text = "Settings";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnProcessImage
            // 
            this.btnProcessImage.Location = new System.Drawing.Point(4, 612);
            this.btnProcessImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnProcessImage.Name = "btnProcessImage";
            this.btnProcessImage.Size = new System.Drawing.Size(112, 49);
            this.btnProcessImage.TabIndex = 4;
            this.btnProcessImage.Text = "Process Image";
            this.btnProcessImage.UseVisualStyleBackColor = true;
            this.btnProcessImage.Click += new System.EventHandler(this.btnProcessImage_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 612);
            this.Controls.Add(this.gbResult);
            this.Controls.Add(this.gbTemplateImage);
            this.Controls.Add(this.gbSearchImage);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmMain";
            this.Text = "Template Matcher";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.imbSearchImage)).EndInit();
            this.gbSearchImage.ResumeLayout(false);
            this.gbTemplateImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imbTemplateImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imbProcessedImage)).EndInit();
            this.gbResult.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imbSearchImage;
        private System.Windows.Forms.GroupBox gbSearchImage;
        private System.Windows.Forms.Button btnSelectSearchImage;
        private System.Windows.Forms.GroupBox gbTemplateImage;
        private System.Windows.Forms.Button btnSelectTemplateImage;
        private Emgu.CV.UI.ImageBox imbTemplateImage;
        private Emgu.CV.UI.ImageBox imbProcessedImage;
        private System.Windows.Forms.GroupBox gbResult;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnProcessImage;
        private System.Windows.Forms.Button btnViewPyramid;
    }
}

