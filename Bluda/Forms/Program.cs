using ClassLibrary.ImplementationsDB;
using ClassLibrary.Interface;
using System;
using System.Data.Entity;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace ClassLibrary
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<Form1>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();

            currentContainer.RegisterType<DbContext, DbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBluda, BludaDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IProduct, ProductDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportService, ReportDB>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
