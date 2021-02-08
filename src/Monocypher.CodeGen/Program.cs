using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CppAst;
using CppAst.CodeGen.Common;
using CppAst.CodeGen.CSharp;
using Zio.FileSystems;

namespace Monocypher.CodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            var srcFolder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\..\..\ext\Monocypher\src"));
            var destFolder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\..\Monocypher"));

            if (!Directory.Exists(srcFolder))
            {
                throw new DirectoryNotFoundException($"The source folder `{srcFolder}` doesn't exist");
            }
            if (!Directory.Exists(destFolder))
            {
                throw new DirectoryNotFoundException($"The destination folder `{destFolder}` doesn't exist");
            }


            var marshalNoFreeNative = new CSharpMarshalAttribute(CSharpUnmanagedKind.CustomMarshaler) { MarshalTypeRef = "typeof(UTF8MarshallerNoFree)" };

            var csOptions = new CSharpConverterOptions()
            {
                DefaultClassLib = "monocypher",
                DefaultNamespace = "Monocypher",
                DefaultOutputFilePath = "/monocypher.generated.cs",
                DefaultDllImportNameAndArguments = "MonocypherDll",
                GenerateAsInternal = false,
                DispatchOutputPerInclude = false,

                MappingRules =
                {
                    e => e.Map<CppField>("crypto_blake2b_vtable").Discard(),
                    e => e.Map<CppField>("crypto_sha512_vtable").Discard(),
                }
            };
            csOptions.Plugins.Insert(0, new FixedArrayTypeConverter());
            csOptions.IncludeFolders.Add(srcFolder);
            var files = new List<string>()
            {
                Path.Combine(srcFolder, "monocypher.h"),
                Path.Combine(srcFolder, "optional", "monocypher-ed25519.h"),
            };

            var csCompilation = CSharpConverter.Convert(files, csOptions);

            if (csCompilation.HasErrors)
            {
                foreach (var message in csCompilation.Diagnostics.Messages)
                {
                    Console.Error.WriteLine(message);
                }
                Console.Error.WriteLine("Unexpected parsing errors");
                Environment.Exit(1);
            }

            var fs = new PhysicalFileSystem();

            {
                var subfs = new SubFileSystem(fs, fs.ConvertPathFromInternal(destFolder));
                var codeWriter = new CodeWriter(new CodeWriterOptions(subfs));
                csCompilation.DumpTo(codeWriter);
            }
        }
    }
}
