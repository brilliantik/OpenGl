namespace Project
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel_control = new System.Windows.Forms.Panel();
            this.button_set_color_mesh = new System.Windows.Forms.Button();
            this.button_set_color_wells = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.trackBar_scale_y = new System.Windows.Forms.TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDown_num_face = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_tex = new System.Windows.Forms.CheckBox();
            this.checkBox_well = new System.Windows.Forms.CheckBox();
            this.checkBox_st = new System.Windows.Forms.CheckBox();
            this.numericUpDown_scale_r_wells = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox_mesh = new System.Windows.Forms.CheckBox();
            this.checkBox_axis = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar_blend = new System.Windows.Forms.TrackBar();
            this.trackBar_depth = new System.Windows.Forms.TrackBar();
            this.trackBar_psi = new System.Windows.Forms.TrackBar();
            this.trackBar_phi = new System.Windows.Forms.TrackBar();
            this.colorDialog_wells = new System.Windows.Forms.ColorDialog();
            this.colorDialog_mesh = new System.Windows.Forms.ColorDialog();
            this.fontDialog_axis = new System.Windows.Forms.FontDialog();
            this.button_set_font_axis = new System.Windows.Forms.Button();
            this.panel_control.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_scale_y)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_num_face)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scale_r_wells)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_blend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_depth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_psi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_phi)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_control
            // 
            this.panel_control.Controls.Add(this.label7);
            this.panel_control.Controls.Add(this.trackBar_scale_y);
            this.panel_control.Controls.Add(this.panel1);
            this.panel_control.Controls.Add(this.label4);
            this.panel_control.Controls.Add(this.label3);
            this.panel_control.Controls.Add(this.label2);
            this.panel_control.Controls.Add(this.label1);
            this.panel_control.Controls.Add(this.trackBar_blend);
            this.panel_control.Controls.Add(this.trackBar_depth);
            this.panel_control.Controls.Add(this.trackBar_psi);
            this.panel_control.Controls.Add(this.trackBar_phi);
            this.panel_control.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_control.Location = new System.Drawing.Point(1357, 0);
            this.panel_control.Name = "panel_control";
            this.panel_control.Size = new System.Drawing.Size(200, 963);
            this.panel_control.TabIndex = 0;
            // 
            // button_set_color_mesh
            // 
            this.button_set_color_mesh.Location = new System.Drawing.Point(5, 206);
            this.button_set_color_mesh.Name = "button_set_color_mesh";
            this.button_set_color_mesh.Size = new System.Drawing.Size(183, 23);
            this.button_set_color_mesh.TabIndex = 15;
            this.button_set_color_mesh.Text = "Цвет сеток";
            this.button_set_color_mesh.UseVisualStyleBackColor = true;
            this.button_set_color_mesh.Click += new System.EventHandler(this.button_set_color_mesh_Click);
            // 
            // button_set_color_wells
            // 
            this.button_set_color_wells.Location = new System.Drawing.Point(5, 177);
            this.button_set_color_wells.Name = "button_set_color_wells";
            this.button_set_color_wells.Size = new System.Drawing.Size(183, 23);
            this.button_set_color_wells.TabIndex = 1;
            this.button_set_color_wells.Text = "Цвет скважин";
            this.button_set_color_wells.UseVisualStyleBackColor = true;
            this.button_set_color_wells.Click += new System.EventHandler(this.button_set_color_wells_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 481);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Множитель вертикальной координаты";
            // 
            // trackBar_scale_y
            // 
            this.trackBar_scale_y.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_scale_y.Location = new System.Drawing.Point(0, 446);
            this.trackBar_scale_y.Maximum = 1000;
            this.trackBar_scale_y.Minimum = 10;
            this.trackBar_scale_y.Name = "trackBar_scale_y";
            this.trackBar_scale_y.Size = new System.Drawing.Size(200, 45);
            this.trackBar_scale_y.TabIndex = 13;
            this.trackBar_scale_y.Value = 100;
            this.trackBar_scale_y.Scroll += new System.EventHandler(this.trackBar_scale_y_Scroll);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_set_font_axis);
            this.panel1.Controls.Add(this.button_set_color_mesh);
            this.panel1.Controls.Add(this.numericUpDown_num_face);
            this.panel1.Controls.Add(this.button_set_color_wells);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.checkBox_tex);
            this.panel1.Controls.Add(this.checkBox_well);
            this.panel1.Controls.Add(this.checkBox_st);
            this.panel1.Controls.Add(this.numericUpDown_scale_r_wells);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.checkBox_mesh);
            this.panel1.Controls.Add(this.checkBox_axis);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 180);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 266);
            this.panel1.TabIndex = 12;
            // 
            // numericUpDown_num_face
            // 
            this.numericUpDown_num_face.Location = new System.Drawing.Point(155, 32);
            this.numericUpDown_num_face.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_num_face.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_num_face.Name = "numericUpDown_num_face";
            this.numericUpDown_num_face.Size = new System.Drawing.Size(41, 20);
            this.numericUpDown_num_face.TabIndex = 15;
            this.numericUpDown_num_face.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_num_face.ValueChanged += new System.EventHandler(this.numericUpDown_num_face_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Кол-во граней на скважине";
            // 
            // checkBox_tex
            // 
            this.checkBox_tex.AutoSize = true;
            this.checkBox_tex.Checked = true;
            this.checkBox_tex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_tex.Location = new System.Drawing.Point(3, 154);
            this.checkBox_tex.Name = "checkBox_tex";
            this.checkBox_tex.Size = new System.Drawing.Size(86, 17);
            this.checkBox_tex.TabIndex = 14;
            this.checkBox_tex.Text = "Draw texture";
            this.checkBox_tex.UseVisualStyleBackColor = true;
            this.checkBox_tex.CheckedChanged += new System.EventHandler(this.checkBox_tex_CheckedChanged);
            // 
            // checkBox_well
            // 
            this.checkBox_well.AutoSize = true;
            this.checkBox_well.Checked = true;
            this.checkBox_well.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_well.Location = new System.Drawing.Point(3, 131);
            this.checkBox_well.Name = "checkBox_well";
            this.checkBox_well.Size = new System.Drawing.Size(77, 17);
            this.checkBox_well.TabIndex = 13;
            this.checkBox_well.Text = "Draw wells";
            this.checkBox_well.UseVisualStyleBackColor = true;
            this.checkBox_well.CheckedChanged += new System.EventHandler(this.checkBox_well_CheckedChanged);
            // 
            // checkBox_st
            // 
            this.checkBox_st.AutoSize = true;
            this.checkBox_st.Checked = true;
            this.checkBox_st.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_st.Location = new System.Drawing.Point(3, 108);
            this.checkBox_st.Name = "checkBox_st";
            this.checkBox_st.Size = new System.Drawing.Size(109, 17);
            this.checkBox_st.TabIndex = 12;
            this.checkBox_st.Text = "Draw stream lines";
            this.checkBox_st.UseVisualStyleBackColor = true;
            this.checkBox_st.CheckedChanged += new System.EventHandler(this.checkBox_st_CheckedChanged);
            // 
            // numericUpDown_scale_r_wells
            // 
            this.numericUpDown_scale_r_wells.Location = new System.Drawing.Point(155, 9);
            this.numericUpDown_scale_r_wells.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown_scale_r_wells.Name = "numericUpDown_scale_r_wells";
            this.numericUpDown_scale_r_wells.Size = new System.Drawing.Size(41, 20);
            this.numericUpDown_scale_r_wells.TabIndex = 5;
            this.numericUpDown_scale_r_wells.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown_scale_r_wells.ValueChanged += new System.EventHandler(this.numericUpDown_scale_r_wells_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Множитель радиуса скважин";
            // 
            // checkBox_mesh
            // 
            this.checkBox_mesh.AutoSize = true;
            this.checkBox_mesh.Checked = true;
            this.checkBox_mesh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_mesh.Location = new System.Drawing.Point(3, 62);
            this.checkBox_mesh.Name = "checkBox_mesh";
            this.checkBox_mesh.Size = new System.Drawing.Size(79, 17);
            this.checkBox_mesh.TabIndex = 4;
            this.checkBox_mesh.Text = "Draw mesh";
            this.checkBox_mesh.UseVisualStyleBackColor = true;
            this.checkBox_mesh.CheckedChanged += new System.EventHandler(this.checkBox_mesh_CheckedChanged);
            // 
            // checkBox_axis
            // 
            this.checkBox_axis.AutoSize = true;
            this.checkBox_axis.Checked = true;
            this.checkBox_axis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_axis.Location = new System.Drawing.Point(3, 85);
            this.checkBox_axis.Name = "checkBox_axis";
            this.checkBox_axis.Size = new System.Drawing.Size(72, 17);
            this.checkBox_axis.TabIndex = 6;
            this.checkBox_axis.Text = "Draw axis";
            this.checkBox_axis.UseVisualStyleBackColor = true;
            this.checkBox_axis.CheckedChanged += new System.EventHandler(this.checkBox_axis_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Прозрачность";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Приближение";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Вращение от. оси y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Вращение от. оси x";
            // 
            // trackBar_blend
            // 
            this.trackBar_blend.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_blend.Location = new System.Drawing.Point(0, 135);
            this.trackBar_blend.Maximum = 100;
            this.trackBar_blend.Name = "trackBar_blend";
            this.trackBar_blend.Size = new System.Drawing.Size(200, 45);
            this.trackBar_blend.TabIndex = 3;
            this.trackBar_blend.Value = 100;
            this.trackBar_blend.Scroll += new System.EventHandler(this.trackBar_blend_Scroll);
            // 
            // trackBar_depth
            // 
            this.trackBar_depth.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_depth.Location = new System.Drawing.Point(0, 90);
            this.trackBar_depth.Maximum = 1500;
            this.trackBar_depth.Minimum = 100;
            this.trackBar_depth.Name = "trackBar_depth";
            this.trackBar_depth.Size = new System.Drawing.Size(200, 45);
            this.trackBar_depth.TabIndex = 2;
            this.trackBar_depth.Value = 1500;
            this.trackBar_depth.Scroll += new System.EventHandler(this.trackBar_depth_Scroll);
            // 
            // trackBar_psi
            // 
            this.trackBar_psi.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_psi.Location = new System.Drawing.Point(0, 45);
            this.trackBar_psi.Maximum = 360;
            this.trackBar_psi.Name = "trackBar_psi";
            this.trackBar_psi.Size = new System.Drawing.Size(200, 45);
            this.trackBar_psi.TabIndex = 1;
            this.trackBar_psi.Scroll += new System.EventHandler(this.trackBar_psi_Scroll);
            // 
            // trackBar_phi
            // 
            this.trackBar_phi.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_phi.Location = new System.Drawing.Point(0, 0);
            this.trackBar_phi.Maximum = 90;
            this.trackBar_phi.Minimum = -90;
            this.trackBar_phi.Name = "trackBar_phi";
            this.trackBar_phi.Size = new System.Drawing.Size(200, 45);
            this.trackBar_phi.TabIndex = 0;
            this.trackBar_phi.Scroll += new System.EventHandler(this.trackBar_phi_Scroll);
            // 
            // button_set_font_axis
            // 
            this.button_set_font_axis.Location = new System.Drawing.Point(5, 235);
            this.button_set_font_axis.Name = "button_set_font_axis";
            this.button_set_font_axis.Size = new System.Drawing.Size(183, 23);
            this.button_set_font_axis.TabIndex = 16;
            this.button_set_font_axis.Text = "Шрифт осей";
            this.button_set_font_axis.UseVisualStyleBackColor = true;
            this.button_set_font_axis.Click += new System.EventHandler(this.button_set_font_axis_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1557, 963);
            this.Controls.Add(this.panel_control);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel_control.ResumeLayout(false);
            this.panel_control.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_scale_y)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_num_face)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scale_r_wells)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_blend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_depth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_psi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_phi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_control;
        private System.Windows.Forms.TrackBar trackBar_depth;
        private System.Windows.Forms.TrackBar trackBar_psi;
        private System.Windows.Forms.TrackBar trackBar_phi;
        private System.Windows.Forms.TrackBar trackBar_blend;
        private System.Windows.Forms.CheckBox checkBox_mesh;
        private System.Windows.Forms.NumericUpDown numericUpDown_scale_r_wells;
        private System.Windows.Forms.CheckBox checkBox_axis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox_st;
        private System.Windows.Forms.CheckBox checkBox_well;
        private System.Windows.Forms.CheckBox checkBox_tex;
        private System.Windows.Forms.TrackBar trackBar_scale_y;
        private System.Windows.Forms.NumericUpDown numericUpDown_num_face;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_set_color_wells;
        private System.Windows.Forms.ColorDialog colorDialog_wells;
        private System.Windows.Forms.Button button_set_color_mesh;
        private System.Windows.Forms.ColorDialog colorDialog_mesh;
        private System.Windows.Forms.Button button_set_font_axis;
        private System.Windows.Forms.FontDialog fontDialog_axis;
    }
}

