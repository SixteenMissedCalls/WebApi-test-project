using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using CurrencyRateGateway.Entities.Models;

namespace CurrencyRateGateway.Application.Common.Dto
{ 
    [XmlRoot("ValCurs")]
    public class ValCursXmlDto
    {
        [XmlAttribute("Date")]
        public string Date { get; set; }

        [XmlElement("Valute")]
        public List<ValuteDto> Valutes { get; set; } = new List<ValuteDto>();
    }

    public class ValuteDto
    {
        [XmlAttribute("ID")] 
        public string Id { get; set; }

        [XmlElement("NumCode")] 
        public string NumCode { get; set; }

        [XmlElement("CharCode")] 
        public string CharCode { get; set; }

        [XmlElement("Nominal")] 
        public int Nominal { get; set; }

        [XmlElement("Name")] 
        public string Name { get; set; }

        [XmlElement("Value")] 
        public string Value { get; set; }
            
        public CurrencyRate ToDomain(string date)
        {
            var parsedValue = decimal.TryParse(Value.Replace(",", "."),
                NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                ? result
                : 0;

            return new CurrencyRate
            {
                Id = Id,
                NumCode = NumCode,
                CharCode = CharCode,
                Nominal = Nominal,
                Name = Name,
                Value = parsedValue,
                Date = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture)
            };
        }
    }
}