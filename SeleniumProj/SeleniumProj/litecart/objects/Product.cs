using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumProj.litecart.objects
{
    public class Product
    {
        public string name { get; set; }

        public string campaign_price { get; set; }
        public string reg_price { get; set; }


        public Color colorCampPrice { get; set; }
        public Color colorRegPrice { get; set; }


        public float sizeCampPrice { get; set; }
        public float sizeRegPrice { get; set; }


        public bool isRGBEqualColorRegPrice() =>
            colorRegPrice.R.Equals(colorRegPrice.B) && colorRegPrice.R.Equals(colorRegPrice.G);


        protected bool Equals(Product other)
        {
            return string.Equals(name, other.name) && string.Equals(campaign_price, other.campaign_price) &&
                   string.Equals(reg_price, other.reg_price);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Product) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (campaign_price != null ? campaign_price.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (reg_price != null ? reg_price.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Product left, Product right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Product left, Product right)
        {
            return !Equals(left, right);
        }
    }
}