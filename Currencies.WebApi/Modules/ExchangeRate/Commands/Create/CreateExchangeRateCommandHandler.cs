using System.Text.Json;
using Currencies.Abstractions.Contracts.Exceptions;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using Currencies.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Currencies.WebApi.Modules.ExchangeRate.Commands.Create;

public class CreateExchangeRateCommandHandler(IExchangeRateService exchangeRate, TableContext dbContext)
    : IRequestHandler<CreateExchangeRateCommand, List<ExchangeRateDto>?>
{
    public async Task<List<ExchangeRateDto?>> Handle(CreateExchangeRateCommand request, CancellationToken cancellationToken)
    {
        var currencyExchangeRateList = new List<CurrencyExchangeRateDto>();
        var currencies = await dbContext.Currencies
            .AsQueryable()
            .ToListAsync(cancellationToken);

        var httpClient = new HttpClient();

        foreach (var currency in currencies)
        {
            var apiUrl = $"https://api.nbp.pl/api/exchangerates/rates/c/{currency.Symbol}/{request.Date:yyyy-MM-dd}/?format=json";
            try
            {
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        var root = doc.RootElement;
                        var rates = root.GetProperty("rates");
                        if (rates.GetArrayLength() > 0)
                        {
                            var rate = rates[0];

                            var exchangeRate = new CurrencyExchangeRateDto
                            {
                                Code = root.GetProperty("code").GetString(),
                                Ask = rate.GetProperty("ask").GetDecimal(),
                                Bid = rate.GetProperty("bid").GetDecimal()
                            };
                            currencyExchangeRateList.Add(exchangeRate);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new BadRequestException(e.Message);
            }
        }

        httpClient.Dispose();

        return await exchangeRate.CreateAsync(currencyExchangeRateList, cancellationToken);
    }
}