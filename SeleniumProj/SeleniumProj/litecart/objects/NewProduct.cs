namespace SeleniumProj.litecart.objects
{
    public class NewProduct
    {
        //general
        public string status { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string product_group { get; set; }
        public string quantity { get; set; }
        public string filename { get; set; }
        public string date_from { get; set; }
        public string date_to { get; set; }


        //information
        public string manufacturer_id { get; set; }
        public string keywords { get; set; }
        public string short_description { get; set; }
        public string description { get; set; }
        public string head_title { get; set; }
        public string meta_description { get; set; }


        //prices
        public string purchase_price { get; set; }
        public string purchase_price_currency_code { get; set; }
        public string gross_prices_USD { get; set; }
        public string gross_prices_EUR { get; set; }


        protected bool Equals(NewProduct other)
        {
            return string.Equals(status, other.status) && string.Equals(name, other.name) &&
                   string.Equals(code, other.code) && string.Equals(product_group, other.product_group) &&
                   string.Equals(quantity, other.quantity) &&
                   string.Equals(date_from, other.date_from) && string.Equals(date_to, other.date_to) &&
                   string.Equals(manufacturer_id, other.manufacturer_id) && string.Equals(keywords, other.keywords) &&
                   string.Equals(short_description, other.short_description) &&
                   string.Equals(description, other.description) && string.Equals(head_title, other.head_title) &&
                   string.Equals(meta_description, other.meta_description) &&
                   string.Equals(purchase_price, other.purchase_price) &&
                   string.Equals(purchase_price_currency_code, other.purchase_price_currency_code) &&
                   string.Equals(gross_prices_USD, other.gross_prices_USD) &&
                   string.Equals(gross_prices_EUR, other.gross_prices_EUR);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NewProduct) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (status != null ? status.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (code != null ? code.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (product_group != null ? product_group.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (quantity != null ? quantity.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (filename != null ? filename.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (date_from != null ? date_from.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (date_to != null ? date_to.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (manufacturer_id != null ? manufacturer_id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (keywords != null ? keywords.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (short_description != null ? short_description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (description != null ? description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (head_title != null ? head_title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (meta_description != null ? meta_description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (purchase_price != null ? purchase_price.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (purchase_price_currency_code != null
                               ? purchase_price_currency_code.GetHashCode()
                               : 0);
                hashCode = (hashCode * 397) ^ (gross_prices_USD != null ? gross_prices_USD.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (gross_prices_EUR != null ? gross_prices_EUR.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(NewProduct left, NewProduct right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NewProduct left, NewProduct right)
        {
            return !Equals(left, right);
        }
    }
}