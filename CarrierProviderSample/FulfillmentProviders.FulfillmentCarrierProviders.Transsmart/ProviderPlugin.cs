using Orckestra.Overture;
using Orckestra.Overture.Extensibility;
using OrckestraCommerce.FulfillmentProviders.Core.Manager;
using Transsmart;
using Transsmart.Client;

namespace OrckestraCommerce.FulfillmentProviders.FulfillmentCarrierProviders.Transsmart
{
    public class ProviderPlugin : IPlugin
    {
        public void Register(IOvertureHost host)
        {
            host.Register<TranssmartTokenProvider, ITranssmartTokenProvider>(ComponentLifestyle.Singleton);
        }
    }
}
