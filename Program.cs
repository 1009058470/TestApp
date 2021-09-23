
using System;
using System.Drawing;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Codec;
using FellowOakDicom.Imaging.LUT;
using FellowOakDicom.Imaging.NativeCodec;
using FellowOakDicom.Imaging.Render;

namespace TestApp
{
    class Program
    {

        static void Init()
        {
            new DicomSetupBuilder()
               .RegisterServices(s => s.AddFellowOakDicom().AddTranscoderManager<NativeTranscoderManager>())
               .SkipValidation()
               .Build();
        }
        public enum CompressType
        {
            J2kLossLessCompress,
            JpegLSCompress,
            DeCompress
        }
        public static bool CompressOrDeCompressDcmFile(CompressType compressType, string inPath, string outPath)
        {
            bool ret = true;
            try
            {
                var transferSyntaxOfDcm = DicomTransferSyntax.ExplicitVRLittleEndian;
                if (compressType.Equals(CompressType.J2kLossLessCompress))
                    transferSyntaxOfDcm = DicomTransferSyntax.JPEG2000Lossless;
                else if (compressType.Equals(CompressType.JpegLSCompress))
                    transferSyntaxOfDcm = DicomTransferSyntax.JPEGLSLossless;
                var dcm = FellowOakDicom.DicomFile.Open(inPath);
                var original = dcm.Dataset;
                var changed = original.Clone(transferSyntaxOfDcm);
                var newdcm = new FellowOakDicom.DicomFile(changed);
                newdcm.Save(outPath);
            }
            catch (Exception ex)
            {
                ret = false;
                Console.WriteLine(ex);
            }
            return ret;
        }

        static void Main(string[] args)
        {
            //TestForCompress();
            Init();

            string inPath = "/root/0.dcm",outPath = "/root/1.dcm";
            CompressOrDeCompressDcmFile(CompressType.JpegLSCompress, inPath, outPath);
            Console.Read();
        }
    }
}
