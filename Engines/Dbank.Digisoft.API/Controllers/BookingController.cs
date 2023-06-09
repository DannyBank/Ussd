﻿using Dbank.Digisoft.Api.Abstractions;
using Dbank.Digisoft.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController {
        public readonly ISBClient _sbClient;
        public readonly ILogger <BookingController> _logger;

        public BookingController(ISBClient client, ILogger<BookingController> logger)
        {
            _sbClient = client;
            _logger = logger;
        }

        [HttpGet("getby/{bookingCode}/{transId}")]
        public async Task<BookingResponse?> GetBookings(string bookingCode, long transId)
        {
            try {
                return string.IsNullOrWhiteSpace(bookingCode)
                    ? null : await _sbClient.GetBookingByCode(bookingCode, transId);
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Running Service Endpoint");
                return null;
            }
        }
    }
}
