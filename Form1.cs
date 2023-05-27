using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;


namespace Project
{
    public partial class Form1 : Form
    {
        IntPtr Handle3D;
        IntPtr HDC3D;
        IntPtr HRC3D;
        float psi = 30;
        float phi = 30;
        float r = 1.8f;
        uint Texture;
        double b = 1.0;
        int Font3D = 0;
        List<Well> Wells;
        List<StreamLines> Sts;
        double x_max = 89609.86719;
        double x_min = 84628.07031;
        double y_max = 13980.17676;
        double y_min = 10680.55176;
        //double z_max = 1623;
        //double z_min = -230;
        double y_top = -0.1754;
        double y_bot = -0.2054;
        double[] field;
        Mesh mesh;
        bool draw_mesh = true;
        bool draw_axis_ch = true;
        bool draw_st_ch = true;
        bool draw_well_ch = true;
        bool draw_tex_ch = true;
        double scale_r_wells = 100.0;

        string path_mesh = "default.net";
        string path_field = "field.fun";
        string path_sts = "SLsReg_auto.txt";
        string path_wells = "track.dat";

        double x_mouse;
        double y_mouse;

        double scale_y = 1.0;
        int n_face_wells = 3;
        Color color_wells = Color.Lime;
        Color color_mesh = Color.Black;


        public Form1()
        {
            InitializeComponent();
            Handle3D = Handle;
            HDC3D = User.GetDC(Handle3D);
            Gdi.PIXELFORMATDESCRIPTOR PFD = new Gdi.PIXELFORMATDESCRIPTOR();
            PFD.nVersion = 1;
            PFD.nSize = (short)Marshal.SizeOf(PFD);
            PFD.dwFlags = Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_SUPPORT_OPENGL | Gdi.PFD_DOUBLEBUFFER;
            PFD.iPixelType = Gdi.PFD_TYPE_RGBA;
            PFD.cColorBits = 24;
            PFD.cDepthBits = 32;
            PFD.iLayerType = Gdi.PFD_MAIN_PLANE;

            int nPixelFormat = Gdi.ChoosePixelFormat(HDC3D, ref PFD);

            Gdi.SetPixelFormat(HDC3D, nPixelFormat, ref PFD);

            HRC3D = Wgl.wglCreateContext(HDC3D);
            Wgl.wglMakeCurrent(HDC3D, HRC3D);

            Form1_Resize(null, null);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Texture = LoadTexture("map.bmp");
            CreateFont3D(Font);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            int w = ClientRectangle.Width - panel_control.Width;
            int h = ClientRectangle.Height;
            //Glu.gluLookAt(2.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0, 1, 0);
            //Glu.gluPerspective(30, (double)w / h, 2, 20000);
            Glu.gluPerspective(30, (double)w / h, 0.001, 20000);
            Gl.glViewport(0, 0, w, h);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Gl.glClearColor(1f, 1f, 1f, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glTranslatef(0, 0, -r);
            Gl.glRotatef(phi, 1f, 0, 0);
            Gl.glRotatef(psi, 0, 1f, 0);
            Gl.glScaled(1.0, scale_y, 1.0);

            LoadInfo();

            if (draw_axis_ch == true)
            {
                draw_axis();
            }

            if (draw_well_ch == true)
            {
                DrawWells();
            }

            if (draw_tex_ch == true)
            {
                DrawMapTex();
            }

            if (draw_st_ch == true)
            {
                DrawST();
            }

            DrawMesh();

            Gl.glFinish();
            Gdi.SwapBuffers(HDC3D);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteFont3D();
            Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            Wgl.wglDeleteContext(HRC3D);
            User.ReleaseDC(Handle3D, HDC3D);
        }

        static uint LoadTexture(string Filename)
        {
            uint texObject = 0;
            try
            {
                Bitmap bmp = new Bitmap(Filename);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                texObject = MakeGlTexture(bmpdata.Scan0, bmp.Width, bmp.Height);
                bmp.UnlockBits(bmpdata);
            }
            catch
            {
                MessageBox.Show("Текстура не загружена!");
            }
            return texObject;
        }

        static uint MakeGlTexture(IntPtr pixels, int w, int h)
        {
            uint texObject;
            Gl.glGenTextures(1, out texObject);
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, w, h, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, pixels);
            return texObject;
        }

        //custom methods:
        private void draw_point(int x, int y, int z)
        {
            Gl.glPointSize(10);
            Gl.glColor3f(0, 1, 0);
            Gl.glEnable(Gl.GL_POINT_SMOOTH);
            Gl.glBegin(Gl.GL_POINTS);
            Gl.glVertex3f(x, y, z);
            Gl.glEnd();
        }

        private void draw_axis()
        {
            draw_point(0, 0, 0);
            Gl.glPointSize(10);
            Gl.glLineWidth(1);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3f(1, 0, 0);
            Gl.glVertex3f(0, 0, 0);
            Gl.glVertex3f(0.5f, 0, 0);
            Gl.glColor3f(0, 1, 0);
            Gl.glVertex3f(0, 0, 0);
            Gl.glVertex3f(0, 0.2f, 0);
            Gl.glColor3f(0, 0, 1);
            Gl.glVertex3f(0, 0, 0);
            Gl.glVertex3f(0, 0, 0.5f);
            Gl.glEnd();
            Gl.glColor3f(0, 0, 0);
            OutText3D(0.5f, 0, 0, "X");
            OutText3D(0, 0.2f, 0, "Y");
            OutText3D(0, 0, 0.5f, "Z");
        }

