using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace DataConverter.KLARF
{
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
        public KLARFile ReadFile(string path)
        {
            KLARFile kfile = new KLARFile();
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

        protected void HandleFileVersion(KLARFile kfile, KLARFRecord record)
        {
            kfile.FileVersion.Major = int.Parse(record[0]);
            kfile.FileVersion.Minor = int.Parse(record[1]);
        }

        protected void HandleFileTimestamp(KLARFile kfile, KLARFRecord record)
        {
            kfile.FileTimestamp.Date = record[0];
            kfile.FileTimestamp.Time = record[1];
        }

        protected void HandleTiffSpec(KLARFile kfile, KLARFRecord record)
        {
            kfile.TiffSpec.TiffVersion = record[0];
            kfile.TiffSpec.TiffAlignmentClass = record[1];
            kfile.TiffSpec.TiffImageClass = record[2];
        }

        protected void InspectionStationID(KLARFile kfile, KLARFRecord record)
        {
            kfile.InspectionStationID.ManufacturerID = record[0];
            kfile.InspectionStationID.Model = record[1];
            kfile.InspectionStationID.EquipmentID = record[2];
        }

        protected void HandleSampleType(KLARFile kfile, KLARFRecord record)
        {
            kfile.SampleType = record[0];
        }

        protected void HandleResultTimestamp(KLARFile kfile, KLARFRecord record)
        {
            kfile.ResultTimeStamp.Date = record[0];
            kfile.ResultTimeStamp.Time = record[1];
        }

        protected void HandleResultsID(KLARFile kfile, KLARFRecord record)
        {
            kfile.ResultsID = record[0];
        }

        protected void HandleLotID(KLARFile kfile, KLARFRecord record)
        {
            _current_lot = new LotRecord();
            kfile.Lots.Add(_current_lot);
            _current_lot.LotID = record[0];
        }

        protected void HandleSampleSize(KLARFile kfile, KLARFRecord record)
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

        protected void HandleSetupID(KLARFile kfile, KLARFRecord record)
        {
            _current_lot.SetupID = new SetupIDRecord
            {
                Name = record[0],
                Date = record[1],
                Time = record[2]
            };
        }

        protected void HandleDeviceID(KLARFile kfile, KLARFRecord record)
        {
            _current_lot.DeviceID = record[0];
        }

        protected void HandleStepID(KLARFile kfile, KLARFRecord record)
        {
            _current_lot.StepID = record[0];
        }

        protected void HandleSampleOrientationMarkType(KLARFile kfile, KLARFRecord record)
        {
            _current_lot.SampleOrientationMarkType = record[0];
        }

        protected void HandleOrientationMarkLocation(KLARFile kfile, KLARFRecord record)
        {
            _current_lot.StepID = record[0];
        }

        protected void HandleDiePitch(KLARFile kfile, KLARFRecord record)
        {
            _current_lot.DiePitch.X = Single.Parse(record[0]);
            _current_lot.DiePitch.Y = Single.Parse(record[1]);
        }

        protected void HandleDieOrigin(KLARFile kfile, KLARFRecord record)
        {
            _current_lot.DieOrigin.X = Single.Parse(record[0]);
            _current_lot.DieOrigin.Y = Single.Parse(record[1]);
        }

        protected void HandleWaferID(KLARFile kfile, KLARFRecord record)
        {
            _current_wafer = new WaferRecord();
            _current_lot.Wafers.Add(_current_wafer);
            _current_wafer.WaferID = record[0];
        }

        protected void HandleSlot(KLARFile kfile, KLARFRecord record)
        {
            _current_wafer.SlotID = Int16.Parse(record[0]);
        }

        protected void HandleSampleCenterLocation(KLARFile kfile, KLARFRecord record)
        {
            _current_wafer.SampleCenterLocation.X = float.Parse(record[0]);
            _current_wafer.SampleCenterLocation.Y = float.Parse(record[1]);
        }

        protected void HandleClassLookup(KLARFile kfile, KLARFRecord record)
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

        protected void HandleSampleDieMap(KLARFile kfile, KLARFRecord record)
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

        protected void HandleInspectionTest(KLARFile kfile, KLARFRecord record)
        {
            _current_inspection_test = new InspectionTestRecord();
            _current_wafer.InspectionTests.Add(_current_inspection_test);
            _current_inspection_test.InspectionTest = Int32.Parse(record[0]);
        }

        protected void HandleSampleTestPlan(KLARFile kfile, KLARFRecord record)
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

        protected void HandleAreaPerTest(KLARFile kfile, KLARFRecord record)
        {
            _current_inspection_test.AreaPerTest = Single.Parse(record[0]);
        }

        protected void HandleInspectedAreaOrigin(KLARFile kfile, KLARFRecord record)
        {
            _current_inspection_test.InspectedAreaOrigin.X = Int32.Parse(record[0]);
            _current_inspection_test.InspectedAreaOrigin.Y = Int32.Parse(record[1]);
        }

        protected void HandleInspectedArea(KLARFile kfile, KLARFRecord record)
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

        protected void HandleTestParametersSpec(KLARFile kfile, KLARFRecord record)
        {
            TestParameterSpecRecord tpr = new TestParameterSpecRecord(record[0]);

            _current_inspection_test.TestParametersSpec = tpr;

            for (int i = 0; i < tpr.Count; i++)
            {
                tpr.TestParameterSpecNames[i] = record[i + 1];
            }
        }

        protected void HandleTestParametersList(KLARFile kfile, KLARFRecord record)
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

        protected void HandleDefectClusterSpec(KLARFile kfile, KLARFRecord record)
        {
            DefectClusterSpecRecord dcs = new DefectClusterSpecRecord(record[0]);

            _current_wafer.DefectClusterSpecs = dcs;

            for (int i = 0; i < dcs.Count; i++)
            {
                dcs.ClusterSpecNames[i] = record[i + 1];
            }
        }

        protected void HandleDefectClusterSetup(KLARFile kfile, KLARFRecord record)
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

        protected void HandleDefectRecordSpec(KLARFile kfile, KLARFRecord record)
        {
            DefectRecordSpecRecord drs = new DefectRecordSpecRecord(record[0]);

            _current_inspection_test.DefectRecordSpecs = drs;

            for (int i = 0; i < drs.Count; i++)
            {
                drs.DefectRecordSpecNames[i] = record[i+1];
            }
        }

        protected void HandleDefectList(KLARFile kfile, KLARFRecord record)
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

        protected void HandleSummarySpec(KLARFile kfile, KLARFRecord record)
        {
            SummarySpecRecord ssr = new SummarySpecRecord(record[0]);

            _current_wafer.SummarySpecs = ssr;

            for (int i = 0; i < ssr.Count; i++)
            {
                ssr.Names[i] = record[i + 1];
            }
        }

        protected void HandleSummaryList(KLARFile kfile, KLARFRecord record)
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

        protected void HandleClusterClassificationList(KLARFile kfile, KLARFRecord record)
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

        protected void HandleWaferStatus(KLARFile kfile, KLARFRecord record)
        {
            _current_lot.WaferStatus = record[0];
        }

        protected void HandleLotStatus(KLARFile kfile, KLARFRecord record)
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
