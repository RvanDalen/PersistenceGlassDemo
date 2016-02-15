using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;

namespace PersistenceGlassDemo.ContentSearch
{
    public class SearchInterceptor : IInterceptor
    {
        private static object _loadlock = new object();
        private bool _isLoaded = false;
        private readonly IDictionary<string, object> _fieldValues = (IDictionary<string, object>)new Dictionary<string, object>();
        private readonly ObjectConstructionArgs _args;

        public SitecoreTypeConfiguration TypeConfiguration { get; set; }

        public ID Id { get; set; }

        public SearchInterceptor(ObjectConstructionArgs args)
        {
            this._args = args;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!invocation.Method.IsSpecialName || !invocation.Method.Name.StartsWith("get_") && !invocation.Method.Name.StartsWith("set_"))
                return;
            string str = invocation.Method.Name.Substring(0, 4);
            string key = invocation.Method.Name.Substring(4);
            if (str == "get_")
            {
                if (_fieldValues.ContainsKey(key))
                {
                    object obj = this._fieldValues[key];
                    invocation.ReturnValue = obj;
                }
                else
                {
                    if (this._isLoaded)
                        return;
                    lock (_loadlock)
                    {
                        if (!this._isLoaded)
                        {
                            SitecoreTypeCreationContext local_3 = (SitecoreTypeCreationContext)this._args.AbstractTypeCreationContext;
                            local_3.Item = local_3.SitecoreService.Database.GetItem(this.Id);
                            SitecoreTypeConfiguration local_4 = local_3.SitecoreService.GlassContext.GetTypeConfigurationFromType<SitecoreTypeConfiguration>(invocation.TargetType, false, true) ?? local_3.SitecoreService.GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>((object)this._args.AbstractTypeCreationContext.RequestedType, false, true);
                            AbstractDataMappingContext local_5 = this._args.Service.CreateDataMappingContext(this._args.AbstractTypeCreationContext, (object)null);
                            foreach (AbstractPropertyConfiguration item_0 in Enumerable.Where<AbstractPropertyConfiguration>((IEnumerable<AbstractPropertyConfiguration>)local_4.Properties, (Func<AbstractPropertyConfiguration, bool>)(x => !this._fieldValues.ContainsKey(x.PropertyInfo.Name))))
                                this._fieldValues[item_0.PropertyInfo.Name] = item_0.Mapper.MapToProperty(local_5);
                            this._isLoaded = true;
                        }
                    }
                    if (!this._fieldValues.ContainsKey(key))
                        return;
                    object obj = this._fieldValues[key];
                    invocation.ReturnValue = obj;
                }
            }
            else if (str == "set_")
                this._fieldValues[key] = invocation.Arguments[0];
            else
                throw new MapperException(Glass.Mapper.ExtensionMethods.Formatted("Method with name {0}{1} on type {2} not supported.", (object)str, (object)key, (object)this._args.Configuration.Type.FullName));
        }
    }
}
