using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Tronics.DataConverter.KLARF
{
    public class FileVersionRecord
    {
        public Int32 Major=0;
        public Int32 Minor=0;

        public override string ToString()
        {
            return String.Format("FileVersion {0} {1};\n\r", Major, Minor);
        }

    }

    public class TiffSpecRecord
    {
        public string TiffVersion="";
        public string TiffAlignmentClass="";
        public string TiffImageClass="";

        public override string ToString()
        {
            return String.Format("TiffSpecRecord {0} {1} {2};\n\r", TiffVersion, TiffAlignmentClass, TiffImageClass);
        }
    }

    public class InspectionStationRecord
    {
        public string ManufacturerID="";
        public string Model="";
        public string EquipmentID="";

        public override string ToString()
        {
            return String.Format("InspectionStationID \"{0}\" \"{1}\" \"{2}\";\n\r", ManufacturerID, Model, EquipmentID);
        }

    }

    public class SampleSizeRecord
    {
        public Int32 ShapeType=0;
        public Int32 Dimension1=0;
        public Int32 Dimension2=0;

        public override string ToString()
        {
            return String.Format("SampleSize {0} {1} {2};\n\r", ShapeType, Dimension1, Dimension2);
        }
    }

    public class SetupIDRecord
    {
        public string Name="";
        public string Date="";
        public string Time="";

        public override string ToString()
        {
            return String.Format("SetupID \"{0}\" {1} {2};\n\r", Name, Date, Time);
        }
    
    }

    public class SampleCenterLocationRecord
    {
        public double X=0;
        public double Y=0;

        public override string ToString()
        {
            return String.Format("SampleCenterLocation {0:f3} {1:f3};\n\r",X, Y);
        }

    }

    public class AlignmentPoint
    {
        public Int32 MarkID=0;
        public double X=0;
        public double Y=0;

        public override string ToString()
        {
            return String.Format("{0} {1:f3} {2:f3}\n\r", MarkID, X, Y);
        }

    }

    public class AlignmentPointsRecord
    {
        public Int32 Count;
        public AlignmentPoint[] AlignmentPoints;

        public AlignmentPointsRecord(Int32 points)
        {
            Count = points;
            AlignmentPoints = new AlignmentPoint[Count];
        }

        public void Add(Int32 markid, double x, double y)
        {
            int c = AlignmentPoints.Count() + 1;

            AlignmentPoints[c].X = x;
            AlignmentPoints[c].Y = y;
            AlignmentPoints[c].MarkID = markid;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("AlignmentPoints {0} \n\r", Count));
            foreach(AlignmentPoint p in AlignmentPoints)
            {
                s.Append(p.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class AlignmentImage
    {
        public Int32 MarkID;
        public double X;
        public double Y;
        public Int32 ImageNumber;

        public override string ToString()
        {
            return String.Format("{0} {1:f3} {2:f3} {3}\n\r", MarkID, X, Y, ImageNumber);
        }
    }

    public class AlignmentImagesRecord
    {
        public Int32 Count;
        public AlignmentImage[] AlignmentImages;

        public AlignmentImagesRecord(Int32 points)
        {
            Count = points;
            AlignmentImages = new AlignmentImage[Count];
        }

        public void Add(Int32 markid, double x, double y, Int32 imagenumber)
        {
            int c = AlignmentImages.Count() + 1;

            AlignmentImages[c].X = x;
            AlignmentImages[c].Y = y;
            AlignmentImages[c].MarkID = markid;
            AlignmentImages[c].ImageNumber = imagenumber;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("AlignmentImages {0} \n\r", Count));
            foreach (AlignmentImage a in AlignmentImages)
            {
                s.Append(a.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class AlignmentImageTransformsRecord
    {
        public double[,] TransformMatrix = new double[2,2];
        public Int32 Count;
        public Int32[] AlignmentMarkIDs;

        public AlignmentImageTransformsRecord(Int32 count)
        {
            Count = count;
            AlignmentMarkIDs = new Int32[count];
        }

        public void Add(Int32 markid)
        {
            int c = AlignmentMarkIDs.Count() + 1;
            AlignmentMarkIDs[c] = markid;
        }

        public void SetTransform(double a11, double a12, double a21, double a22)
        {
            TransformMatrix[0, 0] = a11;
            TransformMatrix[0, 1] = a12;
            TransformMatrix[1, 0] = a21;
            TransformMatrix[1, 1] = a22;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("AlignmentImageTransforms {0:f3} {1:f3} {2:f3} {3:f3} {4}\n\r", TransformMatrix[0,0], TransformMatrix[0,1],TransformMatrix[1,0], TransformMatrix[1,1], Count));
            foreach (Int32 i in AlignmentMarkIDs)
            {
                s.Append(String.Format("{0}\n\r", i));
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class DatabaseAlignmentMark
    {
        public Int32 MarkID;
        public double AlignmentImageOriginX;
        public double AlignmentImageOriginY;
        public double AlignmentPointX;
        public double AlignmentPointY;

        public override string ToString()
        {
            return String.Format("{0} {1:f3} {2:f3} {3:f3} {4:f3}\n\r", MarkID, AlignmentImageOriginX, AlignmentImageOriginY, AlignmentPointX, AlignmentPointY);
        }
    }

    public class DatabaseAlignmentMarksRecord
    {
        public Int32 Count;
        public DatabaseAlignmentMark[] DatabaseAlignmentMarks;

        public DatabaseAlignmentMarksRecord(string count)
        {
            Count = Int32.Parse(count);
            DatabaseAlignmentMarks = new DatabaseAlignmentMark[Count];
        }

        public void Add(Int32 markid, double xorigin, double yorigin, double xpoint, double ypoint)
        {
            int c = DatabaseAlignmentMarks.Count() + 1;

            DatabaseAlignmentMarks[c].AlignmentImageOriginX = xorigin;
            DatabaseAlignmentMarks[c].AlignmentImageOriginY = yorigin;
            DatabaseAlignmentMarks[c].AlignmentPointX = xpoint;
            DatabaseAlignmentMarks[c].AlignmentPointY = ypoint;
            DatabaseAlignmentMarks[c].MarkID = markid;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("DatabaseAlignmentMarks {0}\n\r", Count));
            foreach (DatabaseAlignmentMark dba in DatabaseAlignmentMarks)
            {
                s.Append(dba.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class DiePitch
    {
        public double X;
        public double Y;

        public override string ToString()
        {
            return string.Format("{0:f3} {1:f3}\n\r", X, Y);
        }
    }

    public class DieOrigin
    {
        public double X;
        public double Y;

        public override string ToString()
        {
            return string.Format("{0:f3} {1:f3}\n\r", X, Y);
        }
    }

    public class DieIndex
    {
        public Int32 X;
        public Int32 Y;

        public override string ToString()
        {
            return string.Format("{0} {1}\n\r", X, Y);
        }
    }

    public class RemovedDieListRecord
    {
        public Int32 Count;
        public DieIndex[] RemovedDie;

        public RemovedDieListRecord(string count)
        {
            Count = Int32.Parse(count);
            RemovedDie = new DieIndex[Count];
        }

        public void Add(Int32 x, Int32 y)
        {
            int c = RemovedDie.Count() + 1;

            RemovedDie[c].X = x;
            RemovedDie[c].Y = y;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(string.Format("RemovedDieList {0}\n\r", Count));

            foreach (DieIndex di in RemovedDie)
            {
                s.Append(di.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class DefectRecord
    {
        public String Name;
        public Int32 DEFECTID;
        public Single X;
        public Single Y;
        public Single XREL;
        public Single YREL;
        public Int32 XINDEX;
        public Int32 YINDEX;
        public Single XSIZE;
        public Single YSIZE;
        public Single DEFECTAREA;
        public Single DSIZE;
        public Int16 CLASSNUMBER;
        public Int32 TEST;
        public Int16 IMAGECOUNT;
    }

    public class FileTimestampRecord
    {
        public string Date;
        public string Time;

        public override string ToString()
        {
            return String.Format("{0} {1};\n\r", Date, Time);
        }
    }

    public class ResultTimestampRecord
    {
        public string Date;
        public string Time;

        public override string ToString()
        {
            return String.Format("{0} {1};\n\r", Date, Time);
        }
    }

    public class LotRecord
    {
        public string LotID = "";
        public SampleSizeRecord SampleSize;
        public SetupIDRecord SetupID;
        public string DeviceID = "";
        public string StepID = "";
        public string SampleOrientationMarkType = "";
        public string OrientationMarkLocation = "";
        public List<WaferRecord> Wafers = new List<WaferRecord>();
        public DiePitchRecord DiePitch;
        public DieOriginRecord DieOrigin;
        public LotStatusRecord LotStatus;
        public string WaferStatus = "";

        public void Serialize(StreamWriter fs)
        {
            fs.Write(String.Format("LotID {0};\n\r", LotID));
            fs.Write(SampleSize.ToString());
            fs.Write(SetupID.ToString());
            fs.Write(String.Format("DeviceID {0};\n\r", DeviceID));
            fs.Write(String.Format("StepID {0};\n\r", StepID));
            fs.Write(String.Format("SampleOrientationMarkType {0};\n\r", SampleOrientationMarkType));
            fs.Write(String.Format("OrientationMarkLocation {0};\n\r", OrientationMarkLocation));
            fs.Write(DiePitch.ToString());
            fs.Write(DieOrigin.ToString());
            foreach (WaferRecord wr in Wafers)
            {
                wr.Serialize(fs);
            }
            fs.Write(LotStatus.ToString());
            fs.Write(String.Format("WaferStatus {0};\n\r", WaferStatus));
        }
    }

    public class WaferRecord
    {
        public string WaferID = "";
        public Int32 SlotID;
        public string InspectionOrientation = "";
        public string OrientationInstructions = "";
        public string CoordinatesMirrored = "";
        public SampleCenterLocationRecord SampleCenterLocation = null;
        public string TiffFileName = "";
        public AlignmentPointsRecord AlignmentPoints = null;
        public AlignmentImagesRecord AlignmentImages = null;
        public AlignmentImageTransformsRecord AlignmentImageTransforms = null;
        public DatabaseAlignmentMarksRecord DatabaseAlignmentMarks = null;
        public RemovedDieListRecord RemovedDieList = null;
        public SampleDieMapRecord SampleDieMap = null;
        public ClassLookupRecord ClassLookup = null;
        public DefectClusterSpecRecord DefectClusterSpecs = null;
        public ClusterClassificationListRecord ClusterClassificationList = null;
        public List<InspectionTestRecord> InspectionTests = null;
        public SummarySpecRecord SummarySpecs = null;

        public void Serialize(StreamWriter fs)
        {
            if (WaferID != "")
            {
                fs.Write(string.Format("WaferID \"{0}\";\n\r", WaferID));
            }
            
            if (SlotID > 0)
            {
                fs.Write(string.Format("SlotID {0};\n\r", SlotID));
            }

            if (InspectionOrientation != "")
            {
                fs.Write(string.Format("InspectionOrientation {0};\n\r", InspectionOrientation));
            }
            
            if (OrientationInstructions != "")
            {
                fs.Write(string.Format("OrientationInstructions \"{0}\";\n\r", OrientationInstructions));
            }

            if (TiffFileName != "")
            {
                fs.Write(string.Format("TiffFileName \"{0}\";\n\r", TiffFileName));
            }

            if (SampleCenterLocation != null)
            {
                fs.Write(SampleCenterLocation.ToString());
            }

            if (AlignmentPoints != null)
            {
                fs.Write(AlignmentPoints.ToString());
            }

            if (AlignmentImages != null)
            {
                fs.Write(AlignmentImages.ToString());
            }

            if (AlignmentImageTransforms != null)
            {
                fs.Write(AlignmentImageTransforms.ToString());
            }

            if (DatabaseAlignmentMarks != null)
            {
                fs.Write(DatabaseAlignmentMarks.ToString());
            }

            if (RemovedDieList != null)
            {
                fs.Write(RemovedDieList.ToString());
            }

            if (SampleDieMap != null)
            {
                fs.Write(SampleDieMap.ToString());
            }

            if (ClassLookup != null)
            {
                fs.Write(ClassLookup.ToString());
            }

            if (DefectClusterSpecs != null)
            {
                fs.Write(DefectClusterSpecs.ToString());
            }

            if (ClusterClassificationList != null)
            {
                fs.Write(ClusterClassificationList.ToString());
            }

            if (InspectionTests != null)
            {
                foreach (InspectionTestRecord itr in InspectionTests)
                {
                    itr.Serialize(fs);
                }
            }

            if (SummarySpecs != null)
            {
                fs.Write(SummarySpecs.ToString());
            }
        }
    }

    public class SummarySpecRecord
    {
        public Int32 Count;
        public string[] Names;
        public List<string[]> SummarySpecList;

        public SummarySpecRecord(string count)
        {
            Count = Int32.Parse(count);
            Names = new string[Count];
        }

        public Dictionary<string, Type> SummarySpecTypes = new Dictionary<string, Type>() 
        {
            {"TESTNO", typeof(Int32)},
            {"NDEFECT", typeof(Int32)},
            {"DEFDENSITY", typeof(Single)},
            {"NDIE", typeof(Int32)},
            {"NDEFDIE", typeof(Int32)}
        };

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(string.Format("SummarySpec {0} ", Count));

            foreach (string n in Names)
            {
                s.Append(n + " ");
            }
            s.Append(";\n\rSummaryList \n\r");
            foreach (string[] summary in SummarySpecList)
            {
                for(int i=0;i<Count;i++)
                {
                    Type t = SummarySpecTypes[Names[i].ToUpper()];

                    if (t == typeof(Int32) || t == typeof(String) || t == typeof(Int16) || t == typeof(Byte) || t == typeof(Char))
                    {
                        s.Append(string.Format("{0} ", summary[i]));
                    } 
                    else
                    {
                        if (t == typeof(Single) || t == typeof(Double))
                        {
                            s.Append(string.Format("{0:f3} ", Double.Parse(summary[i])));
                        } 
                    }
                }
                s.Append("\n\r");
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }


    public class DefectClass
    {
        public Int32 ClassNumber;
        public string ClassName;

        public override string ToString()
        {
            return string.Format("{0} {1}\n\r", ClassNumber, ClassName);
        }
    }

    public class ClassLookupRecord
    {
        public Int32 Count;
        public List<DefectClass> ClassList;

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(string.Format("ClassLookup {0}\n\r", Count));

            foreach (DefectClass dc in ClassList)
            {
                s.Append(dc.ToString());
            }

            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class ClusterClassificationRecord
    {
        public Int32 ClusterNumber;
        public Int32 Class;

        public override string ToString()
        {
            return string.Format("{0} {1}\n\r", ClusterNumber, Class);
        }
    }

    public class ClusterClassificationListRecord
    {
        public Int32 Count;
        public List<ClusterClassificationRecord> ClusterClassificationList;

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(string.Format("ClusterClassificationList {0}\n\r", Count));

            foreach (ClusterClassificationRecord ccr in ClusterClassificationList)
            {
                s.Append(ccr.ToString());
            }

            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class DefectClusterSpecRecord
    {
        public Int32 Count;
        public string[] ClusterSpecNames;
        public string[] ClusterSpecValues;

        public DefectClusterSpecRecord(string count)
        {
            Count = Int32.Parse(count);
            ClusterSpecNames = new string[Count];
            ClusterSpecValues = new string[Count];
        }
        
        public Dictionary<string, Type> ClusterSpecValueTypes = new Dictionary<string, Type>() 
        {
            {"THRESHOLD", typeof(Single)},
            {"MINSIZE", typeof(Int16)},
            {"MERGTOL", typeof(Int32)}
        };

        public override string ToString()
        {
 	        string s=string.Format("DefectClusterSpec {0} ", Count);
            for(int i=0;i<Count;i++)
            {
                s = s + ClusterSpecNames[i] + " ";
            }
            s=s+"\n\rDefectClusterSetup ";
            for(int i=0;i<Count;i++)
            {
                s = s + ClusterSpecValues[i] + " ";
            }
            s=s.Trim()+";\n\r";
            return s;
        }
    }

    public class DiePitchRecord
    {
        public float X;
        public float Y;

        public override string ToString()
        {
            return string.Format("DiePitch {0} {1};\n\r", X, Y);
        }
    }

    public class DieOriginRecord
    {
        public float X;
        public float Y;

        public override string ToString()
        {
            return string.Format("DieOrigin {0} {1};\n\r", X, Y);
        }
    }

    public class SampleDieMapRecord
    {
        public Int32 Count;
        public List<Die> DieList;

        public SampleDieMapRecord(string count)
        {
            Count = Int32.Parse(count);
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("SampleDieMap {0}\n\r", Count));

            foreach (Die d in DieList)
            {
                s.Append(d.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class Die
    {
        public Int32 X;
        public Int32 Y;

        public override string ToString()
        {
            return string.Format("{0} {1}\n\r", X, Y);
        }
    }

    public class InspectionTestRecord
    {
        public Int32 InspectionTest;
        public SampleTestPlanRecord SampleTestPlan;
        public SampleTestReferencePlanRecord SampleTestReferencePlan;
        public InspectedAreaOriginRecord InspectedAreaOrigin;
        public InspectedAreaRecord InspectedAreas;
        public Single AreaPerTest;
        public TestParameterSpecRecord TestParametersSpec;
        public DefectRecordSpecRecord DefectRecordSpecs;

        public void Serialize(StreamWriter fs)
        {
            if (InspectionTest > 0)
            {
                fs.Write(string.Format("InspectionTest {0};\n\r", InspectionTest));
            }
            if (SampleTestPlan != null)
            {
                fs.Write(SampleTestPlan.ToString());
            }
            if (SampleTestReferencePlan != null)
            {
                fs.Write(SampleTestReferencePlan.ToString());
            }
            if (InspectedAreaOrigin != null)
            {
                fs.Write(InspectedAreaOrigin.ToString());
            }
            if (InspectedAreas != null)
            {
                fs.Write(InspectedAreas.ToString());
            }
            if (AreaPerTest > 0)
            {
                fs.Write(string.Format("AreaPerTest {0:f3};\n\r", AreaPerTest));
            }
            if (TestParametersSpec != null)
            {
                fs.Write(TestParametersSpec.ToString());
            }
            if (DefectRecordSpecs != null)
            {
                fs.Write(DefectRecordSpecs.ToString());
            }
        }
    }

    public class DefectRecordSpecRecord
    {
        public Int32 Count;
        public string[] DefectRecordSpecNames;
        public List<string[]> DefectList;

        public DefectRecordSpecRecord(string count)
        {
            Count = Int32.Parse(count);
            DefectRecordSpecNames = new string[Count];
            DefectList = new List<string[]>();
        }

        public Dictionary<string, Type> DefectRecordSpecValueTypes = new Dictionary<string, Type>() 
        {
            {"DEFECTID", typeof(Int32)},
            {"X", typeof(Single)},
            {"Y", typeof(Single)},
            {"XREL", typeof(Single)},
            {"YREL", typeof(Single)},
            {"XINDEX", typeof(Int32)},
            {"YINDEX", typeof(Int32)},
            {"XSIZE", typeof(Single)},
            {"YSIZE", typeof(Single)},
            {"DEFECTAREA", typeof(Single)},
            {"DSIZE", typeof(Single)},
            {"CLASSNUMBER", typeof(Int32)},
            {"TEST", typeof(Int32)},
            {"IMAGECOUNT", typeof(Int32)},
            {"IMAGELIST", typeof(ImageListRecord)},
            {"CLUSTERNUMBER", typeof(Int32)},
            {"REVIEWSAMPLE", typeof(Int32)},
            {"ROUGHBINNUMBER", typeof(Int32)},
            {"FINEBINNUMBER", typeof(Int32)}
        };

        public override string ToString()
        {
            string s = string.Format("DefectRecordSpec {0} ", Count);
            for (int i = 0; i < Count; i++)
            {
                s = s + DefectRecordSpecNames[i] + " ";
            }
            s = s.Trim() + ";\n\rDefectList ";
            foreach(string[] d in DefectList)
            {
                for (int i = 0; i < Count; i++)
                {
                    Type t = DefectRecordSpecValueTypes[DefectRecordSpecNames[i].ToUpper()];

                    if (t == typeof(Int32) || t == typeof(Int16) || t == typeof(String))
                    {
                        s = s + d[i] + " ";
                    }
                    else
                    {
                        if (t == typeof(Single) || t == typeof(Double))
                        {
                            s = s + String.Format("{0:f3} ", Double.Parse(d[i]));
                        }
                    }
                }
                s = s.Trim() + "\n\r";
            }
            s = s.Trim() + ";\n\r";
            return s;
        }
    }

    public class ImageListRecord
    {
        public Int32 Count;
        public Int32 ImageNumber;
        public Int32 ImageType;

        public override string ToString()
        {
            return String.Format("{0} {1} {2}\n\r", Count, ImageNumber, ImageType);
        }
    }

    public class TestParameterSpecRecord
    {
        public Int32 Count;
        public string[] TestParameterSpecNames;
        public string[] TestParameterSpecValues;

        public TestParameterSpecRecord(string count)
        {
            Count = Int32.Parse(count);
            TestParameterSpecNames = new string[Count];
            TestParameterSpecValues = new string[Count];
        }

        public Dictionary<string, Type> TestParameterSpecValueTypes = new Dictionary<string, Type>() 
        {
            {"PIXELSIZE", typeof(Single)},
            {"INSPECTIONMODE", typeof(Int16)},
            {"SAMPLEPERCENTAGE", typeof(Single)}
        };

        public override string ToString()
        {
            string s = string.Format("DefectClusterSpec {0} ", Count);
            for (int i = 0; i < Count; i++)
            {
                s = s + TestParameterSpecNames[i] + " ";
            }
            s = s + "\n\rDefectClusterSetup ";
            for (int i = 0; i < Count; i++)
            {
                Type t = TestParameterSpecValueTypes[TestParameterSpecNames[i].ToUpper()];

                if (t == typeof(Int32) || t == typeof(Int16) || t == typeof(String))
                {
                    s = s + TestParameterSpecValues[i] + " ";
                }
                else
                {
                    if (t == typeof(Single) || t == typeof(Double))
                    {
                        s = s + String.Format("{0:f3} ", Double.Parse(TestParameterSpecValues[i]));
                    }
                }
            }
            s = s.Trim() + ";\n\r";
            return s;
        }
    }

    public class SampleTestPlanRecord
    {
        public Int32 Count;
        public List<Die> DieList;

        public SampleTestPlanRecord(string count)
        {
            Count = Int32.Parse(count);
            DieList = new List<Die>();
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("SampleTestPlanRecord {0}\n\r", Count));

            foreach (Die d in DieList)
            {
                s.Append(d.ToString());
            }

            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class SampleTestReferencePlanRecord
    {
       public Int32 Count;
       public Die[] SampleDie;
       public Die[] CompareDie;

        public SampleTestReferencePlanRecord(string count)
        {
            Count = Int32.Parse(count);
            SampleDie = new Die[Count];
            CompareDie = new Die[Count];
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("SampleTestReferencePlan {0}\n\r", Count));

            for (int i = 0; i < Count; i++)
            {
                s.Append(SampleDie[i].ToString().Trim() + " " + CompareDie[i].ToString());
            }

            return s.ToString().Trim() + ";\n\r";
        }
    }

    public class InspectedAreaOriginRecord
    {
        public Single X;
        public Single Y;

        public override string ToString()
        {
            return String.Format("{0} {1}\n\r", X, Y);
        }
    }

    public class InspectedAreaRecord
    {
        public Int32 Count;
        public InspectedArea[] InspectedAreas;

        public InspectedAreaRecord(string count)
        {

            Count = Int32.Parse(count);
            InspectedAreas = new InspectedArea[Count];
        }
    }

    public class InspectedArea
    {
        public Single XOffset;
        public Single YOffset;
        public Single XSize;
        public Single YSize;
        public Int32 XRepeat;
        public Int32 YRepeat;
        public Single XPitch;
        public Single YPitch;

        public override string ToString()
        {
            return String.Format("{0:f3} {1:f3} {2:f3} {3:f3} {4:f3} {5} {6:f3} {7:f3}\n\r", XOffset, YOffset, XSize, YSize, XRepeat, YRepeat, XPitch, YPitch);
        }

    }

    public class KLARF
    {
        public FileVersionRecord FileVersion;
        public FileTimestampRecord FileTimestamp;
        public TiffSpecRecord TiffSpec;
        public InspectionStationRecord InspectionStationID;
        public string SampleType;
        public ResultTimestampRecord ResultTimeStamp;
        public string ResultsID;
        public List<LotRecord> Lots;
    }

    public class LotStatusRecord
    {
        public Int32 WafersPassed;
        public Int32 WafersFailed;
        public Int32 WafersTested;

        public override string ToString()
        {
            return String.Format("LotStatus {0} {1} {2};\n\r", WafersPassed, WafersFailed, WafersTested);
        }
    }

    public class KLARFRecord
    {
        public string Tag;
        public List<string> DataFields;

        public KLARFRecord()
        {
            DataFields = new List<string>();
        }

        public string this[int index]
        {
            get
            {
                return DataFields[index];
            }
        }
    }

    public class KLARFParser
    {
        private FileStream _file;
        private readonly char _record_delimiter = ';';
        private KLARFRecord _current_record;
        private LotRecord _current_lot;
        private WaferRecord _current_wafer;
        private InspectionTestRecord _current_inspection_test;
        // private DefectClusterSpecRecord _currentDefectClusterSpec;
        // private TestParameterSpecRecord _currentTestParameterSpec;
        // private SummarySpecRecord _currentSummarySpec;
        private Dictionary<string, MethodInfo> _handlers = new Dictionary<string,MethodInfo>();

        public KLARFParser()
        {
            foreach(MethodInfo mi in this.GetType().GetMethods())
            {
                if(mi.Name.StartsWith("Handle"))
                {
                    _handlers.Add(mi.Name.ToUpper(), mi);
                }
            }
        }

        /// <summary>
        /// Parses a KLARF file
        /// </summary>
        /// <param name="path">Path, including the filename with extension, of the file on disk to parse.</param>
        /// <returns>KLARF object.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public KLARF ReadFile(string path)
        {
            KLARF kfile = new KLARF();
            string handlerName;

            try
            {
                _file = File.OpenRead(path);
            }
            catch 
            {
                return null;
            }

            while (GetNextRecord())
            {
                if (_current_record.Tag.ToUpper() == "ENDOFFILE")  // Check for end of file record
                {
                    break;
                }

                // Generate the handler method name by appending "Handle" to the name of the record found.
                handlerName = "Handle" + _current_record.Tag;

                // If a method matching the Handlexxxxx specification exists, then...
                if(_handlers.ContainsKey(handlerName.ToUpper()))
                {
                    // ...invoke the handler method, passing the target object and the current record
                    _handlers[handlerName].Invoke(this, new object[]{kfile, _current_record});
                }
            }

            return kfile;
        }

        protected void WriteFile(string path)
        {
            _file = File.Open(path, FileMode.Create);
        }


        /// <summary>
        /// Gets the next record in the file.  In a KLARF file, records begin with a record name and end with a semicolon ';'.
        /// Values are delimited by whitespace (space, tab, carriage return, linefeed).
        /// </summary>
        /// <returns>True if the next record was placed into the record buffer.  False if the end of the file was reached.</returns>
        protected bool GetNextRecord()
        {
            bool _quoteState;
            bool _tagFound;
            KLARFRecord rec = new KLARFRecord();
            StringBuilder token = new StringBuilder();
            Char b;
            string ch;

            if (_file == null)
            {
                return false;
            }

            _tagFound = false;
            _quoteState = false;

            b = (Char)_file.ReadByte();
            while (b > 0)
            {
                ch = Char.ConvertFromUtf32(b);

                if (b == '"') // Check for quoted strings
                {
                    if (_quoteState)  // If inside a quote, then end the quote
                    {
                        _quoteState = false;
                        rec.DataFields.Add(token.ToString());  // Add string to the record
                    }
                    else
                    {
                        _quoteState = true;
                    }
                }

                if (_quoteState)  // If inside of a quote, then append the character to the current token
                {
                    token.Append(b);
                    continue;
                }

                // Skip whitespace
                if(char.IsWhiteSpace(b))
                {
                    // If current token has characters in it, then save it.
                    if(token.Length == 0)
                    {
                        if (!_tagFound)
                        {
                            rec.Tag = token.ToString();   // If first token then set the tag as the token
                            _tagFound = true;
                        }
                        else
                        {
                            rec.DataFields.Add(token.ToString());  // If not the first token, then add to the datafiles collection
                        }
                    }

                    continue;
                }

                if (b == _record_delimiter)  // End of record
                {
                    // If current token has characters in it, then save it.
                    if (token.Length == 0)
                    {
                        if (!_tagFound)
                        {
                            rec.Tag = token.ToString();   // If first token then set the tag as the token
                            _tagFound = true;
                        }
                        else
                        {
                            rec.DataFields.Add(token.ToString());  // If not the first token, then add to the datafiles collection
                        }
                    }
                    break;  // End while loop and return record
                }
            }

            _current_record = rec;

            if (b > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

#region Handlers for each defined Record Type in the KLARF Specification

        protected void HandleFileVersion(KLARF kfile, KLARFRecord record)
        {
            kfile.FileVersion.Major = int.Parse(record[0]);
            kfile.FileVersion.Minor = int.Parse(record[1]);
        }

        protected void HandleFileTimestamp(KLARF kfile, KLARFRecord record)
        {
            kfile.FileTimestamp.Date = record[0];
            kfile.FileTimestamp.Time = record[1];
        }

        protected void HandleTiffSpec(KLARF kfile, KLARFRecord record)
        {
            kfile.TiffSpec.TiffVersion = record[0];
            kfile.TiffSpec.TiffAlignmentClass = record[1];
            kfile.TiffSpec.TiffImageClass = record[2];
        }

        protected void InspectionStationID(KLARF kfile, KLARFRecord record)
        {
            kfile.InspectionStationID.ManufacturerID = record[0];
            kfile.InspectionStationID.Model = record[1];
            kfile.InspectionStationID.EquipmentID = record[2];
        }

        protected void HandleSampleType(KLARF kfile, KLARFRecord record)
        {
            kfile.SampleType = record[0];
        }

        protected void HandleResultTimestamp(KLARF kfile, KLARFRecord record)
        {
            kfile.ResultTimeStamp.Date = record[0];
            kfile.ResultTimeStamp.Time = record[1];
        }

        protected void HandleResultsID(KLARF kfile, KLARFRecord record)
        {
            kfile.ResultsID = record[0];
        }

        protected void HandleLotID(KLARF kfile, KLARFRecord record)
        {
            _current_lot = new LotRecord();
            kfile.Lots.Add(_current_lot);
            _current_lot.LotID = record[0];
        }

        protected void HandleSampleSize(KLARF kfile, KLARFRecord record)
        {
            if (_current_lot == null)
            {
                _current_lot = new LotRecord();
            }

            _current_lot.SampleSize.ShapeType = int.Parse(record[0]);
            _current_lot.SampleSize.Dimension1 = int.Parse(record[1]);
            if (_current_lot.SampleSize.ShapeType == 2)
            {
                _current_lot.SampleSize.Dimension2 = int.Parse(record[2]);
            }
        }

        protected void HandleSetupID(KLARF kfile, KLARFRecord record)
        {
            _current_lot.SetupID = new SetupIDRecord
            {
                Name = record[0],
                Date = record[1],
                Time = record[2]
            };
        }

        protected void HandleDeviceID(KLARF kfile, KLARFRecord record)
        {
            _current_lot.DeviceID = record[0];
        }

        protected void HandleStepID(KLARF kfile, KLARFRecord record)
        {
            _current_lot.StepID = record[0];
        }

        protected void HandleSampleOrientationMarkType(KLARF kfile, KLARFRecord record)
        {
            _current_lot.SampleOrientationMarkType = record[0];
        }

        protected void HandleOrientationMarkLocation(KLARF kfile, KLARFRecord record)
        {
            _current_lot.StepID = record[0];
        }

        protected void HandleDiePitch(KLARF kfile, KLARFRecord record)
        {
            _current_lot.DiePitch.X = Single.Parse(record[0]);
            _current_lot.DiePitch.Y = Single.Parse(record[1]);
        }

        protected void HandleDieOrigin(KLARF kfile, KLARFRecord record)
        {
            _current_lot.DieOrigin.X = Single.Parse(record[0]);
            _current_lot.DieOrigin.Y = Single.Parse(record[1]);
        }

        protected void HandleWaferID(KLARF kfile, KLARFRecord record)
        {
            _current_wafer = new WaferRecord();
            _current_lot.Wafers.Add(_current_wafer);
            _current_wafer.WaferID = record[0];
        }

        protected void HandleSlot(KLARF kfile, KLARFRecord record)
        {
            _current_wafer.SlotID = Int16.Parse(record[0]);
        }

        protected void HandleSampleCenterLocation(KLARF kfile, KLARFRecord record)
        {
            _current_wafer.SampleCenterLocation.X = float.Parse(record[0]);
            _current_wafer.SampleCenterLocation.Y = float.Parse(record[1]);
        }

        protected void HandleClassLookup(KLARF kfile, KLARFRecord record)
        {
            ClassLookupRecord clr = new ClassLookupRecord();
            DefectClass dcl;

            _current_wafer.ClassLookup = clr;

            clr.Count = Int32.Parse(record[0]);
            clr.ClassList = new List<DefectClass>();

            for(int i=0;i<clr.Count;i++)
            {
                dcl = new DefectClass
                {
                    ClassNumber = Int32.Parse(record[2 * i + 1]),
                    ClassName = record[2 * i + 2]
                };
                clr.ClassList.Add(dcl);
            }
        }

        protected void HandleSampleDieMap(KLARF kfile, KLARFRecord record)
        {
            SampleDieMapRecord sdm = new SampleDieMapRecord(record[0]);
            Die d;

            _current_wafer.SampleDieMap = sdm;

            for (int i = 0; i < sdm.Count; i++)
            {
                d = new Die
                {
                    X = Int32.Parse(record[2 * i + 1]),
                    Y = Int32.Parse(record[2 * i + 2])
                };
                sdm.DieList.Add(d);
            }
        }

        protected void HandleInspectionTest(KLARF kfile, KLARFRecord record)
        {
            _current_inspection_test = new InspectionTestRecord();
            _current_wafer.InspectionTests.Add(_current_inspection_test);
            _current_inspection_test.InspectionTest = Int32.Parse(record[0]);
        }

        protected void HandleSampleTestPlan(KLARF kfile, KLARFRecord record)
        {
            SampleTestPlanRecord stp = new SampleTestPlanRecord(record[0]);
            Die d;

            for (int i = 0; i < stp.Count; i++)
            {
                d = new Die
                {
                    X = Int32.Parse(record[2 * i + 1]),
                    Y = Int32.Parse(record[2 * i + 2])
                };
                stp.DieList.Add(d);
            }
        }

        protected void HandleAreaPerTest(KLARF kfile, KLARFRecord record)
        {
            _current_inspection_test.AreaPerTest = Single.Parse(record[0]);
        }

        protected void HandleInspectedAreaOrigin(KLARF kfile, KLARFRecord record)
        {
            _current_inspection_test.InspectedAreaOrigin.X = Int32.Parse(record[0]);
            _current_inspection_test.InspectedAreaOrigin.Y = Int32.Parse(record[1]);
        }

        protected void HandleInspectedArea(KLARF kfile, KLARFRecord record)
        {
            InspectedAreaRecord iar = new InspectedAreaRecord(record[0]);
            InspectedArea ia;

            _current_inspection_test.InspectedAreas = iar;

            for (int i = 0; i < iar.Count; i++)
            {
                ia = new InspectedArea();
                iar.InspectedAreas[i] = ia;
                ia.XOffset = Single.Parse(record[i * 8 + 1]);
                ia.YOffset = Single.Parse(record[i * 8 + 2]);
                ia.XSize = Single.Parse(record[i * 8 + 3]);
                ia.YSize = Single.Parse(record[i * 8 + 4]);
                ia.XRepeat = Int32.Parse(record[i * 8 + 5]);
                ia.YRepeat = Int32.Parse(record[i * 8 + 6]);
                ia.XPitch = Single.Parse(record[i * 8 + 7]);
                ia.YPitch = Single.Parse(record[i * 8 + 8]);
            }
        }

        protected void HandleTestParametersSpec(KLARF kfile, KLARFRecord record)
        {
            TestParameterSpecRecord tpr = new TestParameterSpecRecord(record[0]);

            _current_inspection_test.TestParametersSpec = tpr;

            for (int i = 0; i < tpr.Count; i++)
            {
                tpr.TestParameterSpecNames[i] = record[i + 1];
            }
        }

        protected void HandleTestParametersList(KLARF kfile, KLARFRecord record)
        {
            if (_current_inspection_test.TestParametersSpec == null)
            {
                return;
            }

            for (int i = 0; i < _current_inspection_test.TestParametersSpec.Count; i++)
            {
                _current_inspection_test.TestParametersSpec.TestParameterSpecValues[i] = record[i + 1];
            }
        }

        protected void HandleDefectClusterSpec(KLARF kfile, KLARFRecord record)
        {
            DefectClusterSpecRecord dcs = new DefectClusterSpecRecord(record[0]);

            _current_wafer.DefectClusterSpecs = dcs;

            for (int i = 0; i < dcs.Count; i++)
            {
                dcs.ClusterSpecNames[i] = record[i + 1];
            }
        }

        protected void HandleDefectClusterSetup(KLARF kfile, KLARFRecord record)
        {
            if (_current_wafer.DefectClusterSpecs == null)
            {
                return;
            }

            for (int i = 0; i < _current_wafer.DefectClusterSpecs.Count; i++)
            {
                _current_wafer.DefectClusterSpecs.ClusterSpecValues[i] = record[i + 1];
            }
        }

        protected void HandleDefectRecordSpec(KLARF kfile, KLARFRecord record)
        {
            DefectRecordSpecRecord drs = new DefectRecordSpecRecord(record[0]);

            _current_inspection_test.DefectRecordSpecs = drs;

            for (int i = 0; i < drs.Count; i++)
            {
                drs.DefectRecordSpecNames[i] = record[i+1];
            }
        }

        protected void HandleDefectList(KLARF kfile, KLARFRecord record)
        {
            string[] dr;
            int count;
            
            if (_current_inspection_test.DefectRecordSpecs == null)
            {
                return;
            }

            count = record.DataFields.Count() / _current_inspection_test.DefectRecordSpecs.Count;

            for (int i = 0; i < count; i++)
            {
                dr = new string[_current_inspection_test.DefectRecordSpecs.Count];
                _current_inspection_test.DefectRecordSpecs.DefectList.Add(dr);
                for (int j = 0; j < _current_inspection_test.DefectRecordSpecs.Count; j++)
                {
                    dr[j] = record[i * _current_inspection_test.DefectRecordSpecs.Count+j];
                }
            }
        }

        protected void HandleSummarySpec(KLARF kfile, KLARFRecord record)
        {
            SummarySpecRecord ssr = new SummarySpecRecord(record[0]);

            _current_wafer.SummarySpecs = ssr;

            for (int i = 0; i < ssr.Count; i++)
            {
                ssr.Names[i] = record[i + 1];
            }
        }

        protected void HandleSummaryList(KLARF kfile, KLARFRecord record)
        {
            string[] sl;
            int count;

            if (_current_wafer.SummarySpecs == null)
            {
                return;
            }

            count = record.DataFields.Count() / _current_wafer.SummarySpecs.Count;

            for (int i = 0; i < count; i++)
            {
                sl = new string[_current_wafer.SummarySpecs.Count];
                _current_wafer.SummarySpecs.SummarySpecList.Add(sl);
                for (int j = 0; j < _current_wafer.SummarySpecs.Count; j++)
                {
                    sl[j] = record[i * _current_wafer.SummarySpecs.Count + j];
                }
            }
        }

        protected void HandleClusterClassificationList(KLARF kfile, KLARFRecord record)
        {
            ClusterClassificationListRecord ccl = new ClusterClassificationListRecord();
            ClusterClassificationRecord ccr;

            _current_wafer.ClusterClassificationList = ccl;
             
            ccl.Count = Int32.Parse(record[0]);

            for (int i = 0; i < ccl.Count; i++)
            {
                ccr = new ClusterClassificationRecord
                {
                    ClusterNumber = Int32.Parse(record[2 * i + 1]),
                    Class = Int32.Parse(record[2 * i + 2])
                };
            }
        }

        protected void HandleWaferStatus(KLARF kfile, KLARFRecord record)
        {
            _current_lot.WaferStatus = record[0];
        }

        protected void HandleLotStatus(KLARF kfile, KLARFRecord record)
        {
            _current_lot.LotStatus = new LotStatusRecord
            {
                WafersPassed = Int32.Parse(record[0]),
                WafersFailed = Int32.Parse(record[1]),
                WafersTested = Int32.Parse(record[2])
            };
        }

#endregion

    }

}
