﻿using Microsoft.AspNetCore.Mvc;
using TestVadarod.Data.Interfaces.Services;

namespace TestVadarod.Controllers
{
    public class CurrencyRateController : ControllerBase
    {
        private readonly ICurrencyRateService _currencyRateService;
        private readonly ILogger<CurrencyRateController> _logger;

        public CurrencyRateController(ICurrencyRateService currencyRateService, ILogger<CurrencyRateController> logger)
        {
            _currencyRateService = currencyRateService;
            _logger = logger;
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> Post(DateTime date)
        {
            try
            {
                var rates = await _currencyRateService.Add(date);

                if(rates == null)
                    return Ok("Rates have already been added before");

                _logger.LogInformation("Rates added for the specified date");
                
                return Ok("Rates added");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Param: {date} error message: {ex.Message} {ex.StackTrace}");

                return BadRequest();
            }
        }

        [HttpGet("cur_ID/{cur_Id}/date/{date}")] 
        public async Task<IActionResult> Get(int cur_Id,DateTime date)
        {
            try
            {
                var rate = await _currencyRateService.GetByCurrencyAndDate(cur_Id, date);

                if(rate == null)
                {
                    _logger.LogInformation($"Params: {date}; {cur_Id} were not found");

                    return NotFound();
                }
                _logger.LogInformation("The exchange rates have been sent");

                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Params: {date}; {cur_Id} error message: {ex.Message} {ex.StackTrace}");

                return BadRequest();
            }
        }
    }
}
