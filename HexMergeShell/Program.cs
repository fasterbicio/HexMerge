using System;
using System.IO;

namespace HexMergeShell
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3) return;

            string bootloader_path = args[0];
            string app_path = args[1];
            string target_path = args[2];

            long bootloader_size = new FileInfo(bootloader_path).Length;
            long app_size = new FileInfo(app_path).Length;

            if (bootloader_size == 0) return;
            if (app_size == 0) return;

            if (!MergeFiles(bootloader_path, app_path, target_path))
            {
                Console.WriteLine("Merge failed");
            }
            else
            {
                long target_size = new FileInfo(target_path).Length;

                Console.WriteLine(string.Format("Bootloader {0} - {1} bytes", bootloader_path, bootloader_size));
                Console.WriteLine(string.Format("Application {0} - {1} bytes", app_path, app_size));
                Console.WriteLine(string.Format("Merged into {0} - {1} bytes", target_path, target_size));
            }

            //Console.WriteLine("Press any key to exit");
            //Console.ReadKey();
        }

        static private bool MergeFiles(string bl, string app, string target)
        {
            StreamWriter sw;
            string[] bl_file, app_file;

            try
            {
                sw = new StreamWriter(target);
            }
            catch
            {
                return false;
            }

            try
            {
                bl_file = File.ReadAllLines(bl);
            }
            catch
            {
                return false;
            }

            try
            {
                for (long i = 0; i < bl_file.Length - 1; i++)
                    sw.WriteLine(bl_file[i]);
            }
            catch
            {
                return false;
            }

            try
            {
                app_file = File.ReadAllLines(app);
            }
            catch
            {
                return false;
            }

            try
            {
                for (long i = 1; i < app_file.Length; i++)
                    sw.WriteLine(app_file[i]);
            }
            catch
            {
                return false;
            }

            sw.Close();
            return true;
        }
    }
}
