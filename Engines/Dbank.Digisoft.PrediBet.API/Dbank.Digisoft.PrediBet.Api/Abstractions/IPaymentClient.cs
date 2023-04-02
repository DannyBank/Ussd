﻿using Dbank.Digisoft.PrediBet.Api.Data;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Api.Abstractions
{
    public interface IPaymentClient
    {
        Task<PaymentResponse> RequestPayment(PaymentRequest payload);
    }
}