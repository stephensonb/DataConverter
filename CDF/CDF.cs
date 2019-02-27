using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Tronics.DataConverter;

namespace Tronics.DataConverter.CDF
{
    // Converted from original CDF source code by Bob Farrier/Daris Nevil - February 14, 1989
    //
    // Tronics MEMS, Inc.
    //
    // C# CDF Reader/Writer implementation
    //
    // Brian Stephenson, June 4, 2014
    //
    //
    
    /// <summary>
    /// CDF - Compressed Data Format Class
    /// </summary>
    public class CDF : BinaryFormatter 
    {
        protected const Int32 NOMEAS = 0xFFFF;

        public Hdc Header;
        public List<Bth> Batches;
        public List<Crr> CorrelationResults;
        public List<Bdc> BinDescriptions;
        public List<Tdc> TestDescriptions;
        public List<DeviceResult> DieResults;

        public CDF()
        {
            Header = new Hdc();
            Batches = new List<Bth>();
            CorrelationResults = new List<Crr>();
            TestDescriptions = new List<Tdc>();
            DieResults = new List<DeviceResult>();
            BinDescriptions = new List<Bdc>();
        }

        /// <summary>
        /// Initializes the CDF object, clears all records.
        /// </summary>
        public void Initialize()
        {
            Batches.Clear();
            CorrelationResults.Clear();
            TestDescriptions.Clear();
            DieResults.Clear();
            BinDescriptions.Clear();
            Header = new Hdc();
        }

        /// <summary>
        /// Retrieves a die result based on the die index number
        /// </summary>
        /// <param name="die">The index number of the die result to retrieve</param>
        /// <returns>DeviceResult or null if the index is not found in the die result list</returns>
        public DeviceResult GetDieResult(UInt32 die)
        {
            return DieResults.FirstOrDefault(x => x.DieDescription.dienum == die);
        }

