
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class CurrencyModel {
    getCurrencySymbol(currencyId: Currency): string | null {
        const symbol = Currency[currencyId];
        return symbol ? symbol : null;
    }

    getCurrencyName(currencyId: Currency): string | null {
        const name = CurrencyNames[currencyId];
        return name ? name : null;
    }
}

enum Currency {
    USD = 1,
    EUR,
    GBP,
    PLN,
    AUD,
    HUF,
    CHF,
    JPY,
    CZK,
    DKK,
    NOK,
    SEK,
    CAD
}

const CurrencyNames: { [key in Currency]: string } = {
    [Currency.USD]: 'Dolar',
    [Currency.EUR]: 'Euro',
    [Currency.GBP]: 'Funt',
    [Currency.PLN]: 'Polska złotówka',
    [Currency.AUD]: 'Dolar australijski',
    [Currency.HUF]: 'Forint',
    [Currency.CHF]: 'Frank szwajcarski',
    [Currency.JPY]: 'Jen',
    [Currency.CZK]: 'Korona czeska',
    [Currency.DKK]: 'Korona duńska',
    [Currency.NOK]: 'Korona norweska',
    [Currency.SEK]: 'Korona szwedzka',
    [Currency.CAD]: 'Dolar kanadyjski'
};