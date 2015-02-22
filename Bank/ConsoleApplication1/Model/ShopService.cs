using Common;
using Sklep.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sklep.Model
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    class ShopService : IShopService 
    {

      //  private static Dictionary<Guid, Banknote> _baknoteRepository = new Dictionary<Guid, Banknote>();
     
        private IShop _shop;
        private IShopServiceCallback _callback;

        public ShopService()
        {
            this._callback = OperationContext.Current.GetCallbackChannel<IShopServiceCallback>();
            this._shop = new Shop(this, _callback);
        }
    }
}
