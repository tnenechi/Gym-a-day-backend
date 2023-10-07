using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Cryptography;
using System.Data;

public class Gym_a_Day_Backend : I_Gym_a_Day_Backend
    {
        DataClassesDataContext db = new DataClassesDataContext();


        //REGISTER FUNCTION
         public int UserRegister(string userName, string userSurname, string userEmail, string userPhoneNo, string gender, string userPassword)
         {

            //Check if email already exists in the db
            if (!EmailExists(userEmail))
            {
                //if email does NOT exist create new user

                var userType = "";
                if (userEmail.EndsWith("@admin.com")) {
                    userType = "Admin";
                }
                else
                {
                    userType = "Customer";
                }

                var newUser = new UserTable
                {
                    U_Name = userName,
                    U_Surname = userSurname,
                    Email = userEmail,
                    Phone = userPhoneNo,
                    Gender = gender,
                    Password = ToSHA256(userPassword),
                    Type = userType
                };






                db.UserTables.InsertOnSubmit(newUser);

                    try
                    {
                        db.SubmitChanges();
                        //successful registration 
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        ex.GetBaseException();
                        //Exception occured
                        return -1;
                    }

            }
            else
            {
                //email already exists
                return 0;
            }

            

            
         }
     


        //LOGIN FUNCTION - returns ID else 0 if details incorrect
        public int Login(string userEmail, string userPassword)
        {

            var sysUser = (from s in db.UserTables
                           where s.Email.Equals(userEmail) && s.Password.Equals(ToSHA256(userPassword))
                           select s).FirstOrDefault();

            if(sysUser != null)
            {
                return sysUser.ID;
            }
            else
            {
                return 0;
            }

        }


        //GET USER TYPE FUNCTION - returns the usertype
        public string GetUserType(int ID)
        {
            var theUser = GetUser(ID);

            if(theUser != null)
            {
                return theUser.Type;
            }
            else
            {
                return null;
            }
        }


        //GET USER TYPE FUNCTION - returns the user first name
        public string GetUserFirstname(int ID)
        {
            var theUser = GetUser(ID);

            if (theUser != null)
            {
                return theUser.U_Name;
            }
            else
            {
                return null;
            }
        }




        //PRODUCT ADDITION FUNCTION
        public bool AddProduct(string imgPath, string prodName, string prodPrice, String prodDescription, int quantity)
        {
            var aNewProduct = new Product
            {
                Prod_Img_Path = imgPath,
                Prod_Name = prodName,
                Prod_Price = decimal.Parse(prodPrice),
                Prod_Description = prodDescription,
                Prod_Qty = quantity,
                Active = 1
            };

            db.Products.InsertOnSubmit(aNewProduct);


             try
             {
                db.SubmitChanges();
                //successful addition 
                return true;
             }
            catch (Exception ex)
             {
                ex.GetBaseException();
                //Exception occured
                return false;
             }

        }
 



        //EDIT PRODUCT Fucntion - returns 1 if successfull, 0 if product ID does not exist, -1 if an error occured
        public int EditProduct(int ID, string prodName, string prodPrice, String prodDescription, int quantity)
        {
            var theProduct = GetAProduct(ID);

            if(theProduct != null)
            {
                theProduct.Prod_Name = prodName;
                theProduct.Prod_Name = prodPrice;
                theProduct.Prod_Description = prodDescription;
                theProduct.Prod_Qty = quantity;

                try
                {
                    db.SubmitChanges();
                    return 1;
                }catch(Exception e)
                {
                    e.GetBaseException();
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }


        public bool DecreaseProductQty(int ID, int quantityBought)
        {
            var theProduct = GetAProduct(ID);

            if(theProduct != null)
            {
                theProduct.Prod_Qty -= quantityBought;
                
                if(theProduct.Prod_Qty == 0)
                {
                    theProduct.Active = 0;
                }

                try
                {
                    db.SubmitChanges();
                    return true;
                }catch(Exception e)
                {
                    e.GetBaseException();
                }
            }

            return false;
        }




    public int GetProductQty(int ID)
    {
        var theProduct = GetAProduct(ID);

        if(theProduct != null)
        {
            return theProduct.Prod_Qty;
        }
        else
        {
            return 0;
        }
    }

        //DELETE PRODUCT Fucntion - returns 1 if successfull, 0 if product ID does not exist, -1 if an error occured
    public int DeleteProduct(int ID)
           {
                var theProduct = GetAProduct(ID);

                if (theProduct != null)
                {
                    theProduct.Active = 0;

                    try
                    {
                        db.SubmitChanges();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        e.GetBaseException();
                        return -1;
                    }
                }
                else
                {
                    return 0;
                }
        }


        //Method to return all from the product table
        public List<Product> GetProducts()
    {
            dynamic allProducts = (from s in db.Products select s);

            List<Product> listToReturn = new List<Product>();

            if(allProducts != null)
            {
                foreach(Product aProduct in allProducts)
                {
                    var toAdd = new Product
                    {
                        Prod_Id = aProduct.Prod_Id,
                        Prod_Img_Path = aProduct.Prod_Img_Path,
                        Prod_Name = aProduct.Prod_Name,
                        Prod_Price = aProduct.Prod_Price
                    };

                    listToReturn.Add(toAdd);
                }
            }

            return listToReturn;

    }


        //Method to get a specific product
        public Product GetAProduct(int id)
        {
            var AProduct = (from p in db.Products
                            where (p.Prod_Id.Equals(id)) && (p.Active == 1)
                            select p).FirstOrDefault();

            return AProduct;
        }




    //***** HELPER METHODS BELOW ******




    //returns the user with the id
    private UserTable GetUser(int id)
    {
        var toReturn = (from u in db.UserTables
                        where u.ID == id
                        select u).FirstOrDefault();
        return toReturn;
    }
       
    //returns the hashed password
        private static string ToSHA256(string password)
        {
            var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                stringBuilder.Append(bytes[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }



      //returns true if email exists in userTable else false
        private bool EmailExists(string userEmail)
        {
            var sysUser = (from s in db.UserTables
                           where s.Email.Equals(userEmail)
                           select s).FirstOrDefault();

            if (sysUser != null)
            {
                //then the email already exists in the db
                return true;
            }

            return false;
        }

}


