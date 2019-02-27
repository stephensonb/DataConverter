using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Tronics.DataConverter;

namespace Tronics.DataConverter.SINF
{
    public class Die
    {
        public int X;
        public int Y;
        public int bin;
    }

    public class BinCounter
    {
        public int count=0;
        public bool pass=true;
    }

    /// <summary>
    /// SINF - Wafer map binning information file format object.
    /// </summary>
    public class SINF 
    {
        private string _fn_loc;
        private static int _min_row;
        private static int _max_row;
        private static int _min_col;
        private static int _max_col;
        public string Device {get;set;}
        public string LotID {get; set;}
        public string WaferID {get;set;}
        public Dictionary<string, Die> DieList;
        public int Rows {get; private set;}
        public int Cols {get; private set;}
        public float XSize {get; set;}
        public float YSize {get; set;}
        public string Units {get; set;}
        public int XRef {get; set;}
        public int YRef {get; set;}
        public DateTime ProbeEndDate {get; set;}
        public Dictionary<int, BinCounter> BinCounts {get; set;}
        public SINFMapTypeOptions SINFMapType { get; set; }

        public SINF()
        {
            Device = "";
            LotID = "";
            WaferID="";
            FnLoc = "D";  // initialize to flat/notch down
            DieList = new Dictionary<string, Die>();
            BinCounts = new Dictionary<int,BinCounter>();
            Rows=0;
            Cols=0;
            XSize=0;
            YSize=0;
            Units="";
            XRef=0;
            YRef=0;
            ProbeEndDate = DateTime.Now;
            SINFMapType = SINFMapTypeOptions.BinMap;
        }

        protected string FnLoc
        {
            get
            {
                return _fn_loc;
            }

            set
            {
                _fn_loc = CoordinateMapping.NormalizeOrientation(value);
            }
        }
       
        protected void AddDie(int row, int col, int bincode, bool pass=true)
        {
            string key = string.Format("{0:d4}{1:d4}" , row , col);
            try
            {
                DieList.Add(key , new Die() { X = col , Y = row , bin = bincode });
            }
            catch 
            {
                // Comment out the following handling to NOT throw the exception.
                // Instead we will NOP this and just ignore the exception.  We will
                // only keep the first instance of a die in the list, ignoring all others.

                //Die d = DieList[key];
                //if (d.bin != bincode && d.bin != 35)
                //{
                //    throw new ArgumentException(string.Format("Duplicate key {0} found." , key));
                //}
                //else
                //{
                //    return;
                //}
            }
            //if (row > Rows)
            //{
            //    Rows = row;
            //}
            //if (col > Cols)
            //{
            //    Cols = col;
            //}

            if (!BinCounts.ContainsKey(bincode))
            {
                BinCounts.Add(bincode,new BinCounter());
            }

            BinCounts[bincode].count++;
            BinCounts[bincode].pass = pass;
        }

