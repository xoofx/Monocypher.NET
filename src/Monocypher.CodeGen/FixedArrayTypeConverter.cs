// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using CppAst;
using CppAst.CodeGen.CSharp;

namespace Monocypher.CodeGen
{
    [StructLayout(LayoutKind.Explicit)]
    public class FixedArrayTypeConverter : ICSharpConverterPlugin
    {
        public void Register(CSharpConverter converter, CSharpConverterPipeline pipeline)
        {
            pipeline.GetCSharpTypeResolvers.Add(GetCSharpType);
        }

        public static CSharpType GetCSharpType(CSharpConverter converter, CppType cppType, CSharpElement context, bool nested)
        {
            // Check if a particular CppType has been already converted
            var csType = converter.FindCSharpType(cppType);
            if (csType != null)
            {
                return csType;
            }

            switch (cppType.TypeKind)
            {
                case CppTypeKind.Pointer:
                    if (context is CSharpField) return null;
                    var pointerType = (CppPointerType) cppType;
                    var pointerElementType = pointerType.ElementType;
                    if (IsConst(pointerElementType, out var eltType))
                    {
                        pointerElementType = eltType;
                    }
                    pointerElementType = pointerElementType.GetCanonicalType();
                    return pointerElementType.TypeKind == CppTypeKind.Primitive ? CSharpPrimitiveType.IntPtr : null;

                case CppTypeKind.Array:

                    if (context is CSharpField) return null;
                    var arrayType = (CppArrayType)cppType;
                    var arrayElementType = arrayType.ElementType;
                    if (arrayType.Size <= 0) return null;

                    bool isConst = IsConst(arrayElementType, out _);

                    var csArrayElementType = converter.GetCSharpType(arrayElementType, context, true);
                    var elementTypeName = csArrayElementType.ToString();
                    elementTypeName = char.ToUpper(elementTypeName[0]) + elementTypeName.Substring(1);
                    var freeType = new CSharpFreeType($"{elementTypeName}{arrayType.Size}");
                    csType = new CSharpRefType(isConst ? CSharpRefKind.In : CSharpRefKind.Ref, freeType);
                    
                    return csType;

            }

            return null;
        }
        private static bool IsConst(CppType type, out CppType elementType)
        {
            elementType = null;
            if (type is CppQualifiedType qualifiedType && qualifiedType.Qualifier == CppTypeQualifier.Const)
            {
                elementType = qualifiedType.ElementType;
                return true;
            }

            return false;
        }
    }

}
