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
         int I_Gym_a_Day_Backend.UserRegister(string userName, string userSurname, string userEmail, string userPhoneNo, string gender, string userPassword)
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
           //END REGISTER FUNCTION


        //LOGIN FUNCTION
        string I_Gym_a_Day_Backend.Login(string userEmail, string userPassword)
        {

            var sysUser = (from s in db.UserTables
                           where s.Email.Equals(userEmail) && s.Password.Equals(ToSHA256(userPassword))
                           select s).FirstOrDefault();

            if(sysUser != null)
            {
                return sysUser.Type;
            }
            else
            {
                return null;
            }

        }
          //END LOGIN FUNCTION


        // SHA256 HASHING ALGORITHM FUNCTION
        public static string ToSHA256(string password)
        {
            var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            var stringBuilder = new StringBuilder();
            for(int i = 0; i < bytes.Length;i++)
            {
                stringBuilder.Append(bytes[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
        //END SHA256 FUNCTION



        //EMAILEXISTS FUNCTION: returns true if email exists, else false
        bool EmailExists(string userEmail)
        {
                var sysUser = (from s in db.UserTables
                           where s.Email.Equals(userEmail)
                           select s).FirstOrDefault();

                if(sysUser != null)
                {
                    //then the email already exists in the db
                    return true;
                }

                return false;
        }
        //END EMAILEXISTS FUNCTION


        
        //PRODUCT ADDITION FUNCTION
        int I_Gym_a_Day_Backend.AddProduct(string imgPath, string prodName, string prodPrice, String prodDescription, int quantity)
        {
            var aNewProduct = new Product
            {
                Prod_Img_Path = imgPath,
                Prod_Name = prodName,
                Prod_Price = decimal.Parse(prodPrice),
                Prod_Description = prodDescription,
                Prod_Qty = quantity
            };

            db.Products.InsertOnSubmit(aNewProduct);


             try
             {
                db.SubmitChanges();
                //successful addition 
                return 1;
             }
            catch (Exception ex)
             {
                ex.GetBaseException();
                //Exception occured
                return -1;
             }

        }
            //END PRODUCT ADDITION FUNCTION


        //Method to return all from the product table
        IQueryable<Product> I_Gym_a_Day_Backend.GetProducts()
        {
            IQueryable<Product> toReturn = from s in db.Products select s;
            return toReturn;
        }



    Product I_Gym_a_Day_Backend.GetAProduct(int id)
    {
        var AProduct = (from p in db.Products
                       where p.Prod_Id.Equals(id)
                       select p).FirstOrDefault();

        return AProduct;
    }


}

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service2" in code, svc and config file together.

