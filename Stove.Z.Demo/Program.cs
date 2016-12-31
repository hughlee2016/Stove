﻿using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFramework;

namespace Stove.Z.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseStove()
                                               .UseEntityFramework()
                                               .UseDefaultEventBus()
                                               .UseDbContextEfTransactionStrategy()
                                               .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                               .CreateResolver();

            Database.SetInitializer(new CreateDatabaseIfNotExists<DemoDbContext>());

            var uowManager = resolver.Resolve<IUnitOfWorkManager>();

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                var personRepository = resolver.Resolve<IRepository<Person>>();

              

                Person person = personRepository.Get(1);

                uow.Complete();
            }
        }
    }
}
