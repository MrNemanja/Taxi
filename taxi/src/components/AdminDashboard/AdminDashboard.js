import './Admin.css';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import {useState, useEffect } from 'react';
import Profile from '../Profile/Profile';
import AllRides from "./AllRides";
import Verification from "./Verification";

function AdminDashboard() {

    const navigate = useNavigate();
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');
    const clientID = process.env.REACT_APP_CLIENT_ID;
    const [view, setView] = useState(1);
 
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
                if(role === "driver") navigate('/driverDashboard');
                else if(role === "regularUser") navigate('/regularUserDashboard');
    
            }

    }, []);

    const LogOut = async () =>
        {
            localStorage.clear();
            navigate("/");
        }
    
       return(
    
        <div className='AppU'>
            <h1>Welcome {username}!</h1>
            <hr className='hr'/>
            <div className='header'>
             <button className='headerButton' onClick={() => setView(1)}>Profile</button>
             <button className='headerButton' onClick={() => setView(2)}>Verification</button>
             <button className='headerButton' onClick={() => setView(3)}>All rides</button>
             <button className='logoutButton' onClick={LogOut}>Log out</button>
    
             </div>
        {view === 1 ? (  <Profile />
    
        ) : view === 2 ? ( <Verification />
    
        ) : view === 3 ? ( <AllRides />
    
        ): null}
        
       
        </div>
    
       );


}

export default AdminDashboard;
