using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;



    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService2" in both code and config file together.
    [ServiceContract]
    public interface I_Gym_a_Day_Backend
    {
        [OperationContract]
        int UserRegister(string userName, string userSurname, string userEmail, string userPhoneNo, string gender, string userPassword);
        [OperationContract]
         int Login(string userEmail, string userPassword);
        [OperationContract]
        string GetUserType(int ID);

        [OperationContract]
        string GetUserFirstname(int ID);

        [OperationContract]
        bool AddProduct(string imgPath, string prodName, string prodPrice, String prodDescription, int quantity, string category);

        [OperationContract]
        int EditProduct(int ID, string prodName, string prodPrice, String prodDescription, int quantity, string category);

        [OperationContract]
        bool DecreaseProductQty(int ID, int quantityBought);

        [OperationContract]
        int GetProductQty(int ID);

        [OperationContract]
        int DeleteProduct(int ID);

        [OperationContract]
        List<Product> GetProducts();

        [OperationContract]
        Product GetAProduct(int id);

        [OperationContract]
        bool CreateInvoice(int userId, Dictionary<int, int> prodIdQty, decimal totalAmountSpent, DateTime purchaseDate);

        [OperationContract]
        List<Object> GetInvoices(int userId);

        [OperationContract]
        List<Product> GetProductsByCategory(string productCategory);

        [OperationContract]
        List<Product> GetProductByPriceAsc();


        [OperationContract]
        List<Product> GetProductByPriceDsc();


        [OperationContract]
        List<Product> GetProductByNameAsc();


        [OperationContract]
        List<Product> GetProductByNameDsc();

    }




