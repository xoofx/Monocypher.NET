using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CppAst;
using CppAst.CodeGen.Common;
using CppAst.CodeGen.CSharp;
using HtmlAgilityPack;
using Zio.FileSystems;

namespace Monocypher.CodeGen
{
    class Program
    {
        private static readonly string MonocypherFolder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\..\..\ext\Monocypher"));


        private Dictionary<string, FunctionDoc> _functionDocs = new Dictionary<string, FunctionDoc>();


        static void Main(string[] args)
        {
            var program = new Program();
            program.ProcessDoc();
            program.Run();
        }

        


        public void ProcessDoc()
        {

            // mandoc -Thtml advanced/crypto_x25519_public_key.3monocypher


            // wsl --distribution Ubuntu-20.04 --exec mandoc -Thtml ext/Monocypher/doc/man/man3/advanced/crypto_x25519_public_key.3monocypher


            var docFolder  = Path.Combine(MonocypherFolder, "doc");

            foreach (var file in Directory.EnumerateFiles(docFolder, "*.3monocypher", SearchOption.AllDirectories))
            {
                if (file.Contains("deprecated")) continue;

                var htmlFile = Path.ChangeExtension(Path.GetFileName(file), "html");
                string output;
                if (File.Exists(htmlFile))
                {
                    output = File.ReadAllText(htmlFile);
                }
                else
                {
                    output = LinuxUtil.RunLinuxExe("mandoc", $"-Thtml {Path.GetFileName(file)}", currentDirectory: Path.GetDirectoryName(file));
                    File.WriteAllText(Path.ChangeExtension(Path.GetFileName(file), "html"), output);
                }

                var doc = new HtmlDocument();
                doc.LoadHtml(output);

                Console.WriteLine($"Function: {Path.GetFileName(file)}");

                var desc = doc.GetElementbyId("DESCRIPTION");
                var next = desc.NextSibling;

                var builder = new CSharpXmlComment("summary");
                while (next != null)
                {
                    if (next.Name == "h1")
                    {
                        break;
                    }

                    var text = ProcessTextElement(next, out var shouldExit);
                    if (text != null)
                    {
                        builder.Children.Add(text);
                    }
                    if (shouldExit) break;

                    next = next.NextSibling;
                }


                var functionDoc = new FunctionDoc()
                {
                    Summary = builder
                };

                var arguments = FindArguments(next);
                if (arguments != null)
                {
                    FillParameterDocs(arguments, functionDoc.Parameters);
                }

                var functionName = Path.GetFileNameWithoutExtension(file);

                _functionDocs.Add(functionName, functionDoc);
            }

        }

        private void FillParameterDocs(HtmlNode node, List<CSharpParamComment> parameters)
        {
            // <dl class="Bl-tag">
            //     <dt><var class="Fa" title="Fa">blind_salt</var></dt>
            //     <dd>The output point.</dd>
            //     <dt><var class="Fa" title="Fa">private_key</var></dt>
            //     <dd>The private key (scalar) to use. First, the value is clamped; then the
            //         clamped value's multiplicative inverse (modulo the curve order) is
            //         determined; the clamped value's multiplicative inverse then has its
            //         cofactor cleared, and that final value is then used for scalar
            //         multiplication.</dd>
            //     <dt><var class="Fa" title="Fa">curve_point</var></dt>
            //     <dd>The curve point on X25519 to multiply with the multiplicative inverse
            //         (modulo the curve order) of
            //       <var class="Fa" title="Fa">private_key</var>.</dd>
            //   </dl>
            node = node.FirstChild;
            while (node != null)
            {
                if (node.Name == "dt")
                {
                    var varChild = node.FirstChild;
                    if (varChild.Name == "var" && varChild.HasAttributes && varChild.Attributes["class"].Value == "Fa")
                    {
                        var paramName = varChild.InnerText;
                        node = node.NextSibling;

                        while (node != null)
                        {
                            if (node.Name == "dd")
                            {
                                var paramDesc = ProcessTextElement(node, out _);
                                var paramComment = new CSharpParamComment(paramName);
                                paramComment.Children.Add(paramDesc);
                                parameters.Add(paramComment);
                                break;
                            }
                            node = node.NextSibling;
                        }
                    }
                }

                node = node?.NextSibling;
            }
        }

        private static HtmlNode FindArguments(HtmlNode node)
        {
            while (node != null)
            {
                // <dl class="Bl-tag">
                if (node.Name == "dl" && node.HasAttributes && node.Attributes["class"].Value == "Bl-tag")
                {
                    var previousText = node.PreviousSibling;
                    if (previousText is HtmlTextNode textNode && textNode.Text.Contains("The arguments"))
                    {
                        break;
                    }
                }

                node = node.NextSibling;
            }

            return node;
        }

