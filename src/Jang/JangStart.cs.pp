[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.JangStart), "Start")]

namespace $rootnamespace$
{
    public static class JangStart
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            //There are several view engines available for Jang
            //Jazz is the default one
            Jang.Infrastructure.DependencyResolver.Current.Register(typeof(Jang.IViewEngine), new Jang.JazzViewEngine());
        }
    }
}
