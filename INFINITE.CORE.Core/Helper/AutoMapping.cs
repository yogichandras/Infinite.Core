using AutoMapper;
using INFINITE.CORE.Shared.Attributes;
using System.Reflection;
using INFINITE.CORE.Data.Base.Interface;

namespace INFINITE.CORE.Core.Helper
{
    public interface IMapResponse<TModel, TEntity> where TEntity : IEntity
    {
        public void Mapping(IMappingExpression<TEntity, TModel> map);
    }
    public interface IMapRequest<TEntity, TModel> where TEntity : IEntity
    {
        public void Mapping(IMappingExpression<TModel, TEntity> map);
    }
    public interface IListRequest<T>
    {
    }
    public partial class AutoMapping : Profile
    {
        public AutoMapping()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }


        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            MethodInfo mapMethod = this.GetType().GetMethods().First(i => i.Name == "CreateMap" && i.GetGenericArguments().Length == 2);

            var types_response = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapResponse<,>)))
                .ToList();

            var types_request = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapRequest<,>)))
                .ToList();

            var types_list_request = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IListRequest<>)))
                .ToList();

            foreach (var type in types_response)
            {
                var arguments = type.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IMapResponse<,>)).GetGenericArguments();
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                var genericMapMethod1 = mapMethod.MakeGenericMethod(arguments[1], arguments[0]);
                methodInfo?.Invoke(instance, new object[] { genericMapMethod1.Invoke(this, null) });
            }
            foreach (var type in types_request)
            {
                var arguments = type.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IMapRequest<,>)).GetGenericArguments();
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                var genericMapMethod = mapMethod.MakeGenericMethod(arguments[1], arguments[0]);
                methodInfo?.Invoke(instance, new object[] { genericMapMethod.Invoke(this, null) });
            }
            foreach (var type in types_list_request)
            {
                var arguments = type.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IListRequest<>)).GetGenericArguments();
                var instance = Activator.CreateInstance(type);
                mapMethod.MakeGenericMethod(typeof(ListRequest), arguments[0]).Invoke(this,null);
            }
        }

    }

}