        private CSharpComment ProcessTextElement(HtmlNode node, out bool shouldExit)
        {
            shouldExit = false;
            // <a class="Xr" title="Xr">crypto_curve_to_hidden(3monocypher)</a>
            // <code class="Fn" title="Fn">crypto_verify16</code>
            if ((node.Name == "a" && node.HasAttributes && node.Attributes["class"].Value == "Xr") ||
                (node.Name == "code" && node.HasAttributes && node.Attributes["class"].Value == "Fn")
                )
            {
                var name = Regex.Match(node.InnerText.Trim(), @"^\w+").Groups[0].Value;

                return new CSharpXmlComment("see")
                {
                    IsSelfClosing = true,
                    IsInline = true,
                    Attributes =
                    {
                        new CSharpXmlAttribute("cref", name)
                    }
                };
            }

            // Convert <div class="Pp"></div> to <br/>
            if (node.Name == "div" && node.HasAttributes && node.Attributes["class"].Value == "Pp")
            {
                return new CSharpXmlComment("br")
                {
                    IsSelfClosing = true,
                };
            }

            // <var class="Fa" title="Fa">key</var>
            if (node.Name == "var" && node.HasAttributes && node.Attributes["class"].Value == "Fa")
            {
                return new CSharpXmlComment("paramref")
                {
                    IsSelfClosing = true,
                    IsInline = true,
                    Attributes =
                    {
                        new CSharpXmlAttribute("name", node.InnerText)
                    }
                };
            }
            
            // Process sub nodes
            if (node.HasChildNodes)
            {
                var groupComment = new CSharpGroupComment();
                node = node.FirstChild;
                while (node != null)
                {
                    var comment = ProcessTextElement(node, out shouldExit);
                    if (comment != null)
                    {
                        groupComment.Children.Add(comment);
                    }
                    if (shouldExit) break;
                    node = node.NextSibling;
                }

                return groupComment;
            }

            var text = node.InnerText;

            shouldExit = false;
            var indexOfTheArguments = text.IndexOf("The arguments");
            if (indexOfTheArguments == 0) return null;
            if (indexOfTheArguments > 0)
            {
                text = text.Substring(0, indexOfTheArguments - 1);
                shouldExit = true;
            }

            if (string.IsNullOrWhiteSpace(text)) return null;
            return new CSharpTextComment(text) { IsHtmlText = true };
        }

        public void Run()
        {
            var srcFolder = Path.Combine(MonocypherFolder, "src");
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


            var monocypher = csCompilation.Members.OfType<CSharpGeneratedFile>().First().Members.OfType<CSharpNamespace>().First().Members.OfType<CSharpClass>().First();


            ProcessMethods(monocypher);



            var fs = new PhysicalFileSystem();

            {
                var subfs = new SubFileSystem(fs, fs.ConvertPathFromInternal(destFolder));
                var codeWriter = new CodeWriter(new CodeWriterOptions(subfs));
                csCompilation.DumpTo(codeWriter);
            }
        }