        private void draw_axis_2()
        {
            Gl.glPointSize(10);
            //Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glLineWidth(1);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3f(1, 1, 0);
            Gl.glVertex3f(0, 0, 0);
            Gl.glVertex3f(3, 0, 0);
            Gl.glColor3f(0, 1, 1);
            Gl.glVertex3f(0, 0, 0);
            Gl.glVertex3f(0, 3, 0);
            Gl.glColor3f(1, 0, 1);
            Gl.glVertex3f(0, 0, 0);
            Gl.glVertex3f(0, 0, 3);
            Gl.glEnd();
        }

        private void CreateFont3D(Font font)
        {
            Gdi.SelectObject(HDC3D, font.ToHfont());
            Font3D = Gl.glGenLists(256);
            Wgl.wglUseFontBitmapsA(HDC3D, 0, 256, Font3D);
        }

        void OutText3D(float x, float y, float z, string Text)
        {
            Gl.glRasterPos3f(x, y, z);
            Gl.glPushAttrib(Gl.GL_LIST_BIT);
            Gl.glListBase(Font3D);
            byte[] bText = MyGl.RussianEncoding.GetBytes(Text);
            Gl.glCallLists(Text.Length, Gl.GL_UNSIGNED_BYTE, bText);
            Gl.glPopAttrib();
        }

        void DeleteFont3D()
        {
            if (Font3D != 0)
            {
                Gl.glDeleteLists(Font3D, 256);
                Font3D = 0;
            }
        }

        struct Well
        {
            public string name;
            public double[] x;
            public double[] y;
            public double[] z;
        }

        struct Mesh
        {
            public int n_nodes;
            public int n_elem;
            public int[] type;
            public double[] x;
            public double[] y;
            public int[][] conn;
        }

        struct StreamLines
        {
            public int n_nodes;
            public double[] x;
            public double[] y;
        }

        private void LoadInfo()
        {
            LoadSTInfo(path_sts);
            LoadMeshInfo(path_mesh);
            LoadFieldInfo(path_field);
            LoadWellInfo(path_wells);
        }

        private void LoadWellInfo(string path)
        {
            try
            {
                //Well[] wells = new Well[] {};
                var wells = new List<Well>();
                Well well;
                int n = 0;
                string line;

                //var x_max_arr = new List<double>();
                //var y_max_arr = new List<double>();
                //var z_max_arr = new List<double>();

                using (var reader = File.OpenText(path))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        //line = file.ReadLine();
                        well.name = line.Split('\t')[1];
                        line = reader.ReadLine();
                        n = int.Parse(line.Split('\t')[1]);
                        double[] x = new double[n];
                        double[] y = new double[n];
                        double[] z = new double[n];
                        for (int i = 0; i < n; i++)
                        {
                            line = reader.ReadLine();
                            x[i] = (double.Parse(line.Split('\t')[2]) - (x_max + x_min) / 2) / (x_max - x_min);
                            y[i] = (double.Parse(line.Split('\t')[3]) - (y_max + y_min) / 2) / (x_max - x_min);
                            z[i] = double.Parse(line.Split('\t')[4]) / (x_max - x_min);
                        }
                        well.x = x;
                        well.y = z;
                        well.z = y;
                        wells.Add(well);
                    }
                }
                Wells = wells;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ошибка в загрузке файла скважин!");
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadMeshInfo(string path)
        {
            try
            {
                //var wells = new List<Well>();
                Mesh meshh;
                //int n = 0;
                string line;
                string [] lines;

                using (var reader = File.OpenText(path))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        //line = reader.ReadLine();
                        lines = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        meshh.n_nodes = int.Parse(lines[0]);
                        meshh.n_elem = int.Parse(lines[1]);

                        double[] x = new double[meshh.n_nodes];
                        double[] y = new double[meshh.n_nodes];
                        int[] type = new int[meshh.n_nodes];
                        int[][] conn = new int[meshh.n_elem][];

                        for (int i = 0; i < meshh.n_nodes; i++)
                        {
                            line = reader.ReadLine();
                            lines = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            x[i] = (double.Parse(lines[0]) - (x_max + x_min) / 2) / (x_max - x_min); ;
                            y[i] = (double.Parse(lines[1]) - (y_max + y_min) / 2) / (x_max - x_min); ;
                            type[i] = int.Parse(lines[2]);
                        }

                        for (int i = 0; i < meshh.n_elem; i++)
                        {
                            line = reader.ReadLine();
                            lines = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            conn[i] = new int[3] { int.Parse(lines[0]) - 1, int.Parse(lines[1]) - 1, int.Parse(lines[2]) - 1 };
                        }
                        meshh.x = x;
                        meshh.y = y;
                        meshh.type = type;
                        meshh.conn = conn;
                        mesh = meshh;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ошибка в загрузке файла сетки!");
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadFieldInfo(string path)
        {
            try
            {
                string line;
                string [] lines;
                double[] val = new double[mesh.n_nodes];

                using (var reader = File.OpenText(path))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        //line = reader.ReadLine();
                        lines = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        lines = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        val[0] = double.Parse(lines[0]);
                        for (int i = 1; i < val.Length; i++)
                        {
                            line = reader.ReadLine();
                            lines = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            val[i] = double.Parse(lines[0]);
                        }
                    }
                }
                double val_max = val.Max();
                double val_min = val.Min();
                for (int i = 0; i < val.Length; i++)
                {
                    val[i] = val[i] / val_max;
                }
                field = val;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ошибка в загрузке файла поля!");
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadSTInfo(string path)
        {
            try
            {
                string line;
                string[] lines;
                StreamLines st;
                var sts = new List<StreamLines>();
                int n_sts;

                using (var reader = File.OpenText(path))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        //line = reader.ReadLine();
                        lines = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        n_sts = int.Parse(lines[0]);
                        for (int i = 0; i < n_sts; i++)
                        {
                            line = reader.ReadLine();
                            lines = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                            st.n_nodes = int.Parse(lines[3]);
                            double[] x = new double[st.n_nodes];
                            double[] y = new double[st.n_nodes];
                            for (int j = 0; j < st.n_nodes; j++)
                            {
                                line = reader.ReadLine();
                                lines = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                x[j] = (double.Parse(lines[0]) - (x_max + x_min) / 2.0) / (x_max - x_min);
                                y[j] = (double.Parse(lines[1]) - (y_max + y_min) / 2.0) / (x_max - x_min);
                            }
                            st.x = x;
                            st.y = y;
                            sts.Add(st);
                        }
                    }
                }
                Sts = sts;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ошибка в загрузке файла линий тока!");
                MessageBox.Show(ex.ToString());
            }
        }

