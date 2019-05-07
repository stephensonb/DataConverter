using System;
using System.IO;

namespace DataConverter.KLARF
{
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

}
