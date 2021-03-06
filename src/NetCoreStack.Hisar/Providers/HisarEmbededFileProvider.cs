﻿using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace NetCoreStack.Hisar
{
    public class HisarEmbededFileProvider : IFileProvider
    {
        private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars()
            .Where(c => c != '/' && c != '\\').ToArray();
        
        protected IDictionary<string, Assembly> ComponentsAssemblyLookup { get; }

        public HisarEmbededFileProvider(IDictionary<string, Assembly> componentsAssemblyLookup)
        {
            ComponentsAssemblyLookup = componentsAssemblyLookup;
        }

        private string GetBaseNameSpace(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var baseNamespace = assembly?.GetName()?.Name;
            return string.IsNullOrEmpty(baseNamespace) ? string.Empty : baseNamespace + ".";
        }

        protected virtual Assembly EnsureIsComponentPart(string part)
        {
            ComponentsAssemblyLookup.TryGetValue(part, out Assembly componentAssembly);
            return componentAssembly;
        }

        protected virtual IFileInfo FindFile(Assembly componentAssembly, string subpath, string name)
        {
            var baseNamespace = GetBaseNameSpace(componentAssembly);
            var builder = new StringBuilder(baseNamespace.Length + subpath.Length);
            builder.Append(baseNamespace);
            
            if (subpath.StartsWith("/", StringComparison.Ordinal) || subpath.StartsWith("\\", StringComparison.Ordinal))
            {
                builder.Append(subpath, 1, subpath.Length - 1);
            }
            else
            {
                builder.Append(subpath);
            }

            for (var i = baseNamespace.Length; i < builder.Length; i++)
            {
                if (builder[i] == '/' || builder[i] == '\\')
                {
                    builder[i] = '.';
                }
            }

            var resourcePath = builder.ToString();
            if (HasInvalidPathChars(resourcePath))
            {
                return new NotFoundFileInfo(resourcePath);
            }

            if (componentAssembly.GetManifestResourceInfo(resourcePath) == null)
            {
                return new NotFoundFileInfo(name);
            }

            return new EmbeddedResourceFileInfo(componentAssembly, resourcePath, name, DateTimeOffset.UtcNow);
        }

        protected virtual IFileInfo GetViewFileInfo(string componentId, string subpath, string name)
        {
            if (ComponentsAssemblyLookup.TryGetValue(componentId, out Assembly componentAssembly))
            {
                int index = subpath.IndexOf(componentId);
                if (index != -1)
                    subpath = subpath.Substring(index + componentId.Length);
                
                return FindFile(componentAssembly, subpath, name);
            }
            
            return new NotFoundFileInfo(subpath);
        }

        protected virtual IFileInfo GetComponentFileInfo(string subpath, string name)
        {
            var directoryName = Path.GetDirectoryName(subpath);
            var componentId = Path.GetFileName(directoryName);
            var nameTree = name.Split('.');
            if (nameTree.Length > 2)
            {
                var fileComponentId = nameTree[0];
                var viewName = nameTree[1];
                var extension = nameTree[2];

                var componentViewName = string.Empty;
                ComponentsAssemblyLookup.TryGetValue(componentId, out Assembly componentAssembly);

                subpath = Path.Combine(directoryName, $"{viewName}.{extension}");
                if (componentAssembly == null)
                {
                    if (ComponentsAssemblyLookup.TryGetValue(fileComponentId, out componentAssembly))
                    {
                        return FindFile(componentAssembly, subpath, name);
                    }
                }
                else
                    return FindFile(componentAssembly, subpath, name);
            }

            var assembly = EnsureIsComponentPart(componentId);
            if (assembly != null && name == "_ViewImports.cshtml")
            {
                return FindFile(assembly, "/Views/_ViewImports.cshtml", name);
            }

            string[] paths = subpath.Split('/');
            if (paths.Length > 4)
            {
                for (int i = paths.Length; i-- > 0;)
                {
                    assembly = EnsureIsComponentPart(paths[i]);
                    if (assembly != null)
                    {
                        return FindFile(assembly, subpath, name);
                    }
                }
            }

            return new NotFoundFileInfo(subpath);
        }

        /// <summary>
        /// Get embeded file from component assembly for specified subpath
        /// </summary>
        /// <param name="subpath"></param>
        /// <returns <see cref="IFileInfo"/>></returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            var name = Path.GetFileName(subpath);
            if (subpath.StartsWith("/Areas"))
            {
                var regexPattern = "(?<=/Areas)(.*)(?=/Views)";
                var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
                var componentId = regex.Match(subpath).ToString();
                if (!string.IsNullOrEmpty(componentId))
                {
                    if (componentId.StartsWith("/"))
                        componentId = componentId.Substring(1);
                    
                    return GetViewFileInfo(componentId, subpath, name);
                }
            }

            return GetComponentFileInfo(subpath, name);            
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            // The file name is assumed to be the remainder of the resource name.
            if (subpath == null)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            // Relative paths starting with a leading slash okay
            if (subpath.StartsWith("/", StringComparison.Ordinal))
            {
                subpath = subpath.Substring(1);
            }

            // Non-hierarchal.
            if (!subpath.Equals(string.Empty))
            {
                return NotFoundDirectoryContents.Singleton;
            }

            var entries = new List<IFileInfo>();
            return new EnumerableDirectoryContents(entries);
        }

        /// <summary>
        /// Embedded files do not change.
        /// </summary>
        /// <param name="pattern">This parameter is ignored</param>
        /// <returns>A <see cref="NullChangeToken" /></returns>
        public IChangeToken Watch(string pattern)
        {
            return NullChangeToken.Singleton;
        }

        private static bool HasInvalidPathChars(string path)
        {
            return path.IndexOfAny(_invalidFileNameChars) != -1;
        }
    }
}