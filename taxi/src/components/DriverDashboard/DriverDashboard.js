import './Driver.css';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import {useState, useEffect } from 'react';
import NewRides from './NewRides'
import MyRides from './MyRides'
import Profile from '../Profile/Profile';
import CallGetUserRequest from '../Services/DriverService';
import { CallRideD } from '../Services/DriverService';
import Timer from '../Timer/Timer';
import { CallIsBlocked } from '../Services/DriverService';

function DriverDashboard() {

    const navigate = useNavigate();
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');
    const clientID = process.env.REACT_APP_CLIENT_ID;
    const [view, setView] = useState(1);
    const [verified, setVerified] = useState(2);
    const [blocked, setBlocked] = useState(false);
    const [driveSeconds, setDriveSeconds] = useState(0);
    const [driverSeconds, setDriverSeconds] = useState(0);
    const [guidd, setGuidd] = useState();
    const [blockedR, setBlockedR] = useState(false);

    useEffect(() =>
        {
            
            if(token === null)
                {   
                    navigate("/");
                } 
            else
                {
                    
                    const decoded = jwtDecode(token);
                    let currentDate = new Date();
                    if(decoded.exp * 1000 < currentDate.getTime())
                    {
                        localStorage.clear();
                        navigate("/");
                    }
        
                    const role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
                    if(role === "admin") navigate('/adminDashboard');
                    else if(role === "regularUser") navigate('/regularUserDashboard');
                    else
                    {
                        IsVerified();
                        Ride();
                        isBlocked();  
                    }
        
                }
            
    
        }, []);


    const Ride = async () =>
    {
        const result = await CallRideD(token, username);

        setBlocked(result.blocked);
        setDriveSeconds(result.driveSeconds);
        setDriverSeconds(result.driverSeconds);
        setGuidd(result.id);
    }

    const isBlocked = async () =>
    {
        const result = await CallIsBlocked(token, username);

        setBlockedR(result);
    }

    const IsVerified = async () =>
    {
        const result = await CallGetUserRequest(username, token);

        console.log(result);

        if(result === "ACCEPTED") setVerified(0);
        else if(result === "REJECTED") setVerified(1);
        else if(result === "PENDING") setVerified(2);
    }
 
  

    const LogOut = async () =>
        {
            localStorage.clear();
            navigate("/");
        }
    
    
       return(
        <>
        {console.log(blockedR)}
        <div className='AppU'>
            <h1>Welcome {username}!</h1>
            <hr className='hr'/>
            {blocked === false ? (
            <>
            <div className='header'>
            {verified === 0 && blockedR === false ? (
                <>
                <button className='headerButton' onClick={() => setView(1)}>Profile</button>
                <button className='headerButton' onClick={() => setView(2)}>New rides</button>
                <button className='headerButton' onClick={() => setView(3)}>My Rides</button>
                <button className='logoutButton' onClick={LogOut}>Log out</button>
                </>
            ): verified === 0 && blockedR === true ? (
                <>
                <button className='headerButton' onClick={() => setView(1)}>Profile</button>
                <button className='headerButton' onClick={() => setView(3)}>My Rides</button>
                <button className='logoutButton' onClick={LogOut}>Log out</button>
                </> 
            ):verified === 1 ? (
                <>
                <button className='headerButton' onClick={() => setView(1)}>Profile</button>
                <button className='logoutButton' onClick={LogOut}>Log out</button>
                </>

            ): verified === 2 ? (
                <>
                <button className='headerButton' onClick={() => setView(1)}>Profile</button>
                <button className='logoutButton' onClick={LogOut}>Log out</button>
                </>
            ): null}

            </div>
            {view === 1 ? (  <Profile />

            ) : view === 2 ? ( <NewRides/>

            ) : view === 3 ? ( <MyRides/>

            ): null}
            </>
        ): blocked === true ? (
            <>
                <div className='header'>
                    <button className='logoutButton' onClick={LogOut}>Log out</button>
                </div>
                <br/>
                <br/>
                <Timer  time={driverSeconds} id={guidd} type={true} driver={true}/>
                <br/>
                <Timer  time={driveSeconds} id={guidd} type={false} driver={true}/>


            </>
        ) : null}
        
       
        </div>
        </>
    
       );

}

export default DriverDashboard;