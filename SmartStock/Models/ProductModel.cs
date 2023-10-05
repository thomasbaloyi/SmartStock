using System.Xml.Serialization;

namespace SmartStock.Models
{
    [XmlRoot("Product")]
    public class ProductModel
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Category")]
        public int Category { get; set; }

 
        public decimal Price { get; set; }
    }
}
