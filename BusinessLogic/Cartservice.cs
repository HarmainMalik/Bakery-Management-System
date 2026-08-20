using System.Collections.Generic;
using System.Linq;
using Models;
using SweetBakery.Models;

namespace SweetBakery.BusinessLogic
{
    // Cart is per-session (in memory only — matches original C++ behavior)
    public class CartService
    {
        public readonly List<CartItem> _cart = new List<CartItem>();

        public IReadOnlyList<CartItem> Items => _cart.AsReadOnly();

        public int TotalPrice => _cart.Sum(i => i.LineTotal);

        public int ItemCount => _cart.Count;

        // Business rule: cannot add more than available stock
        public (bool success, string error) AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                return (false, "Quantity must be greater than zero.");

            // How much is already in the cart for this product?
            int alreadyInCart = _cart
                .Where(i => i.Product == product)
                .Sum(i => i.Quantity);

            if (alreadyInCart + quantity > product.Stock)
                return (false, $"Only {product.Stock - alreadyInCart} units available.");

            // Merge with existing cart line if the product is already in cart
            var existing = _cart.FirstOrDefault(i => i.Product == product);
            if (existing != null)
                existing.Quantity += quantity;
            else
                _cart.Add(new CartItem { Product = product, Quantity = quantity });

            return (true, null);
        }

        public (bool success, string error) RemoveItem(int index)
        {
            if (index < 0 || index >= _cart.Count)
                return (false, "Invalid cart item.");

            _cart.RemoveAt(index);
            return (true, null);
        }

        // Returns the bill lines for display; caller (SalesService) records the sales
        public List<CartItem> Checkout()
        {
            var snapshot = new List<CartItem>(_cart);
            _cart.Clear();
            return snapshot;
        }

        public void Clear() => _cart.Clear();
    }
}