        private void DrawMapTex()
        {
            Gl.glLoadIdentity();
            Gl.glTranslatef(0, 0, -r);
            Gl.glRotatef(phi, 1f, 0, 0);
            Gl.glRotatef(psi, 0, 1f, 0);
            Gl.glScaled(1.0, scale_y, 1.0);
            //draw_axis_2();

            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, Texture);

            //Top fill
            //Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            //Gl.glColor3f(0, 0, 0);
            double xx_max = mesh.x.Max();
            double yy_max = mesh.y.Max();
            double xx_min = mesh.x.Min();
            double yy_min = mesh.y.Min();
            double dx = xx_max - xx_min;
            double dy = yy_max - yy_min;

            for (int i = 0; i < mesh.n_elem; i++)
            {
                Gl.glBegin(Gl.GL_TRIANGLES);
                for (int j = 0; j < mesh.conn[i].Length; j++)
                {
                    Gl.glTexCoord2d((mesh.x[mesh.conn[i][j]] - Math.Abs(xx_max)) * dy, -(mesh.y[mesh.conn[i][j]] - Math.Abs(yy_max)) * dx );
                    Gl.glVertex3d(mesh.x[mesh.conn[i][j]], 0, mesh.y[mesh.conn[i][j]]);
                }
                Gl.glEnd();
            }
            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonOffset(1f, 1f);

            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glDisable(Gl.GL_POLYGON_OFFSET_FILL);
        }

        private void DrawST()
        {
            //LoadSTInfo(path_sts);

            Gl.glLoadIdentity();
            Gl.glTranslatef(0, 0, -r);
            Gl.glRotatef(phi, 1f, 0, 0);
            Gl.glRotatef(psi, 0, 1f, 0);
            Gl.glScaled(1.0, scale_y, 1.0);

            int n_step = 4;
            double[] yz = new double[n_step];
            for (int i = 0; i < n_step; i++)
            {
                yz[i] = y_top + (i * (y_bot - y_top)) / (n_step - 1.0);
            }

            Gl.glColor3d(0.5, 0.5, 0.5);
            for (int i = 0; i < Sts.Count; i++)
            {
                for (int k = 0; k < Sts[i].n_nodes - 1; k++)
                {
                    for (int j = 0; j < n_step; j++)
                    {
                        Gl.glBegin(Gl.GL_LINES);
                        Gl.glVertex3d(Sts[i].x[k], yz[j], Sts[i].y[k]);
                        Gl.glVertex3d(Sts[i].x[k + 1], yz[j], Sts[i].y[k + 1]);
                        Gl.glEnd();
                    }
                }
            }

            Gl.glColor3d(1, 0, 0);
            for (int i = 0; i < Sts.Count; i++)
            {
                for (int k = 0; k < Sts[i].n_nodes - 1; k++)
                {
                    for (int j = 0; j < n_step - 1; j++)
                    {
                        Gl.glBegin(Gl.GL_LINES);
                        Gl.glVertex3d(Sts[i].x[k], yz[j], Sts[i].y[k]);
                        Gl.glVertex3d(Sts[i].x[k + 1], yz[j], Sts[i].y[k + 1]);
                        Gl.glEnd();

                        Gl.glBegin(Gl.GL_QUADS);
                        Gl.glVertex3d(Sts[i].x[k], yz[j], Sts[i].y[k]);
                        Gl.glVertex3d(Sts[i].x[k], yz[j+1], Sts[i].y[k]);
                        Gl.glVertex3d(Sts[i].x[k + 1], yz[j+1], Sts[i].y[k + 1]);
                        Gl.glVertex3d(Sts[i].x[k + 1], yz[j], Sts[i].y[k + 1]);
                        Gl.glEnd();
                    }
                }
            }
        }

