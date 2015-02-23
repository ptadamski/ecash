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
    public class ShopService : IShopService 
    {

      //  private static Dictionary<Guid, Banknote> _baknoteRepository = new Dictionary<Guid, Banknote>();
     
        private IShop _shop;
        private IShopServiceCallback _callback;

        public ShopService()
        {
            //this._callback = OperationContext.Current.GetCallbackChannel<IShopServiceCallback>();
            //this._shop = new Shop(this, _callback);
        }

        public void doInit(Banknote aBanknote)
        {
            throw new NotImplementedException();
        }

        public void doVerifySignature()
        {
            throw new NotImplementedException();
        }

        public void doChooseSides()
        {
            throw new NotImplementedException();
        }

        public void doVerifyBanknote()
        {
            throw new NotImplementedException();
        }
    }
}
