using System;
using System.Collections.Generic;

namespace OrckestraCommerce.FulfillmentProviders.FulfillmentCarrierProviders.Transsmart.Mapper
{
    /// <summary>
    /// String mapping functionality
    ///   Usage is like this: Item1|Item2!Item3=RESULT
    ///   Any value Item1, Item2 or Item3 will be mapped to RESULT
    ///   Multiple maps are seperated by commas
    ///     e.g. Item1|Item2!Item3=RESULT,Item4|Item5=RESULT2
    /// </summary>
    public class StringMapper
    {
        private Dictionary<string, string> _mappedItems;

        public StringMapper(bool ignoreCase, string mapperCode)
        {
            _mappedItems = ignoreCase ? new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) : new Dictionary<string, string>();
            SetMapperContent(mapperCode);
        }
        
        private void SetMapperContent(string mapperCode)
        {
            if (string.IsNullOrWhiteSpace(mapperCode))
            {
                return;
            }

            foreach (string mapItem in mapperCode.Split(','))
            {
                if (!String.IsNullOrWhiteSpace(mapItem))
                {
                    string[] mapItemArray = mapItem.Split('=');
                    if (mapItemArray.Length != 2)
                    {
                        throw new InvalidOperationException("Unable to split mapper item on '=', see usage.");
                    }
                    foreach (var singleMapItem in mapItemArray[0].Split('|'))
                    {
                        if (_mappedItems.ContainsKey(singleMapItem))
                        {
                            throw new InvalidOperationException($"There is already a map for map item {singleMapItem}");
                        }
                        else
                        {
                            _mappedItems.Add(singleMapItem, mapItemArray[1]);
                        }
                    }
                }
            }
        }

        public string GetMappedValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !_mappedItems.ContainsKey(value))
            {
                return value;
            }

            return _mappedItems[value];
        }
    }
}