        /// <summary>
        /// Writes out the SINF file to disk.
        /// </summary>
        /// <param name="outputDirectoryName">The director name to write the file to.  The file name will be generated based on the lot id and the wafer name:
        /// <code>
        /// LLLLLLLWW.SINF - where 'LLLLLL' is the lot ID, 'WW' is a two digit wafer number and SINF is the file extension.
        /// </code>
        /// </param>
        public void WriteFile(string outputDirectoryName)
        {
            StreamWriter writer = null;
            string prefix = "";
            Die d;
            string goodBinList="";
            string lotname;

            // Set the base filename to the first three letters of the current year,
            // then add the CDF lot name and strip the letter from the end
            lotname = DateTime.Now.ToString("yyyy").Substring(0 , 3) + LotID.Substring(0 , LotID.Length - 1);
            
            // Open a new file for each wafer.  Overwrite it if the file already exists.
            writer = new StreamWriter(Path.Combine(outputDirectoryName, string.Format("{0} - {1:d2}.TXT", lotname, int.Parse(WaferID))));

            Console.Write(string.Format("Processing SINF file.  {0} die to process...", DieList.Count));

            // Output the header
            writer.WriteLine("DEVICE:{0}", Device);
            writer.WriteLine("LOT:{0}", lotname);
            writer.WriteLine("WAFER:{0}", WaferID);
            writer.WriteLine("FNLOC:{0:d}", CoordinateMapping.MarkToDegrees(FnLoc));
            writer.WriteLine("ROWCT:{0:d3}", Rows);
            writer.WriteLine("COLCT:{0:d3}", Cols);

            foreach(KeyValuePair<int, BinCounter> kvp in BinCounts.Where(x => x.Value.pass == true))
            {
                goodBinList = goodBinList + string.Format("{0,2:X2}", kvp.Key) + " ";
            }
            
            writer.WriteLine("BCEQU:{0}", goodBinList.Trim());
            if (XRef > -999)
            {
                writer.WriteLine("REFPX:{0}", XRef);
            }
            if (YRef > -999)
            {
                writer.WriteLine("REFPY:{0}", YRef);
            }
            writer.WriteLine("DUTMS:{0}", Units);
            writer.WriteLine("XDIES:{0:F6}", XSize);
            writer.WriteLine("YDIES:{0:F6}", YSize);
            writer.WriteLine("PROBEENDDATE:{0:yyyy-MM-dd HH:mm:ss}", ProbeEndDate);

            for(int j=_min_row;j<=_max_row;j++)
            {
                writer.Write("RowData:");
                prefix = "";
                for (int i = _min_col; i <= _max_col; i++)
                {
                    try
                    {
                        string key = string.Format("{0:d4}{1:d4}" , j , i);
                        d = DieList[key];
                    }
                    catch 
                    {
                        d = null;
                    }

                    if (d == null)
                    {
                        if(i == XRef && j == YRef)
                        {
                            writer.Write("FF");   // IF no die found in die list for the reference die, then indicate in map with FF
                        } else
                        {
                            writer.Write(prefix + "__"); // No die present represented by two underscores
                        }
                    }
                    else
                    {
                        writer.Write(String.Format("{0}{1,2:X2}",prefix,d.bin));
                    }

                    prefix=" ";
                }
                writer.WriteLine("");
            }

            if (writer != null)
            {
                writer.Close();
            }

            Console.WriteLine("Done.");
        }

        /// <summary>
        /// Parses a CDF object and generates a new SINF object based on the data from teh CDF object.
        /// </summary>
        /// <param name="source">The CDF object to generate the SINF object from</param>
        /// <returns>The newly created SINF object or <i>null</i> if the creation failed.</returns>
        /// <example>
        /// <code>
        /// CDF myCDF = new CDF();  // Create a new CDF object.
        /// 
        /// myCDF.ReadFile("c:\123213.CDF");  // Read a CDF file into the object
        /// 
        /// SINF.Parse(myCDF).WriteFile("c:\sinfdata"); // Parses the CDF file into a new object, then calls 'WriteFile' on the returned object, storing the resulting SINF file in c:\sinfdata.
        /// </code>
        /// </example>
        public static SINF Parse(CDF.CDF source)
        {
            SINF sf = new SINF();

            Console.Write(string.Format("Parsing CDF file into SINF file.  {0} die to parse...",source.Header.ddcs));

            sf.Device = source.Header.catlist;
            sf.FnLoc = source.Header.flat;
            sf.LotID = source.Header.wlot;
            sf.WaferID = source.Header.wnum;
            sf.XSize = source.Header.xdim;
            sf.YSize = source.Header.ydim;
            sf.Units = "mil";
            _min_row = source.Header.ylocmin;
            _max_row = source.Header.ylocmax;
            _min_col = source.Header.xlocmin;
            _max_col = source.Header.xlocmax;
            sf.Rows = source.Header.ylocmax - source.Header.ylocmin + 1;
            sf.Cols = source.Header.xlocmax - source.Header.xlocmin + 1;
            sf.XRef = -999;
            sf.YRef = -999;

            // Set the probe end date.  The CDF specification defines the start and finish 
            // time fields as the number of seconds since Jan 1st, 1970.
            sf.ProbeEndDate = new DateTime(1970, 1, 1).AddSeconds((double)source.Header.finishtime);

            // Go through all of the results and add the die to the SINF map.
            foreach (CDF.DeviceResult dr in source.DieResults)
            {
                // Default is that all bins with code == 1 passes.
                bool pass = (dr.DieDescription.bincode == 1);

                if (dr.DieDescription.bincode == 35)
                {
                    sf.XRef = dr.DieDescription.xpos;
                    sf.YRef = dr.DieDescription.ypos;
                }

                sf.AddDie(dr.DieDescription.ypos, dr.DieDescription.xpos, dr.DieDescription.bincode, pass);
            }

            Console.WriteLine("Done.");

            return sf;
        }

