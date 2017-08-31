using System;
using NStore.Domain;

namespace NStoreSample.Domain
{
    public class ShoppingCartState
    {
        public int ItemCount { get; private set; }

        private void On(ItemAdded i)
        {
            ItemCount++;
        }
    }

    public class ShoppingCart : Aggregate<ShoppingCartState>
    {
        public void Create(string title)
        {
            Emit(new ShoppingCartCreated(title));
        }

        public void AddItem(string itemId, decimal quantity)
        {
            Emit(new ItemAdded(itemId,quantity));
        }
    }


    public class ShoppingCartCreated
    {
        public string Title { get; private set; }

        public ShoppingCartCreated(string title)
        {
            this.Title = title;
        }
    }


    public class ItemAdded
    {
        public ItemAdded(string itemId, decimal quantity)
        {
            this.ItemId = itemId;
            this.Quantity = quantity;
        }

        public string ItemId { get ; set ; }
        public decimal Quantity { get; set; }
    }
}
