// -----------------------------------------------------------------------
// <copyright file="JangStartup.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

[assembly: WebActivator.PreApplicationStartMethod(typeof(Jang.Mvc.JangStartup), "Start")]

namespace Jang.Mvc
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Initializes the route table with a route pointing to the JangController.Views method
    /// </summary>
    public static class JangStartup
    {
        /// <summary>
        /// WebActivator start method
        /// </summary>
        public static void Start()
        {
            // register custom route
            // /jang/views
            RouteTable.Routes.MapRoute(
                "JangViews",
                "jang/views", // url name, should this be configurable?
                new { controller = "jang", action = "views" },
                null,
                new[] { "Jang.Mvc" });

            RouteTable.Routes.MapRoute(
                "JangEngine",
                "jang/engine", // url name, should this be configurable?
                new { controller = "jang", action = "engine" },
                null,
                new[] { "Jang.Mvc" });
        }
    }
}