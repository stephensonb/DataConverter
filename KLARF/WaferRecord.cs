using System;
using System.Collections.Generic;
using System.IO;

namespace DataConverter.KLARF
{
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

}
