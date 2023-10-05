using System.Xml.Serialization;

namespace SmartStock.Models
{
    [XmlRoot("PagedResultsOfProductuQhWRbQJ", Namespace = "http://schemas.datacontract.org/2004/07/NewCo.Models")]
    public class PagedResultsOfProductuQhWRbQJ
    {
        [XmlElement("Ascending")]
        public bool Ascending { get; set; }

        [XmlElement("Filter")]
        public string Filter { get; set; }

        [XmlElement("OrderBy")]
        public string OrderBy { get; set; }

        [XmlElement("PageNumber")]
        public int PageNumber { get; set; }

        [XmlElement("PageSize")]
        public int PageSize { get; set; }

        [XmlArray("Results")]
        [XmlArrayItem("Product")]
        public List<ProductModel>? Results { get; set; }

        [XmlElement("TotalNumberOfPages")]
        public int TotalNumberOfPages { get; set; }

        [XmlElement("TotalNumberOfRecords")]
        public int TotalNumberOfRecords { get; set; }
    }
}