        /// <summary>
        /// Sets the current state of the object from the data read from a file.
        /// </summary>
        /// <param name="path">Path, including the file name, on disk to read the data from.</param>
        /// <returns><b>True</b> if the read was successful, <b>False</b> if not.</returns>
        /// <example>
        /// <code>
        /// 
        /// CDF myCDF = new CDF();  // Create a new CDF object.
        /// myCDF.ReadFile("c:\12345.D00");  // Reads the CDF file from the disk and sets the object state for myCDF
        /// 
        /// </code>
        /// </example>
        public bool ReadFile(string path)
        {
            FileStream fs;
            DeviceResult die;

            try
            {
                if (File.Exists(path))
                {
                    using (fs = File.OpenRead(path))
                    {
                        Console.Write("Parsing CDF File...");

                        // Deserialize the header
                        Header.Deserialize(fs);

                        // Deserialize the batches
                        for (int i = 0; i < Header.bths; i++)
                        {
                            Bth b = new Bth();
                            b.Deserialize(fs);
                            Batches.Add(b);
                        }

                        // Deserialize the CorrelationResult Records
                        for (int i = 0; i < Header.crrs; i++)
                        {
                            Crr c = new Crr();
                            c.Deserialize(fs);
                            CorrelationResults.Add(c);
                        }

                        // Read in the bin result summary records
                        for (int i = 0; i < Header.bdcs; i++)
                        {
                            Bdc b = new Bdc();
                            b.Deserialize(fs);
                            BinDescriptions.Add(b);
                        }

                        // Read in the test description records
                        for (int i = 0; i < Header.tdcs; i++)
                        {
                            Tdc t = new Tdc();
                            t.Deserialize(fs);
                            TestDescriptions.Add(t);
                        }

                        // Read in the device records and measurement results
                        for (UInt32 i = 0; i < Header.ddcs; i++)
                        {
                            die = new DeviceResult();

                            die.DieDescription.Deserialize(fs);

                            for (int j = 0; j < Header.trcs; j++)
                            {
                                Trc tm = new Trc();
                                tm.Deserialize(fs, TestDescriptions[j]);
                                die.TestMeasurements.Add(tm);
                            }
                            DieResults.Add(die);
                        }
                    } // End of using block, close file and release resources

                    Console.WriteLine(string.Format("Done.  {0} die processed." , Header.ddcs));

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// Writes the current state of the object to a file on disk.
        /// </summary>
        /// <param name="path">Path, including the file name, on disk to write to.  If the file does not exist it will be created.  If it does exist, it will be overwritten.</param>
        /// <returns><b>True</b> if the write was successful, <b>False</b> if not.</returns>
        /// <example>
        /// <code>
        /// 
        /// CDF myCDF = new myCDF(); // Create new CDF
        /// myCDF.ReadFile("c:\12345.D00"); // Read CDF data
        /// 
        /// // Now write the CDF file back to disk.
        /// if (myCDF.WriteFile("c:\cdfdir"))
        /// {
        ///     // Write succeeded.
        /// } else
        /// {
        ///     // Write failed.
        /// }
        ///         /// 
        /// </code>
        /// </example>
        public bool WriteFile(string path)
        {
            FileStream fs;
            DeviceResult die;

            try
            {
                fs = File.Open(path, FileMode.Create);

                // Update the header counters with the current state of the other records
                Header.bdcs = (Int16)BinDescriptions.Count();
                Header.bths = (Int16)Batches.Count();
                Header.crrs = (Int16)CorrelationResults.Count();
                Header.ddcs = (UInt16)DieResults.Count();
                Header.tdcs = (Int16)TestDescriptions.Count();
                if (Header.ddcs > 0)
                {
                    Header.trcs = (Int16)DieResults[0].TestMeasurements.Count();
                }
                else
                {
                    Header.trcs = 0;
                }

                Console.Write(string.Format("Outputting CDF File.  {0} die to process...", Header.ddcs));

                // Serialize this object all at once
                this.Serialize(fs);

                // Write device records and measurement results
                for (UInt32 i = 0; i < Header.ddcs; i++)
                {
                    die = DieResults[(int)i];

                    die.Serialize(fs);

                    for (int j = 0; j < Header.trcs; j++)
                    {
                        die.TestMeasurements[j].Serialize(fs, TestDescriptions[j]);
                    }
                }
                Console.WriteLine("Done.");
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Takes a UDF object as input and generates a new CDF object based on the state of the UDF object.
        /// </summary>
        /// <param name="source">The UDF object to generate the CDF object from.</param>
        /// <returns>New CDF object or null if an error occurred.</returns>
        /// <example>
        /// <code>
        /// 
        /// UDF myUDF = new UDF();  // Create a UDF file.
        /// myUDF.ReadFile("c:\12345.UDF"); // Read the UDF file in.
        /// 
        /// CDF myCDF = CDF.ParseUDF(myUDF); // Generate a CDF file from myUDF.  Note, ParseUDF is a static function of the CDF type.
        /// myCDF.WriteFile("c:\1234.CDF");  // Write out the newly generated CDF file.
        /// 
        /// </code>
        /// </example>
        public static CDF ParseUDF(UDF.UDF source)
        {
            CDF newCDF = new CDF();

            // Map the UDF header to the CDF Header
            newCDF.Header.swtyp = source.Header.swtyp;
            newCDF.Header.swver = source.Header.swver;
            newCDF.Header.swname = source.Header.swname;
            newCDF.Header.swdate = source.Header.swdate;
            newCDF.Header.statnum = source.Header.statnum;
            newCDF.Header.starttime = source.Header.starttime;
            newCDF.Header.reserved = source.Header.reserved;
            newCDF.Header.partfmly = source.Header.partfmly;
            newCDF.Header.partdesig = source.Header.partdesig;
            newCDF.Header.operators = source.Header.operators;
            newCDF.Header.mode = source.Header.mode;
            newCDF.Header.tstrnum = source.Header.tstrnum;
            newCDF.Header.tstrtyp = source.Header.tstrtyp;
            newCDF.Header.usvn = source.Header.usvn;
            newCDF.Header.wlot = source.Header.wlot;
            newCDF.Header.wnum = source.Header.wnum;
            newCDF.Header.workorder = source.Header.workorder;
            newCDF.Header.xcen = source.Header.xcen;
            newCDF.Header.xdim = source.Header.xdim;
            newCDF.Header.xlocmax = source.Header.xlocmax;
            newCDF.Header.xlocmin = source.Header.xlocmin;
            newCDF.Header.ycen = source.Header.ycen;
            newCDF.Header.ydim = source.Header.ydim;
            newCDF.Header.ylocmax = source.Header.ylocmax;
            newCDF.Header.ylocmin = source.Header.ylocmin;
            newCDF.Header.flat = source.Header.flat;
            newCDF.Header.finishtime = source.Header.finishtime;
            newCDF.Header.future = source.Header.future;
            newCDF.Header.datecode = source.Header.datecode;
            newCDF.Header.customer = source.Header.customer;
            newCDF.Header.catlist = source.Header.catlist;
            newCDF.Header.bths = source.Header.bths;
            newCDF.Header.bdcs = source.Header.bsrs;
            newCDF.Header.cdfver = "C106";
            newCDF.Header.csvn = "PP";
            newCDF.Header.hdcs = 1;

            // Map the batches
            foreach(UDF.Batch b in source.Batches)
            {
                Bth nb = new Bth
                {
                    future = b.future,
                    lot = b.lot,
                    sublot = b.sublot
                };
                newCDF.Batches.Add(nb);
            }

            // Map UDF Correlation records to CDF correlation records
            foreach(UDF.Crres cr in source.CorrelationResults)
            {
                Crr nr = new Crr
                {
                    testname = cr.testname,
                    testdate = cr.testdate,
                    sernum = cr.sernum,
                    partdesig = cr.partdesig,
                    idealval = cr.idealval,
                    future = cr.future,
                    deltamin = cr.deltamin,
                    deltamax = cr.deltamax,
                    actualval = cr.actualval
                };
                newCDF.CorrelationResults.Add(nr);
            }

            // Map UDF Bin Result Records to CDF Bin Result Records
            foreach(UDF.Bnsum b in source.BinResultSummaries)
            {
                Bdc nb = new Bdc
                {
                    binnum = b.binnum,
                    binname = b.binname,
                    bincnt = b.bincnt,
                    future = b.future
                };
                newCDF.BinDescriptions.Add(nb);
            }

            // Map the test description records
            foreach(UDF.Tstdr td in source.TestDescriptions)
            {
                Tdc nt = new Tdc();

                // Map the test summary data
                UDF.Trsum ts = source.TestResultSummaries.FirstOrDefault(tx => tx.testnum == td.testnum);
                if (ts!=null)
                {
                    nt.execcnt = ts.execcnt;
                    nt.failcnt = ts.failcnt;
                    nt.failcnt2 = ts.failcnt2;
                }

                // Map the test description info
                nt.datasizeb = td.datasizeb;
                nt.datatype = td.datatype;
                nt.future = td.future;
                nt.hilimit = td.hilimit;
                nt.lolimit = td.lolimit;
                nt.anomfltrhi = td.fltrhi;
                nt.anomfltrlo = td.fltrlo;
                nt.seqname = td.seqname;
                nt.statname = td.statname;
                nt.testname = td.testname;
                nt.testnum = td.testnum;
                nt.units = td.units;
                newCDF.TestDescriptions.Add(nt);
            }

            // Map the device records
            foreach(UDF.DeviceDie dd in source.DieResults)
            {
                DeviceResult nd = new DeviceResult();

                nd.DieDescription.bincode = dd.DieDescription.bincode;
                nd.DieDescription.dienum = dd.DieDescription.dienum;
                nd.DieDescription.xpos = dd.DieDescription.xpos;
                nd.DieDescription.ypos = dd.DieDescription.ypos;

                // Note: Results value is not mapped.  Zero value is stored.  Only the bin codes
                // are mapped.  This routine does not perform float value packing like UDF2CDF.  Use
                // the UDF2CDF file to properly process a UDF file into a CDF file.
                foreach(UDF.Tmeas tm in dd.TestMeasurements)
                {
                    Trc t = new Trc();
                    t.SetResult("CF", (Int32)0);
                }
                newCDF.DieResults.Add(nd);
            }

            return newCDF;
        }
    }
}


