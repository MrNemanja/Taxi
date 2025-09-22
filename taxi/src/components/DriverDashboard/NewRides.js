import { useState, useMemo, useLayoutEffect } from "react";
import "./Driver.css";
import { CallGetRides, AcceptRide } from "../Services/DriverService";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";

function NewRides() {

    const navigate = useNavigate();
    const username = localStorage.getItem("username");
    const token = localStorage.getItem("token");
    const [sAddress, setSAddress] = useState([]);
    const [eAddress, setEAddress] = useState([]);
    const [guids, setGuids] = useState([]);

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

    const getRides = async () =>
    {
        const result = await CallGetRides(token, username);

        for (let i = 0; i < result.length; i++)
            {
               setSAddress(sAddress => [...sAddress, result[i].startAddress]);
               setEAddress(eAddress => [...eAddress, result[i].endAddress]);
               setGuids(guids => [...guids, result[i].id]);
            }
    }

    const renderRides = () =>
    {
        const rides = [];
        for (let j = 0; j < sAddress.length; j++) {
          rides.push(
          <>
          <div className="ride">
              <div className="labellD">
                  <b><i><label className="labelD">Start Address:&nbsp;</label></i></b>
                  <b><i><label className="labelD1">{sAddress[j]}</label></i></b>
                  <br/>
                  <br/>
                  <b><i><label className="labelD">End Address:&nbsp;</label></i></b>
                  <b><i><label className="labelD1">{eAddress[j]}</label></i></b>
              </div>
              <div className="buttonsD">
                  <button className="acceptButton" onClick={() => accept(guids[j])}>Accept</button>
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

        return rides;
    }

    const accept = async (id) =>
    {
        console.log(id);
        const result = await AcceptRide(id, username, token);

        if(result === true)
        { 
            alert("You have registered for this ride, wait for the customer to confirm.");
            window.location.reload();
        }

        else alert("Something is wrong.");

    }

    const ret = useMemo(() =>
        {
            getRides();

        }, []); 

    return(

        <div className="backgroundD">
            { renderRides() }
        </div>

    );


}

export default NewRides;