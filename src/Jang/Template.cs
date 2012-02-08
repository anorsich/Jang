// -----------------------------------------------------------------------
// <copyright file="Template.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace Jang
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Template
    {
        public string FullPath { get; set; }

        private string _viewName;
        public string ViewName
        {
            get { return _viewName; }
            set { _viewName = Scrub(value); } 
        }
        public Template(string path, string fullPath)
        {
            // we need to strip out the full path here
            string id = Scrub(path);
            Id = id;
            FullPath = fullPath;
        }

        public string Id { get; private set; }

        static readonly Regex Scrubber = new Regex("[^A-Za-z0-9_\\-\\s]", RegexOptions.Compiled);
        static readonly Regex SpaceRemover = new Regex("\\s+");

        public static string Scrub(string text)
        {
            text = Scrubber.Replace(text, "-").Trim();
            text = SpaceRemover.Replace(text, "-");
            text = text.Trim('-');

            while (text.Contains("--")) text = text.Replace("--", "-");

            return text.ToLower();
        }

    }
}