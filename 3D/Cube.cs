using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _3D
{
    internal class Cube
    {
        private string[] correctCorners = new string[] { "wbo", "wbr", "wgr", "wgo", "ybo", "ybr", "ygr", "ygo" };
        private string[] correctEdges = new string[] { "wb", "wr", "wg", "wo", "bo", "br", "gr", "go", "yb", "yr", "yg", "yo" };
        public char[] centres = { 'w', 'y', 'g', 'b', 'r', 'o' };
        public Corner[] corners;
        public Edge[] edges;

        private Dictionary<char, int[]> cornersOnFace = new Dictionary<char, int[]>()
        {
            { 'u' ,  new int[]{ 0, 1, 2, 3 } },
            { 'd' ,  new int[]{ 7, 6, 5, 4 } },
            { 'f' ,  new int[]{ 3, 2, 6, 7 } },
            { 'b' ,  new int[]{ 1, 0, 4, 5 } },
            { 'l' ,  new int[]{ 0, 3, 7, 4 } },
            { 'r' ,  new int[]{ 2, 1, 5, 6 } },
        };
        private Dictionary<char, int[]> edgesOnFace = new Dictionary<char, int[]>()
        {
            { 'u' ,  new int[]{ 0, 1, 2, 3 } },
            { 'd' ,  new int[]{ 10, 9, 8, 11 } },
            { 'f' ,  new int[]{ 2, 6, 10, 7 } },
            { 'b' ,  new int[]{ 0, 4, 8, 5 } },
            { 'l' ,  new int[]{ 3, 7, 11, 4 } },
            { 'r' ,  new int[]{ 1, 5, 9, 6 } },
            { 'm' , new int[]{ 0, 2, 10, 8 } },
            { 'e', new int[]{ 7, 6, 5, 4 } },
            { 's' , new int[]{ 3, 1, 9, 11 } }
        };
        private Dictionary<char, int[]> centresOnFace = new Dictionary<char, int[]>()
        {
            { 'm' , new int[]{ 0, 2, 1, 3 } },
            { 'e', new int[]{ 2, 4, 3, 5 } },
            { 's' , new int[]{ 0, 4 , 1, 5 } }
        };

        public Cube()
        {
            corners = new Corner[correctCorners.Length];
            edges = new Edge[correctEdges.Length];
            for (int i = 0; i < correctCorners.Length; i++)
            {
                corners[i] = new Corner(correctCorners[i]);
            }
            for (int i = 0; i < correctEdges.Length; i++)
            {
                edges[i] = new Edge(correctEdges[i]);
            }
        }

        public void Rotate(string rotation)
        {
            //input must be lowercase u, d, f etc with optional 2 or ' after
            
            int[] eIndexes = edgesOnFace[rotation[0]];
            Corner cTemp;
            Edge eTemp;

            if (rotation.Length > 1)
            {
                if (rotation[1] == '2')
                {
                    //double rotation
                    if (centresOnFace.ContainsKey(rotation[0]))
                    {
                        int[] ceIndexes = centresOnFace[rotation[0]];
                        char ceTemp = centres[ceIndexes[0]];
                        centres[ceIndexes[0]] = centres[ceIndexes[2]];
                        centres[ceIndexes[2]] = ceTemp;
                        ceTemp = centres[ceIndexes[1]];
                        centres[ceIndexes[1]] = centres[ceIndexes[3]];
                        centres[ceIndexes[3]] = ceTemp;
                    }
                    else
                    {
                        int[] cIndexes = cornersOnFace[rotation[0]];
                        cTemp = corners[cIndexes[0]];
                        corners[cIndexes[0]] = corners[cIndexes[2]];
                        corners[cIndexes[2]] = cTemp;
                        cTemp = corners[cIndexes[1]];
                        corners[cIndexes[1]] = corners[cIndexes[3]];
                        corners[cIndexes[3]] = cTemp;
                    }

                    eTemp = edges[eIndexes[0]];
                    edges[eIndexes[0]] = edges[eIndexes[2]];
                    edges[eIndexes[2]] = eTemp;
                    eTemp = edges[eIndexes[1]];
                    edges[eIndexes[1]] = edges[eIndexes[3]];
                    edges[eIndexes[3]] = eTemp;
                }
                else
                {
                    //anticlockwise rotation
                    if (centresOnFace.ContainsKey(rotation[0]))
                    {
                        int[] ceIndexes = centresOnFace[rotation[0]];
                        char ceTemp = centres[ceIndexes[0]];
                        centres[ceIndexes[0]] = centres[ceIndexes[1]];
                        centres[ceIndexes[1]] = centres[ceIndexes[2]];
                        centres[ceIndexes[2]] = centres[ceIndexes[3]];
                        centres[ceIndexes[3]] = ceTemp;
                    }
                    else
                    {
                        int[] cIndexes = cornersOnFace[rotation[0]];
                        cTemp = corners[cIndexes[0]];
                        corners[cIndexes[0]] = corners[cIndexes[1]];
                        corners[cIndexes[1]] = corners[cIndexes[2]];
                        corners[cIndexes[2]] = corners[cIndexes[3]];
                        corners[cIndexes[3]] = cTemp;
                        foreach (int i in cIndexes)
                        {
                            corners[i].Rotate(rotation[0]);
                        }
                    }

                    eTemp = edges[eIndexes[0]];
                    edges[eIndexes[0]] = edges[eIndexes[1]];
                    edges[eIndexes[1]] = edges[eIndexes[2]];
                    edges[eIndexes[2]] = edges[eIndexes[3]];
                    edges[eIndexes[3]] = eTemp;
                    foreach (int i in eIndexes)
                    {
                        edges[i].Rotate(rotation[0]);
                    }
                }
            }
            else
            {
                //clockwise rotation
                if (centresOnFace.ContainsKey(rotation[0]))
                {
                    int[] ceIndexes = centresOnFace[rotation[0]];
                    char ceTemp = centres[ceIndexes[0]];
                    centres[ceIndexes[0]] = centres[ceIndexes[3]];
                    centres[ceIndexes[3]] = centres[ceIndexes[2]];
                    centres[ceIndexes[2]] = centres[ceIndexes[1]];
                    centres[ceIndexes[1]] = ceTemp;
                }
                else
                {
                    int[] cIndexes = cornersOnFace[rotation[0]];
                    cTemp = corners[cIndexes[0]];
                    corners[cIndexes[0]] = corners[cIndexes[3]];
                    corners[cIndexes[3]] = corners[cIndexes[2]];
                    corners[cIndexes[2]] = corners[cIndexes[1]];
                    corners[cIndexes[1]] = cTemp;
                    foreach (int i in cIndexes)
                    {
                        corners[i].Rotate(rotation[0]);
                    }
                }

                eTemp = edges[eIndexes[0]];
                edges[eIndexes[0]] = edges[eIndexes[3]];
                edges[eIndexes[3]] = edges[eIndexes[2]];
                edges[eIndexes[2]] = edges[eIndexes[1]];
                edges[eIndexes[1]] = eTemp;

                foreach (int i in eIndexes)
                {
                    edges[i].Rotate(rotation[0]);
                }
            }
        }

        public bool IsSolved()
        {
            if (centres[0] != corners[0].ud || centres[0] != corners[1].ud || centres[0] != corners[2].ud || centres[0] != corners[3].ud || centres[0] != edges[0].Edge0() || centres[0] != edges[1].Edge0() || centres[0] != edges[2].Edge0() || centres[0] != edges[3].Edge0()) return false;
            if (centres[1] != corners[4].ud || centres[1] != corners[5].ud || centres[1] != corners[6].ud || centres[1] != corners[7].ud || centres[1] != edges[8].Edge0() || centres[1] != edges[9].Edge0() || centres[1] != edges[10].Edge0() || centres[1] != edges[11].Edge0()) return false;
            if (centres[2] != corners[2].fb || centres[2] != corners[3].fb || centres[2] != corners[6].fb || centres[2] != corners[7].fb || centres[2] != edges[2].Edge1() || centres[2] != edges[6].Edge0() || centres[2] != edges[7].Edge0() || centres[2] != edges[10].Edge1()) return false;
            if (centres[3] != corners[0].fb || centres[3] != corners[1].fb || centres[3] != corners[4].fb || centres[3] != corners[5].fb || centres[3] != edges[0].Edge1() || centres[3] != edges[4].Edge0() || centres[3] != edges[5].Edge0() || centres[3] != edges[8].Edge1()) return false;
            if (centres[4] != corners[1].lr || centres[4] != corners[2].lr || centres[4] != corners[5].lr || centres[4] != corners[6].lr || centres[4] != edges[1].Edge1() || centres[4] != edges[5].Edge1() || centres[4] != edges[6].Edge1() || centres[4] != edges[9].Edge1()) return false;
            if (centres[5] != corners[0].lr || centres[5] != corners[3].lr || centres[5] != corners[4].lr || centres[5] != corners[7].lr || centres[5] != edges[3].Edge1() || centres[5] != edges[4].Edge1() || centres[5] != edges[7].Edge1() || centres[5] != edges[11].Edge1()) return false;
            return true;
        }

        public void Scramble()
        {
            Random rnd = new Random();
            string[] faces = new string[] { "u", "d", "f", "b", "l", "r" };
            string[] directions = new string[] { "", "\'", "2" };
            for (int i = 0; i < 100; i++)
            {
                Rotate(faces[rnd.Next(6)] + directions[rnd.Next(3)]);
            }
        }
    }
    internal class Corner
    {
        //Gives the colour on the up/down face etc
        public char ud, fb, lr;

        public Corner(string colours)
        {
            ud = colours[0];
            fb = colours[1];
            lr = colours[2];
        }

        public void Rotate(char face)
        {
            if (face == 'u' || face == 'd')
            {
                char temp = fb;
                fb = lr;
                lr = temp;
            }
            else if (face == 'f' || face == 'b')
            {
                char temp = ud;
                ud = lr;
                lr = temp;
            }
            else
            {
                char temp = ud;
                ud = fb;
                fb = temp;
            }
        }
    }
    internal class Edge
    {
        public string cols;
        public bool oriented = true;

        public Edge(string colours)
        {
            cols = colours;
        }

        public void Rotate(char face)
        {
            if (face == 'f' || face == 'b' || face == 'm' || face == 'e' || face == 's')
            {
                oriented = !oriented;
            }
        }

        public char Edge0() => oriented ? cols[0] : cols[1];
        public char Edge1() => oriented ? cols[1] : cols[0];
    }
}
