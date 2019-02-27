using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Tronics.DataConverter.UDF
{
    // Converted from original UDF source code by Bob Farrier/Daris Nevil - February 14, 1989
    //
    // Tronics MEMS, Inc.
    //
    // C# UDF Reader/Writer implementation
    //
    // Brian Stephenson, June 4, 2014
    //
    //

    // Object to encapsulate an entire UDF file in memory, read in from a path passed to Open
    public class UDF : BinaryFormatter
    {
        protected const Int32 NOMEAS = 0xFFFF;

        public Headr Header;
        public List<Batch> Batches;
        public List<Crres> CorrelationResults;
        public List<Tstdr> TestDescriptions;
        public List<DeviceDie> DieResults;
        public List<Trsum> TestResultSummaries;
        public List<Bnsum> BinResultSummaries;

        public UDF()
        {
            Header = new Headr();
            Batches = new List<Batch>();
            CorrelationResults = new List<Crres>();
            TestDescriptions = new List<Tstdr>();
            DieResults = new List<DeviceDie>();
            TestResultSummaries = new List<Trsum>();
            BinResultSummaries = new List<Bnsum>();
        }

        public void Initialize()
        {
            Batches.Clear();
            CorrelationResults.Clear();
            TestDescriptions.Clear();
            DieResults.Clear();
            TestResultSummaries.Clear();
            BinResultSummaries.Clear();
            Header = new Headr();
        }

        public DeviceDie GetDieResult(UInt32 die)
        {
            return DieResults.FirstOrDefault(x => x.DieDescription.dienum == die);
        }

        public static UDF ParseCDF(CDF.CDF source, bool headerOnly=true)
        {
            UDF newUDF = new UDF();

            Trsum tsum;

            // Map the CDF header to the UDF Header
            newUDF.Header.swtyp = source.Header.swtyp;
            newUDF.Header.swver = source.Header.swver;
            newUDF.Header.swname = source.Header.swname;
            newUDF.Header.swdate = source.Header.swdate;
            newUDF.Header.statnum = source.Header.statnum;
            newUDF.Header.starttime = source.Header.starttime;
            newUDF.Header.reserved = source.Header.reserved;
            newUDF.Header.partfmly = source.Header.partfmly;
            newUDF.Header.partdesig = source.Header.partdesig;
            newUDF.Header.operators = source.Header.operators;
            newUDF.Header.mode = source.Header.mode;
            newUDF.Header.tstrnum = source.Header.tstrnum;
            newUDF.Header.tstrtyp = source.Header.tstrtyp;
            newUDF.Header.usvn = source.Header.usvn;
            newUDF.Header.wlot = source.Header.wlot;
            newUDF.Header.wnum = source.Header.wnum;
            newUDF.Header.workorder = source.Header.workorder;
            newUDF.Header.xcen = source.Header.xcen;
            newUDF.Header.xdim = source.Header.xdim;
            newUDF.Header.xlocmax = source.Header.xlocmax;
            newUDF.Header.xlocmin = source.Header.xlocmin;
            newUDF.Header.ycen = source.Header.ycen;
            newUDF.Header.ydim = source.Header.ydim;
            newUDF.Header.ylocmax = source.Header.ylocmax;
            newUDF.Header.ylocmin = source.Header.ylocmin;
            newUDF.Header.flat = source.Header.flat;
            newUDF.Header.finishtime = source.Header.finishtime;
            newUDF.Header.future = source.Header.future;
            newUDF.Header.datecode = source.Header.datecode;
            newUDF.Header.customer = source.Header.customer;
            newUDF.Header.catlist = source.Header.catlist;
            newUDF.Header.bths = source.Header.bths;
            newUDF.Header.bsrs = source.Header.bdcs;
            newUDF.Header.udfver = "C106";
            newUDF.Header.usvn = "PP";

            // Map CDF Correlation records to UDF correlation records
            foreach (CDF.Crr cr in source.CorrelationResults)
            {
                Crres nr = new Crres
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
                newUDF.CorrelationResults.Add(nr);
            }

            if (!headerOnly)
            {
                // Map the CDF batches to UDF batches
                foreach (CDF.Bth b in source.Batches)
                {
                    Batch nb = new Batch
                    {
                        future = b.future,
                        lot = b.lot,
                        sublot = b.sublot
                    };
                    newUDF.Batches.Add(nb);
                }

                // Map CDF Bin Result Records to UDF Bin Result Records
                foreach (CDF.Bdc b in source.BinDescriptions)
                {
                    Bnsum nb = new Bnsum
                    {
                        binnum = b.binnum,
                        binname = b.binname,
                        bincnt = b.bincnt,
                        future = b.future
                    };
                    newUDF.BinResultSummaries.Add(nb);
                }

                tsum = null;

                // Map the test description records
                foreach (CDF.Tdc td in source.TestDescriptions)
                {
                    Tstdr nt = new Tstdr
                    {

                        // Map the test description info
                        datasizeb = td.datasizeb,
                        datatype = td.datatype,
                        future = td.future,
                        hilimit = td.hilimit,
                        lolimit = td.lolimit,
                        fltrhi = td.anomfltrhi,
                        fltrlo = td.anomfltrlo,
                        seqname = td.seqname,
                        statname = td.statname,
                        testname = td.testname,
                        testnum = td.testnum,
                        units = td.units
                    };

                    // Map the test summary data
                    if (tsum == null)
                    {
                        tsum = new Trsum
                        {
                            execcnt = td.execcnt,
                            failcnt = td.failcnt,
                            failcnt2 = td.failcnt2,
                            future = td.future
                        };
                        newUDF.TestResultSummaries.Add(tsum);
                    }
                }

                // Map the device records
                foreach (CDF.DeviceResult dd in source.DieResults)
                {
                    DeviceDie nd = new DeviceDie();
                    nd.DieDescription.bincode = dd.DieDescription.bincode;
                    nd.DieDescription.dienum = dd.DieDescription.dienum;
                    nd.DieDescription.xpos = dd.DieDescription.xpos;
                    nd.DieDescription.ypos = dd.DieDescription.ypos;
                }
            }
            return newUDF;
        }

        public static UDF ParseKLARF(KLARF.KLARF source)
        {
            // Not implemented
            throw new NotImplementedException("UDF.ParseKLARF method not implemented yet!  Gosh!!!");
        }

        public bool WriteFile(string path)
        {
            FileStream fs;
            try
            {
                fs = File.Open(path, FileMode.Create);

                // Update the header counters with the current state of the other records
                Header.bsrs = (Int16)BinResultSummaries.Count();
                Header.bths = (Int16)Batches.Count();
                Header.crrs = (Int16)CorrelationResults.Count();
                Header.ddrs = (Int16)DieResults.Count();
                Header.tdrs = (Int16)TestDescriptions.Count();
                if (Header.ddrs > 0)
                {
                    Header.trrs = (Int16)DieResults[0].TestMeasurements.Count();
                }
                else
                {
                    Header.trrs = 0;
                }
                Header.tsrs = (Int16)TestResultSummaries.Count();
                
                // Serialize this object all at once
                this.Serialize(fs);

                return true;
            }
            catch 
            {
                return false;
            }

        }

        public bool ReadFile(string path)
        {
            FileStream fs;
            DeviceDie die;

            try
            {
                if (File.Exists(path))
                {
                    fs = File.OpenRead(path);

                    // Deserialize the header
                    Header.Deserialize(fs);

                    // Deserialize the batches
                    for (int i = 0; i < Header.bths; i++)
                    {
                        Batch b = new Batch();
                        b.Deserialize(fs);
                        Batches.Add(b);
                    }

                    // Deserialize the CorrelationResult Records
                    for (int i = 0; i < Header.crrs; i++)
                    {
                        Crres c = new Crres();
                        c.Deserialize(fs);
                        CorrelationResults.Add(c);
                    }

                    // Read in the test description records
                    for (int i = 0; i < Header.tdrs; i++)
                    {
                        Tstdr t = new Tstdr();
                        t.Deserialize(fs);
                        TestDescriptions.Add(t);
                    }

                    // Read in the device records and measurement results
                    for (UInt32 i = 0; i < Header.ddrs; i++)
                    {
                        die = new DeviceDie();
                        Devdr dr = new Devdr();

                        dr.Deserialize(fs);
                        
                        die.DieDescription = dr;

                        for(int j = 0;j < Header.trrs;j++)
                        {
                            Tmeas tm = new Tmeas();
                            tm.Deserialize(fs);
                            die.TestMeasurements.Add(tm);
                        }
                        DieResults.Add(die);
                    }

                    // Read in the test result summary records
                    for (int i = 0; i < Header.tsrs; i++)
                    {
                        Trsum t = new Trsum();
                        t.Deserialize(fs);
                        TestResultSummaries.Add(t);
                    }

                    // Read in the bin result summary records
                    for (int i = 0; i < Header.bsrs; i++)
                    {
                        Bnsum b = new Bnsum();
                        b.Deserialize(fs);
                        BinResultSummaries.Add(b);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}


