using Ninject;
using Ninject.Parameters;
using Ninject.Extensions.Conventions;

namespace RecoverExcelPassword.Services
{
    public static class Kernel
    {
        #region variables
        private static readonly IKernel kernel;
        #endregion

        static Kernel()
        {
            // create the kernel
            kernel = new StandardKernel();

            // load the modules
            kernel.Bind(k => k.FromAssembliesMatching("RecoverExcelPassword*")
                .SelectAllClasses()
                .BindDefaultInterface()
                .Configure(c => c.InTransientScope()));
        }

        public static T Get<T>()
        {
            return kernel.Get<T>();
        }

        public static T Get<T>(params IParameter[] parameters)
        {
            return kernel.Get<T>(parameters);
        }
    }
}