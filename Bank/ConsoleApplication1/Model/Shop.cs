using Sklep.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklep.Model
{
    public class Shop
    {
        private IShopService _service;
        private IShopServiceCallback _callback;
        private ShopService shopService;
               
        public Shop(ShopService service, IShopServiceCallback callback)
        {
            this._service = service;
            this._callback = callback;
        }

    }
}
