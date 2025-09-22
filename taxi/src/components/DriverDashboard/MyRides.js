import "./Driver.css";
import { useState, useMemo, useLayoutEffect } from "react";
import { CallGetMyRides } from "../Services/DriverService";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";


function NewRide() {

    const navigate = useNavigate();
    const username = localStorage.getItem("username");
    const token = localStorage.getItem("token");
    const [sAddress, setSAddress] = useState([]);
    const [eAddress, setEAddress] = useState([]);
    const [price, setPrice] = useState([]);
    const [name, setName] = useState([]);
    const [surname, setSurname] = useState([]);

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

    const getMyRides = async () =>
    {
        const result = await CallGetMyRides(token, username);

        for (let i = 0; i < result.length; i++)
            {
               setSAddress(sAddress => [...sAddress, result[i].startAddress]);
               setEAddress(eAddress => [...eAddress, result[i].endAddress]);
               setPrice(price => [...price, result[i].price]);
               setName(name => [...name, result[i].name]);
               setSurname(surname => [...surname, result[i].surname]);
            }
    }

    const renderRides = () =>
    {
        const rides = [];
        for (let j = 0; j < sAddress.length; j++) {
          rides.push(
          <>
          <div className="myride">
              <b><label className="labelD">Ride information:</label></b>
                  <br/>
                  <b><i><label className="labelM">Start Address:&nbsp;</label></i></b>
                  <b><i><label className="labelM1">{sAddress[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelM">End Address:&nbsp;</label></i></b>
                  <b><i><label className="labelM1">{eAddress[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelM">Price:&nbsp;</label></i></b>
                  <b><i><label className="labelM1">{price[j]}</label></i></b>
                  <br/>
                  <br/>
              <b><label className="labelD">Customer:</label></b>
                  <br/>
                  <b><i><label className="labelM">Name:&nbsp;</label></i></b>
                  <b><i><label className="labelM1">{name[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelM">Surname:&nbsp;</label></i></b>
                  <b><i><label className="labelM1">{surname[j]}</label></i></b>
          </div>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          </>
          );
        }

        return rides;
    }

    const ret = useMemo(() =>
        {
            getMyRides();

        }, []); 


    return(

        <div className="backgroundD1">
            <div className="backgroundDMy">
            { renderRides() }
            </div>
        </div>

    );


}

export default NewRide;