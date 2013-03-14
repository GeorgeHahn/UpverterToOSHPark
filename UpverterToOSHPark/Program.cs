using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace UpverterToOSHPark
{
    public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    {
        public void Add(T1 item, T2 item2)
        {
            Add(new Tuple<T1, T2>(item, item2));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: UpverterToOSHPark [upverter_zip_file.zip]");
                return;
            }

            string zip = args[0];
            string zipname = Path.GetFileNameWithoutExtension(zip);
            string zipdir = Path.GetDirectoryName(zip);

            string tmpdir = Path.Combine(zipdir, "tmp");
            string outdir = Path.Combine(zipdir, "out");

            ZipFile.ExtractToDirectory(zip, tmpdir);
            Directory.CreateDirectory(outdir);

            var filepairs = new TupleList<string, string>
            {
	            { "layers.cfg", "layers.cfg" },
	            { "hole.ger", "Drills.xln" },
	            { "mechanical_details.ger", "Board Outline.ger" },
	
	            { "top_copper.ger", "Top Layer.ger" },
	            { "top_silkscreen.ger", "Top Silk Screen.ger" },
	            { "top_solder_mask.ger", "Top Solder Mask.ger" },
	
	            { "bottom_copper.ger", "Bottom Layer.ger" },
	            { "bottom_silkscreen.ger", "Bottom Silk Screen.ger" },
	            { "bottom_solder_mask.ger", "Bottom Solder Mask.ger" }
            };

            foreach (var pair in filepairs)
            {
                File.Move(Path.Combine(tmpdir, zipname, pair.Item1), Path.Combine(outdir, pair.Item2));
            }

            ZipFile.CreateFromDirectory(outdir, Path.Combine(zipdir, "osh_" + zipname + ".zip"));

            Directory.Delete(tmpdir, true);
            Directory.Delete(outdir, true);
        }
    }
}
