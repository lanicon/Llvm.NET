﻿// -----------------------------------------------------------------------
// <copyright file="IgnoreSystemHeaders.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using CppSharp.AST;
using CppSharp.Passes;
using LlvmBindingsGenerator.Configuration;

namespace LlvmBindingsGenerator.Passes
{
    // should always be the first pass so that other passes can rely on IsGenerated etc...
    internal class IgnoreSystemHeadersPass
        : TranslationUnitPass
    {
        public IgnoreSystemHeadersPass( IReadOnlyCollection<IncludeRef> ignoredHeaders )
        {
            IgnoredHeaders = from entry in ignoredHeaders
                             select entry.Path;
        }

        public IgnoreSystemHeadersPass( )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassFields = false;
            VisitOptions.VisitClassMethods = false;
            VisitOptions.VisitClassProperties = false;
            VisitOptions.VisitClassTemplateSpecializations = false;
            VisitOptions.VisitEventParameters = false;
            VisitOptions.VisitFunctionParameters = false;
            VisitOptions.VisitFunctionReturnType = false;
            VisitOptions.VisitNamespaceEnums = false;
            VisitOptions.VisitNamespaceEvents = false;
            VisitOptions.VisitNamespaceTemplates = false;
            VisitOptions.VisitNamespaceTypedefs = false;
            VisitOptions.VisitNamespaceVariables = false;
            VisitOptions.VisitPropertyAccessors = false;
            VisitOptions.VisitTemplateArguments = false;
        }

        public override bool VisitTranslationUnit( TranslationUnit unit )
        {
            if( unit.IncludePath == null || !unit.IsValid || unit.IsInvalid )
            {
                unit.GenerationKind = GenerationKind.None;
                return true;
            }

            bool isExplicitlyIgnored = IgnoredHeaders.Contains( unit.FileRelativePath );
            if( isExplicitlyIgnored )
            {
                unit.Ignore = true;
            }
            else
            {
                unit.GenerationKind = ( unit.IsCoreHeader( ) || unit.IsExtensionHeader( ) ) ? GenerationKind.Generate : GenerationKind.None;
            }

            return true;
        }

        private readonly IEnumerable<string> IgnoredHeaders;
    }
}
