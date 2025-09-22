import "./RegularUser.css";
import { useState, useMemo, useLayoutEffect } from "react";
import { CallGetPreviousRides } from "../Services/RegularUserService";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";

function PreviousRides() {

    const username = localStorage.getItem("username");
    const token = localStorage.getItem("token");
    const navigate = useNavigate();
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

    const getPreviousRides = async () =>
    {
        const result = await CallGetPreviousRides(token, username);

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
          <div className="previousride">
              <b><label className="labelRR">Ride information:</label></b>
                  <br/>
                  <b><i><label className="labelR">Start Address:&nbsp;</label></i></b>
                  <b><i><label className="labelR1">{sAddress[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelR">End Address:&nbsp;</label></i></b>
                  <b><i><label className="labelR1">{eAddress[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelR">Price:&nbsp;</label></i></b>
                  <b><i><label className="labelR1">{price[j]}</label></i></b>
                  <br/>
                  <br/>
              <b><label className="labelRR">Driver:</label></b>
                  <br/>
                  <b><i><label className="labelR">Name:&nbsp;</label></i></b>
                  <b><i><label className="labelR1">{name[j]}</label></i></b>
                  <br/>
                  <b><i><label className="labelR">Surname:&nbsp;</label></i></b>
                  <b><i><label className="labelR1">{surname[j]}</label></i></b>
          </div>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          </>
          );
        }

        return rides;
    }

    const ret = useMemo(() =>
        {
            getPreviousRides();

        }, []); 

    return(

        <div className="backgroundPP">
            <div className="backgroundPrevious">
            { renderRides() }
            </div>
        </div>

    );

}

export default PreviousRides;