        /// <summary>
        /// Parses a UDF object and generates a new SINF object based on the data from teh UDF object.
        /// </summary>
        /// <param name="source">The UDF object to generate the SINF object from</param>
        /// <returns>The newly created SINF object or <i>null</i> if the creation failed.</returns>
        /// <example>
        /// <code>
        /// UDF myUDF = new UDF();  // Create a new UDF object.
        /// 
        /// myUDF.ReadFile("c:\123213.UDF");  // Read a UDF file into the object
        /// 
        /// SINF.Parse(myUDF).WriteFile("c:\sinfdata"); // Parses the UDF file into a new object, then calls 'WriteFile' on the returned object, storing the resulting SINF file in c:\sinfdata.
        /// </code>
        /// </example>
        public static SINF Parse(UDF.UDF source)
        {
            SINF sf = new SINF
            {
                Device = source.Header.catlist,
                FnLoc = source.Header.flat,
                LotID = source.Header.wlot,
                WaferID = source.Header.wnum,
                XSize = (float)source.Header.xdim,
                YSize = (float)source.Header.ydim,
                Units = "mil",
                XRef = -999,
                YRef = -999,

                // Set the probe end date.  The CDF specification defines the start and finish 
                // time fields as the number of seconds since Jan 1st, 1970.
                ProbeEndDate = new DateTime(1970, 1, 1).AddSeconds((double)source.Header.finishtime)
            };

            // Go through all of the results and add the die to the SINF map.
            foreach (UDF.DeviceDie dd in source.DieResults)
            {
                // Default is that all bins with code == 1 passes.
                bool pass = (dd.DieDescription.bincode == 1);

                if (dd.DieDescription.bincode == 35)
                {
                    sf.XRef = dd.DieDescription.xpos;
                    sf.YRef = dd.DieDescription.ypos;
                }

                sf.AddDie(dd.DieDescription.ypos, dd.DieDescription.xpos, dd.DieDescription.bincode, pass);
            }

            return sf;
        }
        /// <summary>
        /// Parses a KLARF object and generates a new SINF object based on the data from teh KLARF object.
        /// </summary>
        /// <param name="source">The KLARF object to generate the SINF object from</param>
        /// <returns>The newly created SINF object or <i>null</i> if the creation failed.</returns>
        /// <example>
        /// <code>
        /// KLARF myKLARF = new KLARF();  // Create a new KLARF object.
        /// 
        /// myKLARF.ReadFile("c:\123213.KLARF");  // Read a KLARF file into the object
        /// 
        /// SINF.Parse(myKLARF).WriteFile("c:\sinfdata"); // Parses the KLARF file into a new object, then calls 'WriteFile' on the returned object, storing the resulting SINF file in c:\sinfdata.
        /// </code>
        /// </example>
        public static List<SINF> Parse(KLARF.KLARF source)
        {
            int xindex=0;
            int yindex=0;
            int binindex=0;

            List<SINF> result = new List<SINF>();

            // Only process the wafers in the first lot found in the KLARF file.  Multi lot KLARF files
            // are not handled.
            foreach(KLARF.WaferRecord wr in source.Lots[0].Wafers)
            {
                SINF sf = new SINF
                {
                    Device = source.Lots[0].DeviceID,
                    FnLoc = source.Lots[0].OrientationMarkLocation,
                    LotID = source.Lots[0].LotID,
                    XSize = source.Lots[0].DiePitch.X * 1000, // KLARF units are microns, convert to mm
                    YSize = source.Lots[0].DiePitch.Y * 1000, // KLARF units are in microns, convert to mm
                    Units = "mm",
                    XRef = (int)source.Lots[0].DieOrigin.X,
                    YRef = (int)source.Lots[0].DieOrigin.Y
                };

                foreach (KLARF.InspectionTestRecord itr in wr.InspectionTests)
                {
                    for(int i=0;i<itr.DefectRecordSpecs.Count;i++)
                    {
                        if(itr.DefectRecordSpecs.DefectRecordSpecNames[i] == "XINDEX")
                        {
                            xindex = i;
                        }
                        if (itr.DefectRecordSpecs.DefectRecordSpecNames[i] == "YINDEX")
                        {
                            yindex = i;
                        }
                        if (itr.DefectRecordSpecs.DefectRecordSpecNames[i] == "CLASSNUMBER")
                        {
                            binindex = i;
                        }
                    }

                    foreach(string[] dr in itr.DefectRecordSpecs.DefectList)
                    {
                        sf.AddDie(int.Parse(dr[yindex]), int.Parse(dr[xindex]), int.Parse(dr[binindex]), false);
                    }
                }

                result.Add(sf);
            }

            return result;
        }
    }
}
