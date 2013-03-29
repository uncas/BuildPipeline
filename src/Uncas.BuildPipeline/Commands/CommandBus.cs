using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace Uncas.BuildPipeline.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IServiceLocator _serviceLocator;

        public CommandBus(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        #region ICommandBus Members

        public void Publish(ICommand command)
        {
            Type commandType = command.GetType();
            List<Type> handlers =
                commandType.Assembly.GetTypes().Where(type => IsHandler(type, commandType)).ToList();
            foreach (Type handlerType in handlers)
            {
                object handler = _serviceLocator.GetInstance(handlerType);
                if (handler == null)
                    throw new InvalidOperationException("No handler registered.");
                MethodInfo handleMethod = handlerType.GetMethod("Handle");
                handleMethod.Invoke(handler, new object[] {command});
            }
        }

        #endregion

        private static bool IsHandler(Type x, Type commandType)
        {
            IEnumerable<Type> enumerable = x.GetInterfaces().Where(y => y.Name.Contains("ICommandHandler"));
            return enumerable.SelectMany(y => y.GetGenericArguments()).Any(y => y == commandType);
        }
    }
}