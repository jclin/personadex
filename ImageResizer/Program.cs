using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageResizer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                PrintUsage();
                return;
            }
            if (args[0] != "-i")
            {
                PrintUsage();
                return;
            }
            if (args[3] != "-o")
            {
                PrintUsage();
                return;
            }

            var inputDirectory = args[1];
            if (!Directory.Exists(inputDirectory))
            {
                Console.WriteLine("Error, input directory {0} does not exist.", inputDirectory);
                PrintUsage();
                return;
            }

            Tuple<int, int> outputSize;
            if (!TryParseOutputSize(args[2], out outputSize))
            {
                Console.WriteLine("Error, output size {0} not in the form of [Width]x[Height]", args[2]);
                PrintUsage();
                return;
            }

            string outputDirectory;
            if (!TryUseOrMakeOutputDirectory(args[4], out outputDirectory))
            {
                Console.WriteLine("Error, {0} is not a valid output directory", args[4]);
                PrintUsage();
                return;
            }

            Console.WriteLine("Resizing images in {0} ...", inputDirectory);
            foreach (var inputFileName in Directory.GetFiles(inputDirectory))
            {
                Image inputImage  = null;

                var inputFilePath = Path.Combine(inputDirectory, inputFileName);
                try
                {
                    inputImage = Image.FromFile(inputFilePath);
                    using (var outputImage = new Bitmap(outputSize.Item1, outputSize.Item2))
                    using (var graphics = Graphics.FromImage(outputImage))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode     = SmoothingMode.HighQuality;

                        graphics.DrawImage(inputImage, Rectangle.FromLTRB(0, 0, outputImage.Width, outputImage.Height));

                        string outputFileName = Path.GetFileNameWithoutExtension(inputFileName);
                        outputFileName        = outputFileName.ToLowerInvariant();
                        outputFileName        = outputFileName.Replace(" ", "");
                        outputFileName        = string.Concat(outputFileName, "_", outputSize.Item1, "x", outputSize.Item2, ".png");

                        string outputFilePath = Path.Combine(
                            outputDirectory,
                            outputFileName
                            );
                        outputImage.Save(outputFilePath, ImageFormat.Png);

                        Console.WriteLine("{0} --> {1}", inputFilePath, outputFilePath);
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("{0} is not an image. Skipping.", inputFilePath);
                }
                finally
                {
                    if (inputImage != null)
                    {
                        inputImage.Dispose();
                    }

                    inputImage = null;
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static bool TryParseOutputSize(string arg, out Tuple<int, int> outputSize)
        {
            outputSize = null;

            string[] subStrings = arg.Split('x');
            if (subStrings.Length != 2)
            {
                return false;
            }

            int parsedWidth;
            if (!int.TryParse(subStrings[0], out parsedWidth))
            {
                return false;
            }

            int parsedHeight;
            if (!int.TryParse(subStrings[1], out parsedHeight))
            {
                return false;
            }

            outputSize = new Tuple<int, int>(parsedWidth, parsedHeight);
            return true;
        }

        private static bool TryUseOrMakeOutputDirectory(string arg, out string outputDirectory)
        {
            outputDirectory = string.Empty;

            if (!Path.IsPathRooted(arg))
            {
                arg = Path.Combine(Directory.GetCurrentDirectory(), arg);
            }

            if (!Directory.Exists(arg))
            {
                try
                {
                    Directory.CreateDirectory(arg);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not create directory {0}.", arg);
                    Console.WriteLine(e.Message);
                    return false;
                }
            }

            outputDirectory = arg;
            return true;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: ImageResizer -i sourceDir [Width]x[Height] -o destinationDir");
        }
    }
}
