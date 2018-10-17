using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Pattern
{
    class Program
    {
        abstract class Veggies
        {
            private double _PricePerPound;
            private List<IRestaurant> _restaurants = new List<IRestaurant>();

            public Veggies(double pricePerPound)
            {
                _PricePerPound = pricePerPound;
            }

            public void Attach(IRestaurant restaurant)
            {
                _restaurants.Add(restaurant);
            }

            public void Detach(IRestaurant restaurant)
            {
                _restaurants.Remove(restaurant);
            }

            public void Notify()
            {
                foreach (IRestaurant restaurant in _restaurants)
                {
                    restaurant.Update(this);
                }

                Console.WriteLine("");
            }
            public double PricePerPound
            {
                get { return _PricePerPound; }
                set
                {
                    if (_PricePerPound != value)
                    {
                        _PricePerPound = value;
                        Notify(); // Automatically notify observers of price changes
                    }
                }
            }
        }

        class Carrots : Veggies
        {
            public Carrots (double price) : base(price) { }
        }

        interface IRestaurant
        {
            void Update(Veggies veggies);
        }

        class Restaurant : IRestaurant
        {
            private string _name;
            private Veggies _veggies;
            private double _purchaseThreshold;

            public Restaurant(string name, double purchaseThreshold)
            {
                _name = name;
                _purchaseThreshold = purchaseThreshold;
            }

            public void Update(Veggies veggie)
            {
                Console.WriteLine(
                    "Notified {0} of {1}'s " + " price change to {2:C} per pound.",
                    _name, veggie.GetType().Name, veggie.PricePerPound);
                if (veggie.PricePerPound < _purchaseThreshold)
                {
                    Console.WriteLine(_name + " wants to buy some " + veggie.GetType().Name + "!");
                }
            }
        }

        //Note that the Restaurants will want to buy veggies if the price dips below a certain threshold
        //amount, which differs per restaurant.
        //To put this all together, in our Main method we can define a few restaurants that want to
        //observe the price of carrots, then fluctuate that price:

        static void Main(string[] args)
        {
            Carrots carrots = new Carrots(0.82);
            carrots.Attach(new Restaurant("Mackay's", 0.77));
            carrots.Attach(new Restaurant("Johnny's Sports Bar", 0.74));
            carrots.Attach(new Restaurant("Salad Kingdom", 0.75));
            // Fluctuating carrot prices will notify subscribing restaurants.
            carrots.PricePerPound = 0.79;
            carrots.PricePerPound = 0.76;
            carrots.PricePerPound = 0.74;
            carrots.PricePerPound = 0.81;
            Console.ReadKey();
        }
    }
}
