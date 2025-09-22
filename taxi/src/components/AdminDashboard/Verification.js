import "./Admin.css";
import CallGetPendingDrivers from "../Services/AdminService";
import {useMemo, useState, useLayoutEffect } from "react";
import { CallRejectVerification } from "../Services/AdminService";
import { CallAcceptVerification } from "../Services/AdminService";
import { CallBlockDriver } from "../Services/AdminService";
import { CallUnblockDriver } from "../Services/AdminService";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

function Verification() {

    const [names, setNames] = useState([]);
    const [surnames, setSurnames] = useState([]);
    const [usernames, setUsernames] = useState([]);
    const [requests, setRequests] = useState([]);
    const [rate, setRate] = useState([]);
    const [blocked, setBlocked] = useState([]);
    const token = localStorage.getItem('token');
    const navigate = useNavigate();

    useLayoutEffect(() =>
        {
            const decoded = jwtDecode(token);
            let currentDate = new Date();
            if(decoded.exp * 1000 < currentDate.getTime())
            {
                localStorage.clear();
                navigate("/");
            }

        }, []);

    const getPendingDrivers = async () => 
        {
            const result = await CallGetPendingDrivers(token);
            console.log(result);

            for (let i = 0; i < result.length; i++)
            {
               setNames(names => [...names, result[i].name]);
               setSurnames(surnames => [...surnames, result[i].surname]);
               setUsernames(usernames => [...usernames, result[i].username]);
               setRequests(requests => [...requests, result[i].request])
               setRate(rate => [...rate, result[i].rating]);
               setBlocked(blocked => [...blocked, result[i].blocked]);
            }
        }

    const renderDrivers = () =>  {
        
        const drivers = [];
        for (let j = 0; j < names.length; j++) {
          console.log("usao");
          drivers.push(
          <>
          <div className="verification">
              <div className="labell">
                  <b><i><label className="labelA">Name:&nbsp;</label></i></b>
                  <b><i><label className="labelA1">{names[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelA">Surname:&nbsp;</label></i></b>
                  <b><i><label className="labelA1">{surnames[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelA">Request:&nbsp;</label></i></b>
                  <b><i><label className="labelA1">{requests[j]}</label></i></b>
              </div>
              <div className="buttons">
                  {requests[j] === "PENDING" ? (
                  <>
                  <button className="verificationButton" onClick={() => accept(usernames[j])}>Accept</button>
                  <button className="verificationButton" onClick={() => reject(usernames[j])}>Reject</button>
                  </>
                   ): requests[j] === "ACCEPTED" ? (
                    <>
                    <b><i><label className="labelA">Rating:&nbsp;</label></i></b>
                    <b><i><label className="labelA1">{rate[j]}/5</label></i></b>
                    <br/>
                    { blocked[j] === true ? (
                        <>
                        <button className="verificationButton" onClick={() => unblock(usernames[j])}>Unblock</button>
                        </>
                    ): blocked[j] === false ? (
                        <>
                        <button className="verificationButton" onClick={() => block(usernames[j])}>Block</button></>
                    ): null}
                    </>
                    
                   ): null
                  }
              </div>
          </div>
          <br/>
          <br/>
          <br/>
          <br/>
          <br/>
          </>
          );
        }

        return drivers;
      }
    
   const reject = async (username) =>
   {
        const result = await CallRejectVerification(token, username);

        if(result === true)
        {
            alert("Driver has been successfully rejected.");
            window.location.reload();
        }
        else alert("Something is wrong..");

   }

   const accept = async (username) =>
   {
        const result = await CallAcceptVerification(token, username);

        if(result === true)
        {
            alert("Driver has been successfully accepted.");
            window.location.reload();
        }
        else alert("Something is wrong");

   }

   const block = async (username) =>
   {
        const result = await CallBlockDriver(token, username);

        if(result === true)
        {
            alert("Driver has been successfully blocked.");
            window.location.reload();
        }
        else alert("Something is wrong");
   }

   const unblock = async (username) =>
   {
    const result = await CallUnblockDriver(token, username);

    if(result === true)
    {
        alert("Driver has been successfully unblocked.");
        window.location.reload();
    }
    else alert("Something is wrong");
   }

   const ret = useMemo(() =>
    {
         getPendingDrivers();

    }, []); 


    return(

        <div className="backgroundA">

            {renderDrivers()}                  
        </div>

    );
}

export default Verification;