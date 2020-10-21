using OrckestraCommerce.FulfillmentProviders.FulfillmentCarrierProviders.Transsmart.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transsmart.Client
{
    /// <summary>
    /// Transsmart mappers for UOM fields
    /// </summary>
    public class TranssmartMapper
    {
        /// <summary>
        /// Create the class with mappers identified
        /// </summary>
        /// <param name="linearUomMapper">linear uom mapper</param>
        /// <param name="weightUomMapper">weight uom mapper</param>
        /// <param name="quantityUomMapper">quantity uom mapper</param>
        /// <param name="packageTypeMapper">package type mapper</param>
        public TranssmartMapper(string linearUomMapper, string weightUomMapper, string quantityUomMapper, string packageTypeMapper)
        {
            LinearUom = new StringMapper(true, linearUomMapper);
            WeightUom = new StringMapper(true, weightUomMapper);
            QuantityUom = new StringMapper(true, quantityUomMapper);
            PackageType = new StringMapper(true, packageTypeMapper);
        }

        /// <summary>
        /// Gets or sets linear uom mapper
        /// </summary>
        public StringMapper LinearUom { get; set; }

        /// <summary>
        /// Gets or sets weight uom mapper
        /// </summary>
        public StringMapper WeightUom { get; set; }

        /// <summary>
        /// Gets or sets quantity uom mapper
        /// </summary>
        public StringMapper QuantityUom { get; set; }

        /// <summary>
        /// Gets or sets package type mapper
        /// </summary>
        public StringMapper PackageType { get; set; }

    }
}
