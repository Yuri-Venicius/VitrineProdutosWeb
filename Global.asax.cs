using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.Mvc5;
using Unity;
using VitrineProdutos.Infra.Repository;
using VitrineProdutos.Infra._common.Interface;
using VitrineProdutos.Infra._common.Repository;

namespace VitrineProdutosWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<ProdutoRepository>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
