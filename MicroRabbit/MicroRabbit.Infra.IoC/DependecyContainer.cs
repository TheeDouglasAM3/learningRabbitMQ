using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroRabbit.Banking.Data.Context;
using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Repository;

namespace MicroRabbit.Infra.IoC
{
    public class DependecyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Domain Bus
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
            });

            //Subscriptions
            services.AddTransient<TransferEventHandler>();

            //Domain Events
            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

            //Domain Banking Commands
            services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

            //Application Services
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITransferService, TransferService>();

            //Data
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();
            services.AddTransient<BankingDbContext>();
            services.AddTransient<TransferDbContext>();
        }
    }
}
