using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Glass.Mapper;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.ContentSearch;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.Data;

namespace PersistenceGlassDemo.ContentSearch
{
    public class GlassDocumentMapperObjectFactory : IIndexDocumentPropertyMapperObjectFactory, ISearchIndexInitializable
    {
        private readonly DefaultDocumentMapperObjectFactory _defaultDocumentMapper = new DefaultDocumentMapperObjectFactory();
        private ISearchIndex _searchIndex;

        public List<string> GetTypeIdentifyingFields(Type baseType, IEnumerable<IExecutionContext> executionContexts)
        {
            SitecoreTypeConfiguration typeConfiguration = Context.Default.GetTypeConfiguration<SitecoreTypeConfiguration>((object)baseType, false, true);
            if (typeConfiguration == null || typeConfiguration.TemplateId == (ID)null)
                return this._defaultDocumentMapper.GetTypeIdentifyingFields(baseType, executionContexts);
            return Enumerable.ToList<string>(Enumerable.Select<AbstractPropertyConfiguration, string>((IEnumerable<AbstractPropertyConfiguration>)typeConfiguration.Properties, (Func<AbstractPropertyConfiguration, string>)(p => this._searchIndex.FieldNameTranslator.GetIndexFieldName((MemberInfo)p.PropertyInfo))));
        }

        public List<Type> GetPotentialCreatedTypes(Type baseType, IEnumerable<IExecutionContext> executionContexts)
        {
            IEnumerable<KeyValuePair<Type, AbstractTypeConfiguration>> source = Enumerable.Where<KeyValuePair<Type, AbstractTypeConfiguration>>((IEnumerable<KeyValuePair<Type, AbstractTypeConfiguration>>)Context.Default.TypeConfigurations, (Func<KeyValuePair<Type, AbstractTypeConfiguration>, bool>)(tc => baseType.IsAssignableFrom(tc.Value.Type) && ((SitecoreTypeConfiguration)tc.Value).TemplateId != (ID)null));
            if (!Enumerable.Any<KeyValuePair<Type, AbstractTypeConfiguration>>(source))
                return this._defaultDocumentMapper.GetPotentialCreatedTypes(baseType, executionContexts);
            return Enumerable.ToList<Type>(Enumerable.Select<KeyValuePair<Type, AbstractTypeConfiguration>, Type>(source, (Func<KeyValuePair<Type, AbstractTypeConfiguration>, Type>)(tc => tc.Value.Type)));
        }

        public object CreateElementInstance(Type baseType, IDictionary<string, object> fieldValues, IEnumerable<IExecutionContext> executionContexts)
        {
            SitecoreTypeConfiguration typeConfiguration = Context.Default.GetTypeConfigurationFromType<SitecoreTypeConfiguration>(baseType, false, true);
            if (typeConfiguration == null || typeConfiguration.TemplateId == (ID)null)
                return this._defaultDocumentMapper.CreateElementInstance(baseType, fieldValues, executionContexts);
            SitecoreContext sitecoreContext = new SitecoreContext();
            SitecoreTypeCreationContext typeCreationContext1 = new SitecoreTypeCreationContext();
            typeCreationContext1.SitecoreService = (ISitecoreService)sitecoreContext;
            typeCreationContext1.RequestedType = baseType;
            typeCreationContext1.InferType = true;
            typeCreationContext1.IsLazy = true;
            typeCreationContext1.TemplateId = ID.Parse(fieldValues["_template"]);
            SitecoreTypeCreationContext typeCreationContext2 = typeCreationContext1;
            using (new SearchSwitcher())
            {
                object obj = sitecoreContext.InstantiateObject((AbstractTypeCreationContext)typeCreationContext2);
                this.SetupProxy(ID.Parse(fieldValues["_group"]), fieldValues, obj as IProxyTargetAccessor);
                return obj;
            }
        }

        protected void SetupProxy(ID id, IDictionary<string, object> fieldValues, IProxyTargetAccessor target)
        {
            if (target == null)
                return;
            SearchInterceptor searchInterceptor = Enumerable.FirstOrDefault<IInterceptor>((IEnumerable<IInterceptor>)target.GetInterceptors(), (Func<IInterceptor, bool>)(x => x is SearchInterceptor)) as SearchInterceptor;
            if (searchInterceptor == null)
                return;
            searchInterceptor.Id = id;
            searchInterceptor.TypeConfiguration = new SitecoreContext().GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>((object)target, false, true);
        }

        public void Initialize(ISearchIndex searchIndex)
        {
            this._defaultDocumentMapper.Initialize(searchIndex);
            this._searchIndex = searchIndex;
        }
    }
}
