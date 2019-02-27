/**
 * DataConverter
 *      
 *      Program to convert various semiconductor data formats from one to the other, where conversion is possible.
 * 
 *      File types to convert / generate:  UDF, CDF, KLARF, SINF
 *      
 *      Available conversions:
 *          UDF -> SINF OR CDF
 *          CDF -> SINF OR UDF
 *          KLARF -> SINF
 *
 *      Usage:
 *      
 *      dataconverter --i ""[input file path]"" --f [input format] --o ""[output directory path]"" --t [output format]
 *      
 *      Brian Stephenson, 7/31/2014
 *      
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using Tronics.DataConverter.UDF;
using Tronics.DataConverter.CDF;
using Tronics.DataConverter.KLARF;
using Tronics.DataConverter.SINF;

namespace Tronics
{
    static class DataCvt
    {

        private static Dictionary<string,string> _options = new Dictionary<string,string>();
        private static List<string> _flags = new List<string>();
        private static List<string> _args = new List<string>();

        private static Dictionary<string , string[]> _valid_conversions = new Dictionary<string , string[]>() {
                {"CDF", new string[] { "SINF", "UDF" }},
                {"UDF", new string[] { "SINF", "CDF" }}, 
                {"KLARF", new string[] { "SINF" }}
            };

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <example>
        /// 
        /// 
        /// </example>
        [STAThread]
        static void Main(string[] args)
        {            
            // Allowed Conversions
            //
            // CDF -> SINF
            // 

            ParseArgs(args);

            if(!TryGetOptionValue("i", out string InputFile))
            {
                PrintUsage();
                Environment.Exit(0);
            }

            if(!TryGetOptionValue("f", out string InputType))
            {
                PrintUsage();
                Environment.Exit(0);
            }

            if(!TryGetOptionValue("o", out string OutputFile))
            {
                PrintUsage();
                Environment.Exit(0);
            }

            if(!TryGetOptionValue("t", out string OutputType))
            {
                PrintUsage();
                Environment.Exit(0);
            }

            if (!_valid_conversions.ContainsKey(InputType))
            {
                Console.WriteLine(string.Format("*** ERROR: Invalid input type {0} specified." , InputType));
                PrintUsage();
                Environment.Exit(0);
            }

            if (!_valid_conversions[InputType].Contains(OutputType))
            {
                Console.WriteLine(string.Format("*** ERROR: Invalid output type {0} for input type {1} specified." , InputType, OutputType));
                Console.WriteLine(string.Format("Valid output formats for the input type {0}:" , InputType));
                foreach (string s in _valid_conversions[InputType])
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine();
                PrintUsage();
                Environment.Exit(0);
            }

            if (InputType == "UDF")
            {
                UDF newUDF = new UDF();
                newUDF.ReadFile(InputFile);

                switch (OutputType)
                {
                    case "CDF":
                        CDF.ParseUDF(newUDF).WriteFile(OutputFile);
                        break;
                    case "SINF":
                        SINF.Parse(newUDF).WriteFile(OutputFile);
                        break;
                    default:
                        PrintUsage();
                        Environment.Exit(0);
                        break;
                }

            }
            else
            {
                if (InputType == "CDF")
                {
                    CDF newCDF = new CDF();
                    newCDF.ReadFile(InputFile);

                    switch (OutputType)
                    {
                        case "UDF":
                            UDF.ParseCDF(newCDF).WriteFile(OutputFile);
                            break;
                        case "SINF":
                            SINF.Parse(newCDF).WriteFile(OutputFile);
                            break;
                        default:
                            PrintUsage();
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    if (InputType == "KLARF")
                    {
                        List<SINF> sf = new List<SINF>();
                        KLARFParser kp = new KLARFParser();
                        KLARF newKLARF = kp.ReadFile(InputFile);

                        switch (OutputType)
                        {
                            case "SINF":
                                sf = SINF.Parse(newKLARF);
                                break;
                            default:
                                PrintUsage();
                                Environment.Exit(0);
                                break;
                        }
                    }
                    else
                    {
                        PrintUsage();
                    }
                }
            }

            Environment.Exit(0);
        }

        private static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine(@"datcvt --i ""[input file path]"" --f [input format] --o ""[output directory path]"" --t [output format]");
            Console.WriteLine("");
            Console.WriteLine(@"--i [input file path]: File path of input file to convert [REQUIRED]");
            Console.WriteLine(@"--f [input format] : Format type of the input file to convert from [REQUIRED]");
            Console.WriteLine(@"--o [output path] : Directory name where to output the converted file [REQUIRED]");
            Console.WriteLine(@"--t [output format]: Format type of the output file to convert to [REQUIRED]");
            Console.WriteLine(@"");
            Console.WriteLine(@"Current valid conversions:");

            foreach (KeyValuePair<string , string[]> kv in _valid_conversions)
            {
                Console.Write(string.Format(kv.Key) + " -> ");
                string s = "";
                foreach (string o in kv.Value)
                {
                    s = s + o + ",";
                }
                Console.WriteLine(s.Trim(','));
            }

            Console.WriteLine();
            Console.WriteLine(@"NOTE: UDF->CDF and CDF->UDF do not pack/unpack results properly in this release.");
        }

        private static void ParseArgs(string[] args)
        {
            string arg;
            string argval;

            _options.Clear();
            _flags.Clear();
            _args.Clear();

            for(int i=0;i<args.Count();i++)
            {
                arg = args[i];
                if(arg.StartsWith("--"))
                {
                    if(i < args.Count()-1)
                    {
                        i++;
                        argval = args[i];
                    }
                    else
                    {
                        argval = "";
                    }

                    arg = arg.Trim('-');
                    
                    if(_options.ContainsKey(arg))
                    {
                        _options[arg] = argval;
                    }
                    else
                    {
                        _options.Add(arg, argval);
                    }
                    continue;
                }

                if(arg.StartsWith("-"))
                {
                    arg = arg.Trim('-');

                    if (!_flags.Contains(arg))
                    {
                        _flags.Add(arg);
                    }
                    continue;
                }

                _args.Add(arg);
            }
        }

        private static bool TryGetOptionValue(string option, out string val)
        {
            if(_options.ContainsKey(option))
            {
                val = _options[option];
                return true;
            }
            val = null;
            return false;
        }

        private static bool HasFlag(string flag)
        {
            if(_flags.Contains(flag))
            {
                return true;
            }

            return false;
        }

    }
}
