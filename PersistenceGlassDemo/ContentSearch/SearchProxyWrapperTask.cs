using Castle.DynamicProxy;
using Glass.Mapper.Pipelines;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.ContentSearch;

namespace PersistenceGlassDemo.ContentSearch
{
    public class SearchProxyWrapperTask : IObjectConstructionTask, IPipelineTask<ObjectConstructionArgs>
    {
        private static volatile ProxyGenerator _generator = new ProxyGenerator();

        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null || !SearchSwitcher.IsSearchContext || args.Configuration.Type.IsSealed)
                return;
            if (args.Configuration.Type.IsInterface)
            {
                args.Result = _generator.CreateInterfaceProxyWithoutTarget(args.Configuration.Type, (IInterceptor)new Glass.Mapper.Sc.ContentSearch.Pipelines.ObjectConstruction.Tasks.SearchProxy.SearchInterceptor(args));
                args.AbortPipeline();
            }
            else
            {
                args.Result = _generator.CreateClassProxy(args.Configuration.Type, new IInterceptor[1]
                {
          (IInterceptor) new SearchInterceptor(args)
                });
                args.AbortPipeline();
            }
        }
    }
}
