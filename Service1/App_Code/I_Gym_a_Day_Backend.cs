//Login/Register interface 
using System;
using System.Collections.Generic;
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
        string Login(string userEmail, string userPassword);

    }




