import './RegularUser.css';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import {useState, useEffect } from 'react';
import Profile from '../Profile/Profile';
import NewRide from './NewRide';
import PreviousRides from './PreviousRides';
import { CallIsBlocked } from '../Services/RegularUserService';
import Timer from '../Timer/Timer';
import Rating from '../Rating/Rating';
import { CallIsRating } from '../Services/RegularUserService';

function RegularUserDashboard() {

    const navigate = useNavigate();
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');
    const clientID = process.env.REACT_APP_CLIENT_ID;
    const [view, setView] = useState(1);
    const [blocked, setBlocked] = useState(false);
    const [rating, setRating] = useState(false);
    const [driveSeconds, setDriveSeconds] = useState(0);
    const [driverSeconds, setDriverSeconds] = useState(0);
    const [guidd, setGuidd] = useState();
    const [rguid, setRguid] = useState();


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
                else if(role === "driver") navigate('/driverDashboard');
                else
                {
                    isBlocked();
                    isRating();
                }
    
            }
    
        }, []);

    const isBlocked = async () =>
    {
        const result = await CallIsBlocked(token, username);

        setBlocked(result.blocked);
        setDriveSeconds(result.driveSeconds);
        setDriverSeconds(result.driverSeconds);
        setGuidd(result.id);
    }

    const isRating = async () =>
    {
        const result = await CallIsRating(token, username);

        console.log(result.item1);
        if(result.item1 === true)
        {
             setRating(true);
             setRguid(result.item2);
        }
        else setRating(false);
    }

    

    const LogOut = async () =>
    {
        localStorage.clear();
        navigate("/");
    }

   return(

    <div className='AppU'>
        <h1>Welcome {username}!</h1>
        <hr className='hr'/>
        {blocked === false && rating === false ? (
            <>
            <div className='header'>
                <button className='headerButton' onClick={() => setView(1)}>Profile</button>
                <button className='headerButton' onClick={() => setView(2)}>New ride</button>
                <button className='headerButton' onClick={() => setView(3)}>Previous rides</button>
                <button className='logoutButton' onClick={LogOut}>Log out</button>

            </div>
            {view === 1 ? (  <Profile />

            ) : view === 2 ? ( <NewRide/>

            ) : view === 3 ? ( <PreviousRides/>

            ): null}
            </>
        ) : blocked === false && rating === true ? (
            <Rating guid={rguid}/>
        ) : blocked === true ? (
            <>
                <div className='header'>
                    <button className='logoutButton' onClick={LogOut}>Log out</button>
                </div>
                <br/>
                <br/>
                <Timer  time={driverSeconds} id={guidd} type={true} driver={false}/>
                <br/>
                <Timer  time={driveSeconds} id={guidd} type={false} driver={false}/>


            </>
        ) : null}
        
    
   
    </div>

   );


}
export default RegularUserDashboard;