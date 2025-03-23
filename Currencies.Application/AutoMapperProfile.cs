using AutoMapper;
using Currencies.Abstractions.Contracts.Helpers;
using Currencies.Application.ModelDtos.Currency;
using Currencies.Application.ModelDtos.ExchangeRate;
using Currencies.Application.ModelDtos.Role;
using Currencies.Application.ModelDtos.User;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using Currencies.Db.Entities;

namespace Currencies.Application;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CurrencyDto, Currency>();
        CreateMap<Currency, CurrencyDto>();
        CreateMap<Currency, BaseCurrencyDto>();
        CreateMap<AnonymousTypeModel, CurrencyDto>();

        CreateMap<RoleDto, Role>();
        CreateMap<Role, RoleDto>();
        CreateMap<Role, BaseRoleDto>();

        CreateMap<ExchangeRateDto, ExchangeRate>();
        CreateMap<ExchangeRate, ExchangeRateDto>();
        CreateMap<ExchangeRate, BaseExchangeRateDto>();

        CreateMap<UserCurrencyAmountDto, UserCurrencyAmount>();
        CreateMap<UserCurrencyAmount, UserCurrencyAmountDto>();
        CreateMap<UserCurrencyAmount, BaseUserCurrencyAmountDto>();

        CreateMap<UserExchangeHistoryDto, UserExchangeHistory>();
        CreateMap<UserExchangeHistory, UserExchangeHistoryDto>();
        CreateMap<UserExchangeHistory, BaseUserExchangeHistoryDto>();

        CreateMap<ApplicationUser, UserDto>();
    }
}