// -----------------------------------------------------------------------
// <copyright file="ViewNotFoundException.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Jang
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ViewNotFoundException : Exception
    {
        public string[] Locations
        {
            get;
            private set;
        }

        public ViewNotFoundException(string location)
            : this(new[] { location })
        {

        }

        public ViewNotFoundException(string[] locations)
        {
            Locations = locations;
        }
    }
}