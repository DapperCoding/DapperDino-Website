using DapperDino.Services.Customer;
using DapperDino.Services.Mandate;
using DapperDino.Services.Payment;
using DapperDino.Services.Payment.Refund;
using DapperDino.Services.PaymentMethod;
using DapperDino.Services.Subscription;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using System;
using System.Collections.Generic;
using System.Text;


namespace DapperDino.Core.Mollie
{
    public static class MollieApiClientServiceExtensions
    {
        public static IServiceCollection AddMollieApi(this IServiceCollection services, string apiKey)
        {
            services.AddScoped<IPaymentClient, PaymentClient>();
            services.AddScoped<IPaymentClient, PaymentClient>(x => new PaymentClient(apiKey));
            services.AddScoped<ICustomerClient, CustomerClient>(x => new CustomerClient(apiKey));
            services.AddScoped<IRefundClient, RefundClient>(x => new RefundClient(apiKey));
            services.AddScoped<IPaymentMethodClient, PaymentMethodClient>(x => new PaymentMethodClient(apiKey));
            services.AddScoped<ISubscriptionClient, SubscriptionClient>(x => new SubscriptionClient(apiKey));
            services.AddScoped<IMandateClient, MandateClient>(x => new MandateClient(apiKey));
            services.AddScoped<IInvoicesClient, InvoicesClient>(x => new InvoicesClient(apiKey));
            services.AddScoped<IPaymentOverviewClient, PaymentOverviewClient>();
            services.AddScoped<ICustomerOverviewClient, CustomerOverviewClient>();
            services.AddScoped<ISubscriptionOverviewClient, SubscriptionOverviewClient>();
            services.AddScoped<IMandateOverviewClient, MandateOverviewClient>();
            services.AddScoped<IPaymentMethodOverviewClient, PaymentMethodOverviewClient>();
            services.AddScoped<IPaymentStorageClient, PaymentStorageClient>();
            services.AddScoped<ICustomerStorageClient, CustomerStorageClient>();
            services.AddScoped<ISubscriptionStorageClient, SubscriptionStorageClient>();
            services.AddScoped<IMandateStorageClient, MandateStorageClient>();
            services.AddScoped<IRefundPaymentClient, RefundPaymentClient>();

            return services;
        }
    }
}