        private void DrawMesh()
        {
            //LoadMeshInfo(path_mesh);
            //LoadFieldInfo(path_field);

            Gl.glLoadIdentity();
            Gl.glTranslatef(0, 0, -r);
            Gl.glRotatef(phi, 1f, 0, 0);
            Gl.glRotatef(psi, 0, 1f, 0);
            Gl.glScaled(1.0, scale_y, 1.0);

            double color_1R = 49.0 / 255.0;
            double color_1G = 108.0 / 255.0;
            double color_1B = 181.0 / 255.0;

            double color_2R = 147.0 / 255.0;
            double color_2G = 82.0 / 255.0;
            double color_2B = 53.0 / 255.0;

            //double color_1R = 0;
            //double color_1G = 0;
            //double color_1B = 0;

            //double color_2R = 0;
            //double color_2G = 0;
            //double color_2B = 0;

            double R;
            double G;
            double B;

            if (draw_mesh == true)
            {
                //Internal line
                Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
                //Gl.glColor3f(0, 0, 0);
                Gl.glColor4d((double)color_mesh.R / 255.0, (double)color_mesh.G / 255.0, (double)color_mesh.B / 255.0, b);
                for (int i = 0; i < mesh.n_elem; i++)
                {
                    Gl.glBegin(Gl.GL_QUADS);
                    for (int j = 0; j < mesh.conn[i].Length; j++)
                    {
                        if (j < mesh.conn[i].Length - 1)
                        {
                            Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_top, mesh.y[mesh.conn[i][j]]);
                            Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_bot, mesh.y[mesh.conn[i][j]]);

                            Gl.glVertex3d(mesh.x[mesh.conn[i][j + 1]], y_bot, mesh.y[mesh.conn[i][j + 1]]);
                            Gl.glVertex3d(mesh.x[mesh.conn[i][j + 1]], y_top, mesh.y[mesh.conn[i][j + 1]]);
                        }
                        else
                        {
                            Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_top, mesh.y[mesh.conn[i][j]]);
                            Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_bot, mesh.y[mesh.conn[i][j]]);

                            Gl.glVertex3d(mesh.x[mesh.conn[i][0]], y_bot, mesh.y[mesh.conn[i][0]]);
                            Gl.glVertex3d(mesh.x[mesh.conn[i][0]], y_top, mesh.y[mesh.conn[i][0]]);
                        }
                    }
                    Gl.glEnd();
                }
                Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
                Gl.glPolygonOffset(1f, 1f);
            }

            Gl.glEnable(Gl.GL_BLEND);
            //Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glLightModeli(Gl.GL_LIGHT_MODEL_TWO_SIDE, 1);


            //Internal Fill
            double x1, y1, z1, x2, y2, z2, vecx, vecy, vecz;
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            //Gl.glColor3f(0, 0, 0);
            Gl.glColor4d(0, 0, 0, b);
            for (int i = 0; i < mesh.n_elem; i++)
            {
                Gl.glBegin(Gl.GL_QUADS);
                for (int j = 0; j < mesh.conn[i].Length; j++)
                {
                    if (j < mesh.conn[i].Length - 1)
                    {
                        R = (color_1R - color_2R) * field[mesh.conn[i][j]] + color_2R;
                        G = (color_1G - color_2G) * field[mesh.conn[i][j]] + color_2G;
                        B = (color_1B - color_2B) * field[mesh.conn[i][j]] + color_2B;
                        //Gl.glColor3d(R, G, B);
                        Gl.glColor4d(R, G, B, b);

                        x1 = mesh.x[mesh.conn[i][j + 1]] - mesh.x[mesh.conn[i][j]];
                        y1 = y_top;
                        z1 = mesh.y[mesh.conn[i][j + 1]] - mesh.y[mesh.conn[i][j]];

                        x2 = 0;
                        y2 = y_bot;
                        z2 = 0;

                        vecx = -y2 * z1 + y1 * z2;
                        vecy = x2 * z1 - x1 * z2;
                        vecz = -x2 * y1 + x1 * y2;

                        Gl.glNormal3d(-vecx, -vecy, -vecz);

                        Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_top, mesh.y[mesh.conn[i][j]]);
                        Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_bot, mesh.y[mesh.conn[i][j]]);

                        R = (color_1R - color_2R) * field[mesh.conn[i][j + 1]] + color_2R;
                        G = (color_1G - color_2G) * field[mesh.conn[i][j + 1]] + color_2G;
                        B = (color_1B - color_2B) * field[mesh.conn[i][j + 1]] + color_2B;
                        //Gl.glColor3d(R, G, B);
                        Gl.glColor4d(R, G, B, b);

                        Gl.glVertex3d(mesh.x[mesh.conn[i][j + 1]], y_bot, mesh.y[mesh.conn[i][j + 1]]);
                        Gl.glVertex3d(mesh.x[mesh.conn[i][j + 1]], y_top, mesh.y[mesh.conn[i][j + 1]]);
                    }
                    else
                    {
                        R = (color_1R - color_2R) * field[mesh.conn[i][j]] + color_2R;
                        G = (color_1G - color_2G) * field[mesh.conn[i][j]] + color_2G;
                        B = (color_1B - color_2B) * field[mesh.conn[i][j]] + color_2B;
                        //Gl.glColor3d(R, G, B);
                        Gl.glColor4d(R, G, B, b);

                        x1 = mesh.x[mesh.conn[i][0]] - mesh.x[mesh.conn[i][j]];
                        y1 = y_top;
                        z1 = mesh.y[mesh.conn[i][0]] - mesh.y[mesh.conn[i][j]];

                        x2 = 0;
                        y2 = y_bot;
                        z2 = 0;

                        vecx = -y2 * z1 + y1 * z2;
                        vecy = x2 * z1 - x1 * z2;
                        vecz = -x2 * y1 + x1 * y2;

                        Gl.glNormal3d(-vecx, -vecy, -vecz);

                        Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_top, mesh.y[mesh.conn[i][j]]);
                        Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_bot, mesh.y[mesh.conn[i][j]]);

                        R = (color_1R - color_2R) * field[mesh.conn[i][0]] + color_2R;
                        G = (color_1G - color_2G) * field[mesh.conn[i][0]] + color_2G;
                        B = (color_1B - color_2B) * field[mesh.conn[i][0]] + color_2B;
                        //Gl.glColor3d(R, G, B);
                        Gl.glColor4d(R, G, B, b);

                        Gl.glVertex3d(mesh.x[mesh.conn[i][0]], y_bot, mesh.y[mesh.conn[i][0]]);
                        Gl.glVertex3d(mesh.x[mesh.conn[i][0]], y_top, mesh.y[mesh.conn[i][0]]);
                    }
                }
                Gl.glEnd();
            }
            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonOffset(1f, 1f);


            //Bot Fill
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            //Gl.glColor3f(0, 0, 0);
            for (int i = 0; i < mesh.n_elem; i++)
            {
                Gl.glBegin(Gl.GL_TRIANGLES);
                for (int j = 0; j < mesh.conn[i].Length; j++)
                {
                    R = (color_1R - color_2R) * field[mesh.conn[i][j]] + color_2R;
                    G = (color_1G - color_2G) * field[mesh.conn[i][j]] + color_2G;
                    B = (color_1B - color_2B) * field[mesh.conn[i][j]] + color_2B;
                    //Gl.glColor3d(R, G, B);
                    Gl.glColor4d(R, G, B, b);
                    Gl.glNormal3f(0, -1f, 0);
                    Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_bot, mesh.y[mesh.conn[i][j]]);
                }
                Gl.glEnd();
            }
            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonOffset(1f, 1f);


            //Top fill
            //Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            //Gl.glColor3f(0, 0, 0);
            for (int i = 0; i < mesh.n_elem; i++)
            {
                Gl.glBegin(Gl.GL_TRIANGLES);
                for (int j = mesh.conn[i].Length - 1; j >= 0; j--)
                {
                    R = (color_1R - color_2R) * field[mesh.conn[i][j]] + color_2R;
                    G = (color_1G - color_2G) * field[mesh.conn[i][j]] + color_2G;
                    B = (color_1B - color_2B) * field[mesh.conn[i][j]] + color_2B;
                    //Gl.glColor3d(R, G, B);
                    Gl.glColor4d(R, G, B, b);
                    Gl.glNormal3f(0, 1f, 0);
                    Gl.glVertex3d(mesh.x[mesh.conn[i][j]], y_top, mesh.y[mesh.conn[i][j]]);
                }
                Gl.glEnd();
            }
            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonOffset(1f, 1f);




            Gl.glDisable(Gl.GL_COLOR_MATERIAL);
            Gl.glDisable(Gl.GL_NORMALIZE);
            Gl.glDisable(Gl.GL_LIGHT0);
            Gl.glDisable(Gl.GL_LIGHTING);




            Gl.glDisable(Gl.GL_BLEND);

        }

        private void DrawWells()
        {
            //LoadWellInfo(path_wells);
            //Gl.glPushMatrix();


            //double[] x = { 1.0, 0.01, 0.05, 0.1, 0.4 };
            //double[] y = { 0.0, -0.25, -0.5, -0.75, -1.0 };
            //double[] z = { 0.1, 0.2, 0.3, 0.4, 0.5 };
            //DrawWell(x, y, z);
            ////Gl.glPopMatrix();

            //double[] x2 = { -1.0, 0.01, 0.05, 0.1, 0.4 };
            //double[] y2 = { 0.0, -0.25, -0.5, -0.75, -1.0 };
            //double[] z2 = { 0.1, 0.2, 0.3, 0.4, 0.5 };
            //DrawWell(x2, y2, z2);

            //Gl.glPopMatrix();
            //double[] x3 = { -1.0, -0.3456, -1.0, -0.2, 0.4 };
            //double[] y3 = { 1.0, -0.3345, 2.5457, -0.374, -1.0 };
            //double[] z3 = { 0.1, -0.3456, 0.7235, -0.2456, 0.3457 };
            //DrawWell(x3, y3, z3);

            for (int i = 0; i < Wells.Count; i++)
            {
                Gl.glPopMatrix();
                DrawWell(Wells[i].x, Wells[i].y, Wells[i].z);
            }
            //Gl.glPopMatrix();
        }

        private void DrawWell_old()
        {
            LoadWellInfo("C:\\Stud\\Mardanov\\track.dat");
            int n_face = 10;
            double r = 0.05;
            double step_phi = 2.0f * Math.PI / n_face;

            double[] x = { 0.0, 0.01, 0.05, 0.1, 0.4 };
            double[] y = { 0.0, -0.25, -0.5, -0.75, -1.0 };
            double[] z = { 0.1, 0.2, 0.3, 0.4, 0.5 };

            int n_point = x.Length;

            double x0, y0, z0, x1, y1, z1;
            double angle;
            double znam0;
            double znam1;
            double dist;

            Gl.glBegin(Gl.GL_POINTS);
            Gl.glColor3f(0, 0, 0);
            for (int i = 0; i < n_point; i++)
            {
                Gl.glVertex3d(x[i], y[i], z[i]);
            }
            Gl.glEnd();


            for (int i = 0; i < n_point; i++)
            {

                Gl.glPointSize(5);
                Gl.glColor3f(0, 0, 1f);
                Gl.glEnable(Gl.GL_POINT_SMOOTH);
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glVertex3d(x[i], y[i], z[i]);
                Gl.glEnd();
                x0 = 0;
                y0 = 1;
                z0 = 0;
                x1 = x[i];
                y1 = y[i];
                z1 = z[i];

                znam0 = Math.Sqrt(x0 * x0 + y0 * y0 + z0 * z0);
                znam1 = Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1);

                dist = Math.Sqrt((x1 - 0) * (x1 - 0) + (y1 - 0) * (y1 - 0) + (z1 - 0) * (z1 - 0));

                angle = (Math.Acos(Math.Abs(x1 * x0 + y1 * y0 + z1 * z0) / (znam0 * znam1))) * 180 / (Math.PI);

                Gl.glRotated(-angle, y0 * z1 - y1 * z0, x1 * z0 - x0 * z1, x0 * y1 - x1 * y0);
                Gl.glTranslated(0, -dist, 0);


                //Gl.glPointSize(10);
                //Gl.glColor3f(1, 0, 0);
                //Gl.glBegin(Gl.GL_POINTS);
                //for (int j = 0; j < n_face; j++)
                //{
                //    if (j == 0)
                //    {
                //        Gl.glColor3f(1, 0, 0);
                //    }
                //    else if (j == 1)
                //    {
                //        Gl.glColor3f(0, 1, 0);
                //    }
                //    else if (j == 2)
                //    {
                //        Gl.glColor3f(0, 0, 1);
                //    }
                //    Gl.glVertex3d(r * Math.Cos(j * step_phi), dist, r * Math.Sin(j * step_phi));
                //}

                //for (int j = 0; j < n_face; j++)
                //{
                //    if (j == 0)
                //    {
                //        Gl.glColor3f(1, 0, 0);
                //    }
                //    else if (j == 1)
                //    {
                //        Gl.glColor3f(0, 1, 0);
                //    }
                //    else if (j == 2)
                //    {
                //        Gl.glColor3f(0, 0, 1);
                //    }
                //    Gl.glVertex3d(r * Math.Cos(j * step_phi), 0, r * Math.Sin(j * step_phi));
                //}
                //Gl.glEnd();

                Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
                Gl.glColor3f(0, 0, 0);
                Gl.glBegin(Gl.GL_QUADS);
                for (int j = 0; j < n_face; j++)
                {
                    Gl.glVertex3d(r * Math.Cos(j * step_phi), 0, r * Math.Sin(j * step_phi));

                    Gl.glVertex3d(r * Math.Cos(j * step_phi), dist, r * Math.Sin(j * step_phi));

                    Gl.glVertex3d(r * Math.Cos((j + 1) * step_phi), dist, r * Math.Sin((j + 1) * step_phi));

                    Gl.glVertex3d(r * Math.Cos((j + 1) * step_phi), 0, r * Math.Sin((j + 1) * step_phi));

                }
                Gl.glEnd();
                Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
                Gl.glPolygonOffset(1f, 1f);

                Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
                Gl.glColor3f(0, 1, 0);
                Gl.glBegin(Gl.GL_QUADS);
                for (int j = 0; j < n_face; j++)
                {
                    Gl.glVertex3d(r * Math.Cos(j * step_phi), 0, r * Math.Sin(j * step_phi));

                    Gl.glVertex3d(r * Math.Cos(j * step_phi), dist, r * Math.Sin(j * step_phi));

                    Gl.glVertex3d(r * Math.Cos((j + 1) * step_phi), dist, r * Math.Sin((j + 1) * step_phi));

                    Gl.glVertex3d(r * Math.Cos((j + 1) * step_phi), 0, r * Math.Sin((j + 1) * step_phi));

                }
                Gl.glEnd();

                Gl.glRotated(angle, y0 * z1 - y1 * z0, x1 * z0 - x0 * z1, x0 * y1 - x1 * y0);
                for (int j = 0; j < n_point; j++)
                {
                    x[j] = x[j] - x1;
                    y[j] = y[j] - y1;
                    z[j] = z[j] - z1;
                }

            }
        }

        private void DrawWell(double[] x, double[] y, double[] z)
        {
            Gl.glLoadIdentity();
            Gl.glTranslatef(0, 0, -r);
            Gl.glRotatef(phi, 1f, 0, 0);
            Gl.glRotatef(psi, 0, 1f, 0);
            Gl.glScaled(1.0, scale_y, 1.0);

            int n_face = n_face_wells;
            double rr = 0.1 / (x_max - x_min) * scale_r_wells;
            double step_phi = 2.0f*Math.PI/n_face;


            int n_point = x.Length;

            double x0, y0, z0, x1, y1, z1;
            double angle;
            double znam0;
            double znam1;
            double dist;

            //Gl.glBegin(Gl.GL_POINTS);
            //Gl.glColor3f(0, 0, 0);
            //for (int i = 0; i < n_point; i++)
            //{
            //    Gl.glVertex3d(x[i], y[i], z[i]);
            //}
            //Gl.glEnd();

            Gl.glTranslated(x[0], y[0], z[0]);
            //draw_axis();

            double dx, dy, dz;
            dx = x[0];
            dy = y[0];
            dz = z[0];

            for (int i = 0; i < n_point; i++)
            {
                x[i] = x[i] - dx + 0.000001;
                y[i] = y[i] - dy + 0.000001;
                z[i] = z[i] - dz + 0.000001;
            }

            for (int i = 0; i < n_point; i++)
            {

                //Gl.glPointSize(5);
                //Gl.glColor3f(0, 0, 1f);
                //Gl.glEnable(Gl.GL_POINT_SMOOTH);
                //Gl.glBegin(Gl.GL_POINTS);
                //Gl.glVertex3d(x[i], y[i], z[i]);
                //Gl.glEnd();
                x0 = 0;
                y0 = 1;
                z0 = 0;
                x1 = x[i];
                y1 = y[i];
                z1 = z[i];

                znam0 = Math.Sqrt(x0 * x0 + y0 * y0 + z0 * z0);
                znam1 = Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1);

                dist = Math.Sqrt((x1 - 0)* (x1 - 0) + (y1 - 0) * (y1 - 0) + (z1 - 0) * (z1 - 0));

                angle = (Math.Acos(Math.Abs(x1 * x0 + y1 * y0 + z1 * z0) / (znam0 * znam1))) * 180 / (Math.PI);

                //Gl.glRotated(-angle, y0 * z1 - y1 * z0, x1 * z0 - x0 * z1, x0 * y1 - x1 * y0);

                if (y1 * y0 < 0)
                {
                    Gl.glRotated(-angle, y0 * z1 - y1 * z0, x1 * z0 - x0 * z1, x0 * y1 - x1 * y0);
                }
                else
                {
                    Gl.glRotated(-180 + angle, y0 * z1 - y1 * z0, x1 * z0 - x0 * z1, x0 * y1 - x1 * y0);
                }


                Gl.glTranslated(0, -dist, 0);
                //draw_axis();

                //draw_axis_2();


                //Gl.glPointSize(10);
                //Gl.glColor3f(1, 0, 0);
                //Gl.glBegin(Gl.GL_POINTS);
                //for (int j = 0; j < n_face; j++)
                //{
                //    if (j == 0)
                //    {
                //        Gl.glColor3f(1, 0, 0);
                //    }
                //    else if (j == 1)
                //    {
                //        Gl.glColor3f(0, 1, 0);
                //    }
                //    else if (j == 2)
                //    {
                //        Gl.glColor3f(0, 0, 1);
                //    }
                //    Gl.glVertex3d(rr * Math.Cos(j * step_phi), dist, rr * Math.Sin(j * step_phi));
                //}

                //for (int j = 0; j < n_face; j++)
                //{
                //    if (j == 0)
                //    {
                //        Gl.glColor3f(1, 0, 0);
                //    }
                //    else if (j == 1)
                //    {
                //        Gl.glColor3f(0, 1, 0);
                //    }
                //    else if (j == 2)
                //    {
                //        Gl.glColor3f(0, 0, 1);
                //    }
                //    Gl.glVertex3d(rr * Math.Cos(j * step_phi), 0, rr * Math.Sin(j * step_phi));
                //}
                //Gl.glEnd();
                if (draw_mesh == true)
                {
                    Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
                    Gl.glColor3d((double)color_mesh.R / 255.0, (double)color_mesh.G / 255.0, (double)color_mesh.B / 255.0);
                    Gl.glBegin(Gl.GL_QUADS);
                    for (int j = 0; j < n_face; j++)
                    {
                        Gl.glVertex3d(rr * Math.Cos(j * step_phi), 0, rr * Math.Sin(j * step_phi));

                        Gl.glVertex3d(rr * Math.Cos(j * step_phi), dist, rr * Math.Sin(j * step_phi));

                        Gl.glVertex3d(rr * Math.Cos((j + 1) * step_phi), dist, rr * Math.Sin((j + 1) * step_phi));

                        Gl.glVertex3d(rr * Math.Cos((j + 1) * step_phi), 0, rr * Math.Sin((j + 1) * step_phi));

                    }
                    Gl.glEnd();
                    Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
                    Gl.glPolygonOffset(1f, 1f);
                }

                Gl.glEnable(Gl.GL_LIGHTING);
                Gl.glEnable(Gl.GL_LIGHT0);
                Gl.glEnable(Gl.GL_NORMALIZE);
                Gl.glEnable(Gl.GL_COLOR_MATERIAL);
                Gl.glLightModeli(Gl.GL_LIGHT_MODEL_TWO_SIDE, 1);


                Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
                Gl.glColor3d((double)color_wells.R / 255.0, (double)color_wells.G / 255.0, (double)color_wells.B / 255.0);
                Gl.glBegin(Gl.GL_QUADS);
                for (int j = 0; j < n_face; j++)
                {


                    Gl.glNormal3d(rr * Math.Cos(j * step_phi), 0, rr * Math.Sin(j * step_phi));

                    Gl.glVertex3d(rr * Math.Cos(j * step_phi), 0, rr * Math.Sin(j * step_phi));

                    Gl.glNormal3d(rr * Math.Cos(j * step_phi), 0, rr * Math.Sin(j * step_phi));

                    Gl.glVertex3d(rr * Math.Cos(j * step_phi), dist, rr * Math.Sin(j * step_phi));

                    Gl.glNormal3d(rr * Math.Cos((j + 1) * step_phi), 0, rr * Math.Sin((j + 1) * step_phi));

                    Gl.glVertex3d(rr * Math.Cos((j + 1) * step_phi), dist, rr * Math.Sin((j + 1) * step_phi));

                    Gl.glNormal3d(rr * Math.Cos((j + 1) * step_phi), 0, rr * Math.Sin((j + 1) * step_phi));

                    Gl.glVertex3d(rr * Math.Cos((j + 1) * step_phi), 0, rr * Math.Sin((j + 1) * step_phi));

                }
                Gl.glEnd();

                Gl.glDisable(Gl.GL_COLOR_MATERIAL);
                Gl.glDisable(Gl.GL_NORMALIZE);
                Gl.glDisable(Gl.GL_LIGHT0);
                Gl.glDisable(Gl.GL_LIGHTING);


                //Gl.glRotated(angle, y0 * z1 - y1 * z0, x1 * z0 - x0 * z1, x0 * y1 - x1 * y0);
                if (y1 * y0 < 0)
                {
                    Gl.glRotated(angle, y0 * z1 - y1 * z0, x1 * z0 - x0 * z1, x0 * y1 - x1 * y0);
                }
                else
                {
                    Gl.glRotated(-(-180 + angle), y0 * z1 - y1 * z0, x1 * z0 - x0 * z1, x0 * y1 - x1 * y0);
                }
                for (int j = 0; j < n_point; j++)
                {
                    x[j] = x[j]-x1;
                    y[j] = y[j]-y1;
                    z[j] = z[j]-z1;
                }

            }
        }

        private void InvalidateRect()
        {
            MyGl.InvalidateRect(Handle, IntPtr.Zero, false);
        }

        protected override void WndProc(ref Message mes)
        {
            base.WndProc(ref mes);
            if (mes.Msg == MyGl.WM_ERASEBKGND)
            {
                mes.Result = IntPtr.Zero;
                InvalidateRect();
            }
        }

        private void trackBar_phi_Scroll(object sender, EventArgs e)
        {
            phi = trackBar_phi.Value;
            InvalidateRect();
        }

        private void trackBar_psi_Scroll(object sender, EventArgs e)
        {
            psi = trackBar_psi.Value;
            InvalidateRect();
        }

        private void trackBar_depth_Scroll(object sender, EventArgs e)
        {
            r = trackBar_depth.Value * 0.001f;
            InvalidateRect();
        }

        private void trackBar_blend_Scroll(object sender, EventArgs e)
        {
            b = trackBar_blend.Value * 0.01;
            InvalidateRect();
        }

        private void checkBox_mesh_CheckedChanged(object sender, EventArgs e)
        {
            draw_mesh = checkBox_mesh.Checked;
            InvalidateRect();
        }

        private void numericUpDown_scale_r_wells_ValueChanged(object sender, EventArgs e)
        {
            scale_r_wells = (double)numericUpDown_scale_r_wells.Value;
            InvalidateRect();
        }

        private void checkBox_axis_CheckedChanged(object sender, EventArgs e)
        {
            draw_axis_ch = checkBox_axis.Checked;
            InvalidateRect();
        }

        private void checkBox_st_CheckedChanged(object sender, EventArgs e)
        {
            draw_st_ch = checkBox_st.Checked;
            InvalidateRect();
        }

        private void checkBox_well_CheckedChanged(object sender, EventArgs e)
        {
            draw_well_ch = checkBox_well.Checked;
            InvalidateRect();
        }

        private void checkBox_tex_CheckedChanged(object sender, EventArgs e)
        {
            draw_tex_ch = checkBox_tex.Checked;
            InvalidateRect();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //x_mouse = e.X;
            //y_mouse = e.Y;
            if (e.Button == MouseButtons.Left)
            {
                phi = (float)(1 / Math.PI * (MousePosition.Y - y_mouse));
                psi = (float)(1 / Math.PI * (MousePosition.X - x_mouse));
                if (-90.0f < phi && phi < 90.0f && psi > 0.0f && psi < 360.0f)
                {
                    trackBar_phi.Value = (int)phi;
                    trackBar_psi.Value = (int)psi;
                    InvalidateRect();
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
           x_mouse = e.X;
           y_mouse = e.Y;
        }

        private void trackBar_scale_y_Scroll(object sender, EventArgs e)
        {
            scale_y = trackBar_scale_y.Value * 0.01;
            InvalidateRect();
        }

        private void numericUpDown_num_face_ValueChanged(object sender, EventArgs e)
        {
            n_face_wells = (int)numericUpDown_num_face.Value;
            InvalidateRect();
        }

        private void button_set_color_wells_Click(object sender, EventArgs e)
        {
            if (colorDialog_wells.ShowDialog() == DialogResult.OK)
            {
                color_wells = colorDialog_wells.Color;
                InvalidateRect();
            }
        }

        private void button_set_color_mesh_Click(object sender, EventArgs e)
        {
            if (colorDialog_mesh.ShowDialog() == DialogResult.OK)
            {
                color_mesh = colorDialog_mesh.Color;
                InvalidateRect();
            }
        }

        private void button_set_font_axis_Click(object sender, EventArgs e)
        {
            if (fontDialog_axis.ShowDialog() == DialogResult.OK)
            {
                DeleteFont3D();
                CreateFont3D(fontDialog_axis.Font);
                InvalidateRect();
            }
        }
    }

    class MyGl
    {
        internal const int WM_ERASEBKGND = 0x0014;
        [DllImport("user32.dll")]
        internal static extern bool InvalidateRect(IntPtr hWind, IntPtr lpRect, bool bErase);
        internal static Encoding RussianEncoding = Encoding.GetEncoding(1251);
    }

}
