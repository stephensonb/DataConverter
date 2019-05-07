using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DataConverter
{

    class CoordinateMapping
    {
        private string _input_orientation;
        private string _normalized_input_orientation;
        private string _output_orientation;
        private string _normalized_output_orientation;
        
        public Point InputMapOrigin
        {
            get;
            set;
        }
        public Point OutputMapOrigin
        {
            get;
            set;
        }

        public Point _inputReferenceDie = new Point();

        public Point _outputReferenceDie = new Point();

        public String WaferOrientation
        {
            get;
            set;
        }

        public int Rows
        {
            get;
            set;
        }

        public int Columns
        {
            get;
            set;
        }
        public Matrix _inputTransform = null;

        public Matrix _outputTransform = null;

        public CoordinateMapping()
        {

        }

        // To translate a die coordinate from one tool coordinate system to another
        // Need to know orientation and map origin of the die map on the input tool and output tools
        // -- Rotate the input matrix either 90, 180 or 270 to match orientation of output map
        // -- Translate input point to output point (calc dx, dy between input and output map origins) and apply to transform matrix
        // If no map origin given, it is assumed map origin is 0,0 in center die of wafer.
        // If there are an odd number of rows, center row is (int)(nrows/2)
        // If there are an even number of rows, center row is nrows/2.
        // If there are an odd number of columns, center column is (int)(ncols/2);
        // If there are an even number of columns, center column is ncols/2


        public String InputOrientation
        {
            get
            {
                return _input_orientation;
            }

            set
            {
                _input_orientation = value;
                _normalized_input_orientation = NormalizeOrientation(value);
            }
        }

        public String OutputOrientation
        {
            get
            {
                return _output_orientation;
            }

            set
            {
                _output_orientation = value;
                _normalized_output_orientation = NormalizeOrientation(value);
            }
        }

        public string NormalizedInputOrientation
        {
            get
            {
                return _normalized_input_orientation;
            }
        }

        public string NormalizedOutputOrientation
        {
            get
            {
                return _normalized_output_orientation;
            }
        }

        public static string NormalizeOrientation(String orientation)
        {
            orientation = orientation.Substring(0, 1).ToUpper();
            string result;

            switch (orientation)
            {
                case "N":
                case "T":
                case "C":
                    result = "U";
                    break;
                case "L":
                case "W":
                    result = "L";
                    break;
                case "D":
                case "B":
                case "S":
                    result = "D";
                    break;
                case "R":
                case "E":
                    result = "R";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        public static int MarkToDegrees(string flatOrientation)
        {
            string fnloc = NormalizeOrientation(flatOrientation);
            int result=0; 

            switch(fnloc)
            {
                case "U":
                    result = 0;
                    break;
                case "R":
                    result = 90;
                    break;
                case "D":
                    result = 180;
                    break;
                case "L":
                    result = 270;
                    break;
                case "":
                    result = 0;
                    break;
            }

            return result;
        }

        public void InitializeAffineTransformMatrix()
        {
            _inputTransform.Reset(); // Reset to the identity matrix
            _outputTransform.Reset(); // Reset to the identity matrix

            // Create a code indication rotation needed for translating an input coordinate to and output coordinate
            // Translating from output to input will be just the opposite for each code.
            string c = string.Format("{0}{1}", NormalizedInputOrientation, NormalizedOutputOrientation);

            switch (c)
            {
                // If input and output orientations match, then no rotation
                case "UU":
                case "DD":
                case "LL":
                case "RR":
                    break;
                // Check to see if need to rotate CW 90 deg., Input -> Output
                case "DL":
                case "LU":
                case "UR":
                case "RD":
                    _inputTransform.Rotate(90.0f);
                    _outputTransform.Rotate(-90.0f); // Opposite direction for output transform to input transform.
                    break;
                // Check to see if need to rotate CCW 90 deg.
                case "DR":
                case "RU":
                case "UL":
                case "LD":
                    _inputTransform.Rotate(-90.0f);
                    _outputTransform.Rotate(90.0f); // Opposite direction for output transform to input transform.
                    break;
                // Check to see if need to rotate 180deg.
                case "DU":
                case "UD":
                case "LR":
                case "RL":
                    _inputTransform.Rotate(180.0f);
                    _outputTransform.Rotate(180.0f); // Same direction for output transform to input transform.
                    break;
            }
        }

        // This routine transforms a point from the input coordinate system to the output coordinate system
        // The steps are given for an example point of (x,y) => (32, 1):
        //
        // 1) Translate 
        // 2) Apply rotation needed to match output orientation
        // 3) Translate back by applying a translation of (+32, +1).
        // 4) Translate the point
        // 
        public Point TransformPoint(Point coordinate)
        {
            string c = string.Format("{0}{1}", NormalizedInputOrientation, NormalizedOutputOrientation);

            return coordinate;

        }
    }
}