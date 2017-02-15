﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Razor.Evolution;

namespace Microsoft.AspNetCore.Mvc.Razor
{
    public class MvcRazorTemplateEngine : RazorTemplateEngine
    {
        public MvcRazorTemplateEngine(
            RazorEngine engine,
            RazorProject project)
            : base(engine, project)
        {
            Options.DefaultImports = GetDefaultImports();
        }

        public override RazorCodeDocument CreateCodeDocument(RazorProjectItem projectItem)
        {
            var codeDocument = base.CreateCodeDocument(projectItem);
            codeDocument.SetRelativePath(projectItem.Path);

            return codeDocument;
        }

        private static RazorSourceDocument GetDefaultImports()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.WriteLine("@using System");
                writer.WriteLine("@using System.Linq");
                writer.WriteLine("@using System.Collections.Generic");
                writer.WriteLine("@using Microsoft.AspNetCore.Mvc");
                writer.WriteLine("@using Microsoft.AspNetCore.Mvc.Rendering");
                writer.WriteLine("@using Microsoft.AspNetCore.Mvc.ViewFeatures");
                writer.WriteLine("@inject Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TModel> Html");
                writer.WriteLine("@inject Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json");
                writer.WriteLine("@inject Microsoft.AspNetCore.Mvc.IViewComponentHelper Component");
                writer.WriteLine("@inject Microsoft.AspNetCore.Mvc.IUrlHelper Url");
                writer.WriteLine("@inject Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider");
                writer.WriteLine("@addTagHelper Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper, Microsoft.AspNetCore.Mvc.Razor");
                writer.Flush();

                stream.Position = 0;
                return RazorSourceDocument.ReadFrom(stream, filename: null, encoding: Encoding.UTF8);
            }
        }
    }
}
