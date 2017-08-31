using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace NStoreSample
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
		public Command LoadItemsCommand { get; set; }
		public Command CreateSampleShoppingCartCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CreateSampleShoppingCartCommand = new Command(async () => await CreateShoppingCartCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Item;
                Items.Add(_item);
                await DataStore.AddItemAsync(_item);
            });
        }

        async Task CreateShoppingCartCommand()
        {
            var repository = AppEngine.GetRepository();
            var cart = await repository.GetById<Domain.ShoppingCart>("cart_1");
            cart.Create("sample shopping cart");
            cart.AddItem("apples", 10);
            await repository.Save(cart, "demo_setup",h=>{h.Add("created-at", DateTime.UtcNow);});

            // restart (to check deserialization)
			repository = AppEngine.GetRepository();
			cart = await repository.GetById<Domain.ShoppingCart>("cart_1");
			cart.AddItem("oranges", 5);
			await repository.Save(cart,"oranges");
		}

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
