// -----------------------------------------------------------------------
// <copyright file="DependencyResolver.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Jang.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IDependencyResolver
    {
        T GetService<T>();
        IEnumerable<T> GetServices<T>();
        void Register(Type serviceType, object activator);
        void Register(Type serviceType, IEnumerable<object> activators);
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<Type, IList<object>> _resolvers = new Dictionary<Type, IList<object>>();
        public static readonly IDependencyResolver Current = new DependencyResolver();

        public DependencyResolver()
        {
            Register(typeof(IViewEngine), new JazzViewEngine());
        }

        public virtual T GetService<T>()
        {
            IList<object> activators;
            if (_resolvers.TryGetValue(typeof(T), out activators))
            {
                if (activators.Count == 0)
                {
                    return default(T);
                }
                if (activators.Count > 1)
                {
                    throw new InvalidOperationException(String.Format("Multiple activators for type {0} are registered. Please call GetServices instead.", typeof(T).FullName));
                }
                return (T)activators[0];
            }
            return default(T);
        }

        public virtual IEnumerable<T> GetServices<T>()
        {
            IList<object> activators;
            if (_resolvers.TryGetValue(typeof(T), out activators))
            {
                if (activators.Count == 0)
                {
                    return null;
                }
                return (activators as IList<T>).ToList();
            }
            return null;
        }

        public virtual void Register(Type serviceType, object activator)
        {
            IList<object> activators;
            if (!_resolvers.TryGetValue(serviceType, out activators))
            {
                activators = new List<object>();
                _resolvers.Add(serviceType, activators);
            }
            else
            {
                activators.Clear();
            }
            activators.Add(activator);
        }

        public virtual void Register(Type serviceType, IEnumerable<object> activators)
        {
            IList<object> list;
            if (!_resolvers.TryGetValue(serviceType, out list))
            {
                list = new List<object>();
                _resolvers.Add(serviceType, list);
            }
            else
            {
                list.Clear();
            }
            foreach (var a in activators)
            {
                list.Add(a);
            }
        }
    }

}
