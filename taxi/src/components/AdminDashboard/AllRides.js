import "./Admin.css";
import { useState, useMemo, useLayoutEffect } from "react";
import { CallGetAllRides } from "../Services/AdminService";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";

function AllRides() {

    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const [sAddress, setSAddress] = useState([]);
    const [eAddress, setEAddress] = useState([]);
    const [price, setPrice] = useState([]);
    const [driverTime, setDriverTime] = useState([]);
    const [driveTime, setDriveTime] = useState([]);
    const [nameD, setNameD] = useState([]);
    const [surnameD, setSurnameD] = useState([]);
    const [nameC, setNameC] = useState([]);
    const [surnameC, setSurnameC] = useState([]);

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

    const getAllRides = async () =>
    {
        const result = await CallGetAllRides(token);

        console.log(result);

        for (let i = 0; i < result.length; i++)
            {
               setSAddress(sAddress => [...sAddress, result[i].startAddress]);
               setEAddress(eAddress => [...eAddress, result[i].endAddress]);
               setPrice(price => [...price, result[i].price]);
               setDriverTime(driverTime => [...driverTime, result[i].driverToMeDuration]);
               setDriveTime(driveTime => [...driveTime, result[i].driveDuration]);
               setNameC(nameC => [...nameC, result[i].nameC]);
               setSurnameC(surnameC => [...surnameC, result[i].surnameC]);
               setNameD(nameD => [...nameD, result[i].nameD]);
               setSurnameD(surnameD => [...surnameD, result[i].surnameD]);
            }
    }

    const renderRides = () =>
    {
        const rides = [];
        for (let j = 0; j < sAddress.length; j++) {
          rides.push(
          <>
          
          <div className="allride">
              <b><label className="labelN">Ride information:</label></b>
                  <br/>
                  <b><i><label className="labelAA">Start Address:&nbsp;</label></i></b>
                  <b><i><label className="labelAA1">{sAddress[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelAA">End Address:&nbsp;</label></i></b>
                  <b><i><label className="labelAA1">{eAddress[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelAA">Price:&nbsp;</label></i></b>
                  <b><i><label className="labelAA1">{price[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelAA">Ride ends in:&nbsp;</label></i></b>
                  <b><i><label className="labelAA1">{driveTime[j]}</label></i></b>
                  <br/>
              <b><label className="labelN">Driver information:</label></b>
                  <br/>
                  <b><i><label className="labelAA">Name:&nbsp;</label></i></b>
                  <b><i><label className="labelAA1">{nameD[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelAA">Surname:&nbsp;</label></i></b>
                  <b><i><label className="labelAA1">{surnameD[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelAA">Driver arrives in:&nbsp;</label></i></b>
                  <b><i><label className="labelAA1">{driverTime[j]}</label></i></b>
                  <br/>
              <b><label className="labelN">Customer:</label></b>
                  <br/>
                  <b><i><label className="labelAA">Name:&nbsp;</label></i></b>
                  <b><i><label className="labelAA1">{nameC[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelAA">Surname:&nbsp;</label></i></b>
                  <b><i><label className="labelAA1">{surnameC[j]}</label></i></b>
          </div>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          </>
          );
        }

        return rides;
    }

    const ret = useMemo(() =>
        {
            getAllRides();

        }, []); 


    return(

        <div className="backgroundA1">
            <div className="backgroundAA">
            { renderRides() }
            </div>
        </div>

    );



}

export default AllRides;