        private void ProcessMethods(CSharpClass monocypher)
        {
            var newMethods = new List<CSharpCustomMethod>();

            foreach (var member in monocypher.Members)
            {
                if (!(member is CSharpMethod csMethod)) continue;

                _functionDocs.TryGetValue(csMethod.Name, out var funcDoc);

                var fullComment = new CSharpFullComment();
                var textComment = funcDoc?.Summary ?? new CSharpXmlComment("summary") {Children = {new CSharpTextComment($"Function {csMethod.Name}")}};
                fullComment.Children.Add(textComment);
                if (funcDoc != null)
                {
                    foreach (var paramDesc in funcDoc.Parameters)
                    {
                        fullComment.Children.Add(paramDesc);
                    }
                }
                csMethod.Comment = fullComment;

                var extraParameters = new List<ParameterExtra>();
                bool processMethod = false;
                foreach (var parameter in csMethod.Parameters)
                {
                    var kind = ParameterKind.Default;
                    bool isReadOnly = false;

                    if (parameter.ParameterType is CSharpRefType refType && refType.ElementType is CSharpArrayLikeType)
                    {
                        kind = ParameterKind.FixedBuffer;
                        isReadOnly = refType.Kind == CSharpRefKind.In;
                        processMethod = true;
                    }
                    else if (parameter.ParameterType is CSharpPrimitiveType primitive && primitive.Kind == CSharpPrimitiveKind.IntPtr)
                    {
                        kind = ParameterKind.Buffer;
                        isReadOnly = parameter.ParameterType.CppElement is CppPointerType pointerType && pointerType.ElementType is CppQualifiedType qualifiedType && qualifiedType.Qualifier == CppTypeQualifier.Const;
                        processMethod = true;
                    }
                    else if (parameter.Name.EndsWith("size"))
                    {
                        kind = ParameterKind.Size;
                    }

                    var desc = new ParameterExtra(parameter, kind, isReadOnly) {InteropParameter = parameter};
                    extraParameters.Add(desc);
                }

                if (!processMethod) continue;


                var newMethod = new CSharpCustomMethod
                {
                    Name = csMethod.Name, 
                    ReturnType = csMethod.ReturnType,
                    InteropMethod = csMethod,
                    Modifiers = CSharpModifiers.Static | CSharpModifiers.Unsafe,
                };
                newMethod.Comment = fullComment.Clone();

                int sizeParameterCount = 0;

                for (var i = 0; i < extraParameters.Count; i++)
                {
                    var extraParameter = extraParameters[i];

                    if (extraParameter.Kind == ParameterKind.Size)
                    {
                        sizeParameterCount++;

                        var paramPrefix = extraParameter.Parameter.Name.EndsWith("_size") ? extraParameter.Parameter.Name.Substring(0, extraParameter.Parameter.Name.Length - "_size".Length) : string.Empty;

                        bool isSizeUsed = false;
                        for (int j = 0; j < i; j++)
                        {
                            var bufferParameter = extraParameters[j];
                            if (bufferParameter.Kind != ParameterKind.Buffer) continue;

                            if (paramPrefix.Length > 0)
                            {
                                if (bufferParameter.Parameter.Name.EndsWith($"_{paramPrefix}") || bufferParameter.Parameter.Name == paramPrefix)
                                {
                                    extraParameter.SizedBufferParameters.Add(bufferParameter);
                                    bufferParameter.SizeParameter = extraParameter;
                                    isSizeUsed = true;
                                }
                            }
                            else
                            {
                                Debug.Assert(extraParameter.SizedBufferParameters.Count == 0);
                                extraParameter.SizedBufferParameters.Add(bufferParameter);
                                bufferParameter.SizeParameter = extraParameter;
                                isSizeUsed = true;
                            }
                        }

                        if (isSizeUsed)
                        {
                            continue;
                        }
                    }

                    var newCsParameter = new CSharpCustomParameter
                    {
                        Name = extraParameter.Parameter.Name,
                        Extra = extraParameter,
                        InteropParameter = extraParameter.Parameter,
                        ParameterType = extraParameter.Kind == ParameterKind.Default || extraParameter.Kind == ParameterKind.Size ? extraParameter.Parameter.ParameterType : extraParameter.IsReadOnly ? new CSharpFreeType("ReadOnlySpan<byte>") : new CSharpFreeType("Span<byte>"),
                    };
                    newMethod.Parameters.Add(newCsParameter);
                }

                // Cleanup comment parameters if they are not part of the signature
                CleanupCommentParameters(csMethod);
                CleanupCommentParameters(newMethod);

                newMethod.Body = (writer, element) =>
                {
                    var parameters = newMethod.Parameters;

                    // Add parameter verification
                    var checkedParameters = new HashSet<string>();
                    foreach (var param in parameters.OfType<CSharpCustomParameter>())
                    {
                        if (param.Extra.Kind == ParameterKind.FixedBuffer)
                        {
                            writer.WriteLine($"ExpectSize{param.Extra.ArraySize}(nameof({param.Name}), {param.Name}.Length);");
                        }
                        else if (param.Extra.Kind == ParameterKind.Buffer)
                        {
                            var sizeParameter = param.Extra.SizeParameter;
                            if (param.Extra.SizeParameter != null)
                            {
                                if (sizeParameter.SizedBufferParameters.Count >= 2)
                                {
                                    var leftArg = param.Extra.Parameter.Name;
                                    var rightArg = sizeParameter.SizedBufferParameters.First(x => x.Parameter.Name != leftArg).Parameter.Name;
                                    var leftArgAdded = checkedParameters.Add(leftArg);
                                    var rightArgAdded = checkedParameters.Add(rightArg);
                                    if (leftArgAdded || rightArgAdded)
                                    {
                                        writer.WriteLine($"ExpectSameBufferSize(nameof({leftArg}), {leftArg}.Length, nameof({rightArg}), {rightArg}.Length);");
                                    }
                                }
                            }
                        }
                    }

                    foreach (var param in parameters.OfType<CSharpCustomParameter>())
                    {
                        if (param.Extra.Kind == ParameterKind.Buffer)
                        {
                            writer.WriteLine($"fixed(void* {param.Extra.Parameter.Name}_ptr = {param.Extra.Parameter.Name})");
                        }
                    }


                    bool hasReturn = newMethod.ReturnType is CSharpPrimitiveType primitive && primitive.Kind != CSharpPrimitiveKind.Void;

                    var builder = new StringBuilder();
                    if (hasReturn)
                    {
                        builder.Append("return ");
                    }

                    builder.Append($"{newMethod.Name}(");


                    bool isFirst = true;
                    foreach (var interopParameter in newMethod.InteropMethod.Parameters)
                    {
                        if (!isFirst) builder.Append(", ");
                        isFirst = false;

                        var inputParameter = extraParameters.FirstOrDefault(p => p.InteropParameter == interopParameter);

                        if (inputParameter != null)
                        {
                            switch (inputParameter.RefKind)
                            {
                                case CSharpRefKind.In:
                                    builder.Append("in ");
                                    break;
                                case CSharpRefKind.Out:
                                    builder.Append("out ");
                                    break;
                                case CSharpRefKind.Ref:
                                    builder.Append("ref ");
                                    break;
                                case CSharpRefKind.RefReadOnly:
                                    builder.Append("ref readonly ");
                                    break;
                            }

                            if (inputParameter.Kind == ParameterKind.Buffer)
                            {
                                builder.Append($"new IntPtr({interopParameter.Name}_ptr)");
                            }
                            else if (inputParameter.Kind == ParameterKind.FixedBuffer)
                            {
                                var interopParameterType = (CSharpFreeType)((CSharpRefType) interopParameter.ParameterType).ElementType;
                                builder.Append($"MemoryMarshal.AsRef<{interopParameterType.Text}>({inputParameter.Parameter.Name})");
                            }
                            else if (inputParameter.Kind == ParameterKind.Size && inputParameter.SizedBufferParameters.Count > 0)
                            {
                                builder.Append($"({inputParameter.InteropParameter.ParameterType.GetName()}){inputParameter.SizedBufferParameters[0].Parameter.Name}.Length");
                            }
                            else
                            {
                                builder.Append(inputParameter.Parameter.Name);
                            }
                        }
                        else
                        {
                            builder.Append(interopParameter.Name);
                        }
                    }

                    builder.Append(");");
                    writer.WriteLine(builder.ToString());

                };

                newMethods.Add(newMethod);
            }

            // Insert methods
            foreach (var cSharpCustomMethod in newMethods)
            {
                var interopMethodIndex = monocypher.Members.IndexOf(cSharpCustomMethod.InteropMethod);
                monocypher.Members.Insert(interopMethodIndex + 1, cSharpCustomMethod);
            }
        }

