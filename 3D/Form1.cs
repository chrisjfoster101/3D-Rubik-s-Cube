using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3D
{
    public partial class Form1 : Form
    {
        Camera c;
        List<Vec3> points;
        List<Color> pointColors;
        List<int[]> lines;
        List<Color> lineColors;
        List<int[]> polygons;
        List<Color> polygonColors;
        Bitmap image;
        bool mouseDown = false;
        int[] prevMousePos = { 0, 0 };

        Cube cube = new Cube();
        Dictionary<char, Color> charToColour = new Dictionary<char, Color>()
        {
            { 'w', Color.White },
            { 'y', Color.Yellow },
            { 'g', Color.Green },
            { 'b', Color.Blue },
            { 'r', Color.Red },
            { 'o', Color.Orange }
        };
        Dictionary<Color, char> colourToChar = new Dictionary<Color, char>()
        {
            { Color.White , 'w' },
            { Color.Yellow , 'y' },
            { Color.Green , 'g' },
            { Color.Blue , 'b' },
            { Color.Red , 'r' },
            { Color.Orange , 'o' }
        };
        List<char> validFaces = new List<char> { 'u', 'd', 'f', 'b', 'r', 'l' , 'm', 'e', 's' };
        List<List<char>> facesOnSlice = new List<List<char>>
        {
            new List<char> { 'u', 'f', 'd', 'b'},
            new List<char> { 'f', 'r', 'b', 'l'},
            new List<char> { 'u', 'r', 'd', 'l'}
        };
        List<char> slices = new List<char> { 'm', 'e', 's' };

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Width = ClientSize.Width;
            pictureBox1.Height = ClientSize.Height;
            c = new Camera(new Vec3(0, -7, 0), 0, Math.PI / 2);
            SetupPoints();
            SetupPolygons();
            Display();
        }

        private void SetupPoints()
        {
            points = new List<Vec3>();
            pointColors = new List<Color>();
            //points.Add(new Vec3(2.95, 2.95, -2.95));
            //points.Add(new Vec3(2.95, -2.95, -2.95));
            //points.Add(new Vec3(-2.95, -2.95, -2.95));
            //points.Add(new Vec3(-2.95, 2.95, -2.95));
            //points.Add(new Vec3(2.95, 2.95, 2.95));
            //points.Add(new Vec3(2.95, -2.95, 2.95));
            //points.Add(new Vec3(-2.95, -2.95, 2.95));
            //points.Add(new Vec3(-2.95, 2.95, 2.95));
            //for (int i = 0; i < 8; i++) pointColors.Add(Color.Red);

            for (int i = 3; i >= -3; i -= 2) for (int j = -3; j <= 3; j += 2) points.Add(new Vec3(j, i, 3));
            for (int i = 3; i >= -3; i -= 2) for (int j = -3; j <= 3; j += 2) points.Add(new Vec3(j, i, -3));
            for (int i = 3; i >= -3; i -= 2) for (int j = -3; j <= 3; j += 2) points.Add(new Vec3(j, -3, i));
            for (int i = 3; i >= -3; i -= 2) for (int j = -3; j <= 3; j += 2) points.Add(new Vec3(j, 3, i));
            for (int i = 3; i >= -3; i -= 2) for (int j = -3; j <= 3; j += 2) points.Add(new Vec3(3, j, i));
            for (int i = 3; i >= -3; i -= 2) for (int j = -3; j <= 3; j += 2) points.Add(new Vec3(-3, j, i));

            for (int i = 0; i < 96; i++) pointColors.Add(Color.Red);
        }
        //private void SetupLines()
        //{
        //    lines = new List<int[]>();
        //    lineColors = new List<Color>();
        //    for (int i = 0; i < 21; i++)
        //    {
        //        for (int j = 0; j < 20; j++)
        //        {
        //            lines.Add(new int[] { 21 * j + i, 21 * (j + 1) + i });
        //            lineColors.Add(Color.Black);
        //        }
        //    }
        //}
        private void SetupPolygons()
        {
            polygons = new List<int[]>();
            polygonColors = new List<Color>();
            //u
            polygons.Add(new int[] { 0, 1, 5, 4 });
            polygons.Add(new int[] { 1, 2, 6, 5 });
            polygons.Add(new int[] { 2, 3, 7, 6 });
            polygons.Add(new int[] { 4, 5, 9, 8 });
            polygons.Add(new int[] { 5, 6, 10, 9 });
            polygons.Add(new int[] { 6, 7, 11, 10 });
            polygons.Add(new int[] { 8, 9, 13, 12 });
            polygons.Add(new int[] { 9, 10, 14, 13 });
            polygons.Add(new int[] { 10, 11, 15, 14 });
            //d
            polygons.Add(new int[] { 16, 17, 21, 20 });
            polygons.Add(new int[] { 17, 18, 22, 21 });
            polygons.Add(new int[] { 18, 19, 23, 22 });
            polygons.Add(new int[] { 20, 21, 25, 24 });
            polygons.Add(new int[] { 21, 22, 26, 25 });
            polygons.Add(new int[] { 22, 23, 27, 26 });
            polygons.Add(new int[] { 24, 25, 29, 28 });
            polygons.Add(new int[] { 25, 26, 30, 29 });
            polygons.Add(new int[] { 26, 27, 31, 30 });
            //f
            polygons.Add(new int[] { 32, 33, 37, 36 });
            polygons.Add(new int[] { 33, 34, 38, 37 });
            polygons.Add(new int[] { 34, 35, 39, 38 });
            polygons.Add(new int[] { 36, 37, 41, 40 });
            polygons.Add(new int[] { 37, 38, 42, 41 });
            polygons.Add(new int[] { 38, 39, 43, 42 });
            polygons.Add(new int[] { 40, 41, 45, 44 });
            polygons.Add(new int[] { 41, 42, 46, 45 });
            polygons.Add(new int[] { 42, 43, 47, 46 });
            //b
            polygons.Add(new int[] { 48, 49, 53, 52 });
            polygons.Add(new int[] { 49, 50, 54, 53 });
            polygons.Add(new int[] { 50, 51, 55, 54 });
            polygons.Add(new int[] { 52, 53, 57, 56 });
            polygons.Add(new int[] { 53, 54, 58, 57 });
            polygons.Add(new int[] { 54, 55, 59, 58 });
            polygons.Add(new int[] { 56, 57, 61, 60 });
            polygons.Add(new int[] { 57, 58, 62, 61 });
            polygons.Add(new int[] { 58, 59, 63, 62 });
            //r
            polygons.Add(new int[] { 64, 65, 69, 68 });
            polygons.Add(new int[] { 65, 66, 70, 69 });
            polygons.Add(new int[] { 66, 67, 71, 70 });
            polygons.Add(new int[] { 68, 69, 73, 72 });
            polygons.Add(new int[] { 69, 70, 74, 73 });
            polygons.Add(new int[] { 70, 71, 75, 74 });
            polygons.Add(new int[] { 72, 73, 77, 76 });
            polygons.Add(new int[] { 73, 74, 78, 77 });
            polygons.Add(new int[] { 74, 75, 79, 78 });
            //l
            polygons.Add(new int[] { 80, 81, 85, 84 });
            polygons.Add(new int[] { 81, 82, 86, 85 });
            polygons.Add(new int[] { 82, 83, 87, 86 });
            polygons.Add(new int[] { 84, 85, 89, 88 });
            polygons.Add(new int[] { 85, 86, 90, 89 });
            polygons.Add(new int[] { 86, 87, 91, 90 });
            polygons.Add(new int[] { 88, 89, 93, 92 });
            polygons.Add(new int[] { 89, 90, 94, 93 });
            polygons.Add(new int[] { 90, 91, 95, 94 });
            for (int i = 0; i < 545; i++) polygonColors.Add(Color.Black);
            ColourCube();
        }

        private void ColourCube()
        {
            //u
            polygonColors[0] = charToColour[cube.corners[0].ud];
            polygonColors[1] = charToColour[cube.edges[0].Edge0()];
            polygonColors[2] = charToColour[cube.corners[1].ud];
            polygonColors[3] = charToColour[cube.edges[3].Edge0()];
            polygonColors[4] = charToColour[cube.centres[0]];
            polygonColors[5] = charToColour[cube.edges[1].Edge0()];
            polygonColors[6] = charToColour[cube.corners[3].ud];
            polygonColors[7] = charToColour[cube.edges[2].Edge0()];
            polygonColors[8] = charToColour[cube.corners[2].ud];
            //d
            polygonColors[9] = charToColour[cube.corners[4].ud];
            polygonColors[10] = charToColour[cube.edges[8].Edge0()];
            polygonColors[11] = charToColour[cube.corners[5].ud];
            polygonColors[12] = charToColour[cube.edges[11].Edge0()];
            polygonColors[13] = charToColour[cube.centres[1]];
            polygonColors[14] = charToColour[cube.edges[9].Edge0()];
            polygonColors[15] = charToColour[cube.corners[7].ud];
            polygonColors[16] = charToColour[cube.edges[10].Edge0()];
            polygonColors[17] = charToColour[cube.corners[6].ud];
            //f
            polygonColors[18] = charToColour[cube.corners[3].fb];
            polygonColors[19] = charToColour[cube.edges[2].Edge1()];
            polygonColors[20] = charToColour[cube.corners[2].fb];
            polygonColors[21] = charToColour[cube.edges[7].Edge0()];
            polygonColors[22] = charToColour[cube.centres[2]];
            polygonColors[23] = charToColour[cube.edges[6].Edge0()];
            polygonColors[24] = charToColour[cube.corners[7].fb];
            polygonColors[25] = charToColour[cube.edges[10].Edge1()];
            polygonColors[26] = charToColour[cube.corners[6].fb];
            //b
            polygonColors[27] = charToColour[cube.corners[0].fb];
            polygonColors[28] = charToColour[cube.edges[0].Edge1()];
            polygonColors[29] = charToColour[cube.corners[1].fb];
            polygonColors[30] = charToColour[cube.edges[4].Edge0()];
            polygonColors[31] = charToColour[cube.centres[3]];
            polygonColors[32] = charToColour[cube.edges[5].Edge0()];
            polygonColors[33] = charToColour[cube.corners[4].fb];
            polygonColors[34] = charToColour[cube.edges[8].Edge1()];
            polygonColors[35] = charToColour[cube.corners[5].fb];
            //r
            polygonColors[36] = charToColour[cube.corners[2].lr];
            polygonColors[37] = charToColour[cube.edges[1].Edge1()];
            polygonColors[38] = charToColour[cube.corners[1].lr];
            polygonColors[39] = charToColour[cube.edges[6].Edge1()];
            polygonColors[40] = charToColour[cube.centres[4]];
            polygonColors[41] = charToColour[cube.edges[5].Edge1()];
            polygonColors[42] = charToColour[cube.corners[6].lr];
            polygonColors[43] = charToColour[cube.edges[9].Edge1()];
            polygonColors[44] = charToColour[cube.corners[5].lr];
            //l
            polygonColors[45] = charToColour[cube.corners[3].lr];
            polygonColors[46] = charToColour[cube.edges[3].Edge1()];
            polygonColors[47] = charToColour[cube.corners[0].lr];
            polygonColors[48] = charToColour[cube.edges[7].Edge1()];
            polygonColors[49] = charToColour[cube.centres[5]];
            polygonColors[50] = charToColour[cube.edges[4].Edge1()];
            polygonColors[51] = charToColour[cube.corners[7].lr];
            polygonColors[52] = charToColour[cube.edges[11].Edge1()];
            polygonColors[53] = charToColour[cube.corners[4].lr];
        }
        public bool DoRotations(string rotations)
        {
            if (rotations.Length == 0) return false;
            if (rotations.ToLower() == "scramble")
            {
                cube.Scramble();
                return true;
            }
            if (rotations.ToLower() == "reset")
            {
                cube = new Cube();
                return true;
            }
            rotations = rotations.ToLower().Replace(" ", "").Replace("’", "'");
            List<string> rotationsList = new List<string>();
            for (int i = 0; i < rotations.Length; i++)
            {
                if (!validFaces.Contains(rotations[i])) return false;
                if (i == rotations.Length - 1)
                {
                    rotationsList.Add(rotations[i].ToString());
                }
                else if (rotations[i + 1] == '2' || rotations[i + 1] == '\'')
                {
                    rotationsList.Add(rotations.Substring(i, 2));
                    i++;
                }
                else
                {
                    rotationsList.Add(rotations[i].ToString());
                }
            }
            foreach (string rotation in rotationsList)
            {
                cube.Rotate(MapRotation(rotation));
                ColourCube();
            }
            return true;
        }
        public string MapRotation(string input)
        {
            List<int> centrePolygons = new List<int> { 4, 13, 22, 31, 40, 49 };
            List<Vec3> centrePoints = centrePolygons.Select(x => new Vec3(polygons[x].Select(p => points[p].x).Average(), polygons[x].Select(p => points[p].y).Average(), polygons[x].Select(p => points[p].z).Average())).ToList();
            double extreme;
            int index, sliceIndex;
            char face, uFace, fFace, rFace;
            bool reverse = false;
            switch (input[0])
            {
                case 'u':
                    extreme = centrePoints.Select(p => p.z).Max();
                    index = centrePolygons.Select((x, i) => centrePoints[i].z == extreme ? x : -1).Max();
                    face = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    return input.Replace(input[0], face);
                case 'd':
                    extreme = centrePoints.Select(p => p.z).Min();
                    index = centrePolygons.Select((x, i) => centrePoints[i].z == extreme ? x : -1).Max();
                    face = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    return input.Replace(input[0], face);
                case 'f':
                    extreme = centrePoints.Select(p => p.y).Min();
                    index = centrePolygons.Select((x, i) => centrePoints[i].y == extreme ? x : -1).Max();
                    face = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    return input.Replace(input[0], face);
                case 'b':
                    extreme = centrePoints.Select(p => p.y).Max();
                    index = centrePolygons.Select((x, i) => centrePoints[i].y == extreme ? x : -1).Max();
                    face = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    return input.Replace(input[0], face);
                case 'r':
                    extreme = centrePoints.Select(p => p.x).Max();
                    index = centrePolygons.Select((x, i) => centrePoints[i].x == extreme ? x : -1).Max();
                    face = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    return input.Replace(input[0], face);
                case 'l':
                    extreme = centrePoints.Select(p => p.x).Min();
                    index = centrePolygons.Select((x, i) => centrePoints[i].x == extreme ? x : -1).Max();
                    face = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    return input.Replace(input[0], face);
                case 'm':
                    extreme = centrePoints.Select(p => p.z).Max();
                    index = centrePolygons.Select((x, i) => centrePoints[i].z == extreme ? x : -1).Max();
                    uFace = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    extreme = centrePoints.Select(p => p.y).Min();
                    index = centrePolygons.Select((x, i) => centrePoints[i].y == extreme ? x : -1).Max();
                    fFace = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    sliceIndex = facesOnSlice.Select((x, i) => x.Contains(uFace) && x.Contains(fFace) ? i : -1).Max();
                    input = input.Replace(input[0], slices[sliceIndex]);
                    if (facesOnSlice[sliceIndex].IndexOf(fFace) == (facesOnSlice[sliceIndex].IndexOf(uFace) + 3) % 4) reverse = true;
                    break;
                case 'e':
                    extreme = centrePoints.Select(p => p.y).Min();
                    index = centrePolygons.Select((x, i) => centrePoints[i].y == extreme ? x : -1).Max();
                    fFace = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    extreme = centrePoints.Select(p => p.x).Max();
                    index = centrePolygons.Select((x, i) => centrePoints[i].x == extreme ? x : -1).Max();
                    rFace = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    sliceIndex = facesOnSlice.Select((x, i) => x.Contains(fFace) && x.Contains(rFace) ? i : -1).Max();
                    input = input.Replace(input[0], slices[sliceIndex]);
                    if (facesOnSlice[sliceIndex].IndexOf(rFace) == (facesOnSlice[sliceIndex].IndexOf(fFace) + 3) % 4) reverse = true;
                    break;
                case 's':
                    extreme = centrePoints.Select(p => p.z).Max();
                    index = centrePolygons.Select((x, i) => centrePoints[i].z == extreme ? x : -1).Max();
                    uFace = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    extreme = centrePoints.Select(p => p.x).Max();
                    index = centrePolygons.Select((x, i) => centrePoints[i].x == extreme ? x : -1).Max();
                    rFace = validFaces[cube.centres.ToList().IndexOf(colourToChar[polygonColors[index]])];
                    sliceIndex = facesOnSlice.Select((x, i) => x.Contains(uFace) && x.Contains(rFace) ? i : -1).Max();
                    input = input.Replace(input[0], slices[sliceIndex]);
                    if (facesOnSlice[sliceIndex].IndexOf(rFace) == (facesOnSlice[sliceIndex].IndexOf(uFace) + 3) % 4) reverse = true;
                    break;
            }
            if (reverse)
            {
                if (input.Length == 1) return input + '\'';
                if (input[1] == '\'') return input.Substring(0, 1);
            }
            return input;
        }
        public void RotatePoints(double alpha, double beta)
        {
            double ca = Math.Cos(alpha);
            double sa = Math.Sin(alpha);
            double cb = Math.Cos(beta);
            double sb = Math.Sin(beta);
            points = points.Select(p => new Vec3(p.x * cb - p.y * sb, p.x * ca * sb + p.y * ca * cb - p.z * sa, p.x * sa * sb + p.y * sa * cb + p.z * ca)).ToList();
        }

        private void SetupImage()
        {
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }
        public bool IsWithinImage(int x, int y) => x >= 0 && y >= 0 && x < image.Width && y < image.Height;
        public void Display() 
        {
            SetupImage();
            Tuple<int, int>[] screenPos = points.Select(x => c.project(x, image.Width, image.Height)).ToArray();
            double[] distSqFromCamera = points.Select(p => Math.Pow(p.x - c.GetPos().x, 2) + Math.Pow(p.y - c.GetPos().y, 2) + Math.Pow(p.z - c.GetPos().z, 2)).ToArray();
            double[] polygonRank = polygons.Select(x => x.Select(p => distSqFromCamera[p]).Average()).ToArray();
            int[] polygonOrder = new int[polygons.Count].Select((x, i) => i).OrderByDescending(x => polygonRank[x]).ToArray();
            //for (int i = 0; i < lines.Count; i++)
            //{
            //    DrawLine(screenPos[lines[i][0]].Item1, screenPos[lines[i][0]].Item2, screenPos[lines[i][1]].Item1, screenPos[lines[i][1]].Item2, lineColors[i]);
            //}
            //for (int i = 0; i < screenPos.Length; i++)
            //{
            //    if (screenPos[i].Item1 == int.MaxValue || screenPos[i].Item2 == int.MaxValue) continue;
            //    for (int x = -2; x < 3; x++)
            //    {
            //        for (int y = -2; y < 3; y++)
            //        {
            //            DrawPoint(screenPos[i].Item1 + x, screenPos[i].Item2 + y, pointColors[i]);
            //        }
            //    }
            //}
            for (int i = 0; i < polygons.Count; i++)
            {
                int[] pointX = new int[polygons[polygonOrder[i]].Length];
                int[] pointY = new int[polygons[polygonOrder[i]].Length];
                for (int j = 0; j < pointX.Length; j++)
                {
                    pointX[j] = screenPos[polygons[polygonOrder[i]][j]].Item1;
                    pointY[j] = screenPos[polygons[polygonOrder[i]][j]].Item2;
                }
                DrawPolygon(pointX, pointY, polygonColors[polygonOrder[i]]);
            }
            pictureBox1.Image = image;
        }

        public void DrawPoint(int x, int y, Color c)
        {
            if (IsWithinImage(x, y))
            {
                image.SetPixel(x, y, c);
            }
        }
        public void DrawLine(int x1, int y1, int x2, int y2, Color c)
        {
            if (IsWithinImage(x1,y1) && IsWithinImage(x2, y2))
            {
                Graphics g = Graphics.FromImage(image);
                Pen p = new Pen(c);
                g.DrawLine(p, x1, y1, x2, y2);
            }
        }
        public void DrawPolygon(int[] x, int[] y, Color c)
        {
            for (int i = 0; i < x.Length; i++) if (!IsWithinImage(x[i], y[i])) return;
            Graphics g = Graphics.FromImage(image);
            Brush b = new SolidBrush(c);
            Point[] points = new Point[x.Length];
            for (int i = 0; i < x.Length; i++) points[i] = new Point(x[i], y[i]);
            g.FillPolygon(b, points);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            pictureBox1.Width = ClientSize.Width;
            pictureBox1.Height = ClientSize.Height;
            HelpButton.Left = ClientSize.Width - 51;
            Display();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            prevMousePos = new int[] { e.X, e.Y };
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                //double scale = 0.005;
                //c.AdjustA(scale * (prevMousePos[1] - e.Y));
                //c.AdjustB(scale * (prevMousePos[0] - e.X));

                double scale = 0.005;
                RotatePoints(scale * (e.Y - prevMousePos[1]), scale * (e.X - prevMousePos[0]));

                prevMousePos = new int[] { e.X, e.Y };
                Display();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (DoRotations(textBox1.Text))
                {
                    label1.Hide();
                    ColourCube();
                    Display();
                    if (cube.IsSolved()) label2.Show();
                    else label2.Hide();
                }
                else label1.Show();
            }
            else if (e.KeyCode == Keys.Escape) pictureBox1.Focus();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            new HelpForm().ShowDialog();
        }
    }
    public struct Vec3
    {
        public double x, y, z;
        public Vec3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    public class Camera
    {
        private Vec3 pos; //position of camera plane
        private double alpha, beta; //rotations (alpha up/down, beta about z-axis)
        private double l = 20; //focal length
        private double zoom = 1;

        private Vec3 pos2; //position of camera point
        private double ca, sa, cb, sb; //sines and cosines of angles
        private void CalculateExtraValues()
        {
            ca = Math.Cos(alpha);
            sa = Math.Sin(alpha);
            cb = Math.Cos(beta);
            sb = Math.Sin(beta);
            pos2 = new Vec3(pos.x - l * ca * cb, pos.y - l * ca * sb, pos.z - l * sa);
        }

        public Camera(Vec3 pos, double alpha, double beta)
        {
            this.pos = pos;
            this.alpha = alpha;
            this.beta = beta;
            CalculateExtraValues();
        }

        public Vec3 GetPos() => pos;
        public void SetPos(Vec3 p)
        {
            pos = p;
            CalculateExtraValues();
        }
        public double GetA() => alpha;
        public double GetB() => beta;
        public void SetA(int alpha)
        {
            this.alpha = alpha;
            CalculateExtraValues();
        }
        public void SetB(int beta)
        {
            this.beta = beta;
            CalculateExtraValues();
        }
        public double GetL() => l;
        public void SetL(double l)
        {
            this.l = l;
            CalculateExtraValues();
        }
        public double GetZoom() => zoom;
        public double SetZoom(double zoom) => this.zoom = zoom;

        public void AdjustPos(Vec3 v)
        {
            pos.x += v.x;
            pos.y += v.y;
            pos.z += v.z;
            CalculateExtraValues();
        }
        public void AdjustA(double dAlpha)
        {
            alpha += dAlpha;
            if (alpha > 1.56) alpha = 1.56;
            else if (alpha < -1.56) alpha = -1.56;
            CalculateExtraValues();
        }
        public void AdjustB(double dBeta)
        {
            beta += dBeta;
            CalculateExtraValues();
        }

        public Tuple<int, int> project(Vec3 point, int w, int h)
        {
            if (point.x * ca * cb + point.y * ca * sb + point.z * sa < l + pos2.x * ca * cb + pos2.y * ca * sb + pos2.z * sa) return new Tuple<int, int>(int.MaxValue, int.MaxValue);
            double lambda = l / ((point.x - pos2.x) * ca * cb + (point.y - pos2.y) * ca * sb + (point.z - pos2.z) * sa);
            Vec3 v  = new Vec3(lambda*(point.x-pos2.x) - l*ca*cb, lambda * (point.y - pos2.y) - l * ca * sb, lambda * (point.z - pos2.z) - l * sa);
            double r = Math.Sqrt(v.x*v.x+ v.y*v.y + v.z*v.z);
            if (r == 0) return Tuple.Create(w/2, h/2);
            double ctheta = (v.x * sb - v.y * cb) / r;
            double xInPlane = zoom * r * ctheta;
            double yInPlane = zoom * r * Math.Sqrt(1 - ctheta * ctheta);
            if (lambda * (point.z - pos2.z) - l * sa < 0) yInPlane = -yInPlane;

            return Tuple.Create((int)(w/2*(1+xInPlane/10)),(int)(h/2*(1-(yInPlane*w)/(10*h))));
        }
    }
}