        private static void CleanupCommentParameters(CSharpMethod method)
        {
            var toRemoveParamCommentList = new List<CSharpParamComment>();
            foreach (var paramComment in method.Comment.Children.OfType<CSharpParamComment>())
            {
                if (method.Parameters.All(x => x.Name != paramComment.Name))
                {
                    toRemoveParamCommentList.Add(paramComment);
                }
            }

            foreach (var toRemoveParamComment in toRemoveParamCommentList)
            {
                method.Comment.Children.Remove(toRemoveParamComment);
            }
        }

        private static bool IsSpanLikeParameter(CSharpType type) => type is CSharpRefType refType && refType.ElementType is CSharpArrayLikeType;


        private class CSharpCustomMethod : CSharpMethod
        {
            public CSharpMethod InteropMethod { get; set; }
        }


        private class CSharpCustomParameter : CSharpParameter
        {
            public ParameterExtra Extra { get; set; }

            public CSharpParameter InteropParameter { get; set; }
        }


        private class ParameterExtra
        {
            public ParameterExtra(CSharpParameter param, ParameterKind kind, bool isReadOnly)
            {
                Parameter = param;
                Kind = kind;
                IsReadOnly = isReadOnly;
                SizedBufferParameters = new List<ParameterExtra>();
                if (Parameter.ParameterType is CSharpRefType refType)
                {
                    RefKind = refType.Kind;
                    if (refType.ElementType is CSharpArrayLikeType arrayLikeType)
                    {
                        ArraySize = arrayLikeType.Size;
                    }
                }
            }

            public readonly CSharpParameter Parameter;

            public ParameterKind Kind { get; }

            public CSharpRefKind RefKind { get; }
            
            public bool IsReadOnly { get; }

            public int ArraySize { get; }

            public CSharpParameter InteropParameter { get; set; }

            public ParameterExtra SizeParameter { get; set; }

            public List<ParameterExtra> SizedBufferParameters { get; }
        }

        private enum ParameterKind
        {
            Default,
            Buffer,
            FixedBuffer,
            Size,
        }

        private class FunctionDoc
        {
            public FunctionDoc()
            {
                Parameters = new List<CSharpParamComment>();
            }

            public CSharpXmlComment Summary { get; set; }


            public List<CSharpParamComment> Parameters { get; }

        }
    }
}
